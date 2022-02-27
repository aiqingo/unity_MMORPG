using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class ItemService:Singleton<ItemService>
    {
        public ItemService() { MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
        MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(this
            .OnItemEquip);
        }

        public void Init()
        {

        }

        void OnItemBuy(NetConnection<NetSession> sender,ItemBuyRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemBuy: character:{0}:Shop:{1} ShopItEM:{2}",character.Id,request.shopId,request.shopItemId);
            var reqult = ShopManager.Instance.BuyItem(sender, request.shopId, request.shopItemId);
            sender.Session.Response.itemBuy=new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = reqult;
            sender.SendResponse();
        }

        void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemEquip: :character:{0}:Slot{1} item:{2} Equip:{3}",character.Id,request.Slot,request.itenId,request.isEquip);
            var reqult = EquipManager.Instance.EquipItem(sender, request.Slot, request.itenId,request.isEquip);
            sender.Session.Response.itemEquip = new ItemEquipResponse();
            sender.Session.Response.itemEquip.Result = reqult;
            sender.SendResponse();

        }
    }
}
