using System.Collections;
using System.Collections.Generic;
using Common.Data;
using Managers;
using UnityEngine;

namespace Managers
{
    public class TestManager : Singleton<TestManager>
    {

        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeShop, OnNpcInvokeShop);
            NPCManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
        }

        private bool OnNpcInvokeShop(NpcDefine npc)
        {
           Debug.LogFormat("TestManager.OnMapInvokeShop :NPC:[{0}:{1}] Type: {2} Func: {3}", npc.ID, npc.Name, npc.Type, npc.Function);
            UIManager.Instance.Show<UITest>();
            return true;
        }

        private bool OnNpcInvokeInsrance(NpcDefine npc)
        {
            Debug.LogFormat("TestManager.OnNpcInvokeInsrance :NPC:[{0}:{1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
            MessageBox.Show("点击了NPC:" + npc.Name, "NPC对话");
            return true;
        }

    }

}
