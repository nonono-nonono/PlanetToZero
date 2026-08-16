using TMPro;
using UnityEngine;

// Registers an end screen with the game manager. This appears when the player dies.
public class RegisterEndScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _endScreen;
    [SerializeField] private TextMeshProUGUI _scoreText;

    void Start()
    {
        GameManager.Instance.RegisterEndGameScreen(_endScreen, _scoreText);
        gameObject.SetActive(false);
    }
}
