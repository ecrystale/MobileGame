using UnityEngine;

public class WorldBound
{
    private Rect _rectBound;

    public WorldBound()
    {
        float upperXBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        float lowerXBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        float lowerYBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
        float upperYBound = -Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).y;
        _rectBound = new Rect(lowerXBound, lowerYBound, upperXBound - lowerXBound, upperYBound - lowerYBound);
    }

    public bool CheckIsWithinBound(Vector3 position)
    {
        return _rectBound.Contains(position);
    }

    public Vector2 BounceBack(Vector2 position, Vector2 velocity)
    {
        int componentX = position.x > _rectBound.xMax || position.x < _rectBound.xMin ? -1 : 1;
        int componentY = position.y > _rectBound.yMax || position.y < _rectBound.yMin ? -1 : 1;
        return velocity * new Vector2(componentX, componentY);
    }

    public Vector2 ClampBound(Vector3 position)
    {
        return new Vector2(Mathf.Clamp(position.x, _rectBound.xMin, _rectBound.xMax), Mathf.Clamp(position.y, _rectBound.yMin, _rectBound.yMax));
    }
}