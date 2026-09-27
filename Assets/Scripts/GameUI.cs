using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private WaveController waveController;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private Button restartButton;

    private int lastScore = -1;
    private int lastAmmo = -1;
    private int lastWave = -1;
    private bool gameOverShown;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(gameManager.RestartGame);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(gameManager.RestartGame);
    }

    private void Update()
    {
        if (lastScore != gameManager.Score)
        {
            lastScore = gameManager.Score;
            scoreText.text = $"Score: {lastScore}";
            if (gameOverShown)
            {
                finalScoreText.text = $"Final Score: {lastScore}";
            }
        }

        if (lastAmmo != gameManager.RemainingAmmo)
        {
            lastAmmo = gameManager.RemainingAmmo;
            ammoText.text = $"Ammo: {lastAmmo}";
        }

        if (lastWave != waveController.CurrentWave)
        {
            lastWave = waveController.CurrentWave;
            waveText.text = $"Wave: {lastWave}";
        }

        if (!gameOverShown && gameManager.IsGameOver)
        {
            gameOverShown = true;
            finalScoreText.text = $"Final Score: {gameManager.Score}";
            gameOverPanel.SetActive(true);
        }
    }
}
