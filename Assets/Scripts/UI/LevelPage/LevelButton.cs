using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : ButtonBehaviour
{
    public bool ContinueMode;
    public Text Text;
    public int levelIDToLoad;

    public UIComponent[] Components;

    private void Awake()
    {
        if (Text == null) throw new ArgumentNullException($"Text is missing on {gameObject.name}");
        Button = GetComponent<Button>();
        Components = GetComponents<UIComponent>();
    }

    protected override void HandleClick()
    {
        if (ContinueMode)
        {
            Game.CurrentGame.Continue();
            return;
        }

        if (Game.CurrentGame.LevelProgress >= levelIDToLoad)
        {
            Debug.Log($"Loading level {levelIDToLoad}...");
            Game.CurrentGame.LoadLevel(levelIDToLoad);
        }
    }

    public void HandleProgress(int level)
    {
        Button.interactable = level >= levelIDToLoad;
    }
}
