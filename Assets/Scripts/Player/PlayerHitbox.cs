using System;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public GameObject Player;
    public event Action<GameObject> PlayerDied;
    public event Action<GameObject> PlayerCollectedCoin;
    public bool Dead = false;

    void Start()
    {
        Player = transform.parent.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyShot") && !Dead)
        {
            Dead = true;
            if (PlayerDied != null) PlayerDied(Player);
            Player.SetActive(false);
        }

        // Just destroy for now
        if (other.gameObject.CompareTag("Coin"))
        {
            if (PlayerCollectedCoin != null) PlayerCollectedCoin(Player);
            Destroy(other);
        }
    }
}
