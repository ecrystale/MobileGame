using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    public void shoot()
    {
        objectPooler.instantiateObjFromPool("PlayerShot", transform.position, Quaternion.identity);
    }
}
