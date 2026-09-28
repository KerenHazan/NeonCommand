using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum GameState
{
    MainMenu,
    WaveIntro,
    Playing,
    WaveClear,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private City[] cities;
    [SerializeField] private int startingAmmoPerWave = 10;
    [SerializeField] private int scorePerMissile = 100;

    [SerializeField] private int chainBonusPerGeneration = 50;
    [SerializeField] private int scorePerSurvivingCity = 100;
    [SerializeField] private int scorePerUnusedShot = 10;
    private const string BestScoreKey = "BestScore";

    public int BestScore { get; private set; }
    public int LastWaveBonus { get; private set; }
    public int Score { get; private set; }
    public int RemainingAmmo { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    public bool IsGameOver => CurrentState == GameState.GameOver;
    public bool CanFire => CurrentState == GameState.Playing && Time.frameCount > stateChangedFrame;

    private GameState stateBeforePause;
    private int stateChangedFrame;
    private static bool playAfterReload;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStartup()
    {
        playAfterReload = false;
    }

    private void Awake()
    {
        BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (playAfterReload)
        {
            playAfterReload = false;
            StartGame();
        }
    }

    private void SetState(GameState state)
    {
        CurrentState = state;
        stateChangedFrame = Time.frameCount;
    }

    public void StartGame()
    {
        if (CurrentState == GameState.MainMenu)
        {
            SetState(GameState.WaveIntro);
        }
    }

    public void ShowWaveIntro()
    {
        if (!IsGameOver)
        {
            SetState(GameState.WaveIntro);
        }
    }

    public void CompleteWave()
    {
        if (CurrentState == GameState.Playing)
        {
            int livingCities = 0;
            foreach (City city in cities)
            {
                if (city != null && city.IsAlive) livingCities++;
            }
            LastWaveBonus = livingCities * scorePerSurvivingCity + RemainingAmmo * scorePerUnusedShot;
            Score += LastWaveBonus;
            SetState(GameState.WaveClear);
        }
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Paused)
        {
            ResumeGame();
        }
        else if (CurrentState == GameState.Playing || CurrentState == GameState.WaveIntro ||
                 CurrentState == GameState.WaveClear)
        {
            stateBeforePause = CurrentState;
            SetState(GameState.Paused);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            SetState(stateBeforePause);
        }
    }

    private void Update()
    {
        CheckGameOver();
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private void CheckGameOver()
    {
        if (CurrentState == GameState.MainMenu || CurrentState == GameState.Paused ||
            IsGameOver || cities == null || cities.Length == 0)
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

        SetState(GameState.GameOver);
        if (Score > BestScore)
        {
            BestScore = Score;
            PlayerPrefs.SetInt(BestScoreKey, BestScore);
            PlayerPrefs.Save();
        }
    }

    public void AddMissileDestroyedScore(int chainGeneration = 0)
    {
        if (CurrentState == GameState.Playing)
        {
            Score += scorePerMissile + Mathf.Max(0, chainGeneration) * chainBonusPerGeneration;
        }
    }

    public bool TryUseAmmo()
    {
        CheckGameOver();
        if (!CanFire || RemainingAmmo <= 0)
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
            SetState(GameState.Playing);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        playAfterReload = true;
        SceneManager.LoadScene(gameObject.scene.path);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        playAfterReload = false;
        SceneManager.LoadScene(gameObject.scene.path);
    }
}
