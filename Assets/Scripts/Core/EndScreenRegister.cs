using TMPro;
using UnityEngine;

public class EndScreenRegister : MonoBehaviour
{
    [SerializeField] private CanvasGroup _endScreen;
    [SerializeField] private TextMeshProUGUI _scoreText;

    void Start()
    {
        GameManager.Instance.RegisterEndGameScreen(_endScreen, _scoreText);
    }
}
