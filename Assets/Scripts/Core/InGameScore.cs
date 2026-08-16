using TMPro;
using UnityEngine;

// Shows current score while the player is playing.
public class InGameScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    // Updates the score text every frame.
    void Update()
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // Checks if score manager and time manager exists in current scene and calculates the score to show to the player.
        if (ScoreManager.Instance != null && TimeManager.Instance != null)
        {
            _text.text = $"Score: {ScoreManager.Instance.GetScore() + TimeManager.Instance.GetSeconds() * 10}";
        }
    }
}
