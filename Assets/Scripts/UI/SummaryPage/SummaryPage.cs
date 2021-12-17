using System;
using UnityEngine.UI;

public class SummaryPage : PageManager
{
    public SummaryText Score, Destruction, CompletionTime, Coin;
    public Text Title;
    public LevelButton RestartButton;

    public void Setup(SummaryData data)
    {
        Title.text = data.Cleared ? $"[{data.ID}] {data.LevelName} Cleared" : "Game Over";
        RestartButton.Text.text = data.Cleared ? "Continue" : "Restart";
        RestartButton.levelIDToLoad = data.Cleared ? data.ID + 1 : data.ID;
        RestartButton.gameObject.SetActive(Game.CurrentGame.IsLevelValid(RestartButton.levelIDToLoad));

        Coin.UpdateValue(data.Coin.ToString());
        Score.UpdateValue(data.Score.ToString());
        Destruction.UpdateValue(String.Format("{0:P2}", data.DestructionRate));
        CompletionTime.UpdateValue(String.Format("{0:F2}s", data.CompletionTime));
    }
}
