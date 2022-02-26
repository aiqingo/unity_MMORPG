using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Managers;
using Models;
using Services;
using SkillBridge.Message;

namespace Assets.Scripts.Managers
{
    class EquipManager:Singleton<EquipManager>
    {

        public delegate void OnEquipChangerHandler();

        public event OnEquipChangerHandler OnEquipChanged;
        public Item[] Equips=new Item[(int)EquipSlot.SlotMax];
        private byte[] Data;

        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            this.ParseEquipData(data);
        }

        public Item GetEquip(EquipSlot slot)
        {
            return Equips[(int) slot];
        }

        unsafe void ParseEquipData(byte[] data)
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < this.Equips.Length; i++)
                {
                    int itemId = *(int*) (pt + i * sizeof(int));
                    if (itemId>0)
                    {
                        Equips[i] = ItemManager.Instance.Items[itemId];
                    }
                    else
                    {
                        Equips[i] = null;
                    }
                }
            }
        }

        unsafe public byte[] GetEquipData()
        {
            fixed (byte* pt = Data)
            {
                for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemid = (int*) (pt + i * sizeof(int));
                    if (Equips[i]==null)
                    {
                        *itemid = 0;
                    }
                    else
                    {
                        *itemid = Equips[i].Id;
                    }
                }
            }
            return this.Data;
        }

        public bool Contains(int equipId)
        {
            for (int i = 0; i < this.Equips.Length; i++)
            {
                if (Equips[i]!=null&&Equips[i].Id==equipId)
                {
                    return true;
                }
            }
            return false;
        }

        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }

        public void OnEquipItem(Item equip)
        {
            if (this.Equips[(int)equip.EquipInfo.Slot]!=null&&this.Equips[(int)equip.EquipInfo.Slot].Id==equip.Id)
            {
                return;
            }
            
            this.Equips[(int) equip.EquipInfo.Slot] = ItemManager.Instance.Items[equip.Id];
            if (OnEquipChanged!=null)
            {
                OnEquipChanged();
            }
        }

        public void OnUnEquipItem(EquipSlot slot)
        {
            if (this.Equips[(int)slot]!=null)
            {
                this.Equips[(int) slot] = null;
                if (OnEquipChanged!=null)
                {
                    OnEquipChanged();
                }
            }
        }
    }
}
