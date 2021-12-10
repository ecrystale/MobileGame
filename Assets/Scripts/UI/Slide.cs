using UnityEngine;

public class Slide : UIComponent
{
    public Vector3 StartOffset;
    public float duration;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector3 _dest;
    private float _speed;

    protected override void Start()
    {
        base.Start();
        _startPos = transform.position + StartOffset;
        _endPos = transform.position;
        _speed = 1 / duration;
        _dest = _endPos;

        if (!page.Showed)
        {
            _dest = _startPos;
            transform.position = _startPos;
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _dest, _speed * Time.deltaTime);
    }

    public override void HandlePageShowed(PageManager page)
    {
        _dest = _endPos;
    }

    public override void HandlePageHid(PageManager page)
    {
        _dest = _startPos;
    }
}