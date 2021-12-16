using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool _isMoving;
    private Vector2 initialPos;
    private float xOffset;
    private float yOffset;
    private float upperXBound;
    private float lowerXBound;
    private float lowerYBound;
    private float upperYBound;
    private Touch lastTouch;

    public GameObject closestEnemy;
    public float closestEnemyDistance = float.PositiveInfinity;

    void Start()
    {
        Input.multiTouchEnabled = false;
        upperXBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerXBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        lowerYBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
        upperYBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
    }

    private void ResetOffsetFromTouchToPlayer(Touch touch)
    {
        initialPos = Camera.main.ScreenToWorldPoint(touch.position);
        xOffset = initialPos.x - transform.position.x;
        yOffset = initialPos.y - transform.position.y;
    }

    void Update()
    {
        if (Input.touchCount == 1)
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

                Vector2 target = new Vector2(Mathf.Clamp(newX, lowerXBound, upperXBound), Mathf.Clamp(newY, lowerYBound, upperYBound));
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
