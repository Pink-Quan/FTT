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
        [TextArea]
        public string info;
    }
    [Serializable]
    public class HealFlaskConfig
    {
        public HealFlask heal;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
        [TextArea]
        public string info;
    }
    [Serializable]
    public class FoodConfig
    {
        public Food food;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
        [TextArea]
        public string info;
    }
    [Serializable]
    public class GunConfig
    {
        public Gun gun;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
        [TextArea]
        public string info;
    }
    [Serializable]
    public class NormalItemConfig
    {
        public NormalItem item;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
        [TextArea]
        public string info;
    }
    [Serializable]
    public class SwordConfig
    {
        public Sword sword;
        public string name;
        public int amount = 1;
        public int maxAmount = 10;
        public int itemValue;
        [TextArea]
        public string info;
    }
    #endregion ConfigClass

    #region List
    public List<ManaFlaskConfig> manaRcoveryConfigList;
    public List<HealFlaskConfig> healFlaskConfigList;
    public List<FoodConfig> foodConfigList;
    public List<GunConfig> gunConfigList;
    public List<NormalItemConfig> normalItemConfigList;
    public List<SwordConfig> swordConfigList;
    #endregion

    #region GetConfig

    #region Mana
    public ManaFlask GetManaFlaskConfig(string name)
    {
        for (int i = 0; i < manaRcoveryConfigList.Count; i++)
            if (name == manaRcoveryConfigList[i].name)
                return manaRcoveryConfigList[i].mana;
        Debug.LogError("Cant find the item: " + name);
        return new ManaFlask();
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
                item.info = manaRcoveryConfigList[i].info;
                return item;
            }
        Debug.LogError("Cant find the item: " + name);
        return null;
    }
    #endregion

    #region HP

    public HealFlask GetHPFlaskConfig(string name)
    {
        for (int i = 0; i < healFlaskConfigList.Count; i++)
            if (name == healFlaskConfigList[i].name)
                return healFlaskConfigList[i].heal;
        Debug.LogError("Cant find the item: " + name);
        return new HealFlask();
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
                item.info = healFlaskConfigList[i].info;
                return item;
            }
        Debug.LogError("Cant find the item: " + name);
        return null;
    }

    #endregion

    #region Food

    public Food GetFoodConfig(string name)
    {
        for (int i = 0; i < foodConfigList.Count; i++)
            if (name == foodConfigList[i].name)
                return foodConfigList[i].food;
        Debug.LogError("Cant find the item: " + name);
        return new Food();
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
                item.info = foodConfigList[i].info;
                return item;
            }
        Debug.LogError("Cant find the item: " + name);
        return null;
    }

    #endregion

    #region Gun

    public Gun GetGunConfig(string name)
    {
        for (int i = 0; i < gunConfigList.Count; i++)
            if (name == gunConfigList[i].name)
                return gunConfigList[i].gun;
        Debug.LogError("Cant find the item: " + name);
        return new Gun();
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
                item.info = gunConfigList[i].info;
                return item;
            }
        Debug.LogError("Cant find the item: " + name);
        return null;
    }

    #endregion

    #region Book

    public NormalItem GetBookConfig(string name)
    {
        for (int i = 0; i < normalItemConfigList.Count; i++)
            if (name == normalItemConfigList[i].name)
                return normalItemConfigList[i].item;
        Debug.LogError("Cant find the item: " + name);
        return new NormalItem();
    }
    public Item GetBookItemConfig(string name)
    {
        for (int i = 0; i < normalItemConfigList.Count; i++)
            if (name == normalItemConfigList[i].name)
            {
                Item item = new Item();
                item.amount = normalItemConfigList[i].amount;
                item.maxAmount = normalItemConfigList[i].maxAmount;
                item.itemValue=normalItemConfigList[i].itemValue;
                item.itemData = JsonUtility.ToJson(normalItemConfigList[i].item);
                item.itemName = name;
                item.slotId = -1;
                item.itemType=ItemType.NormalItem;
                item.info = normalItemConfigList[i].info;
                return item;
            }
        Debug.LogError("Cant find the item: " + name);
        return null;
    }

    #endregion

    #region Sword

    public Sword GetSwordConfig(string name)
    {
        for (int i = 0; i < swordConfigList.Count; i++)
            if (name == swordConfigList[i].name)
                return swordConfigList[i].sword;
        Debug.LogError("Cant find the item: " + name);
        return new Sword();
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
                item.info = swordConfigList[i].info;
                return item;
            }
        Debug.LogError("Cant find the item: " + name);
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
            case ItemType.NormalItem:
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
                Debug.LogError("Cant find the item: " + name);
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
            case ItemType.NormalItem:
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
                Debug.LogError("Cant find the item: " + name);
                break;
        }
    }
}


