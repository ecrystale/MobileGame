using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchPage : TimeoutBehaviour
{
    public MenuManager Menu;
    public PageManager TargetPage;
    public bool Back;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnSwitch);
    }

    public void OnSwitch()
    {
        if (CheckAndReset(PublicVars.DEBOUNCE_INTERVAL))
        {
            if (Back) Menu.Back();
            else Menu.PushPage(TargetPage);
        }
    }
}