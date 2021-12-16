using UnityEngine;
using System.Collections;

public class ClosestPosition : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement _playerMovement;
    private float distanceFromPlayer;
    public Material a;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);

        if(distanceFromPlayer < _playerMovement.getClosestEnemyDistance()){
            _playerMovement.setClosestEnemy(gameObject);
            _playerMovement.setClosestEnemyDistance(distanceFromPlayer);
        }

        if(_playerMovement.getClosestEnemy() == gameObject){
            gameObject.GetComponent<SpriteRenderer>().material = a;
        }

    }
}
