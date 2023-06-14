using System;

[Serializable]
public class Item
{
    public ItemType itemType;
    public string itemData;
    public string itemName;
    public string itemInformation;
    public int itemValue;

    //For inventory
    public int slotId;
    public int amount;
    public int maxAmount;

    public Item() 
    {
        itemType = ItemType.NULL;
    }

    public Item(ItemType itemType, string itemData, string itemName, int slotId, int amount,int maxAmount)
    {
        this.itemType = itemType;
        this.itemData = itemData;
        this.itemName = itemName;
        this.slotId = slotId;
        this.amount = amount;
        this.maxAmount = maxAmount;
    }
    public Item(ItemType itemType, string itemName)
    {
        this.itemType = itemType;
        this.itemName = itemName;

        slotId=-1;
        amount=1;
        maxAmount=10;
    }
}

[Serializable]
public enum ItemType
{
    Sword=0,
    Gun=1,
    Coin=2,
    LifeFlask=3,
    ManaFlask=4,
    Bullet=5,
    NormalItem=6,
    Food=7,
    NULL=8,
}

[Serializable]
public struct ManaFlask
{
    public int level;
    public float manaRecovery;
}

[Serializable]
public struct HealFlask 
{
    public int level;
    public float manaRecovery;
}

[Serializable]
public struct Food
{
    public int level;
    public float hungerFill;
}

[Serializable]
public struct Coin
{
    string data;
}

[Serializable]
public struct Sword
{
    public int durability;
    public int maxDurability;
    public int damage;
    public int level;
}

[Serializable]
public struct Gun 
{
    public int durability;
    public int maxDurability;
    public int damage;
    public int level;
    public int bullet;
    public int maxBullet;
}

[Serializable]
public struct NormalItem
{
    public string data;
}


