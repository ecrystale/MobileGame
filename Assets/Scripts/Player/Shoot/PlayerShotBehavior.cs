using UnityEngine;

public class PlayerShotBehavior : MonoBehaviour
{
    public int angularVelocity;

    public int Damage => Game.CurrentGame.PlayerData.Damage;
    private float _shotSpeed => Game.CurrentGame.PlayerData.ShotSpeed;
    private bool _homing => Game.CurrentGame.PlayerData.AbilitiesEnabled[((int)Ability.Homing)];
    private int _totalBouncingCount => Game.CurrentGame.PlayerData.AbilitiesEnabled[((int)Ability.Bouncy)] ? Game.CurrentGame.PlayerData.UpgradesLevels[((int)Purchasable.Bouncy)] : 0;
    private WorldBound bound => Game.CurrentGame.WorldBound;

    private GameObject target;
    private int _remainingBouncingCount;
    private Vector2 _direction;

    public void Setup()
    {
        Setup(Vector2.up);
    }

    public void Setup(Vector2 direction)
    {
        _direction = direction;
        _remainingBouncingCount = _totalBouncingCount;
    }

    void Update()
    {
        if (_homing)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }

            target = player.GetComponent<PlayerMovement>().getClosestEnemy();

            if (target == null)
            {
                transform.Translate(_direction * _shotSpeed * Time.deltaTime);
                return;
            }

            Vector2 direction = (Vector2)target.transform.position - (Vector2)player.transform.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            transform.Rotate(new Vector3(0, 0, -angularVelocity * rotateAmount), Space.Self);
        }

        transform.Translate(_direction * _shotSpeed * Time.deltaTime);

        if (!bound.CheckIsWithinBound(transform.position))
        {
            if (_remainingBouncingCount > 0)
            {
                if (_remainingBouncingCount-- == _totalBouncingCount && _direction == Vector2.up)
                {
                    _direction = new Vector2(Random.Range(-1f, 1f), -0.1f).normalized;
                }
                else
                {
                    _direction = bound.BounceBack(transform.position, _direction);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
