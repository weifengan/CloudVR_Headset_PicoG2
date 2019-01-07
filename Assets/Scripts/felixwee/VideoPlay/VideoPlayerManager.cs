/*************************************
			
		VideoPlayerManager 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using Pvr_UnitySDKAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum VideoPlayScreenType
{
    //普通,3D左右,3D上下
    Basic, LeftRight3D, UpDown3D
}
public class VideoPlayerManager : MonoBehaviour
{

    private MediaPlayerCtrl m_player;
    private VideoItemConfig videoData;

    public VideoPlayerProgressBar m_ProgressBar;
    public VideoPlayerUIController m_playUI;

    private float UIShowTime = 3;
    private float curUIShowTime = 0;

    public VideoPlayScreenType m_PlayMode = VideoPlayScreenType.Basic;


    private Camera m_MainCamera;
    //音量是否可用
    private bool VolEnable = false;
    // Use this for initialization
    void Start()
    {
        m_player = this.GetComponent<MediaPlayerCtrl>();
        m_playUI = this.transform.Find("MediaPlayerUI").GetComponent<VideoPlayerUIController>();

        if (Global.Instance != null)
        {
            videoData = Global.Instance.currentVideoData;
            m_MainCamera = Global.Instance.m_MainCamera;
        }
        else
        {

            m_MainCamera = GameObject.Find("Head").GetComponent<Camera>();

            Canvas playeruicanvas = m_playUI.GetComponent<Canvas>();
            if (playeruicanvas.worldCamera == null && m_MainCamera!=null)
            {
                playeruicanvas.worldCamera = m_MainCamera;
            }

            #region DEBUG 单独场景模拟
            videoData = new VideoItemConfig();
            videoData.TitleEn = "test";
            videoData.TitleZh = "测试";

            //1803DLR
            videoData.VideoUrl = videoData.StbUrl = "http://192.168.10.126/LiveVideo/Hot/4K-SpringFestivalGalaSilkRoad_1803DLR_54/4K-SpringFestivalGalaSilkRoad_1803DLR_54.m3u8";
            videoData.ScreenType = "1803DLR";

            //3602D
            videoData.VideoUrl = videoData.StbUrl = "http://192.168.10.126/Education/Hot/UnderwaterCoral_3602D_85/UnderwaterCoral_3602D_85.m3u8";
            videoData.ScreenType = "3602D";
            //videoData.VideoUrl = videoData.StbUrl = "http://192.168.10.126/FunnyVideo/Hot/4K-VRBoyFirend2_1803DLR_99/4K-VRBoyFirend2_1803DLR_99.m3u8";

            //Normal2D
            videoData.VideoUrl = videoData.StbUrl = "http://192.168.10.126/BigScreen/Hot/ChineseOperaMask_Normal2D_98/ChineseOperaMask_Normal2D_98.m3u8";
            videoData.ScreenType = "Normal2D";

            //Normal3DLR
            videoData.VideoUrl = videoData.StbUrl = "http://192.168.10.126/BigScreen/Hot/girlsgeneration/girlsgeneration.mp4";
            videoData.ScreenType = "Normal3DLR";

            videoData.VideoUrl = videoData.StbUrl = "http://192.168.10.126/FunnyVideo/Other/PavilionCup_3603DUD_11/PavilionCup_3603DUD_11.m3u8";
            videoData.ScreenType = "Normal3DUD";

            #endregion
        }
        curUIShowTime = 0;
       
        m_player.OnReady += OnVideoReady;
        m_player.OnVideoFirstFrameReady += OnVideoFirstFrameReady;
        m_player.OnVideoError += OnVideoError;
        m_player.OnEnd += OnVideoEnd;
        m_player.m_bLoop = true;

       

        //设置文件信息
        string title = LanguageManager.Instance.CurLanguage == "English" ? videoData.TitleEn : videoData.TitleZh;
        m_playUI.SetVideoTitle(title);
        m_playUI.SetPlayControlButtonStatus(true);
        m_playUI.SetVideoCurrentTime(0);


        m_playUI.OnActionHandler += OnPlayerUIActionHandler;
        InitPlayer();
    }

 
     /// <summary>
    /// 用户点击进度条，快进视频
    /// </summary>
    /// <param name="obj"></param>
    private void OnSeekProgressHandler(int targetTime)
    {
        ///同步视频进度
        if (NetworkManager.Instance.TCPConnected)
        {
            NetMessage nm = new NetMessage(NetMessageId.SeekVideo);
            SeekVideoVo svv = new SeekVideoVo();
            svv.targetTime = targetTime;
            nm.WriteJson(JsonConvert.SerializeObject(svv));

            NetworkManager.Instance.SendNetMessage(nm);
        }
        m_player.SeekTo(targetTime);
    }

    /// <summary>
    /// 初始化播放器，并开始加载视频
    /// </summary>
    /// <param name="url"></param>
    protected virtual void InitPlayer()
    {
        switch (videoData.ScreenType)
        {
            case "Normal2D":  //普通格式
            case "3602D":
            case "1802D":
                m_PlayMode = VideoPlayScreenType.Basic;
                break;
            case "1803DLR":   // 3D 左右
            case "Normal3DLR":
            case "3603DLR":
                m_PlayMode = VideoPlayScreenType.LeftRight3D;
                break;
            case "Normal3DUD":  //3D上下格式
            case "3603DUD":
            case "1803DUD":
                m_PlayMode = VideoPlayScreenType.UpDown3D;
                break;
            default:
                m_PlayMode = VideoPlayScreenType.Basic;
                break;
        }

        switch (m_PlayMode)
        {
            case VideoPlayScreenType.Basic:
                SetPlayMode2D();
                break;
            case VideoPlayScreenType.LeftRight3D:
                SetPlayMode3DLR();
                break;
            case VideoPlayScreenType.UpDown3D:
                SetPlayMode3DUD();
                break;
        }
        m_player.Load(videoData.StbUrl);
    }

    public  void OnPlayerUIActionHandler(VideoPlayerUIAction act)
    {
        Global.LogInfo("播放器面板action=" + act);
        switch (act)
        {
            case VideoPlayerUIAction.Pause:

                m_player.Pause();
                m_playUI.SetPlayControlButtonStatus(false);
                //发送Pause消息到机顶盒
                if (NetworkManager.Instance.TCPConnected)
                {
                    NetMessage nm = new NetMessage(NetMessageId.MediaPause);
                    NetworkManager.Instance.SendNetMessage(nm);
                }

                break;
            case VideoPlayerUIAction.Play:
                    m_playUI.SetPlayControlButtonStatus(true);
                    m_player.Play();
                    //发送Play消息到机顶盒
                    if (NetworkManager.Instance.TCPConnected)
                    {
                        NetMessage nm = new NetMessage(NetMessageId.MediaPlay);
                        NetworkManager.Instance.SendNetMessage(nm);
                    }
                break;
            case VideoPlayerUIAction.Exit:
                Global.LogInfo("===============场景" + Global.Instance.PreviousSceneName);
                m_player.Stop();
                Global.Instance.SwitchScene(Global.Instance.PreviousSceneName);
                break;
        }
    }
    public void SetPlayMode3DLR()
    {
        if (m_MainCamera != null)
        {
            Camera leftCamera = m_MainCamera.transform.Find("LeftEye").GetComponent<Camera>();
            leftCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenRight"));
            Camera RightCamera = m_MainCamera.transform.Find("RightEye").GetComponent<Camera>();
            RightCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenLeft"));
        }
    }

    public void SetPlayMode3DUD()
    {
        if (m_MainCamera != null)
        {
            Camera leftCamera = m_MainCamera.transform.Find("LeftEye").GetComponent<Camera>();
            leftCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenDown"));
            Camera RightCamera = m_MainCamera.transform.Find("RightEye").GetComponent<Camera>();
            RightCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenTop"));
        }
    }

    public void SetPlayMode2D()
    {
        if (m_MainCamera != null)
        {
            Camera leftCamera = m_MainCamera.transform.Find("LeftEye").GetComponent<Camera>();
            leftCamera.cullingMask = -1;
            Camera RightCamera = m_MainCamera.transform.Find("RightEye").GetComponent<Camera>();
            RightCamera.cullingMask = -1;
        }
    }

    void OnVideoReady()
    {
        Debug.Log("OnReady");

    }

    void OnVideoFirstFrameReady()
    {
        Debug.Log("OnFirstFrameReady");

        videoData.Width = m_player.GetVideoWidth().ToString();
        videoData.Height = m_player.GetVideoHeight().ToString();
        videoData.Duration = m_player.GetDuration().ToString();

        Global.Instance.currentVideoData.Width = m_player.GetVideoWidth().ToString();
        Global.Instance.currentVideoData.Height = m_player.GetVideoHeight().ToString();
        Global.Instance.currentVideoData.Duration = SecondsToHMS(m_player.GetDuration()/1000);

        //想管理后台同步视频WH
        AsyncCVRAdminManager.Instance.AsyncVideoInfo(Global.Instance.currentVideoData.Subject, Global.Instance.currentVideoData);


        m_playUI.SetVideoCurrentTime(0);
        m_playUI.SetVideoTotalTime(m_player.GetDuration());

        BufferFinishState bfs = BufferFinishState.None;
        switch (m_player.GetCurrentState())
        {
            case MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING:
                bfs = BufferFinishState.Play;
                break;
            case MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED:
                bfs = BufferFinishState.Pause;
                break;
        }

        ///向STB同步缓冲结束状态
        if (NetworkManager.Instance.TCPConnected)
        {
            NetMessage nm = new NetMessage(NetMessageId.OnVideoBufferFinish);
            VideoBufferVo vbv = new VideoBufferVo();
            vbv.targetTime = m_player.GetSeekPosition();
            vbv.playerstate = bfs;
            nm.WriteJson(JsonConvert.SerializeObject(vbv));
            NetworkManager.Instance.SendNetMessage(nm);
        }
 
    }

    void OnVideoEnd()
    {
        Debug.Log("OnEnd");
    }

    void OnVideoResize()
    {
        Debug.Log("OnResize");
    }

    void OnVideoError(MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCode, MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCodeExtra)
    {
        Debug.Log("OnError" + errorCode + "  " + errorCodeExtra);
    }



    // Update is called once per frame
    void Update()
    {

 
        //**********播放器UI显示与隐藏***********
        if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(0, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(0, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetMouseButtonUp(0))
        {
            curUIShowTime = UIShowTime;
            m_playUI.gameObject.SetActive(true);
        }

        curUIShowTime -= Time.deltaTime;
        if (curUIShowTime < 0)
        {
            m_playUI.gameObject.SetActive(false);
            curUIShowTime = -10;
        }

        

    }

    public string SecondsToHMS(int seconds)
    {
        TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
        int hour = ts.Hours;
        int minute = ts.Minutes;
        int second = ts.Seconds;

        return string.Format("{0:#00}:{1:#00}:{2:#00}", hour, minute, second);
    }
}
