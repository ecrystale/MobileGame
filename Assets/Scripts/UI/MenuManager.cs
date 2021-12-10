using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool Showed;
    public bool CanHide = true;
    public Stack<PageManager> PrevPages;
    public PageManager CurrentPage;
    public GameObject Background;

    public event Action<MenuManager> MenuShowed;
    public event Action<MenuManager> MenuHid;

    private void Awake()
    {
        if (CurrentPage != null)
        {
            CurrentPage.Showed = true;
        }
    }

    private void Start()
    {
        PrevPages = new Stack<PageManager>();
        StartCoroutine(DelayedShowMenu());
    }

    IEnumerator<WaitForEndOfFrame> DelayedShowMenu()
    {
        yield return new WaitForEndOfFrame();
        ShowMenu();
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
            if (Input.GetKeyDown(KeyCode.Escape))
                Back();
    }

    public void ShowMenu()
    {
        Showed = true;
        CurrentPage.ShowPage();
        if (MenuShowed != null) MenuShowed(this);
    }

    public void HideMenu()
    {
        if (!CanHide) return;

        Showed = false;
        CurrentPage.HidePage();
        if (MenuShowed != null) MenuHid(this);
    }

    public void ToggleMenu()
    {
        if (Showed) HideMenu();
        else ShowMenu();
    }

    /// <summary>Push a page onto the page stack</summary>
    public void PushPage(PageManager page)
    {
        if (CurrentPage == page) return;
        if (CurrentPage != null)
        {
            PrevPages.Push(CurrentPage);
            CurrentPage.HidePage();
        }

        CurrentPage = page;
        if (Showed) CurrentPage.ShowPage();
    }

    public void Back()
    {
        if (PrevPages.Count == 0)
        {
            HideMenu();
            return;
        }

        if (CurrentPage != null)
        {
            CurrentPage.HidePage();
        }

        CurrentPage = PrevPages.Pop();
        if (Showed) CurrentPage.ShowPage();
    }
}
