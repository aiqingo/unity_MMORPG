using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

public class RideController : MonoBehaviour
{

    public Transform mountPoint;

    public EntityController rider;
    public Vector3 offset;
    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        this.anim = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.mountPoint == null || this.rider == null)
        {
            return;
        }
        this.rider.SetRidePotision(this.mountPoint.position+this.mountPoint.TransformDirection(this.offset));
	}

    public void SetRider(EntityController rider)
    {
        this.rider = rider;
    }

    public void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move",false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move",true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }
}
