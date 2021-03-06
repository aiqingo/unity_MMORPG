using System.Collections;
using System.Collections.Generic;
using Common.Data;
using Managers;
using Models;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public int npcID;
    private SkinnedMeshRenderer renderer;
    private Animator anim;
    private Color orignColor;
    private bool inInteractive = false;
    private NpcDefine npc;

    private NpcQuestStatus questStatus;

    // Use this for initialization 
	void Start ()
    {
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
        orignColor = renderer.sharedMaterial.color;
        npc = NPCManager.Instance.GetNpcDefine(this.npcID);
        NPCManager.Instance.UpdateNpcPosition(this.npcID,this.transform.position);
        this.StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanaged;
    }

    void OnQuestStatusChanaged(Quest quest)
    {
        this.RefreshNpcStatus();
    }

    void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcID);
        UIWorldElementManager.Instance.AddNpcQuestSatus(this.transform,questStatus);
    }

    void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanaged;
        if (UIWorldElementManager.Instance!=null)
        {
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
        }
    }



    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }

            this.Relax();
        }
    }

    // Update is called once per frame
	void Update () {
		
	}

    void Relax()
    {
        anim.SetTrigger("Relax");
    }

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo))>5)
        {
            this.gameObject.transform.forward =
                Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    void OnMouseDown()
    {
        if (Vector3.Distance(this.transform.position,User.Instance.CurrentCharacterObject.transform.position)>2f)
        {
            User.Instance.CurrentCharacterObject.StactNav(this.transform.position);
        }
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlisgh(true);
    }

    private void OnMouseEnter()
    {
        Highlisgh(true);
    }

    private void OnMouseExit()
    {
        Highlisgh(false);
    }

    void Highlisgh(bool highlight)
    {
        if (highlight)
        {
            if (renderer.sharedMaterial.color!=Color.white)
            {
                renderer.sharedMaterial.color = Color.white;
            }
        }
        else
        {
            if (renderer.sharedMaterial.color!=orignColor)
            {
                renderer.sharedMaterial.color = orignColor;
            }
        }
    }
}
