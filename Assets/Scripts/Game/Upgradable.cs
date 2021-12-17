using UnityEngine;

public class Upgradable
{
    public Purchasable Purchasable;
    public string Name;
    public int BasePrice;
    public int PriceIncrement;
    public float PriceMultiplier;
    public float BasePower;
    public float PowerIncrement;
    public float PowerMultiplier;
    public int Level;
    public int MaxLevel;
    public bool IsAbility;

    public int CurrentPrice { get; private set; }
    public float CurrentPower { get; private set; }
    public int CurrentPowerInt => ((int)CurrentPower);
    public bool CanUpgrade => Level < MaxLevel && Game.CurrentGame.PlayerData.Coins > CurrentPrice;

    public Upgradable(Purchasable purchasable, string name, float basePower, float powerIncrement, float powerMultiplier, int basePrice, int priceIncrement, float priceMultiplier = 1, int maxLevel = 1, int level = 0, bool isAbility = false)
    {
        Purchasable = purchasable;
        Name = name;
        BasePower = CurrentPower = basePower;
        PowerIncrement = powerIncrement;
        PowerMultiplier = powerMultiplier;

        BasePrice = CurrentPrice = basePrice;
        PriceIncrement = priceIncrement;
        PriceMultiplier = priceMultiplier;

        MaxLevel = maxLevel;
        Level = level;

        IsAbility = isAbility;

        for (; level > 0; level--)
        {
            CurrentPrice = ((int)(CurrentPrice * PriceMultiplier)) + PriceIncrement;
            CurrentPower = ((int)(CurrentPower * PowerMultiplier)) + PowerIncrement;
        }
    }

    public void Upgrade()
    {
        if (Level > MaxLevel) return;
        Level++;
        CurrentPrice = ((int)(CurrentPrice * PriceMultiplier)) + PriceIncrement;
        CurrentPower = (CurrentPower * PowerMultiplier) + PowerIncrement;
    }
}