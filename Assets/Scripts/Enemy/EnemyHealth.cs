using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public event Action<EnemyHealth> Destroyed;

    private int _health;

    private void Start()
    {
        _health = MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerShot"))
        {
            PlayerShotBehavior playerShotBehavior = other.GetComponent<PlayerShotBehavior>();
            _health -= playerShotBehavior.Damage;
            if (_health <= 0)
            {
                // Invoke the destruction event
                Destroy(gameObject);
            }
        }
    }
}