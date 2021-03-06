using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public event Action<EnemyHealth> Destroyed;
    public GameObject explode;
    public Slider HealthBar;

    private int _health;
    private bool destroyed = false;

    public void SetHealth(int health)
    {
        _health = MaxHealth = health;
    }

    private void Start()
    {
        _health = MaxHealth;
    }

    private void Update()
    {
        if (HealthBar == null) return;

        HealthBar.transform.position = (Vector2)(transform.position) + Vector2.up * 0.5f;
        HealthBar.transform.rotation = Quaternion.identity;
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

        if (HealthBar != null) HealthBar.value = _health / (float)MaxHealth;

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
            if (player == null)
            {
                return;
            }
            player.GetComponent<PlayerMovement>().setClosestEnemyDistance(float.PositiveInfinity);
        }
    }

}
