using System;
using UnityEngine.UI;

public class SummaryPage : PageManager
{
    public SummaryText Score, Destruction, CompletionTime, Coin;
    public Text Title;

    public void Setup(SummaryData data)
    {
        Title.text = data.Cleared ? "Level Cleared" : "Game Over";
        Coin.UpdateValue(data.Coin.ToString());
        Score.UpdateValue(data.Score.ToString());
        Destruction.UpdateValue(String.Format("{0:P2}", data.DestructionRate));
        CompletionTime.UpdateValue(String.Format("{0:F2}s", data.CompletionTime));
    }
}
