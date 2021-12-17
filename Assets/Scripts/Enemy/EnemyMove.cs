using UnityEngine;
using System;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    public Vector3 endpos = new Vector3(0, 0, 0);
    public float speed;
    public event Action<EnemyMove> Gone;

    private GameObject _player;
    private PlayerMovement _playerMovement;
    private float distanceFromPlayer;
    private bool _isExiting;

    void Start()
    {
        _isExiting = false;

        _player = Game.CurrentGame.PlayerHitbox.Player;
        speed = 3f;
    }

    void Update()
    {
        if (_player == null) return;

        transform.position = Vector3.Lerp(transform.position, endpos, speed * Time.deltaTime);
        distanceFromPlayer = Vector2.Distance(_player.transform.position, transform.position);

        if (_isExiting && !Game.CurrentGame.WorldBound.CheckIsWithinBound(transform.position))
        {
            Debug.Log("enemy no longer active");
            gameObject.SetActive(false);
            if (Gone != null) Gone(this);
        }

        if (distanceFromPlayer < _playerMovement.getClosestEnemyDistance())
        {
            _playerMovement.setClosestEnemy(gameObject);
            _playerMovement.setClosestEnemyDistance(distanceFromPlayer);
        }

    }

    public void Spawner(Vector3 pos, Vector3 exitpos, float duration)
    {
        if (duration == -1) return;
        endpos = pos;
        float timeToExit = Vector3.Distance(exitpos, endpos) / speed;
        //Destroy(this.gameObject, duration);
        StartCoroutine(DelayExit(exitpos, duration - timeToExit));
    }

    IEnumerator DelayExit(Vector3 exitpos, float delay)
    {
        yield return new WaitForSeconds(delay);
        _isExiting = true;
        endpos = exitpos;
    }
}
