using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemConfig", fileName = "Intem Config")]
public class ItemConfig : ScriptableObject
{
    #region ConfigClass
    [Serializable]
    public class ManaFlaskConfig
    {
        public ManaFlask mana;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
    }
    [Serializable]
    public class HealFlaskConfig
    {
        public HealFlask heal;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
    }
    [Serializable]
    public class FoodConfig
    {
        public Food food;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
    }
    [Serializable]
    public class GunConfig
    {
        public Gun gun;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
    }
    [Serializable]
    public class BooKConfig
    {
        public Book book;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
    }
    [Serializable]
    public class SwordConfig
    {
        public Sword sword;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
    }
    #endregion ConfigClass

    #region List
    public List<ManaFlaskConfig> manaRcoveryConfigList;
    public List<HealFlaskConfig> healFlaskConfigList;
    public List<FoodConfig> foodConfigList;
    public List<GunConfig> gunConfigList;
    public List<BooKConfig> bookConfigList;
    public List<SwordConfig> swordConfigList;
    #endregion

    #region GetConfig

    #region Mana
    public ManaFlask GetManaFlaskConfig(string name)
    {
        for (int i = 0; i < manaRcoveryConfigList.Count; i++)
            if (name == manaRcoveryConfigList[i].name)
                return manaRcoveryConfigList[i].mana;
        return null;
    }

    public Item GetManaFlaskItemConfig(string name)
    {
        for (int i = 0; i < manaRcoveryConfigList.Count; i++)
            if (name == manaRcoveryConfigList[i].name)
            {
                Item item = new Item();
                item.amount = manaRcoveryConfigList[i].amount;
                item.maxAmount = manaRcoveryConfigList[i].maxAmount;
                item.itemValue=manaRcoveryConfigList[i].itemValue;
                item.itemData = JsonUtility.ToJson(manaRcoveryConfigList[i].mana);
                item.itemName = name;
                item.slotId = -1;
                item.itemType=ItemType.ManaFlask;
                return item;
            }
        return null;
    }
    #endregion

    #region HP

    public HealFlask GetHPFlaskConfig(string name)
    {
        for (int i = 0; i < healFlaskConfigList.Count; i++)
            if (name == healFlaskConfigList[i].name)
                return healFlaskConfigList[i].heal;
        return null;
    }

    public Item GetHPFlaskItemConfig(string name)
    {
        for (int i = 0; i < healFlaskConfigList.Count; i++)
            if (name == healFlaskConfigList[i].name)
            {
                Item item = new Item();
                item.amount = healFlaskConfigList[i].amount;
                item.maxAmount = healFlaskConfigList[i].maxAmount;
                item.itemValue=healFlaskConfigList[i].itemValue;
                item.itemData = JsonUtility.ToJson(healFlaskConfigList[i].heal);
                item.itemName = name;
                item.slotId = -1;
                item.itemType=ItemType.LifeFlask;
                return item;
            }
        return null;
    }

    #endregion

    #region Food

    public Food GetFoodConfig(string name)
    {
        for (int i = 0; i < foodConfigList.Count; i++)
            if (name == foodConfigList[i].name)
                return foodConfigList[i].food;
        return null;
    }

    public Item GetFoodItemConfig(string name)
    {
        for (int i = 0; i < foodConfigList.Count; i++)
            if (name == foodConfigList[i].name)
            {
                Item item = new Item();
                item.amount = foodConfigList[i].amount;
                item.maxAmount = foodConfigList[i].maxAmount;
                item.itemValue=foodConfigList[i].itemValue;
                item.itemData = JsonUtility.ToJson(foodConfigList[i].food);
                item.itemName = name;
                item.slotId = -1;
                item.itemType=ItemType.Food;
                return item;
            }
        return null;
    }

    #endregion

    #region Gun

