using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private float _elapsedTime = 0f;
    private bool _isPaused = false;

    [SerializeField] private TextMeshProUGUI _timerText;

    private void Awake()
    {
        Instance = this;

        BallController.OnDeathBegan += PauseTimerForPlayer;
    }

    private void Start()
    {
        // Reset to default timesScales
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void OnDestroy()
    {
        BallController.OnDeathBegan -= PauseTimerForPlayer;
    }

    private void UpdateTimer(float deltaTime)
    {
        if (!_isPaused)
        {
            _elapsedTime += deltaTime;
        }
    }

    private void PauseTimerForPlayer(Transform player) => PauseTimer();

    public static void ResetTimer()
    {
        Instance._elapsedTime = 0f;
    }

    public static void PauseTimer()
    {
        Instance._isPaused = true;
    }

    public static void ResumeTimer()
    {
        Instance._isPaused = false;
    }

    public static string GetFormattedTime()
    {
        int minutes = (int)(Instance._elapsedTime / 60);
        int seconds = (int)(Instance._elapsedTime % 60);
        int milliseconds = (int)((Instance._elapsedTime * 100) % 100); // Two-digit milliseconds

        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
    }

    private void Update()
    {
        UpdateTimer(Time.deltaTime);
        _timerText.text = GetFormattedTime();
    }
}
