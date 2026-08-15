using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _seconds = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        _seconds += Time.deltaTime;
        _timerText.text = ConvertTimeToString(_seconds);
    }

    private string ConvertTimeToString(float time)
    {
        int totalSeconds = Mathf.RoundToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes}:{seconds:D2}";
    }

    public int GetSeconds()
    {
        return Mathf.RoundToInt(_seconds);
    }
}
