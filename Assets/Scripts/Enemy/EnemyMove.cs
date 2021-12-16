using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    public Vector3 endpos = new Vector3(0, 0, 0);
    public float speed;

    private GameObject _player;
    private PlayerMovement _playerMovement;
    private float distanceFromPlayer;

    void Start()
    {
        _player = Game.CurrentGame.PlayerHitbox.Player;
        _playerMovement = _player.GetComponent<PlayerMovement>();
        speed = 3f;
    }

    void Update()
    {
        if (_player == null) return;

        transform.position = Vector3.Lerp(transform.position, endpos, speed * Time.deltaTime);
        distanceFromPlayer = Vector2.Distance(_player.transform.position, transform.position);

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
        endpos = exitpos;
    }
}
