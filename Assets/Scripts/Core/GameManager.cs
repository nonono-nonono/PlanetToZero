using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Possible game states
public enum GameState
{
    Main,
    Playing,
    GameEnded
}

// Central game manager which controls the whole flow of the game.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameObject PlayerObject;
    private GameState _gameState = GameState.Playing;
    private CanvasGroup _endScreen;
    private TextMeshProUGUI _scoreText;

    // Singleton pattern which ensures only 1 GameManager exists. Game Manager is not destroyed when swapping scenes.
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

    // Gets current game state which is used to see if something should run or not in other components.
    public GameState GetGameState()
    {
        return _gameState;
    }

    // Sets game state to main and goes back to the main menu with the title and the play button
    public void ReturnToMainMenu()
    {
        _gameState = GameState.Main;
        StartCoroutine(LoadSceneNextFrame("Main"));
    }

    // Set game state to playing and starts the game by loading the game scene
    public void StartGame()
    {
        _gameState = GameState.Playing;
        StartCoroutine(LoadSceneNextFrame("Game"));
    }

    // Sets the game state to game ended and shows the end screen.
    public void EndGame()
    {
        _gameState = GameState.GameEnded;
        
        // Check if score manager and time manager exists in current scene.
        if (ScoreManager.Instance != null && TimeManager.Instance != null)
        {
            // Calculate final score which is time * 10 + score (based on enemies killed)
            int finalScore = TimeManager.Instance.GetSeconds() * 10 + ScoreManager.Instance.GetScore();

            // Check if end screen and score text exists. If it exists, enabled the end screen, make it appear (alpha = 1) and show the final score. Otherwise, log an error.SS
            if (_endScreen != null & _scoreText != null)
            {
                _endScreen.gameObject.SetActive(true);
                _endScreen.alpha = 1;
                _scoreText.text = $"Score: {finalScore}";
            }
            else
            {
                Debug.LogError("No Ending Screen Registered in this scene!");
            }
        }
    }

    // Registers a gameObj into PlayerObject. Other components can reference this.
    public void RegisterPlayerObject(GameObject gameObj)
    {
        PlayerObject = gameObj;
    }

    // Registers the end game screen. End screen can differ per scene if wanted.
    public void RegisterEndGameScreen(CanvasGroup screen, TextMeshProUGUI scoreText)
    {
        _endScreen = screen;
        _scoreText = scoreText;
    }

    // Scene swapping is deferred to next frame as doing it synchronously can interfere with Unity's physics system which can crash the game.
    private IEnumerator LoadSceneNextFrame(string sceneName)
    {
        yield return null;
        _endScreen = null;
        _scoreText = null;
        SceneManager.LoadScene(sceneName);
    }
}
