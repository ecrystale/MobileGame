public class SwitchScene : ButtonBehaviour
{
    public MenuManager Menu;
    public string TargetScene;

    protected override void HandleClick()
    {
        PublicVars.TransitionManager.FadeToScene(TargetScene, 0.5f);
    }
}