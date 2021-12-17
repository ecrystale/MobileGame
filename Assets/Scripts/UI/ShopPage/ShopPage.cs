using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : PageManager
{
    public GameObject ItemGroupPrefab;
    public GameObject ItemsContainer;

    public void Setup()
    {
        foreach (Upgradable upgradable in Game.CurrentGame.Upgradables)
        {
            GameObject itemGroupGameObject = Instantiate(ItemGroupPrefab, ItemsContainer.transform);
            ShopItemGroup itemGroup = itemGroupGameObject.GetComponent<ShopItemGroup>();
            itemGroup.Setup(upgradable);
        }
    }
}
