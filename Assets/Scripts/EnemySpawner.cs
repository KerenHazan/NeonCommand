using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private EnemyMissile enemyMissilePrefab;
    [SerializeField] private City[] cities;

    private readonly HashSet<EnemyMissile> activeMissiles = new HashSet<EnemyMissile>();

    private readonly Stack<EnemyMissile> availableMissiles = new Stack<EnemyMissile>();
    private readonly List<City> aliveCities = new List<City>();

    public int PoolSize { get; private set; }
    public int ReuseCount { get; private set; }
    public int ActiveMissileCount => activeMissiles.Count;

    public bool HasLivingCities()
    {
        if (cities == null)
        {
            return false;
        }

        foreach (City city in cities)
        {
            if (city != null && city.IsAlive)
            {
                return true;
            }
        }

        return false;
    }

    public void NotifyMissileResolved(EnemyMissile missile)
    {
        if (activeMissiles.Remove(missile))
        {
            availableMissiles.Push(missile);
        }
    }

    public void SpawnMissile()
    {
        if (mainCamera == null || enemyMissilePrefab == null || cities == null)
        {
            return;
        }

        aliveCities.Clear();
        foreach (City city in cities)
        {
            if (city != null && city.IsAlive)
            {
                aliveCities.Add(city);
            }
        }

        if (aliveCities.Count == 0)
        {
            return;
        }

        City targetCity = aliveCities[Random.Range(0, aliveCities.Count)];
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(
            new Vector3(Random.Range(0.05f, 0.95f), 0.95f, -mainCamera.transform.position.z));
        spawnPosition.z = 0f;

        EnemyMissile missile;
        if (availableMissiles.Count > 0)
        {
            missile = availableMissiles.Pop();
            ReuseCount++;
        }
        else
        {
            missile = Instantiate(enemyMissilePrefab, transform);
            PoolSize++;
        }
        activeMissiles.Add(missile);
        missile.Launch(targetCity, spawnPosition, this);
    }
}
