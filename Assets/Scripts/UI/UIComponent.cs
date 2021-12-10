using UnityEngine;

public abstract class UIComponent : MonoBehaviour
{
    public PageManager page;

    protected virtual void Start()
    {
        page.PageShowed += HandlePageShowed;
        page.PageHid += HandlePageHid;
    }

    public abstract void HandlePageShowed(PageManager page);
    public abstract void HandlePageHid(PageManager page);
}