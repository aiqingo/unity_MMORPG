using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services;
using Common.Data;
using UnityEngine;

namespace Managers
{
    class ShopManager:Singleton<ShopManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeShop,OnOpenShop);
        }

        private bool OnOpenShop(NpcDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }

        public void ShowShop(int shopId)
        {
            ShopDefine shop;
            if (DataManager.Instance.Shops.TryGetValue(shopId,out shop))
            {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if (uiShop!=null)
                {
                    uiShop.SetShop(shop);
                }
            }
        }

        //public long Golg
        //{
        //    get { return this.Data.Gold; }
        //    set
        //    {
        //        if (this.Data.Gold==value)
        //            return;
        //        this.StatuManager
        //    }
        //}

        public bool BuyItem(int shopId, int ShopItemId)
        {
            ItemService.Instance.SendBuyItem(shopId, ShopItemId);
            return true;
        }
    }
}
