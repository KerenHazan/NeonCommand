using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private City[] cities;
    [SerializeField] private int startingAmmoPerWave = 10;
    [SerializeField] private int scorePerMissile = 100;

    public int Score { get; private set; }
    public int RemainingAmmo { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Update()
    {
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (IsGameOver || cities == null || cities.Length == 0)
        {
            return;
        }

        foreach (City city in cities)
        {
            if (city != null && city.IsAlive)
            {
                return;
            }
        }

        IsGameOver = true;
    }

    public void AddMissileDestroyedScore()
    {
        Score += scorePerMissile;
    }

    public bool TryUseAmmo()
    {
        CheckGameOver();
        if (IsGameOver || RemainingAmmo <= 0)
        {
            return false;
        }

        RemainingAmmo--;
        return true;
    }

    public void BeginWave(int waveNumber)
    {
        CheckGameOver();
        if (!IsGameOver)
        {
            RemainingAmmo = startingAmmoPerWave;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameObject.scene.path);
    }
}
