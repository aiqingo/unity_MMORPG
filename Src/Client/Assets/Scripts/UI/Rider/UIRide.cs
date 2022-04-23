using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Managers;
using Models;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow
{

    public Text desceript;
    public GameObject itemPrefab;
    public ListView listMain;
    public UIRideItem selectedItem;
	void Start ()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }

    private void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem=item as UIRideItem;
        ;
        this.desceript.text = this.selectedItem.item.Define.Description;
    }

    void RefreshUI()
    {
        ClearItem();
        InitItem();
    }

    void InitItem()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type==ItemType.Ride&&(kv.Value.Define.LimitClass==CharacterClass.None||kv.Value.Define.LimitClass==User.Instance.CurrentCharacter.Class))
            {
       
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetRideItem(kv.Value,this,false);
                this.listMain.AddItem(ui);
            }
        }
    }

    void ClearItem()
    {
        this.listMain.RemoveAll();
    }

    public void DoRide()
    {
        if (this.selectedItem==null)
        {
            MessageBox.Show("请选坐骑");
            return;
        }
        User.Instance.Ride(this.selectedItem.item.Id);
    }
}
