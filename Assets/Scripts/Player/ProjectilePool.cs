using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initialize pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // Get a projectile from the pool
    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true); // Activate before returning
                return obj;
            }
        }

        // Expand pool if needed
        GameObject newObj = Instantiate(projectilePrefab, transform);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }

    // Return a projectile to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
