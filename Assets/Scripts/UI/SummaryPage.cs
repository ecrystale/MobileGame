using System;
using UnityEngine;

public class SummaryPage : PageManager
{
    public SummaryText Score, Destruction, CompletionTime, Coin;

    public void Setup(SummaryData data)
    {
        Coin.UpdateValue(data.Coin.ToString());
        Score.UpdateValue(data.Score.ToString());
        Destruction.UpdateValue(String.Format("{0:P2}", data.DestructionRate));
        CompletionTime.UpdateValue(String.Format("{0:F2}s", data.CompletionTime));
    }
}
