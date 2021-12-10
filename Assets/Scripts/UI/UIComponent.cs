using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIComponent : MonoBehaviour
{
    public float EnterDelay;
    public float ExitDelay;
    public PageManager page;

    protected virtual void Start()
    {
        Register();
    }

    public void Register()
    {
        if (page == null) return;
        page.PageShowed += (PageManager page) => DelayedExecute(EnterDelay, () => HandlePageShowed(page));
        page.PageHid += (PageManager page) => DelayedExecute(ExitDelay, () => HandlePageHid(page));
    }

    protected void DelayedExecute(float delay, Action callback)
    {
        if (delay == 0) callback();
        else StartCoroutine(HandleDelayExeuction(delay, callback));
    }

    private IEnumerator<WaitForSeconds> HandleDelayExeuction(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    public abstract void HandlePageShowed(PageManager page);
    public abstract void HandlePageHid(PageManager page);
}