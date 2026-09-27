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
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject waveIntroPanel;
    [SerializeField] private GameObject waveClearPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text waveIntroText;
    [SerializeField] private TMP_Text waveClearText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseRestartButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button gameOverMenuButton;

    private int lastScore = -1;
    private int lastAmmo = -1;
    private int lastWave = -1;
    private GameState? lastState;

    private void Awake()
    {
        restartButton.onClick.AddListener(gameManager.RestartGame);
        playButton.onClick.AddListener(gameManager.StartGame);
        pauseButton.onClick.AddListener(gameManager.TogglePause);
        resumeButton.onClick.AddListener(gameManager.ResumeGame);
        pauseRestartButton.onClick.AddListener(gameManager.RestartGame);
        pauseMenuButton.onClick.AddListener(gameManager.ReturnToMainMenu);
        gameOverMenuButton.onClick.AddListener(gameManager.ReturnToMainMenu);
    }

    private void OnDestroy()
    {
        if (restartButton != null) restartButton.onClick.RemoveListener(gameManager.RestartGame);
        if (playButton != null) playButton.onClick.RemoveListener(gameManager.StartGame);
        if (pauseButton != null) pauseButton.onClick.RemoveListener(gameManager.TogglePause);
        if (resumeButton != null) resumeButton.onClick.RemoveListener(gameManager.ResumeGame);
        if (pauseRestartButton != null) pauseRestartButton.onClick.RemoveListener(gameManager.RestartGame);
        if (pauseMenuButton != null) pauseMenuButton.onClick.RemoveListener(gameManager.ReturnToMainMenu);
        if (gameOverMenuButton != null) gameOverMenuButton.onClick.RemoveListener(gameManager.ReturnToMainMenu);
    }

    private void Update()
    {
        if (lastScore != gameManager.Score)
        {
            lastScore = gameManager.Score;
            scoreText.text = $"Score: {lastScore}";
            if (gameManager.IsGameOver)
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
            waveIntroText.text = $"WAVE {Mathf.Max(1, lastWave)}";
            waveClearText.text = $"WAVE {lastWave} CLEAR";
        }

        if (lastState != gameManager.CurrentState)
        {
            lastState = gameManager.CurrentState;
            mainMenuPanel.SetActive(lastState == GameState.MainMenu);
            waveIntroPanel.SetActive(lastState == GameState.WaveIntro);
            waveClearPanel.SetActive(lastState == GameState.WaveClear);
            pausePanel.SetActive(lastState == GameState.Paused);
            gameOverPanel.SetActive(lastState == GameState.GameOver);
            hud.SetActive(lastState != GameState.MainMenu);
            pauseButton.interactable = lastState == GameState.Playing ||
                lastState == GameState.WaveIntro || lastState == GameState.WaveClear;
            if (lastState == GameState.GameOver)
            {
                finalScoreText.text = $"Final Score: {gameManager.Score}";
            }
        }
    }
}
