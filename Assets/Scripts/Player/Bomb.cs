using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float maxRadius;
    public float expandRate;
    public float acceleration;

    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            print("hello");
            _spriteRenderer.enabled = true;
            while(_collider.radius < maxRadius){
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x * expandRate, transform.localScale.y * expandRate, 0f), expandRate * Time.deltaTime);
                expandRate += acceleration;
            }
        }
    }
}
