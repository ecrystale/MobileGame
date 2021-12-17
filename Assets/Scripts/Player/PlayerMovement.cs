using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool _isMoving;
    private Vector2 initialPos;
    private float xOffset;
    private float yOffset;
    private Touch lastTouch;

    public GameObject closestEnemy;
    public float closestEnemyDistance = float.PositiveInfinity;

    void Start()
    {
        Input.multiTouchEnabled = false;
    }

    private void ResetOffsetFromTouchToPlayer(Touch touch)
    {
        initialPos = Camera.main.ScreenToWorldPoint(touch.position);
        xOffset = initialPos.x - transform.position.x;
        yOffset = initialPos.y - transform.position.y;
    }

    void Update()
    {
        if (Input.touchCount == 1 && !Game.CurrentGame.Paused)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                ResetOffsetFromTouchToPlayer(touch);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                float newX = Camera.main.ScreenToWorldPoint(touch.position).x - xOffset;
                float newY = Camera.main.ScreenToWorldPoint(touch.position).y - yOffset;

                Vector2 target = Game.CurrentGame.WorldBound.ClampBound(new Vector2(newX, newY));
                float distance = Vector2.Distance(transform.position, target);

                if (distance > PublicVars.FRAME_MOVEMENT_SPEED_CAP)
                {
                    ResetOffsetFromTouchToPlayer(touch);
                    return;
                }

                transform.position = target;
            }
        }

        // print(closestEnemyDistance);
    }

    public void setClosestEnemy(GameObject enemy)
    {
        closestEnemy = enemy;
    }

    public GameObject getClosestEnemy()
    {
        return closestEnemy;
    }

    public float getClosestEnemyDistance()
    {
        return closestEnemyDistance;
    }

    public void setClosestEnemyDistance(float dist)
    {
        closestEnemyDistance = dist;
    }

}
