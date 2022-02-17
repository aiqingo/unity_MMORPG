using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Managers;
using Models;
using SkillBridge.Message;

namespace Managers
{
    class BagManager:Singleton<BagManager>
    {
        public int Unlocked;

        public BagItem[] Items;

        private NBagInfo Info;

        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items=new BagItem[this.Unlocked];
            if (info.Items != null && info.Items.Length>=this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                Info.Items=new byte[sizeof(BagItem)+this.Unlocked];
                Reset();
            }
            
        }

        public void Reset()
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items)
            {
                if (kv.Value.Count<=kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort) kv.Key;
                    this.Items[i].Count = (ushort) kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count>kv.Value.Define.StackLimit)
                    {
                        this.Items[i].ItemId = (ushort) kv.Key;
                        this.Items[i].Count = (ushort) kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }

                    this.Items[i].ItemId = (ushort) kv.Key;
                    this.Items[i].Count = (ushort) count;
                }

                i++;
            }
        }

        unsafe void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*) (pt + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*) (pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }

            return this.Info;
        }

        public void AddItem(int itemId, int count)
        {
            ushort addCount = (ushort) count;
            for (int i = 0; i < Items.Length; i++)
            {
                if (this.Items[i].ItemId==itemId)
                {
                    ushort canAdd = (ushort) (DataManager.Instance.Items[itemId].StackLimit - this.Items[i].Count);
                    if (canAdd>=addCount)
                    {
                        this.Items[i].Count += addCount;
                        addCount = 0;
                        break;
                    }
                    else
                    {
                        this.Items[i].Count += canAdd;
                        addCount -= canAdd;
                    }
                }
            }

            if (addCount>0)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (this.Items[i].ItemId==0)
                    {
                        this.Items[i].ItemId = (ushort) itemId;
                        this.Items[i].Count = addCount;
                    }
                }
            }
        }
        public void RemoveItem(int itemId,int count) { }
    }
}
