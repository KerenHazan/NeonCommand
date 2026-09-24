using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int startingMissilesPerWave = 5;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private float timeBetweenWaves = 2.0f;

    public int CurrentWave { get; private set; }
    public bool IsWaveRunning { get; private set; }

    private IEnumerator Start()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("WaveController requires an EnemySpawner reference.", this);
            yield break;
        }

        while (enemySpawner.HasLivingCities())
        {
            CurrentWave++;
            IsWaveRunning = true;
            int missilesThisWave = startingMissilesPerWave + (CurrentWave - 1);

            for (int i = 0; i < missilesThisWave; i++)
            {
                if (!enemySpawner.HasLivingCities())
                {
                    IsWaveRunning = false;
                    yield break;
                }

                enemySpawner.SpawnMissile();
                if (i < missilesThisWave - 1)
                {
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            while (enemySpawner.ActiveMissileCount > 0)
            {
                if (!enemySpawner.HasLivingCities())
                {
                    IsWaveRunning = false;
                    yield break;
                }

                yield return null;
            }

            IsWaveRunning = false;
            if (!enemySpawner.HasLivingCities())
            {
                yield break;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
}
