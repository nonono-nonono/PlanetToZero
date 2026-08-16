using UnityEngine;

// Registers player to game manager.
public class RegisterPlayer : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.RegisterPlayerObject(gameObject);
    }
}
