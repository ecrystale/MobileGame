using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchScene : TimeoutBehaviour
{
    public MenuManager Menu;
    public string TargetScene;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnSwitch);
    }

    public void OnSwitch()
    {
        if (CheckAndReset(PublicVars.DEBOUNCE_INTERVAL))
        {
            PublicVars.TransitionManager.FadeToScene(TargetScene, 0.5f);
        }
    }
}