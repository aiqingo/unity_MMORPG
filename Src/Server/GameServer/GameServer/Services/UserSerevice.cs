﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }

      


        public void Init()
        {

        }

        void OnLogin(NetConnection<NetSession> sender,UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}",request.User,request.Passward);
            NetMessage message = new NetMessage();
            message.Response=new NetMessageResponse();
            message.Response.userLogin=new UserLoginResponse();
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();

            if (user == null)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户不存在";
            }
            else if (user.Password!=request.Passward)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "密码错误";
            }
      
            else
            {
                sender.Session.User = user;

                message.Response.userLogin.Result = Result.Success;
                message.Response.userLogin.Errormsg = "None";
                message.Response.userLogin.Userinfo=new NUserInfo();
                message.Response.userLogin.Userinfo.Id = (int)user.ID;
                message.Response.userLogin.Userinfo.Player=new NPlayerInfo();
                message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info=new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Type = CharacterType.Player;
                    info.Class = (CharacterClass) c.Class;
                    info.Tid = c.ID;
                    message.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }



        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "None";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacterRequest: Name:{0}  Class:{1}", request.Name, request.Class);

            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                Class = (int)request.Class,
                TID = (int)request.Class,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820,
                Gold = 100000,
            };
            var  bag=new TCharacterBag();
            bag.Owner = character;
            bag.Items=new byte[0];
            bag.Unloked = 20;
            character.Bag = DBService.Instance.Entities.CharacterBags.Add(bag);

            //检测创建失败是加  tra
            character= DBService.Instance.Entities.Characters.Add(character);
            sender.Session.User.Player.Characters.Add(character);
            //操作完表保存表
            DBService.Instance.Entities.SaveChanges();



            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();


            message.Response.createChar.Result = Result.Success;
            message.Response.createChar.Errormsg = "None";

            foreach (var c in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = 0;
                info.Name = c.Name;
                info.Type = CharacterType.Player;
                info.Class = (CharacterClass)c.Class;
                info.Tid = c.ID;
                message.Response.createChar.Characters.Add(info);

            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }


        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest: characterID: {0}:{1} Map:{2}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);


            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameEnter = new UserGameEnterResponse();
            message.Response.gameEnter.Result = Result.Success;
            message.Response.gameEnter.Errormsg = "None";
            //进入成功 发送初始角色信息
            message.Response.gameEnter.Character = character.Info;


            //给某人增加某个道具  GM增加道具
            //if (character.Id==1)
            //{
            //    int itemId = 2;
            //    bool hasItem = character.ItemManager.HasItem(itemId);
            //    Log.InfoFormat("HasItem:[{0}]{1}", itemId, hasItem);
            //    if (hasItem)
            //    {
            //        character.ItemManager.RemoveItem(itemId, 1);
            //    }
            //    else
            //    {
            //        character.ItemManager.AddItem(itemId, 2);
            //    }

            //    Models.Item item = character.ItemManager.GetItem(itemId);
            //    Log.InfoFormat("Item:[{0}][{1}]", itemId, item);

            //}
            //道具系统测试
           int itemId = 1;
           bool hasItem = character.ItemManager.HasItem(itemId);
           Log.InfoFormat("HasItem:[{0}]{1}",itemId,hasItem);
           if (hasItem)
           {
               //character.ItemManager.RemoveItem(itemId, 1);
           }
           else
           {
               character.ItemManager.AddItem(1, 200);
               character.ItemManager.AddItem(2, 100);
               character.ItemManager.AddItem(3, 30);
               character.ItemManager.AddItem(4, 120);
           }

           Models.Item item = character.ItemManager.GetItem(itemId);
           Log.InfoFormat("Item:[{0}][{1}]",itemId,item);

           DBService.Instance.Save();

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
            sender.Session.Character = character;
            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGamLeaveReques: characterId:{0}:{1} Map:{2}", character.Id, character.Info.Name, character.Info.mapId);

            CharacterLeave(character);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameLeave = new UserGameLeaveResponse();
            message.Response.gameLeave.Result = Result.Success;
            message.Response.gameLeave.Errormsg = "None";

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);

        }

        public void CharacterLeave(Character character)
        {
            CharacterManager.Instance.RemoveCharacter(character.Id);
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }
    }
}
