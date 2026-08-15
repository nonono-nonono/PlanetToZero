using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Main,
    Playing,
    GameEnded
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameState _gameState = GameState.Playing;
    private CanvasGroup _endScreen;
    private TextMeshProUGUI _scoreText;

    void Awake()
    {
         if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public void ReturnToMainMenu()
    {
        _gameState = GameState.Main;
        StartCoroutine(LoadSceneNextFrame("Main"));
    }

    public void StartGame()
    {
        _gameState = GameState.Playing;
        StartCoroutine(LoadSceneNextFrame("Game"));
    }

    public void EndGame()
    {
        _gameState = GameState.GameEnded;
        
        if (ScoreManager.Instance != null && TimeManager.Instance != null)
        {
            int finalScore = TimeManager.Instance.GetSeconds() * ScoreManager.Instance.GetScore();
            if (_endScreen != null & _scoreText != null)
            {
                _endScreen.alpha = 1;
                _scoreText.text = $"Score: {finalScore}";
            }
            else
            {
                Debug.LogError("No Ending Screen Registered in this scene!");
            }
        }
    }

    public void RegisterEndGameScreen(CanvasGroup screen, TextMeshProUGUI scoreText)
    {
        _endScreen = screen;
        _scoreText = scoreText;
    }

    private IEnumerator LoadSceneNextFrame(string sceneName)
    {
        yield return null;
        _endScreen = null;
        _scoreText = null;
        SceneManager.LoadScene(sceneName);
    }
}
