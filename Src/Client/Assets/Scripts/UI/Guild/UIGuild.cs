using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Services;
using SkillBridge.Message;
using UnityEngine;

public class UIGuild : UIWindow {

    public GameObject itemPrefad;

    public ListView listMain;

    public Transform itemRoot;

    public UIGuildInfo uiInfo;

    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;

    // Use this for initialization
    void Start ()
    {
        UpdateUI();
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
    }

    void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    // Update is called once per frame
	void UpdateUI ()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title>GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title==GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem =item as UIGuildMemberItem;
        
    }
    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefad, this.itemRoot.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        this.listMain.RemoveAll();
    }

    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }
    public void OnClickLeave()
    {
        MessageBox.Show("扩展作业");
        
    }
    public void OnClickChat()
    {
        MessageBox.Show("暂未开放");
    }
    public void OnClickExpel()
    {
        if (selectedItem==null)
        {
            MessageBox.Show("请选择要提出的成员");
            return;
        }

        MessageBox.Show(String.Format("要踢【{0}】出公会吗？", this.selectedItem.Info.Info
            .Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
            };
    }

    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }

        if (selectedItem.Info.Title!=GuildTitle.None)
        {
            MessageBox.Show("对方已是管理员");
            return;
        }

        MessageBox.Show(String.Format("要晋升【{0}】为管理员吗？", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm,
                "确定", "取消").OnYes =
            () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote,this.selectedItem.Info.Info.Id);
            };
    }
    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要撤职的成员");
            return;
        }

        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方已是无职位");
            return;
        }

        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("你想给会长撤职吗？");
            return;
        }

        MessageBox.Show(String.Format("要罢免【{0}】的职位吗？", this.selectedItem.Info.Info.Name), "罢免",MessageBoxType.Confirm, "确定",
                "取消").OnYes =
            () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost,this.selectedItem.Info.Info.Id);
            };

    }
    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要吧会长转让给的成员");
            return;
        }
        MessageBox.Show(String.Format("要转让给【{0}】会长吗吗？", this.selectedItem.Info.Info.Name),"转让会长", MessageBoxType.Confirm, "确定",
                "取消").OnYes =
            () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
            };
    }
    public void OnClickSetNotice()
    {

        MessageBox.Show("扩展作业");
    }


}
