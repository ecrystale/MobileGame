using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    private GameObject player;
    public float fallSpeed = 2;
    public float speed = 15;
    public float time;
    public bool magnetized = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!magnetized)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Magnet") && Game.CurrentGame.PlayerData.AbilitiesEnabled[((int)Ability.Magnet)])
        {
            Debug.Log("magnetized");
            magnetized = true;
        }
    }
}
