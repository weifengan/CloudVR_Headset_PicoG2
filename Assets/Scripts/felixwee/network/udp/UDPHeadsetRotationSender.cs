/*************************************
			
		UDPHeadsetRotationSender 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPHeadsetRotationSender : MonoBehaviour
{


    public Quaternion lastRotation;

    // Use this for initialization
    void Start()
    {
        lastRotation = this.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!checkSame(lastRotation, this.transform.localRotation))
        {
            AsyncRotationVo aq = new AsyncRotationVo();
            aq.x = this.transform.localRotation.x;
            aq.y = this.transform.localRotation.y;
            aq.z = this.transform.localRotation.z;
            aq.w = this.transform.localRotation.w;
            NetMessage msg = new NetMessage(NetMessageId.AsyncHeadSetRotation);
            msg.WriteJson(JsonConvert.SerializeObject(aq));
       

            if (Global.Instance.broadcastHeadRotation)
            {
                if (NetworkManager.Instance.UDPSender != null)
                {
                    NetworkManager.Instance.UDPSender.SendNetMessage(msg);
                    lastRotation = this.transform.localRotation;
                }
            }
           
        }
     }

    public bool checkSame(Quaternion q1, Quaternion q2)
    {
        if (q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w)
        {
            return true;
        }
        return false;
    }
}





