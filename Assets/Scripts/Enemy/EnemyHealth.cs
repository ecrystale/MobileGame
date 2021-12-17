using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public event Action<EnemyHealth> Destroyed;
    public GameObject explode;

    private int _health;
    private bool destroyed = false;

    private void Start()
    {
        _health = MaxHealth;
    }

    // REFACTOR later, this is super redundant rn
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerShot"))
        {
            PlayerShotBehavior playerShotBehavior = other.GetComponent<PlayerShotBehavior>();
            _health -= playerShotBehavior.Damage;

            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            Bomb bomb = other.GetComponent<Bomb>();
            _health -= bomb.Damage;
        }

        if (_health <= 0)
        {
            // Invoke the destruction event
            if (Destroyed != null && !destroyed)
            {
                destroyed = true;
                if (Destroyed != null) Destroyed(this);
                Instantiate(explode, transform.position, transform.rotation);
            };
            Destroy(gameObject);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerMovement>().setClosestEnemyDistance(float.PositiveInfinity);
        }
    }

}
