using System;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public GameObject Player;
    public GameObject Explode;
    public event Action<GameObject> PlayerDied;
    public event Action<GameObject> PlayerCollectedCoin;
    public bool Dead = false;
    public AudioSource AudioSource;
    public AudioClip GoldClip;

    void Start()
    {
        Player = transform.parent.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyShot") && !Dead)
        {
            Dead = true;
            Instantiate(Explode, transform.position, transform.rotation);
            if (PlayerDied != null) PlayerDied(Player);
            Player.SetActive(false);
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            if (PlayerCollectedCoin != null) PlayerCollectedCoin(Player);
            AudioSource.PlayOneShot(GoldClip);
            Destroy(other.gameObject);
        }
    }
}
