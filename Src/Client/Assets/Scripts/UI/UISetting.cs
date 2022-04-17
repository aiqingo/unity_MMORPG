using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow {

    public void ExitToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGamLeave();
    }

    public void ExitGame()
    {
        //Services.UserService.Instance.SendGameLeaver(true);
    }
}
