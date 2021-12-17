using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemGroup : MonoBehaviour
{
    public Purchase PurchaseButton;
    public ToggleAbility ToggleButton;
    public Text ItemName;
    public Text PriceTag;
    public Text ItemLevel;

    private Upgradable _upgradable;

    public void Setup(Upgradable upgradable)
    {
        _upgradable = upgradable;

        if (upgradable.IsAbility)
        {
            ToggleButton.Ability = (Ability)(upgradable.Purchasable - ((int)Ability.Split));
            ToggleButton.Enabled = Game.CurrentGame.PlayerData.AbilitiesEnabled[((int)_upgradable.Purchasable - (int)Ability.Split)];
            ToggleButton.UpdateText();
        }

        PurchaseButton.purchasable = upgradable.Purchasable;
        Game.CurrentGame.CoinsChanged += HandleCoinsChanged;
        PurchaseButton.Purchased += Game.CurrentGame.HandlePurchase;
        PurchaseButton.Purchased += HandlePurchase;
        Refresh();
    }

    private void Refresh()
    {
        ToggleButton.gameObject.SetActive(_upgradable.IsAbility && _upgradable.Level > 0);
        ItemName.text = _upgradable.Name;
        PriceTag.text = _upgradable.CurrentPrice.ToString();
        ItemLevel.text = $"{_upgradable.Level}/{_upgradable.MaxLevel}";
        PurchaseButton.Button.interactable = _upgradable.CanUpgrade;
    }

    private void HandleCoinsChanged(int coins)
    {
        PurchaseButton.Button.interactable = _upgradable.CanUpgrade;
    }

    private void HandlePurchase(Purchasable purchasable)
    {
        Refresh();
    }
}
