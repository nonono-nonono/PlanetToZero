using UnityEngine;

// Spawns a prefab every x seconds. 
public class Spawner : MonoBehaviour
{

    [SerializeField] private float _interval;
    [SerializeField] private GameObject _spawnPrefab;
    private float _elapsed;

    void Update()
    {
        // Doesn't spawn if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        // Incrementing elapsed by deltaTime until it hits interval.
        if (_elapsed < _interval)
        {
            _elapsed += Time.deltaTime;
            return;
        }

        // Resetting elapsed for the next spawn.
        _elapsed = 0f;

        // Spawning in prefab at spawner position.
        GameObject newPrefab = Instantiate(_spawnPrefab);
        newPrefab.transform.position = transform.position;
    }
}
