/*************************************
			
		TCPSocketClient 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TCPSocketClient : MonoBehaviour {

    private Socket _socket;
    public bool isConnected = false;
    private Thread initThread;
    private Thread receiveThread;
    private string _host = "127.0.0.1";
    private int _port = 9339;
    private bool _loop = true;
    private byte[] _buffers = new byte[4096];

    private Thread heartBeatThread;
    private bool ReconnectFlag = false;
    private System.Timers.Timer reconnectTimer;

    public Queue<NetMessage> msgList = new Queue<NetMessage>();
    /// <summary>
    /// 初始化TCP客户端
    /// </summary>
    public void Init(string host="127.0.0.1",int port=9339)
    {
        _host = host;
        _port = port;

        //初始化网络
        initThread = new Thread(InitNetwork);
        initThread.IsBackground = true;
        initThread.Start();
    }

 

    /// <summary>
    /// 初始化网络
    /// </summary>
    private void InitNetwork()
    {
        //防止关闭App后线程还在执行
        if (_loop)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Global.LogInfo("【TCP】开始连接服务器", _host, _port);
                _socket.Connect(_host, _port);
                isConnected = true;
                Global.LogInfo("【TCP】成功连接服务器" + _host + ":" + _port);

                ///开启接收Thread
                receiveThread = new Thread(new ThreadStart(ReceiveMsg));
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (SocketException se)
            {
                if (_loop)
                {
                    ReconnectFlag = true;
                    Global.LogInfo("【TCP】连接服务器" + _host + ":" + _port + " 失败,5秒后将自动重试                  原因:" + se.Message);

                    reconnectTimer= new System.Timers.Timer(5000);
                    reconnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnReconnectTimeoutHandler); //到达时间的时候执行事件；   
                    reconnectTimer.AutoReset = false;
                    reconnectTimer.Enabled = true;
                    isConnected = false;
                }
                return;
            }         
        }
    }



    void Update()
    {

        while (msgList.Count > 0)
        {
            NetMessage nm = msgList.Dequeue();
            MsgManager.Instance.Dispatch(nm);
        }
    }


    /// <summary>
    /// 重连服务器
    /// </summary>
    /// <returns></returns>
    private void OnReconnectTimeoutHandler(object source, System.Timers.ElapsedEventArgs e)
    {
        initThread.Interrupt();
        initThread.Abort();
        initThread = new Thread(InitNetwork);
        initThread.IsBackground = true;
        initThread.Start();
    }


    //是否与服务器连线
    public bool isOnline
    {
        get
        {
            return isConnected;
        }
    }


    

    private void ReceiveMsg()
    {
        while (_loop)
        {
            try
            {
                int length = _socket.Receive(_buffers);

                byte[] buffers = new byte[length];
                Buffer.BlockCopy(_buffers, 0, buffers, 0, length);
                using (MemoryStream ms = new MemoryStream(buffers))
                using (BinaryReader br = new BinaryReader(ms))
                {
                    NetMessageId id = (NetMessageId)br.ReadInt32();
                    int len = br.ReadInt32();
                    byte[] msgbytes = br.ReadBytes(len);

                    if (id == NetMessageId.HeartBeat)
                    {
                        
                        NetMessage msg = new NetMessage(NetMessageId.HeartBeat);
                        SendNetMessage(msg);
                    }

                    NetMessage tmpMsg = new NetMessage(id);
                    tmpMsg.WriteBytes(msgbytes);
                    msgList.Enqueue(tmpMsg);
                }
            }
            catch (SocketException ex)
            {
                Global.LogError("Socket异常:" + ex.Message);
                isConnected = false;
                Shutdown();
            }
            catch(Exception ex)
            {
                Global.LogError("Exception 异常 " + ex.Message);
                isConnected = false;
                Shutdown();
            }
        }
    }


    private void Shutdown()
    {
        _loop = false;
        if(receiveThread!=null && receiveThread.IsAlive)
        {
            receiveThread.Interrupt();
            receiveThread.Abort();
            receiveThread = null;
            Global.LogInfo("销毁TCP接收Thread");
            isConnected = false;
        }

        if (initThread.IsAlive)
        {
            initThread.Interrupt();
            initThread.Abort();
            initThread = null;
            Debug.Log("Destory initThread");
        }

        if (_socket.Connected)
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket = null;
            Debug.Log("Socket is closed");
        }
   }

    /// <summary>
    /// 向服务器发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void SendNetMessage(NetMessage msg)
    {
        if (isConnected) {
            _socket.Send(msg.Bytes);
            //过滤掉心跳消息回复打印
            Global.LogInfo("发送消息" + msg.Id + "  " + msg.Json);
        }
    }


    public void SendNetMessage(byte[] bytes)
    {
        if (isConnected)
        {
            _socket.Send(bytes);
            Global.LogInfo("发送消息");
        }
    }

 


    void OnDestroy()
    {
        //如果重连Timer启用，停止时，停止Timer
        if (reconnectTimer != null)
        {
            reconnectTimer.Close();
        }
        StopAllCoroutines();
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Interrupt();
            receiveThread.Abort();
            receiveThread = null;
            Global.LogInfo("销毁TCP接收Thread");
            isConnected = false;
        }

        if (initThread.IsAlive)
        {
            initThread.Interrupt();
            initThread.Abort();
            initThread = null;
            Debug.Log("Destory initThread");
        }

        if (_socket.Connected)
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket = null;
            Debug.Log("Socket is closed");
        }
    }
    

}
