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
        page.PageShowed += (PageManager page) => DelayedTask.Wrapper(() => HandlePageShowed(page), EnterDelay, true);
        page.PageHid += (PageManager page) => DelayedTask.Wrapper(() => HandlePageHid(page), ExitDelay, true);
    }

    public abstract void HandlePageShowed(PageManager page);
    public abstract void HandlePageHid(PageManager page);
}