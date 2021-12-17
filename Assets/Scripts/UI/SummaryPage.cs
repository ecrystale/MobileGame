using System;
using UnityEngine;

public class SummaryPage : PageManager
{
    public SummaryText Score, Destruction, CompletionTime, Coin;

    public void Setup(SummaryData data)
    {
        Score.UpdateValue(data.Score.ToString());
        Destruction.UpdateValue(data.DestructionRate.ToString());
        CompletionTime.UpdateValue(String.Concat(data.CompletionTime, "s"));
        Coin.UpdateValue(data.Coin.ToString());
    }
}
