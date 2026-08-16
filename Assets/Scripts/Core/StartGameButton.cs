using UnityEngine;
using UnityEngine.UI;

// Starts the game when button is clicked.
// Not done through inspector due to constant scene swapping erasing the connection.
[RequireComponent(typeof(Button))]
public class StartGameButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(GameManager.Instance.StartGame);
    }
}
