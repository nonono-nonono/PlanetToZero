using System.Collections.Generic;
using UnityEngine;

// A class to represent a bullet pool for 1 bullet prefab.
public class BulletPool
{
    public GameObject PooledPrefab;
    public Transform PoolParent;
    public List<GameObject> InUse = new();
    public List<GameObject> Available = new();

    public BulletPool(GameObject bulletPrefab, Transform poolParent)
    {
        PooledPrefab = bulletPrefab;
        PoolParent = poolParent;
    }

    // Gives a bullet game object from the bullet pool.
    public GameObject FetchBullet()
    {
        GameObject bullet;
        int count = Available.Count;

        // Checks if there are any available deactivated bullet game object. Sets bullet variable to an available bullet if true, otherwise creates a new bullet game object.
        if (count > 0)
        {
            bullet = Available[count - 1];
            Available.RemoveAt(count - 1);
        }
        else
        {
            GameObject newObj = bullet = GameObject.Instantiate(PooledPrefab, PoolParent);
            newObj.GetComponent<PooledBullet>().OriginPrefab = PooledPrefab;
        }

        // Activates bullet game object, add to list of bullets InUse and returns the bullet game object.
        bullet.SetActive(true);
        InUse.Add(bullet);
        return bullet;
    }

    // Returns a bullet game object to the bullet pool.
    public void ReturnBullet(GameObject bulletObject)
    {
        // If available already contains this bullet game object, ignore.
        if (Available.Contains(bulletObject)) return;

        // Makes sure bullet game object given is actually tracked by the pool. 
        // If true, deactivate the bullet and return it to the list of available bullet game objects, else destroy the game object.
        
        if (InUse.Contains(bulletObject))
        {
            InUse.Remove(bulletObject);
            bulletObject.SetActive(false);
            Available.Add(bulletObject);
        }
        else
        {
            GameObject.Destroy(bulletObject);
        }
    }
    
    // Allocates a certain amount of available but deactivated bullets at runtime.S
    public void AllocateBullets(int amount)
    {
        // Instantiating new bullet game objects, deactivating them and adding them to list of available bullet game objects.
        for (int i = 0; i < amount; i++)
        {
            GameObject newObj = GameObject.Instantiate(PooledPrefab, PoolParent);
            newObj.GetComponent<PooledBullet>().OriginPrefab = PooledPrefab;
            newObj.SetActive(false);

            Available.Add(newObj);
        }
    }
}

// Manages all bullet pools and allows other components to fetch and return bullets to their respective pools without looking for them manually.
public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance;
    [SerializeField] private int _poolAmount;
    [SerializeField] private GameObject _poolsParent;
    [SerializeField] private List<GameObject> _poolOnStart;
    private Dictionary<GameObject, BulletPool> _bulletPools;

    // Singleton pattern to ensure only 1 bullet pool manager exists in a scene.
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _bulletPools = new Dictionary<GameObject, BulletPool>();
    }

    // Create a bullet pool for each bullet that was marked to be pooled when the game starts.
    void Start()
    {
        foreach (GameObject bulletPrefab in _poolOnStart)
        {
            if (bulletPrefab.GetComponent<PooledBullet>() == null)
            {
                Debug.LogWarning($"{bulletPrefab} as no pooled bullet component!");
                continue;
            }

            CreatePool(bulletPrefab);
        }
    }

    // Fetches a bullet from a bullet pool given the prefab of that pool.
    public GameObject FetchBullet(GameObject bulletPrefab)
    {
        BulletPool pool;

        // If game object given does not have an origin prefab (stored in PooledBullet) ignore.
        if (bulletPrefab.GetComponent<PooledBullet>() == null)
        {
            Debug.LogError($"Failed to fetch a clone for {bulletPrefab}, it has no pooled bullet component!");
            return null;
        }

        // Sees if pool for given bullet game object already exists. If true just use that pool, otherwise create a pool.
        if (_bulletPools.TryGetValue(bulletPrefab, out BulletPool p))
        {
            pool = p;
        } 
        else
        {
            pool = CreatePool(bulletPrefab);
        }

        // Gives a bullet game object from that bullet pool.
        return pool.FetchBullet();
    }

    // Returns a bullet game object to its pool.
    public void ReturnBullet(GameObject bulletObject)
    {
        // If game object given does not have an origin prefab (stored in PooledBullet) ignore.
        if (bulletObject.GetComponent<PooledBullet>() == null)
        {
            Debug.LogError($"Failed to return {bulletObject} to a pool, it has no PooledBullet component!");
            return;
        }

        // Grab the origin prefab, find its pool. If the pool exists, return to that pool, otherwise create a new pool and return to that pool instead.
        PooledBullet poolObj = bulletObject.GetComponent<PooledBullet>();
        BulletPool pool = _bulletPools.TryGetValue(poolObj.OriginPrefab, out BulletPool p) ? p : CreatePool(poolObj.OriginPrefab);
        pool.ReturnBullet(bulletObject);
    }

    // Creates a pool for bullets.
    private BulletPool CreatePool(GameObject bulletPrefab)
    {
        // Create an empty game object for all pooled bullets for this prefab to live under.
        GameObject poolParent = new($"{bulletPrefab.name}Pool");
        poolParent.transform.SetParent(_poolsParent.transform);

        // Create a bullet pool instance and allocate bullets to it.
        BulletPool newPool = _bulletPools[bulletPrefab] = new BulletPool(bulletPrefab, poolParent.transform);
        newPool.AllocateBullets(_poolAmount);

        // Gives back the newly created pool.
        return newPool;
    }
}
