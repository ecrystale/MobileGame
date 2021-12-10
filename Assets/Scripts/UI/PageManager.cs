using System;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public MenuManager menu;
    public bool Showed;
    public GameObject[] PassiveObjects;

    public event Action<PageManager> PageShowed;
    public event Action<PageManager> PageHid;

    private void Start()
    {
        if (Showed) ShowPage();
        else HidePage();
    }

    public void HandleMenuToggle()
    {

    }

    public void ShowPage()
    {
        Showed = true;
        foreach (GameObject gameObject in PassiveObjects)
        {
            gameObject.SetActive(true);
        }
        if (PageShowed != null) PageShowed(this);
    }

    public void HidePage()
    {
        Showed = false;
        foreach (GameObject gameObject in PassiveObjects)
        {
            gameObject.SetActive(false);
        }
        if (PageShowed != null) PageHid(this);
    }

    public void TogglePage()
    {
        if (Showed) HidePage();
        else ShowPage();
    }
}