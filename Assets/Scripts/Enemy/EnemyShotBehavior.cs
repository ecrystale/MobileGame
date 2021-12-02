using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotBehavior : MonoBehaviour
{

    private float shotSpeed;
    private Vector2 direction;

    private float upperXBound;
    private float lowerXBound;
    private float lowerYBound;
    private float upperYBound;

    void Start()
    {
        upperXBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerXBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerYBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
        upperYBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
    }

    void Update()
    {
        if (transform.position.x > upperXBound || transform.position.x < lowerXBound ||
           transform.position.y > upperYBound || transform.position.y < lowerYBound)
        {
            gameObject.SetActive(false);
        }
        transform.Translate(direction * shotSpeed * Time.deltaTime);
    }

    public void setDirection(Vector2 direction) { this.direction = direction; }
    public void setShotSpeed(float speed) { shotSpeed = speed; }

}
