using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Image choosedImage;
    [SerializeField] private List<Button> buttonList;
    [SerializeField] private Text priceText;
    [SerializeField] private ItemConfig itemConfig;

    public Inventory inventory;

    private string choosedName = "sword";

    private void Start()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            int tempI = i;
            buttonList[i].onClick.AddListener(() =>
            {
                ChooseItem(tempI);
            });
        }

    }
    public void ChooseItem(int i)
    {
        choosedName = buttonList[i].GetComponent<Image>().sprite.name;
        choosedImage.sprite = buttonList[i].GetComponent<Image>().sprite;
    }

    public void BuyItem()
    {
        int amount;
        try { amount = int.Parse(inputField.text); }
        catch { amount = 1;}

        if (amount >= 1)
            switch (choosedName)
            {
                case "food":
                    AddFoodToInventory(amount);
                    break;
                case "mana":
                    AddManaToInventory(amount);
                    break;
                case "hp":
                    AddHPToInventory(amount);
                    break;
                case "book":
                    AddBookToInventory(amount);
                    break;
                case "sword":
                    AddSwordToInventory(amount);
                    break;
                case "gun":
                    AddGunToInventory(amount);
                    break;
            }
    }

    public void OnChangePrice(string input)
    {
        int amount;
        try
        {
            amount = int.Parse(inputField.text);
        }
        catch
        {
            amount = 1;
        }
        priceText.text = amount.ToString();
        // switch (choosedName)
        // {
        //     case "food":
        //         for (int i = 0; i < itemConfig.foodConfigList.Count; i++)
        //             if (itemConfig.foodConfigList[i].name == "food")
        //                 priceText.text = (amount * itemConfig.foodConfigList[i].itemValue * amount).ToString();
        //         break;
        // }
    }
    public void AddManaToInventory(int amount)
    {
        Item mana = new Item(ItemType.ManaFlask, "mana");
        itemConfig.GetItemConfig(ref mana);
        mana.amount = amount;
        inventory.AddItemToInventory(mana);
    }
    public void AddBookToInventory(int amount)
    {
        Item book = new Item(ItemType.NormalItem, "book");
        book = itemConfig.GetItemConfig(book);
        book.amount = amount;
        inventory.AddItemToInventory(book);
    }
    public void AddGunToInventory(int amount)
    {
        Item Gun = new Item(ItemType.Gun, "gun");
        Gun = itemConfig.GetItemConfig(Gun);
        Gun.amount = amount;
        inventory.AddItemToInventory(Gun);
    }
    public void AddSwordToInventory(int amount)
    {
        Item sword = new Item(ItemType.Sword, "sword");
        sword = itemConfig.GetItemConfig(sword);
        sword.amount = amount;
        inventory.AddItemToInventory(sword);
    }
    public void AddHPToInventory(int amount)
    {
        Item hp = new Item(ItemType.LifeFlask, "hp");
        hp = itemConfig.GetItemConfig(hp);
        hp.amount = amount;
        inventory.AddItemToInventory(hp);
    }
    public void AddFoodToInventory(int amount)
    {
        Item food = new Item(ItemType.Food, "food");
        food = itemConfig.GetItemConfig(food);
        food.amount = amount;
        inventory.AddItemToInventory(food);
    }

}
