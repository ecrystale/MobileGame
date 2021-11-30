using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotSpawner : MonoBehaviour{
    public GameObject playerShotPrefab;

    public void shoot(){
        Instantiate(playerShotPrefab, transform.position, Quaternion.identity);
    }
}
