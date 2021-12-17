using UnityEngine;
using System.Collections;

public class ClosestPosition : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement _playerMovement;
    private float distanceFromPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        print("calculating");
        distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);

        if(distanceFromPlayer < _playerMovement.getClosestEnemyDistance())
        {
            print("updating");
            _playerMovement.setClosestEnemy(gameObject);
            _playerMovement.setClosestEnemyDistance(distanceFromPlayer);
        }

        if(_playerMovement.getClosestEnemy() == gameObject && distanceFromPlayer > _playerMovement.getClosestEnemyDistance())
        {
            _playerMovement.setClosestEnemyDistance(distanceFromPlayer);
        }

    }
}
