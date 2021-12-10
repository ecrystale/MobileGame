using UnityEngine;

public class MenuController : MonoBehaviour
{
    public MenuManager menu;

    public void HandleOnClick()
    {
        menu.ToggleMenu();
    }
}