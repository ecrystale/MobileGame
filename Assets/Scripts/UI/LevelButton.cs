using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButton : TimeoutBehaviour
{
    public Button Button { get; private set; }
    public Text Text;
    public int levelIDToLoad;

    public UIComponent[] Components;

    private void Awake()
    {
        if (Text == null) throw new ArgumentNullException($"Text is missing on {gameObject.name}");
        Button = GetComponent<Button>();
        Components = GetComponents<UIComponent>();
        Button.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        if (CheckAndReset(PublicVars.DEBOUNCE_INTERVAL))
        {
            Debug.Log(levelIDToLoad);
            Game.CurrentGame.LoadLevel(levelIDToLoad);
        }
    }
}
