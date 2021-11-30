using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    
    private Vector2 initialPos;
    private float xOffset;
    private float yOffset;
    private float upperXBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
    private float lowerXBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
    private float lowerYBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
    private float upperYBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;

    void Update(){
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began){
                initialPos = Camera.main.ScreenToWorldPoint(touch.position);
                xOffset = initialPos.x - transform.position.x;
                yOffset = initialPos.y - transform.position.y;
            }

            if(touch.phase == TouchPhase.Moved){
                float newX = Camera.main.ScreenToWorldPoint(touch.position).x - xOffset;
                float newY = Camera.main.ScreenToWorldPoint(touch.position).y - yOffset;
                
                transform.position = new Vector2(Mathf.Clamp(newX, lowerXBound, upperXBound), Mathf.Clamp(newY, lowerYBound, upperYBound));
                
            }
        }
    }

}
