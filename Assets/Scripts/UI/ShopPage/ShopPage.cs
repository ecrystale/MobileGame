using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : PageManager
{
    public GameObject ItemGroupPrefab;
    public GameObject ItemsContainer;
    public Text Coins;

    public void Setup()
    {
        foreach (Upgradable upgradable in Game.CurrentGame.Upgradables)
        {
            GameObject itemGroupGameObject = Instantiate(ItemGroupPrefab, ItemsContainer.transform);
            ShopItemGroup itemGroup = itemGroupGameObject.GetComponent<ShopItemGroup>();
            Coins.text = Game.CurrentGame.PlayerData.Coins.ToString();
            Game.CurrentGame.CoinsChanged += HandleCoinsChanged;
            itemGroup.Setup(upgradable);
        }
    }

    private void HandleCoinsChanged(int coins)
    {
        Coins.text = coins.ToString();
    }
}
