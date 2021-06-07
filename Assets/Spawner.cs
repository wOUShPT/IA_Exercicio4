using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public List<GameObject> projectilePool;
    void Awake()
    {
        projectilePool = new List<GameObject>();
        CreatePool(5);
    }

    private void Start()
    {
        InvokeRepeating("ShootProjectile", 1, 1);
    }


    GameObject GetProjectile()
    {
        foreach (var projectile in projectilePool)
        {
            if (projectile.activeSelf == false)
            {
                return projectile;
            }
        }
        return null;
    }
    

    void CreatePool(int poolCapacity)
    {
        for (int i = 0; i < poolCapacity; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectilePool.Add(projectile);
            projectile.transform.SetParent(transform.parent);
            projectile.SetActive(false);
        }
    }

    void ShootProjectile()
    {
        GameObject projectile = GetProjectile();
        projectile.transform.position = transform.position;
        projectile.transform.rotation = projectilePrefab.transform.rotation;
        projectile.transform.localPosition = new Vector3(Random.Range(-4f,4f), transform.localPosition.y, transform.localPosition.z);
        projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
        projectile.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        projectile.SetActive(true);
    }
}
