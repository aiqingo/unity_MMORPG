using System.Collections;
using System.Collections.Generic;
using Managers;
using Services;
using UnityEngine;

public class UIGuild : UIWindow {

    public GameObject itemPrefad;

    public ListView listMain;

    public Transform itemRoot;

    public UIGuildInfo uiInfo;

    public UIGuildItem selectedItem;
    // Use this for initialization
    void Start ()
    {
        UpdateUI();
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
    }

    void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate = null;
    }

    // Update is called once per frame
	void UpdateUI ()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem =item as UIGuildItem;
        
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

    }
    public void OnClickLeave()
    {

    }
    public void OnClickChat()
    {

    }
    public void OnClickKickout()
    {

    }
    public void OnClickPromote()
    {

    }
    public void OnClickDepose()
    {

    }
    public void OnClickTransfer()
    {

    }
    public void OnClickSetNotice()
    {

    }


}
