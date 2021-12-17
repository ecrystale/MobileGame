using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotBehavior : MonoBehaviour
{
    public int Damage = 25;
    private float _shotSpeed;

    public bool homing;
    public int angularVelocity;
    private GameObject target;

    void Start()
    {
        homing = true;
        _shotSpeed = Game.CurrentGame.PlayerData.shotSpeed;
    }

    void Update()
    {
        if (homing)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }

            target = player.GetComponent<PlayerMovement>().getClosestEnemy();

            if (target == null)
            {
                transform.Translate(Vector2.up * _shotSpeed * Time.deltaTime);
                return;
            }

            Vector2 direction = (Vector2)target.transform.position - (Vector2)player.transform.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            transform.Rotate(new Vector3(0, 0, -angularVelocity * rotateAmount), Space.Self);
        }

        transform.Translate(Vector2.up * _shotSpeed * Time.deltaTime);

        if (!Game.CurrentGame.WorldBound.CheckIsWithinBound(transform.position))
        {
            gameObject.SetActive(false);
        }
    }
}
