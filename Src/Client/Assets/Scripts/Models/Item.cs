using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

namespace Models
{
    public class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;

        public Item(NItemInfo item) : 
            this(item.Id, item.Count)
        {
        }

        public Item(int id, int count)
        {
            this.Id = id;
            this.Count = count;
            this.Define = DataManager.Instance.Items[this.Id];
        }

        public override string ToString()
        {
            return string.Format("Id:{0},Count:{1}", this.Id, this.Count);
        }
    }
}
