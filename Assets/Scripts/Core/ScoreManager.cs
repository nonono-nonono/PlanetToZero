using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private float _score;

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
    
    public void ResetScore()
    {
        _score = 0;
    }

    public int GetScore()
    {
        return Mathf.RoundToInt(_score);
    }
}
