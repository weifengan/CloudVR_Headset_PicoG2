/*************************************
			
		Global 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum VRDeviceType{
    //创维S800
    SkyworthS8K,PicoG2
}

public class Global : MonoBehaviour
{
    [Header("发布版本号")]
    public string MajorVersion = "2";
    public string Minor_Version = "0";
    public string Revision_Version = "0";
    public VRDeviceType currentDeviceType = VRDeviceType.SkyworthS8K;
    [Space(15)]
    [Header("打开后可查看Debug日志")]
    public bool isDebugMode = true;

    [Space(15)]
    [Header("全局存储数据")]
    public bool SkipAuthorized = true;
    /// <summary>
    /// 是否广播头盔陀螺仪数据
    /// </summary>
    public bool broadcastHeadRotation = true;
    public string DESKey = "letin_vr";
    public Camera m_MainCamera;
    public UIAlert m_Alert;

    ///上一个场景名称
    public string PreviousSceneName = "";
    /// <summary>
    /// 最后一次页码数，用于播放视频时返回之前页面
    /// </summary>
    public int LastPageIndex = 0;
    public int CurrentPageIndex = 0;
    /// <summary>
    /// 当前所选择主题类型
    /// </summary>
    public ContentSubjectType currentContentSubjectType;
    /// <summary>
    /// 当前所点选视频信息
    /// </summary>
    public VideoItemConfig currentVideoData;

    private static Global _instance = null;

    
    public static Global Instance
    {
        get { return _instance; }
    }
    // Use this for initialization
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
    }


    public void Init()
    {
        if (m_Alert == null)
        {
            m_Alert = this.transform.GetComponentInChildren<UIAlert>();
        }

        ConfigManager.Instance.onCompleteHandler += OnConfigComplate;
        ConfigManager.Instance.DESKey = DESKey;
        ConfigManager.Instance.SkipAuthorized = SkipAuthorized;
        ConfigManager.Instance.currentDeviceType = currentDeviceType;
        //开始加载配置
        ConfigManager.Instance.LoadSystemConfig();
    }
    

    /// <summary>
    /// 加载配置成功后，程序主入口
    /// </summary>
    private void OnConfigComplate()
    {
      
        //隐藏提示
        HideAllAlert();

        
        Global.LogInfo("所有配置处理通过，正在启动Launcher......");
        //初始化网络模块
        NetworkManager.Instance.Init();

        AsyncCVRAdminManager.Instance.Init(ConfigManager.Instance.HttpServer);

        SwitchScene(SceneList.Launcher);
    }

    public void QuitApp()
    {
        if (NetworkManager.Instance.TCPConnected)
        {
            NetMessage nm = new NetMessage(NetMessageId.QuitApp);
            NetworkManager.Instance.SendNetMessage(nm);
        }
        Debug.Log("退出APP");
        Application.Quit();
    }
    public void SwitchScene(string newSceneName, params object[] args)
    {
       
        if (NetworkManager.Instance.TCPConnected)
        {
            //切换场景消息
            NetMessage nm = new NetMessage(NetMessageId.SwitchScene);
            SwitchSceneVo ssv = new SwitchSceneVo();
            ssv.previousSceneName = PreviousSceneName;
            ssv.newSceneName = newSceneName;
            ssv.currentContentType = Global.Instance.currentContentSubjectType;
            ssv.currentPageIndex = Global.Instance.CurrentPageIndex;
            ssv.lastPageIndex = Global.Instance.LastPageIndex;
            nm.WriteJson(JsonConvert.SerializeObject(ssv));
            NetworkManager.Instance.SendNetMessage(nm);
        }
        SceneManager.LoadScene(newSceneName);
    }


    public void SwitchSceneForceToMenu() {

        if (NetworkManager.Instance.TCPConnected)
        {
            //切换场景消息
            NetMessage nm = new NetMessage(NetMessageId.SwitchScene);
            SwitchSceneVo ssv = new SwitchSceneVo();
            ssv.previousSceneName = PreviousSceneName;
            ssv.newSceneName = SceneList.VideoContentMenu;
            ssv.currentContentType = Global.Instance.currentContentSubjectType;
            ssv.currentPageIndex = Global.Instance.CurrentPageIndex;
            ssv.lastPageIndex = Global.Instance.LastPageIndex;
            nm.WriteJson(JsonConvert.SerializeObject(ssv));
            NetworkManager.Instance.SendNetMessage(nm);
        }
        //想管理后台同步视频退出
        AsyncCVRAdminManager.Instance.AsyncQuitVideo();
        Global.Instance.PreviousSceneName = SceneList.VideoContentMenu;
        SceneManager.LoadScene(SceneList.VideoContentMenu);
    }


    public void Switch2VideoPlayScene()
    {

        PreviousSceneName = SceneManager.GetActiveScene().name;

        Global.LogInfo("开始进入视频播放场景" + Global.Instance.currentVideoData.VideoUrl);

        string gotoScene = SceneList.None;
        switch (currentVideoData.ScreenType)
        {
            case "Normal2D":
                gotoScene = SceneList.VideoPlayNormal2D;
                break;
            case "3602D":
                gotoScene = SceneList.VideoPlay3602D;
                break;
            case "1802D":
                gotoScene = SceneList.VideoPlay1802D;
                break;
            ///3D视频 左右格式  
            case "Normal3DLR":
                gotoScene = SceneList.VideoPlayNormal3DLR;
                break;
            case "1803DLR":
                gotoScene = SceneList.VideoPlay1803DLR;
                break;
            case "3603DLR":
                gotoScene = SceneList.VideoPlay3603DLR;
                break;
            //3D视频 上下格式
            case "Normal3DUD":
                gotoScene = SceneList.VideoPlayNormal3DUD;
                break;
            case "1803DUD":
                gotoScene = SceneList.VideoPlay1803DUD;
                break;
            case "3603DUD":
                gotoScene = SceneList.VideoPlay3603DUD;
                break;
            default:
                gotoScene = SceneList.VideoPlayNormal2D;
                break;  
           
        }

        if (NetworkManager.Instance.TCPConnected)
        {
            //创建播放消息
            NetMessage nm = new NetMessage(NetMessageId.PlayVideo);
            PlayVideoVo pvv = new PlayVideoVo();
            pvv.previousSceneName = PreviousSceneName;
            pvv.lastPageIndex = Global.Instance.LastPageIndex;
            pvv.videoData = Global.Instance.currentVideoData;
            nm.WriteJson(JsonConvert.SerializeObject(pvv));
            NetworkManager.Instance.SendNetMessage(nm);
        }
        //向管理后台同步视频信息
        AsyncCVRAdminManager.Instance.AsyncVideoInfo(Global.Instance.currentContentSubjectType, Global.Instance.currentVideoData);
        SceneManager.LoadScene(gotoScene);
    }

    //应用程序暂停
    private void OnApplicationPause(bool pause)
    {
        broadcastHeadRotation = !pause;
        //获取当前场景是否为视频播放场景,对视频播放进行处理
        if (SceneManager.GetActiveScene().name.Contains("VideoPlay_"))
        {
            VideoPlayerManager vpm = GameObject.FindObjectOfType<VideoPlayerManager>();
            if (vpm != null)
            {
                if (pause)
                {
                    vpm.OnPlayerUIActionHandler(VideoPlayerUIAction.Pause);
                }
            }
        }
        string curScene = SceneManager.GetActiveScene().name;

        //矫正机顶盒视角
        if (pause)
        {
            //向机顶盒发送暂停指令
            if (NetworkManager.Instance.TCPConnected)
            {
                NetMessage nm = new NetMessage(NetMessageId.AppPause);
                NetworkManager.Instance.SendNetMessage(nm);
            }
        }
    }



    #region 消息提示框
        //*****************消息框相关****************/

    public static void Alert(string msg, bool quit = false)
    {
        Global.LogInfo(msg);
        _instance.m_Alert.Show(msg, quit);
    }


    public static void HideAllAlert()
    {
        _instance.m_Alert.HideAlert();
    }

    #endregion


    #region Debug日志输入函数
    /// <summary>
    /// 打印系统日志
    /// </summary>
    /// <param name="args"></param>
    public static void LogInfo(params object[] args)
    {
        if (Global.Instance != null && !Global.Instance.isDebugMode) return;
        StringBuilder sb = new StringBuilder();
        foreach (var item in args)
        {
            sb.Append(item.ToString() + " ");
        }
        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// 打印系统日志
    /// </summary>
    /// <param name="args"></param>
    public static void LogWarning(params object[] args)
    {
        if (!Global.Instance.isDebugMode) return;
        StringBuilder sb = new StringBuilder();
        foreach (var item in args)
        {
            sb.Append("  " + item.ToString());
        }
        Debug.LogWarning(sb.ToString());
    }

    /// <summary>
    /// 打印系统日志
    /// </summary>
    /// <param name="args"></param>
    public static void LogError(params object[] args)
    {
        if (!Global.Instance.isDebugMode) return;
        StringBuilder sb = new StringBuilder();
        foreach (var item in args)
        {
            sb.Append("  " + item.ToString());
        }
        Debug.LogError(sb.ToString());
    }

    #endregion


    // Update is called once per frame
    void Update()
    {
        if (m_MainCamera == null)
        {
            Global.LogWarning("Global.m_MainCamera未指定!,将自动获取主摄像机");
            m_MainCamera = GameObject.Find("Head").GetComponent<Camera>();
        }

        if (m_MainCamera == null) return;

        Canvas[] cnvs = GameObject.FindObjectsOfType<Canvas>();

        foreach (var item in cnvs)
        {
            if (item.worldCamera == null)
            {
                item.worldCamera = m_MainCamera;
            }
        }
    }
}
