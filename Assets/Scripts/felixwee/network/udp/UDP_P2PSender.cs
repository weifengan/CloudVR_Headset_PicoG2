/*************************************
			
		UDP_P2PSender 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDP_P2PSender : MonoBehaviour
{

    private int _broadcastPort = 9339;
    private IPEndPoint broadcastEndPoint;
    private Socket broadcastSocket;
    private bool _loop = true;
    private string _targetIp;

    private IPEndPoint _targetIpe;
    /// <summary>
    /// 初始化UDP网络通信服务
    /// </summary>
    public void Init(int broadcastPort = 9339)
    {
        _broadcastPort = broadcastPort;
        try
        {
            broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //设置socket为广播模式
            broadcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            //创建目标IPE
            broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, _broadcastPort);

            Debug.Log("【UDP】启动UDP广播服务器OK--------广播端口:" + _broadcastPort);

            _loop = true;
        }
        catch (Exception ex)
        {
            Debug.Log("【UDP】启动UDP服务器Fail--------广播端口:" + _broadcastPort + " 原因:" + ex.Message);
            _loop = false;
        }


    }

    public void SendNetMessage(NetMessage msg)
    {
        if (_loop)
        {
            broadcastSocket.SendTo(msg.Bytes, broadcastEndPoint);
            //Global.LogInfo("发送头盔同步数据"+msg.Json);
        }
    }


    void OnDestroy()
    {
        _loop = false;
        if (broadcastSocket != null)
        {
            broadcastSocket.Close();
        }
       
        Debug.Log("【UDP】关闭UDP发送器OK--------发送端口:" + _broadcastPort);
    }

}
