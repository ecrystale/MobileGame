using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotBehavior : ShotBehaviour
{
    private float shotSpeed;
    private Vector2 direction;
    private float _deltaTime => IsUI ? Time.unscaledDeltaTime : Time.deltaTime;

    void Update()
    {
        if (!Game.CurrentGame.WorldBound.CheckIsWithinBound(transform.position))
        {
            gameObject.SetActive(false);
        }
        transform.Translate(direction * shotSpeed * _deltaTime);
    }

    public void setDirection(Vector2 direction) { this.direction = direction; }
    public void setShotSpeed(float speed) { shotSpeed = speed; }

}
