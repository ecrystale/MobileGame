public class SummaryData
{
    public int Score, Coin;
    public float CompletionTime, DestructionCount, TotalEnemies;
    public float DestructionRate => (TotalEnemies != 0) ? (DestructionCount / TotalEnemies) : 1;
}