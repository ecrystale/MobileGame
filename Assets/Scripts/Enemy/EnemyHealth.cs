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

            if (Game.CurrentGame.PlayerData.AbilitiesEnabled[((int)Ability.Split)])
            {
                Vector2 spawnOffset = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
                GameObject shot = Game.CurrentGame.ObjectPooler.instantiateObjFromPool("PlayerShot", ((Vector2)transform.position) + spawnOffset, Quaternion.identity);
                shot.gameObject.transform.right = spawnOffset;

                if (shot != null)
                {
                    PlayerShotBehavior shotBehavior = shot.GetComponent<PlayerShotBehavior>();
                    shotBehavior.Setup(spawnOffset);
                }
            }

            other.gameObject.SetActive(false);
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
            if(player == null){
                return;
            }
            player.GetComponent<PlayerMovement>().setClosestEnemyDistance(float.PositiveInfinity);
        }
    }

}
