using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    public GameObject PooledPrefab;
    public Transform PoolParent;
    public List<GameObject> InUse = new List<GameObject>();
    public List<GameObject> Available = new List<GameObject>();

    public BulletPool(GameObject bulletPrefab, Transform poolParent)
    {
        PooledPrefab = bulletPrefab;
        PoolParent = poolParent;
    }

    public GameObject FetchBullet()
    {
        GameObject bullet;
        int count = Available.Count;

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

        bullet.SetActive(true);
        InUse.Add(bullet);
        return bullet;
    }

    public void ReturnBullet(GameObject bulletObject)
    {
        if (Available.Contains(bulletObject)) return;

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

    public void AllocateBullets(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newObj = GameObject.Instantiate(PooledPrefab, PoolParent);
            newObj.GetComponent<PooledBullet>().OriginPrefab = PooledPrefab;
            newObj.SetActive(false);

            Available.Add(newObj);
        }
    }
}

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance;
    [SerializeField] private int _poolAmount;
    [SerializeField] private GameObject _poolsParent;
    [SerializeField] private List<GameObject> _poolOnStart;
    private Dictionary<GameObject, BulletPool> _bulletPools;

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

    public GameObject FetchBullet(GameObject bulletPrefab)
    {
        BulletPool pool;

        if (bulletPrefab.GetComponent<PooledBullet>() == null)
        {
            Debug.LogError($"Failed to fetch a clone for {bulletPrefab}, it has no pooled bullet component!");
            return null;
        }

        if (_bulletPools.TryGetValue(bulletPrefab, out BulletPool p))
        {
            pool = p;
        } 
        else
        {
            pool = CreatePool(bulletPrefab);
        }

        return pool.FetchBullet();
    }

    public void ReturnBullet(GameObject bulletObject)
    {
        if (bulletObject.GetComponent<PooledBullet>() == null)
        {
            Debug.LogError($"Failed to return {bulletObject} to a pool, it has no PooledBullet component!");
            return;
        }

        PooledBullet poolObj = bulletObject.GetComponent<PooledBullet>();
        BulletPool pool = _bulletPools.TryGetValue(poolObj.OriginPrefab, out BulletPool p) ? p : CreatePool(poolObj.OriginPrefab);
        pool.ReturnBullet(bulletObject);
    }

    private BulletPool CreatePool(GameObject bulletPrefab)
    {
        GameObject poolParent = new GameObject($"{bulletPrefab.name}Pool");
        poolParent.transform.SetParent(_poolsParent.transform);

        BulletPool newPool = _bulletPools[bulletPrefab] = new BulletPool(bulletPrefab, poolParent.transform);
        newPool.AllocateBullets(_poolAmount);

        return newPool;
    }
}
