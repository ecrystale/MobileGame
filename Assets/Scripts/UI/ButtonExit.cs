using UnityEngine;

public class ButtonExit : ButtonBehaviour
{
    protected override void HandleClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}