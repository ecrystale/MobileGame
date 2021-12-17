public class Upgradable
{
    public int BasePrice;
    public int PriceIncrement;
    public float PriceMultiplier;
    public float BasePower;
    public float PowerIncrement;
    public float PowerMultiplier;
    public int Level;
    public int MaxLevel;

    public int CurrentPrice { get; private set; }
    public float CurrentPower { get; private set; }
    public int CurrentPowerInt => ((int)CurrentPower);

    public Upgradable(float basePower, float powerIncrement, float powerMultiplier, int basePrice, int priceIncrement, float priceMultiplier = 1, int maxLevel = 1, int level = 0)
    {
        BasePower = CurrentPower = basePower;
        PowerIncrement = powerIncrement;
        PowerMultiplier = powerMultiplier;

        BasePrice = CurrentPrice = basePrice;
        PriceIncrement = priceIncrement;
        PriceMultiplier = priceMultiplier;

        MaxLevel = maxLevel;
        Level = level;

        for (; level > 0; level--)
        {
            CurrentPrice = ((int)(CurrentPrice * PriceMultiplier)) + PriceIncrement;
            CurrentPower = ((int)(CurrentPower * PowerMultiplier)) + PowerIncrement;
        }
    }

    public void Upgrade()
    {
        CurrentPrice = ((int)(CurrentPrice * PriceMultiplier)) + PriceIncrement;
        CurrentPower = ((int)(CurrentPower * PowerMultiplier)) + PowerIncrement;
    }
}