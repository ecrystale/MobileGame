using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class LevelGroup : MonoBehaviour
{
    public List<LevelButton> LevelButtons;

    /// <summary>Set up level buttons within an inclusive range</summary>
    public void Setup(int startLevel, int endLevel, GameObject buttonPrefab, PageManager page)
    {
        LevelButtons?.ForEach(button => Destroy(button));
        LevelButtons = new List<LevelButton>();
        for (int i = startLevel; i <= endLevel; i++)
        {
            LevelButton button = Instantiate(buttonPrefab, transform).GetComponent<LevelButton>();
            LevelButtons.Add(button);
            button.levelIDToLoad = i;
            button.Text.text = $"{i + 1}";
            button.Button.interactable = i <= Game.CurrentGame.LevelProgress;
            // Subcribe the button to ProgressMade in order to update its state
            Game.CurrentGame.ProgressMade += button.HandleProgress;
            foreach (UIComponent component in button.Components)
            {
                component.page = page;
                component.Register();
            }
        }
    }
}
