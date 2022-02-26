﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class EquipManager:Singleton<EquipManager>
    {
        public Result EquipItem(NetConnection<NetSession> sender, int slot, int itemId, bool isEquip)
        {
            Character character = sender.Session.Character;
            if (!character.ItemManager.Items.ContainsKey(itemId))
            {
                return Result.Failed;
            }

            UpdateEquip(character.Data.Equips, slot, itemId, isEquip);
            DBService.Instance.Save();
            return Result.Success;
        }

        unsafe void UpdateEquip(byte[] equipDate, int slot, int itemId, bool isEquip)
        {
            fixed (byte* pt = equipDate)
            {
                int* slotid = (int*) (pt + slot * sizeof(int));
                if (isEquip)
                {
                    *slotid = itemId;
                }
                else
                {
                    *slotid = 0;
                }
            }
        }
    }
}
