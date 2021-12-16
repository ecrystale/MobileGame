using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int Damage = 100;
    public float maxRadius;
    public float timeToGrow;

    public bool canGrow = true;

    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Vector2 origScale;
    private Color origColor;

    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        origScale = transform.localScale;
        origColor = _spriteRenderer.material.color;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            if(canGrow)
            {
                StartCoroutine(activate());
            }
        }
    }

    private IEnumerator activate()
    {
        canGrow = false;
        _spriteRenderer.enabled = true;
        _collider.enabled = true;

        Vector2 startScale = transform.localScale;
        Vector2 maxScale = new Vector2(transform.localScale.x * maxRadius, transform.localScale.y * maxRadius);

        float time = 0f;

        while (time <= timeToGrow)
        {
            transform.localScale = Vector3.Lerp(startScale, maxScale, time / timeToGrow);
            time += Time.deltaTime;
            yield return null;
        }
        
        // cooldown
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(fadeout());
    }

    private IEnumerator fadeout()
    {
        for(float i = 1f; i >= -0.1f; i -= 0.1f)
        {
            Color c = _spriteRenderer.material.color;
            c.a = i;
            _spriteRenderer.material.color = c;

            yield return new WaitForSeconds(0.01f);
        }

        _spriteRenderer.material.color = origColor;
        _spriteRenderer.enabled = false;
        _collider.enabled = true;;
        transform.localScale = origScale;
        canGrow = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyShot")
        {
            other.gameObject.SetActive(false);
        }
    }
}