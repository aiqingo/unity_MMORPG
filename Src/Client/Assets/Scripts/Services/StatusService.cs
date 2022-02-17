using System;
using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using Models;
using Network;

namespace Services
{
    public class StatusService : Singleton<StatusService>,IDisposable
    {
        public delegate bool StatysNotifyHandler(NStatus status);

        private Dictionary<StatusType, StatysNotifyHandler> eventMap = new Dictionary<StatusType, StatysNotifyHandler>();

        public void Init()
        {
        }

        public void RegistrerStatusNofity(StatusType function, StatysNotifyHandler action)
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

        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }

        public void Dispose()
        {
          MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }

        private void OnStatusNotify(object sender, StatusNotify notify)
        {
            foreach (NStatus status in notify.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
           Debug.LogFormat("StatusNotify:[{0}][{1}]{2}:{3}",
               status.Type,status.Action,status.Id,status.Value);
           if (status.Type==StatusType.Money)
           {
               if (status.Action==StatusAction.Add)
               {
                    User.Instance.AddGold(status.Value);
               }
               else if(status.Action==StatusAction.Delete)
               {
                   User.Instance.AddGold(-status.Value);
               }
           }

           StatysNotifyHandler handler;
           if (eventMap.TryGetValue(status.Type , out handler))
           {
               handler(status);
           };
        }
    }
}