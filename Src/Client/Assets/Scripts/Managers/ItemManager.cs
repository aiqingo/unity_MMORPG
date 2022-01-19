using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

namespace Managers
{
    class ItemManager:Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        internal void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.ID, item);
                Debug.LogFormat("ItemManager:Init[{0}]",item);
            }
        }

        public  ItemDefine GetItem(int itemId)
        {
            return null;
        }

        public bool UseItem(int itemId)
        {
            return false;
        }

        public bool UseItem(ItemDefine item)
        {
            return false;
        }

    }
}
