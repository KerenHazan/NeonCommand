using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private EnemyMissile enemyMissilePrefab;
    [SerializeField] private City[] cities;

    private readonly HashSet<EnemyMissile> activeMissiles = new HashSet<EnemyMissile>();

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
        activeMissiles.Remove(missile);
    }

    public void SpawnMissile()
    {
        if (mainCamera == null || enemyMissilePrefab == null || cities == null)
        {
            return;
        }

        List<City> aliveCities = new List<City>();
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

        EnemyMissile missile = Instantiate(enemyMissilePrefab);
        activeMissiles.Add(missile);
        missile.Launch(targetCity, spawnPosition, this);
    }
}
