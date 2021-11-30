using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour{

    ObjectPooler objectPooler;
    bool canShoot = true;

    void Start(){
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    void Update(){
        transform.Rotate(0, 0, 360 * Time.deltaTime);
        if(canShoot){
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot(){
        canShoot = false;
        objectPooler.instantiateObjFromPool("EnemyShotType1", transform.position, transform.rotation);
        yield return new WaitForSeconds(0.01f);
        canShoot = true;
    }
}
