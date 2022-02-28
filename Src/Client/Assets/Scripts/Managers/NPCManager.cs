﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;

namespace Managers
{
    class NPCManager:Singleton<NPCManager>
    {
        public delegate bool NpcActionHandler(NpcDefine npc);

        private Dictionary<NpcDefine.NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcDefine.NpcFunction, NpcActionHandler>();

        public void RegisterNpcEvent(NpcDefine.NpcFunction function, NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
        }

        public NpcDefine GetNpcDefine(int npcId)
        {
            NpcDefine npc = null;
            DataManager.Instance.Npcs.TryGetValue(npcId, out npc);
            return npc;

        }

        public bool Interactive(int npcId)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcId))
            {
                var npc = DataManager.Instance.Npcs[npcId];
                return Interactive(npcId);
            }
            return false;
        }

        public bool Interactive(NpcDefine npc)
        {
            if (DoTaskInteractive(npc))
            {
                return true;
            }
            else if (npc.Type==NpcDefine.NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }

            return false;
        }

        private bool DoTaskInteractive(NpcDefine npc)
        {
            var straus = QuestManager.Instance.GetQuestStatusByNpc(npc.ID);
            if (straus==NpcQuestStatus.None)
            {
                return false;
            }

            return QuestManager.Instance.OpenNpcQuest(npc.ID);
        }

        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if (npc.Type!=NpcDefine.NpcType.Functional)
            {
                return false;
            }
            if (!eventMap.ContainsKey(npc.Function))
            {
                return false;
            }
            return eventMap[npc.Function](npc);
        }


    }
}
