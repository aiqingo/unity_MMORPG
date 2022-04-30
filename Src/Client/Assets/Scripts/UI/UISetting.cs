using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow {

    public void ExitToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        Services.UserService.Instance.SendGamLeave();
    }

    public void SystemConfig()
    {
        UIManager.Instance.Show<UISyayemConfig>();
        this.Close();
    }

    public void ExitGame()
    {
        Services.UserService.Instance.SendGamLeave(true);
    }
}