    public Gun GetGunConfig(string name)
    {
        for (int i = 0; i < gunConfigList.Count; i++)
            if (name == gunConfigList[i].name)
                return gunConfigList[i].gun;
        return null;
    }

    public Item GetGunItemConfig(string name)
    {
        for (int i = 0; i < gunConfigList.Count; i++)
            if (name == gunConfigList[i].name)
            {
                Item item = new Item();
                item.amount = gunConfigList[i].amount;
                item.maxAmount = gunConfigList[i].maxAmount;
                item.itemValue=gunConfigList[i].itemValue;
                item.itemData = JsonUtility.ToJson(gunConfigList[i].gun);
                item.itemName = name;
                item.slotId = -1;
                item.itemType=ItemType.Gun;
                return item;
            }
        return null;
    }

    #endregion

    #region Book

    public Book GetBookConfig(string name)
    {
        for (int i = 0; i < bookConfigList.Count; i++)
            if (name == bookConfigList[i].name)
                return bookConfigList[i].book;
        return null;
    }
    public Item GetBookItemConfig(string name)
    {
        for (int i = 0; i < bookConfigList.Count; i++)
            if (name == bookConfigList[i].name)
            {
                Item item = new Item();
                item.amount = bookConfigList[i].amount;
                item.maxAmount = bookConfigList[i].maxAmount;
                item.itemValue=bookConfigList[i].itemValue;
                item.itemData = JsonUtility.ToJson(bookConfigList[i].book);
                item.itemName = name;
                item.slotId = -1;
                item.itemType=ItemType.Book;
                return item;
            }
        return null;
    }

    #endregion

    #region Sword

    public Sword GetSwordConfig(string name)
    {
        for (int i = 0; i < swordConfigList.Count; i++)
            if (name == swordConfigList[i].name)
                return swordConfigList[i].sword;
        return null;
    }

    public Item GetSwordItemConfig(string name)
    {
        for (int i = 0; i < swordConfigList.Count; i++)
            if (name == swordConfigList[i].name)
            {
                Item item = new Item();
                item.amount = swordConfigList[i].amount;
                item.maxAmount = swordConfigList[i].maxAmount;
                item.itemValue=swordConfigList[i].itemValue;
                item.itemData = JsonUtility.ToJson(swordConfigList[i].sword);
                item.itemName = name;
                item.slotId = -1;
                item.itemType=ItemType.Sword;
                return item;
            }
        return null;
    }

    #endregion

    #endregion GetConfig

    /// <summary>
    /// Get Config data when item have only name
    /// </summary>
    /// <param name="item"></param>
    public Item GetItemConfig(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Book:
                item = GetBookItemConfig(item.itemName);
                return item;
            case ItemType.Gun:
                item = GetGunItemConfig(item.itemName);
                return item;
            case ItemType.Food:
                item = GetFoodItemConfig(item.itemName);
                return item;
            case ItemType.LifeFlask:
                item = GetHPFlaskItemConfig(item.itemName);
                return item;
            case ItemType.Sword:
                item = GetSwordItemConfig(item.itemName);
                return item;
            case ItemType.ManaFlask:
                item = GetManaFlaskItemConfig(item.itemName);
                return item;
            default:
                return null;
        }
    }

    /// <summary>
    /// Get Config data when item have only name
    /// </summary>
    /// <param name="item"></param>
    public void GetItemConfig(ref Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Book:
                item = GetBookItemConfig(item.itemName);
                break;
            case ItemType.Gun:
                item = GetGunItemConfig(item.itemName);
                break;
            case ItemType.Food:
                item = GetFoodItemConfig(item.itemName);
                break;
            case ItemType.LifeFlask:
                item = GetHPFlaskItemConfig(item.itemName);
                break;
            case ItemType.Sword:
                item = GetSwordItemConfig(item.itemName);
                break;
            case ItemType.ManaFlask:
                item=GetManaFlaskItemConfig(item.itemName);
                break;
            default:
                break;
        }
    }
}


