using UnityEngine;

[RequireComponent(typeof(MenuManager))]
public class DummyMenuPage : PageManager
{
    MenuManager _menu;
    private void Start()
    {
        _menu = GetComponent<MenuManager>();
        _menu.MenuShowed += (menu) => this.InvokePageShowed();
        _menu.MenuHid += (menu) => this.InvokePageHid();
    }
}
