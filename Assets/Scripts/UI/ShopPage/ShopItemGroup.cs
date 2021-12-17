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
        ToggleButton.gameObject.SetActive(_upgradable.IsAbility);
        Game.CurrentGame.CoinsChanged += HandleCoinsChanged;
        PurchaseButton.Purchased += HandlePurchase;
        PurchaseButton.Purchased += Game.CurrentGame.HandlePurchase;
        Refresh();
    }

    private void Refresh()
    {
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
