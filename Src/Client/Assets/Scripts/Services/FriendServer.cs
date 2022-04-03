using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    class FriendServer:Singleton<FriendServer>,IDisposable
    {
        public UnityAction OnFriendUpdate;
        public void Init()
        {
        }
        public FriendServer()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this
                .OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this
                .OnFriendRemove);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this
                .OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this
                .OnFriendRemove);
        }
        /// <summary>
        /// 发送好友请求
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="friendName"></param>
        public void SendFriendAddRequest(int friendId, string friendName)
        {
            Debug.Log("SendFriendAdd");
            NetMessage message=new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddReq=new FriendAddRequest();
            message.Request.friendAddReq.FromId = User.Instance.CurrentCharacter.Id;
            message.Request.friendAddReq.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.friendAddReq.ToId = friendId;
            message.Request.friendAddReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }
        /// <summary>
        /// 发送好友请求结果
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="request"></param>
        public void SendFriendAddResponse(bool accept, FriendAddRequest request)
        {
            Debug.Log("SendFriendAdd");
            NetMessage message=new NetMessage();
            message.Request=new NetMessageRequest();
            message.Request.friendAddRes=new FriendAddResponse();
            message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.friendAddRes.Errormsg = accept ? "对方同意" : "对方拒绝了你的请求";
            message.Request.friendAddRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        private void OnFriendAddRequest(object sender, FriendAddRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0}请求加你为好友", request.FromName), "好友请求", MessageBoxType.Confirm,
                "接受", "拒绝");
            confirm.OnYes = () =>
            { this.SendFriendAddResponse(true, request); };
            confirm.OnNo = () =>
            { this.SendFriendAddResponse(false,request); };
        }

        /// <summary>
        /// 收到好友请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnFriendAddResponse(object sender, FriendAddResponse message)
        {
            if (message.Result==Result.Success)
            {
                MessageBox.Show(message.Request.ToName + "接受您的请求", "添加好友成功");
            }
            else
            {
                MessageBox.Show(message.Errormsg, "添加好友失败");
            }
        }

        private void OnFriendList(object seder, FriendListResponse message)
        {
            Debug.Log("OnFriendList");
            FriendManager.Instance.allFriends = message.Friends;
            if (this.OnFriendUpdate!=null)
            {
                this.OnFriendUpdate();
            }
        }


        /// <summary>
        /// 发送删除好友
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="friendId"></param>
        public void SendFriendRemoveRequest(int Id, int friendId)
        {
            Debug.Log("SendFriendRemoveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove=new FriendRemoveRequest();
            message.Request.friendRemove.Id = Id;
            message.Request.friendRemove.friendId = friendId;
            NetClient.Instance.SendMessage(message);
        }

    


        private void OnFriendRemove(object sender, FriendRemoveResponse message)
        {
            if (message.Result==Result.Success)
            {
                MessageBox.Show("删除成功", "删除好友");
            }
            else
            {
                MessageBox.Show("删除失败", "删除好友", MessageBoxType.Error);
            }
        }

 
    }
}
