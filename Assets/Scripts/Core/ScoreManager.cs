using UnityEngine;

// Keeps track of current score derived from changed score reaction for current play through.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private float _score;

    // Singleton pattern which ensures only 1 score manager exists.
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ChangeScore(float amount)
    {
        _score += amount;
    }
    

    public int GetScore()
    {
        return Mathf.RoundToInt(_score);
    }
}
