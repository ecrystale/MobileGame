using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotBehavior : MonoBehaviour{

    private float shotSpeed = PublicVars.shotSpeed;
    private float upperXBound;
    private float lowerXBound; 
    private float lowerYBound; 
    private float upperYBound;

    void Start(){
        upperXBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerXBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerYBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
        upperYBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
    }

    void Update(){
        if(transform.position.x > upperXBound || transform.position.x < lowerXBound ||
           transform.position.y >  upperYBound || transform.position.y < lowerYBound){
               gameObject.SetActive(false);
        }
        transform.Translate(Vector2.up * shotSpeed * Time.deltaTime);
    }
}
