using TMPro;
using UnityEngine;

// Keeps track of current time alive.SS
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _seconds = 0f;

    // Singleton pattern ensures only 1 time manager exists.
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Updates time every frame using time.delaTime.
    void Update()
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        _seconds += Time.deltaTime;
        _timerText.text = ConvertTimeToString(_seconds);
    }

    // Converts time in seconds cleanly to 0:00 format
    private string ConvertTimeToString(float time)
    {
        int totalSeconds = Mathf.RoundToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        
        // D2 pads the seconds to 00. (appears as 01 02 03)
        return $"{minutes}:{seconds:D2}";
    }

    // Returns total seconds passed for this playthrough
    public int GetSeconds()
    {
        return Mathf.RoundToInt(_seconds);
    }
}
