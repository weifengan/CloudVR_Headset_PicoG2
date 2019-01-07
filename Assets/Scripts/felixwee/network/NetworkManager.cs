/*************************************
			
		NetworkManager 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private UDP_P2PSender m_UdpSender;

    private TCPSocketClient m_TcpClient;

    public UDP_P2PSender UDPSender
    {
        get
        {
            return m_UdpSender;
        }
    }

    private static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("_" + typeof(NetworkManager).Name);
                _instance = go.AddComponent<NetworkManager>();

                if (Global.Instance != null)
                {
                    _instance.transform.SetParent(Global.Instance.transform);
                }
            }
            return _instance;
        }
    }

    public void Start()
    {
        _instance = this;
    }




    public bool isUdpOk = false;

    public void Init()
    {
        Global.LogInfo("初始化SOCKET网路管理器" + typeof(NetworkManager).Name);

        m_UdpSender = this.gameObject.AddComponent<UDP_P2PSender>();

        m_UdpSender.Init(ConfigManager.Instance.NetworkCfg.UDPPort);

        ///初始化TCP客户端
        m_TcpClient = this.gameObject.AddComponent<TCPSocketClient>();

        m_TcpClient.Init(ConfigManager.Instance.NetworkCfg.TCPServerIP, ConfigManager.Instance.NetworkCfg.TCPPort);
        //m_TcpClient.Init();

    }


    /// <summary>
    /// TCP是否连接成功
    /// </summary>
    public bool TCPConnected
    {
        get
        {
            if (m_TcpClient != null)
            {
                return m_TcpClient.isOnline;
            }
            return false;
            
        }
    }


    public void SendNetMessage(NetMessage msg)
    {
        m_TcpClient.SendNetMessage(msg);
    }



    public void Update()
    {
         
 
        
    }
}
