using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

namespace Models
{
    public class Item
    {
        public int ID;
        public int Count;
        public ItemDefine Define;
        
        public Item(NItemInfo item)
        {
            this.ID = item.Id;
            this.Count = item.Count;
            this.Define = DataManager.Instance.Items[item.Id];
        }

        public override string ToString()
        {
            return string.Format("Id:{0},Count:{1}", this.ID, this.Count);
        }
    }
}
