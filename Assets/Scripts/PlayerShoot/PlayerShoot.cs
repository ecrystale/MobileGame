using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour{

    ArrayList shotSpawners = new ArrayList();
    private float rateOfFire = PublicVars.rateOfFire;

    void Start(){
        foreach(Transform tr in transform){
            if(tr.tag == "ShotSpawner"){
                shotSpawners.Add(tr);
            }
        }
    }

    void Update(){
        if(Input.touchCount > 0){
            foreach (Transform shotSpawner in shotSpawners){
                if(shotSpawner.GetComponent<PlayerShotSpawner>().canFire){
                    StartCoroutine(shoot(shotSpawner));
                }
            }
        }  
    }

    IEnumerator shoot(Transform shotSpawner){
        shotSpawner.GetComponent<PlayerShotSpawner>().canFire = false;
        shotSpawner.GetComponent<PlayerShotSpawner>().shoot();
        yield return new WaitForSeconds(rateOfFire);
        shotSpawner.GetComponent<PlayerShotSpawner>().canFire = true;
    }
}
