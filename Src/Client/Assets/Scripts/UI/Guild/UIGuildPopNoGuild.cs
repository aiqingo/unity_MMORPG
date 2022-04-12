using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuildPopNoGuild : UIWindow
{

    public override void OnYesClick()
    {
        UIManager.Instance.Show<UIGuildPopCreate>();
    }

    public override void OnNoClick()
    {
        UIManager.Instance.Show<UIGuildList>();
    }
}
