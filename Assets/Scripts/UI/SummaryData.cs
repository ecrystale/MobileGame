public class SummaryData
{
    public bool Cleared;
    public int Score, Coin;
    public float CompletionTime, DestructionCount, TotalEnemies;
    public float DestructionRate => (TotalEnemies != 0) ? (DestructionCount / TotalEnemies) : 0;

    public SummaryData()
    {
        Cleared = false;
    }
}