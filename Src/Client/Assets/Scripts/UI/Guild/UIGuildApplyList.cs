using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Managers;
using Services;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class UIGuildApplyList : UIWindow
{

    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;

    // Use this for initialization
	void Start ()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList;
        GuildService.Instance.SendGuildListRequest();
        this.UpdateList();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateList;
    }

    // Update is called once per frame
	void UpdateList ()
    {
        ClearList();
        InitItem();
    }
    void InitItem()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Applies)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildApplyItem ui = go.GetComponent<UIGuildApplyItem>();
            ui.SetItemInfo(item);
            this.listMain.AddItem(ui);
        }
    }
    void ClearList()
    {
        this.listMain.RemoveAll();
    }

}
