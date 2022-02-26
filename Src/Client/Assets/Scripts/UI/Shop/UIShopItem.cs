using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour,ISelectHandler
{

    public Image icon;
    public Text title;
    public Text price;
    public Text linitClass;
    public Text count;

    public Image background;
    public Sprite normalBg;
    public Sprite SelectedBg;

    private  bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? SelectedBg : normalBg;
        }
    }

    public int ShopItemID { get; set; }
    private UIShop shop;
    private ItemDefine item;
    private ShopItemDefine ShopItem { get; set; }

    public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {
        this.shop = owner;
        this.ShopItemID = id;
        this.ShopItem = shopItem;
        this.item = DataManager.Instance.Items[this.ShopItem.ItemID];

        this.title.text = this.item.Name;
        this.count.text = "x"+ShopItem.Count.ToString();
        this.price.text = ShopItem.Price.ToString();
        this.linitClass.text = this.item.LimitClass.ToString();
        this.icon.overrideSprite = Resources.Load<Sprite>(item.Icon);
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.selected = true;
        this.shop.SelectShopItem(this);
    }


}
