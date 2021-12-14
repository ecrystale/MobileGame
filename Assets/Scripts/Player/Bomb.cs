using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //  Can easily add damage instead of destroying other obj

    public float time;
    public float timeToGrow;
    public float maxRadius;

    public bool canGrow = true;

    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Vector2 origScale;

    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        origScale = transform.localScale;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            if(canGrow)
            {
                _spriteRenderer.enabled = true;
                StartCoroutine(activate());
            }
        }
    }

    private IEnumerator activate()
    {
        Vector2 startScale = transform.localScale;
        Vector2 maxScale = new Vector2(transform.localScale.x * maxRadius, transform.localScale.y * maxRadius);

        while(time < timeToGrow)
        {
            transform.localScale = Vector3.Lerp(startScale, maxScale, time / timeToGrow);
            time += Time.deltaTime;
            yield return null;
        }

        canGrow = false;

        StartCoroutine(fadeout());
    }

    private IEnumerator fadeout()
    {
        for(float i = 1f; i >= -0.05f; i -= 0.05f)
        {
            Color c = _spriteRenderer.material.color;
            c.a = i;
            _spriteRenderer.material.color = c;

            yield return new WaitForSeconds(0.05f);
        }

        _spriteRenderer.enabled = false;
        transform.localScale = origScale;
        time = 0;
        canGrow = true;
    }
}

