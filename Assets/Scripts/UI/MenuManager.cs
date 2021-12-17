using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool Showed;
    public bool CanHide = true;
    public bool Locked = false;
    public bool Paused { get; private set; }
    public Stack<PageManager> PrevPages;
    public PageManager CurrentPage;
    public GameObject Background;
    public PageManager DeathScreen;
    public PageManager WinScreen;
    public SummaryPage SummaryPage;
    public LevelPage LevelPage;
    public ShopPage ShopPage;
    public MenuController MenuController;

    public AudioClip swoosh;
    AudioSource _audiosrc;

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
        _audiosrc = GetComponent<AudioSource>();
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

    public void SetCanHide(bool canHide)
    {
        if (CanHide == canHide) return;
        CanHide = canHide;
        MenuController.gameObject.SetActive(CanHide);
    }

    public void ShowMenu()
    {
        Showed = true;
        CurrentPage.ShowPage();
        Time.timeScale = 0.01f;
        Paused = true;
        if (MenuShowed != null) MenuShowed(this);
    }

    public void HideMenu()
    {
        if (!CanHide) return;

        Showed = false;
        CurrentPage.HidePage();
        Time.timeScale = 1;
        Paused = false;
        if (MenuShowed != null) MenuHid(this);
    }

    public void ToggleMenu()
    {
        if (Showed) HideMenu();
        else ShowMenu();
    }

    public void LockDisplay(PageManager page, float duration)
    {
        if (Locked) throw new InvalidOperationException("Cannot invoke LockDisplay on top of a locked menu");
        if (!Showed) ShowMenu();
        PushPage(page);
        Locked = true;
        StartCoroutine(UnlockDisplay(duration));
    }

    IEnumerator<WaitForSecondsRealtime> UnlockDisplay(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if (Locked)
        {
            Locked = false;
            Back();
        }
    }

    /// <summary>Push a page onto the page stack</summary>
    public void PushPage(PageManager page)
    {
        if (Locked || CurrentPage == page) return;
        if (CurrentPage != null)
        {
            PrevPages.Push(CurrentPage);
            CurrentPage.HidePage();
        }

        CurrentPage = page;
        _audiosrc.PlayOneShot(swoosh);
        if (Showed) CurrentPage.ShowPage();
    }

    public void Back()
    {
        if (Locked) return;

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
        _audiosrc.PlayOneShot(swoosh);
        if (Showed) CurrentPage.ShowPage();
    }
}
