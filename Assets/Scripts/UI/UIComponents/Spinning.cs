using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float AngularSpeed = 120f;

    private void Update()
    {
        gameObject.transform.Rotate(0, 0, AngularSpeed * Time.unscaledDeltaTime);
    }
}