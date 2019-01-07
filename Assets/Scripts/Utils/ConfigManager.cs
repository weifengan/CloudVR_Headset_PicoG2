/*************************************
			
		ConfigManager 
		
   @desction: 核心配置加载工具类
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigManager : MonoBehaviour
{

    public Action onCompleteHandler = null;
    public string HttpServer = "http://127.0.0.1/";
    public string HttpServerIp = "127.0.0.1";
    public int HttpServerPort = 8080;

    public bool SkipAuthorized = false;

    public string DESKey = "letin_vr";

    public LiceneFileModel LicencesCfg;

    public DataBaseConfig DBCfg;

    public NetworkConfig NetworkCfg;

    public VRDeviceType currentDeviceType;

    private static ConfigManager _instance;
    public static ConfigManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("_" + typeof(ConfigManager).Name);
                _instance = go.AddComponent<ConfigManager>();

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
    /// <summary>
    /// 加载本地配置文件
    /// </summary>
    public void LoadSystemConfig()
    {
        Global.Alert("正在加载本地配置config.json......", false);
        string filePath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = "/sdcard/letinstreaming/config.json";
        }
        else
        {
            filePath = Application.dataPath + "/letinstreaming/config.json";
        }

        if (!File.Exists(filePath))
        {
            Global.Alert("本地配置文件config.json不存在，请确保配置文件存在......", true);
            return;
        }

        //读取配置
        HttpServer = File.ReadAllText(filePath);
        string tmps = HttpServer.Replace("http://", "");
        string[] tmpS = tmps.Split(new char[1] { ':' });
        HttpServerIp = tmpS[0].ToLower().Replace("http://", "");
        HttpServerPort = int.Parse(tmpS[1]);

        //暂未添加设备标志
        //HttpServer = HttpServer + "/" + currentDeviceType.ToString().ToLower();

        //检测是否路过授权
        if (SkipAuthorized)
        {
            LoadNetworkConfig();
        }
        else
        {
            CheckLicence();
        }
    }


    #region 加载授权文件
    /// <summary>
    ///  检查授权配置文件
    /// </summary>
    private void CheckLicence()
    {
        Global.Alert("读取授权配置DevicesLicences.letin............", false);
        HttpHelper.Instance.GET(HttpServer + "/DevicesLicences.letin", (long responseCode, string json) =>
        {
            switch (responseCode)
            {
                case 200:
                    try
                    {
                        //解密
                        string config = EncryptHelper.DESDecrypt(json, DESKey);

                        LicencesCfg = JsonConvert.DeserializeObject<LiceneFileModel>(config);

                        if (HasAuthorized)
                        {
                            Global.Alert("已经通过授权认证............", false);
                            LoadNetworkConfig();
                        }
                        else
                        {
                            Global.Alert("非授权设备，无法使用,请联系服务商!", true);
                        }

                    }
                    catch (Exception ex)
                    {
                        Global.Alert("解析授权文件出现异常" + ex.Message, true);
                    }
                    break;
                case 404:
                    Global.Alert("授权文件DevicesLicences.letin丢失或已经被损坏！", true);
                    break;
                case 0:
                    Global.Alert("无法读取授权证书文件授权文件DevicesLicences.letin!", true);
                    break;
                default:
                    Global.Alert("无法读取授权证书文件授权文件DevicesLicences.letin!", true);
                    break;

            }
        });
    }

    #endregion


    #region 加载网路配置文件

    public void LoadNetworkConfig()
    {
        Global.Alert("读取网络配置NetworkConfig.json......", false);
        HttpHelper.Instance.GET(HttpServer + "/NetworkConfig.json", (long responseCode, string json) =>
        {
            switch (responseCode)
            {
                case 200:
                    try
                    {
                        NetworkCfg = JsonConvert.DeserializeObject<NetworkConfig>(json);

                        Global.Alert("初始化网络配置成功", false);
                        //加载内容配置
                        VideoConfigManager.Instance.Init(HttpServer + "/VideoConfig.json", () =>
                        {
                            LoadLanguageConfig();
                        });

                    }
                    catch (Exception ex)
                    {
                        Global.Alert("读取网络配置NetworkConfig.json出错:" + ex.Message, true);
                    }
                    break;
                case 404:
                    Global.Alert("请确保网络配置文件NetworkConfig.json存在!", true);
                    break;
                case 0:
                    Global.Alert("无法读取网络配置文件NetworkConfig.json!", true);
                    break;
                
            }
        });
    }

    #endregion

    
    #region 加载多语言支持
    public void LoadLanguageConfig()
    {
        Global.Alert("读取多语言配置Languages.csv.....", false);
        HttpHelper.Instance.GET(HttpServer + "/Languages.csv", (long responseCode, string json) =>
        {
            if (responseCode != 200)
            {
                Global.Alert("读取多语言配置Languages.csv失败.....", true);
                return;
            }
                try
                {
                    Global.Alert("读取多语言配置Languages.csvOK.....", false);
                    LanguageManager.Instance.Init(json, OnInitConfigComplete);
                }
                catch (Exception ex)
                {

                    Global.Alert("读取多语言配置Languages.csv"+ex.Message, true);
                }
 

        });

    }

    #endregion


    #region 检测设备是否授权
    public bool HasAuthorized
    {
        get
        {
            foreach (var item in LicencesCfg.list)
            {
                if (item.cDeviceUID.Equals(SystemInfo.deviceUniqueIdentifier))
                {
                    return true;
                }
            }

            return false;
        }
    }
    #endregion


    #region 所有配置处理完成 TODO : XXXX

    public void OnInitConfigComplete()
    {

        Global.Alert("正在启动Launcher...", false);
        if (onCompleteHandler != null)
        {
            onCompleteHandler();
        }
        
     }

    #endregion


}


//数据库配置
public class DataBaseConfig
{
    public string DBServer;
    public int DBServerPort = 3306;
    public string DBName;
    public string DBUser;
    public string DBPwd;
}

public class NetworkConfig
{
    public string TCPServerIP { set; get; }
    public int TCPPort { set; get; }
    public string UDPServerIP { set; get; }
    public int UDPPort { set; get; }
    public float TCPDelay { set; get; }
    public float UDPDelay { set; get; }
}


/// <summary>
/// 授权相关序列类
/// </summary>
[System.Serializable]
public class SingleDevice
{
    public string cType = "";
    public string cIndex = "";
    public string cTitle = "";
    public string cDeviceUID = "";
    public string cCpu = "";
    public string cMac = "";
}


[System.Serializable]
public class LiceneFileModel
{
    public List<SingleDevice> list = new List<SingleDevice>();
}
