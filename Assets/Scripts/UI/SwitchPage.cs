public class SwitchPage : ButtonBehaviour
{
    public MenuManager Menu;
    public PageManager TargetPage;
    public bool Back;

    protected override void HandleClick()
    {
        if (Back) Menu.Back();
        else Menu.PushPage(TargetPage);
    }
}