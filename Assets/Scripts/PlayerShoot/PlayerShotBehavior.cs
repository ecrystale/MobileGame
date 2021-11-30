using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotBehavior : MonoBehaviour{

    private float shotSpeed = PublicVars.shotSpeed;

    void Update(){
        transform.Translate(Vector2.up * shotSpeed * Time.deltaTime);
    }
}
