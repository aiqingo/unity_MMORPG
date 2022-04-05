using System.Collections;
using System.Collections.Generic;
using Managers;
using Models;
using Services;
using UnityEngine;

public class UIFriends : UIWindow {

    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIFriendItem selectedItem;

    // Use this for initialization
    void Start()
    {
        RefreshUI();
        FriendServer.Instance.OnFriendUpdate += RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }

    public void OnClickFriendAdd()
    {
        InputBox.Show("请输入要添加好友的名称或ID", "添加好友").OnSubmit += OnFriendAddSubmit;
    }


    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(input, out friendId))
        {
            friendName = input;
        }

        if (friendId == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
        {
            tips = "开玩笑，你想添加自己";
            return false;
        }

        FriendServer.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }

    public void OnClickFriendChat()
    {
        MessageBox.Show("暂未开放");
    }


    public void OnClickFriendTeamInvite()
    {
        if (selectedItem==null)
        {
            MessageBox.Show("请选择要邀请好友");
            return;
        }

        if (selectedItem.Info.Status == 0)
        {
            MessageBox.Show("请选择在线好友");
            return;
        }

        MessageBox.Show(string.Format("确定邀请【{0}】加入队伍吗？", selectedItem.Info.friendInfo.Name), "邀请好友组队",MessageBoxType.Confirm, "邀请", "取消").OnYes= () => 
        {
            TeamService.Instance.SendTeamInviteRequest(this.selectedItem.Info.friendInfo.Id, this.selectedItem.Info.friendInfo.Name);
        };
    }


    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }

        MessageBox.Show(string.Format("确定要删除【{0}】吗？", selectedItem.Info.friendInfo.Name), "删除好友",
            MessageBoxType.Confirm, "删除", "取消").OnYes= () =>
        {
            FriendServer.Instance.SendFriendRemoveRequest(this.selectedItem.Info.Id,this.selectedItem.Info.friendInfo.Id);
        };
    }


    void RefreshUI()
    {
        ClearFriendLis();
        InitFriendItems();
    }


    public void  InitFriendItems()
    {
        foreach (var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this
                .listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearFriendLis()
    {
        this.listMain.RemoveAll();
    }
}
