using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int startingMissilesPerWave = 5;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private float timeBetweenWaves = 2.0f;

    public int CurrentWave { get; private set; }
    public bool IsWaveRunning { get; private set; }

    private IEnumerator Start()
    {
        if (enemySpawner == null || gameManager == null)
        {
            Debug.LogError("WaveController requires EnemySpawner and GameManager references.", this);
            yield break;
        }

        while (!gameManager.IsGameOver && enemySpawner.HasLivingCities())
        {
            CurrentWave++;
            gameManager.BeginWave(CurrentWave);
            IsWaveRunning = true;
            int missilesThisWave = startingMissilesPerWave + (CurrentWave - 1);

            for (int i = 0; i < missilesThisWave; i++)
            {
                if (gameManager.IsGameOver || !enemySpawner.HasLivingCities())
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
                if (gameManager.IsGameOver || !enemySpawner.HasLivingCities())
                {
                    IsWaveRunning = false;
                    yield break;
                }

                yield return null;
            }

            IsWaveRunning = false;
            if (gameManager.IsGameOver || !enemySpawner.HasLivingCities())
            {
                yield break;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
}
