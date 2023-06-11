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

    public Item() { }

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
    Book=6,
    Food=7,
}

[Serializable]
public class ConsumableItem
{
    public int level;
    
}

[Serializable]
public class ManaFlask:ConsumableItem
{
    public float manaRecovery;
}

[Serializable]
public class HealFlask : ConsumableItem
{
    public float manaRecovery;
}

[Serializable]
public class Food : ConsumableItem
{
    public float hungerFill;
}

[Serializable]
public class Coin
{

}

[Serializable]
public class Weapon
{
    public int durability;
    public int maxDurability;
    public int damage;
    public int level;
}

[Serializable]
public class Sword:Weapon
{
   
}

[Serializable]
public class Gun : Weapon
{
    public int bullet;
    public int maxBullet;
}

[Serializable]
public class Book
{
    public string data;
}


