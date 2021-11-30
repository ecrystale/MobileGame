using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour{

    ArrayList shotSpawners = new ArrayList(); 

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
                shotSpawner.GetComponent<PlayerShotSpawner>().shoot();
            }
        }  
    }
}
