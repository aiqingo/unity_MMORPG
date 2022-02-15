using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

public class Login : MonoBehaviour {

	// Use this for initialization
	void Start () {
       //连接服务器
        Network.NetClient.Instance.Init("127.0.0.1",8000);
        Network.NetClient.Instance.Connect();
        /* 
        //创建消息
        SkillBridge.Message.NetMessage msg=new SkillBridge.Message.NetMessage();

        SkillBridge.Message.FirstTestRequest firstTestRequest = new FirstTestRequest();
        firstTestRequest.HelloWorld = "Hello Worid";

        msg.Request.firstRequest = firstTestRequest;
        //发送消息
        Network.NetClient.Instance.SendMessage(msg);*/

        //创建主消息
        SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();
        msg.Request=new SkillBridge.Message.NetMessageRequest();
        //创建自己定义的消息   就是服务器定义的消息
        //msg.Request.firstRequest=new SkillBridge.Message.FirstTestRequest();
        //给自己定义的消息填充数据   
        //msg.Request.firstRequest.HelloWorld = "Hello Worid";
        //发送消息
        Network.NetClient.Instance.SendMessage(msg);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
