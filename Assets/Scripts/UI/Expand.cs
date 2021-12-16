using UnityEngine;

public class Expand : UIComponent
{
    private Vector3 StartScale = Vector3.zero;
    public float duration;

    private Vector3 _startScale;
    private Vector3 _endScale;
    private Vector3 _destScale;
    private float _speed;

    protected override void Start()
    {
        base.Start();
        _endScale = transform.localScale;
        _speed = 1 / duration;

        _destScale = _endScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _destScale, _speed * Time.unscaledDeltaTime);
    }

    public override void HandlePageShowed(PageManager page)
    {
        _destScale = _endScale;
    }

    public override void HandlePageHid(PageManager page)
    {
        _destScale = _startScale;
    }
}