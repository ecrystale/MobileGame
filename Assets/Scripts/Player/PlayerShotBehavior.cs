using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotBehavior : MonoBehaviour
{
    public int Damage = 5;
    private float shotSpeed = PublicVars.shotSpeed;
    private float upperXBound;
    private float lowerXBound;
    private float lowerYBound;
    private float upperYBound;

    public bool homing;
    public int angularVelocity;
    private GameObject target;

    void Start()
    {
        upperXBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerXBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerYBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
        upperYBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;

        homing = true;
    }

    void Update()
    {
        if(!homing){
            if (transform.position.x > upperXBound || transform.position.x < lowerXBound ||
                transform.position.y > upperYBound || transform.position.y < lowerYBound)
            {
                gameObject.SetActive(false);
            }
        }

        if(homing){
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            target = player.GetComponent<PlayerMovement>().getClosestEnemy();

            if(target == null){
                transform.Translate(Vector2.up * shotSpeed * Time.deltaTime);
                return;
            }

            Vector2 direction = (Vector2)target.transform.position - (Vector2)player.transform.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            transform.Rotate(new Vector3(0, 0, -angularVelocity * rotateAmount), Space.Self);
        }

        transform.Translate(Vector2.up * shotSpeed * Time.deltaTime);

        if (transform.position.x > upperXBound || transform.position.x < lowerXBound ||
           transform.position.y > upperYBound || transform.position.y < lowerYBound)
        {
            gameObject.SetActive(false);
        }
    }
}
