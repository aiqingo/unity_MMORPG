using System.Collections;
using System.Collections.Generic;
using Models;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class UITeam : MonoBehaviour
{

    public Text teamTitle;
    public UITeamItem[] Members;
    public ListView list;
	void Start () {
        if (User.Instance.TeamInfo==null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        foreach (var item in Members)
        {
            this.list.AddItem(item);
        }
	}

    void OnEnable()
    {
        UpdateTeamUI();
    }

    public void ShowTeam(bool show)
    {
        this.gameObject.SetActive(show);
        if (show)
        {
            UpdateTeamUI();
        }
    }

    public void UpdateTeamUI () {

        if (User.Instance.TeamInfo==null)
        {
            return;
        }

        this.teamTitle.text = string.Format("我的队伍{0}/5", User.Instance.TeamInfo.Members.Count);

        for (int i = 0; i < 5; i++)
        {
            if (i<User.Instance.TeamInfo.Members.Count)
            {
                this.Members[i].SetMemberInfo(i,User.Instance.TeamInfo.Members[i],User.Instance.TeamInfo.Members[i].Id==User.Instance.TeamInfo.Leader);
                this.Members[i].gameObject.SetActive(true);
            }
            else
            {
                this.Members[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickLeave()
    {
        MessageBox.Show("确定要离开队伍吗？", "退出队伍", MessageBoxType.Confirm, "确定后离开队伍", "取消").OnYes = () =>
        {
            TeamService.Instance.SendTeamLeaveRequest(User.Instance.TeamInfo.Id);
        };
    }
}
