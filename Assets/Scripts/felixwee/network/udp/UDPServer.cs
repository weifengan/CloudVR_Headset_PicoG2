/*************************************
			
		UDPServer 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPServer : MonoBehaviour
{

    //用于的广播Socket
    private Socket _broadcastSocket;
    private Socket _receiveSocket;

    private IPEndPoint _broadcastIEP;
    private IPEndPoint _receiveIPE;

    //广播端口
    private int _broadcastPort = 9339;

    //接收变量
    private bool _receiveLoop = true;

    private Thread _receiveThread = null;

    private byte[] _receiveBuffer = new byte[4096];

    private static UDPServer _instance;


    public static UDPServer Instance
    {
        get
        {
            return _instance;
        }
    }

         

    private Queue<NetMessage> msgList = new Queue<NetMessage>();

    public void Start()
    {
        _instance = this;
    }

    public UDPServer Init(int broadcastPort = 9339)
    {
        this._broadcastPort = broadcastPort;

        try
        {
            _broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _broadcastIEP = new IPEndPoint(IPAddress.Any, _broadcastPort);
            Debug.Log("【UDP】启动UDP广播服务器OK--------广播端口:"+_broadcastPort);
        }
        catch (Exception ex)
        {
            Debug.Log("【UDP】启动UDP服务器Fail--------广播端口:" + _broadcastPort + " 原因:"+ex.Message);
        }
        return _instance;
    }

    private void OnUDPReceiveHandler()
    {
        EndPoint ep = (EndPoint)_receiveIPE;
        while (_receiveLoop)
        {

            try
            {
                int receiveSize = _receiveSocket.ReceiveFrom(_receiveBuffer, ref ep);

                byte[] buffers = new byte[receiveSize];
                Buffer.BlockCopy(_receiveBuffer, 0, buffers, 0, receiveSize);
                using (MemoryStream ms = new MemoryStream(buffers))
                using (BinaryReader br = new BinaryReader(ms))
                {

                    NetMessageId id = (NetMessageId) br.ReadInt32();
                    int len = br.ReadInt32();
                    byte[] msgbytes = br.ReadBytes(len);
                    string json = Encoding.UTF8.GetString(msgbytes);
               
                    MsgManager.Instance.Dispatch(id, json);

                }
                //解析数据
            }catch(Exception ex)
            {
                Debug.Log("UDP Error" + ex.Message);
            }
        }
    }

    public void SendNetMessage(string str)
    {
        byte[] data = Encoding.UTF8.GetBytes(str);
        _broadcastSocket.SendTo(data, _broadcastIEP);
    }

    public void SendNetMessage(NetMessage msg)
    {

        try
        {
            _broadcastSocket.SendTo(msg.Bytes, _broadcastIEP);
            //Debug.Log("发送数据OK");

        }
        catch (Exception ex)
        {
            Debug.Log("发送数据失败" + ex.Message);
        }

    }


    public void Update()
    {
        while (msgList.Count > 0)
        {
            NetMessage msg = msgList.Dequeue();

            //收到消息
            MsgManager.Instance.Dispatch(msg.Id, msg.Json);


        }
    }


    public void OnDestroy()
    {
        ShutDown();
    }


    public void ShutDown()
    {
        _receiveLoop = false;
        if (_broadcastSocket != null)
        {
            _broadcastSocket.Close();
        }
       
        _broadcastSocket = null;
        Debug.Log("【UDP】关闭UDP服务器OK--------广播端口:" + _broadcastPort);
    }


}
