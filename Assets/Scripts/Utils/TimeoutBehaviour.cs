using UnityEngine;

public class TimeoutBehaviour : MonoBehaviour
{
    private float _timeout = 0f;

    protected virtual void Update()
    {
        _timeout -= Time.deltaTime;
        if (_timeout <= 0) _timeout = 0;
    }

    protected bool CheckAndReset(float interval)
    {
        if (_timeout <= 0)
        {
            _timeout = interval;
            return true;
        }
        return false;
    }
}