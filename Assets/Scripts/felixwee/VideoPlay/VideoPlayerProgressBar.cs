/*************************************
			
		VideoPlayerProgressBar 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoPlayerProgressBar : MonoBehaviour
{

    public Text txtCurrentTime;
    public Text txtTotalTime;
    public Slider progressBar;
    private VideoPlayerUIController mPlayerUI;
    private long TotalTime;

 


    /// <summary>
    /// 是否因UI操作改变值
    /// </summary>
    bool IsVoluntary;
    // Use this for initialization
    void Start () {
        if (mPlayerUI == null)
        {
            mPlayerUI = this.transform.GetComponentInParent<VideoPlayerUIController>();
        }
 
        IsVoluntary = false;
        progressBar.onValueChanged.AddListener(onValueChanged);
    }

    private void onValueChanged(float f)
    {
        if (IsVoluntary)
        {
            IsVoluntary = false;
            return;
        }
        long times = (long)(f * TotalTime);
        int seconds = (int)(times / 1000);
        txtCurrentTime.text = SecondsToHMS(seconds);

        ///同步视频进度
        if (NetworkManager.Instance.TCPConnected)
        {
            NetMessage nm = new NetMessage(NetMessageId.SeekVideo);
            SeekVideoVo svv = new SeekVideoVo();
            svv.targetTime = times;
            nm.WriteJson(JsonConvert.SerializeObject(svv));

            NetworkManager.Instance.SendNetMessage(nm);
        }

        BufferFinishState bfs = BufferFinishState.None;
        switch (mPlayerUI.m_player.GetCurrentState())
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
            vbv.targetTime = times;
            vbv.playerstate = bfs;
            nm.WriteJson(JsonConvert.SerializeObject(vbv));
            NetworkManager.Instance.SendNetMessage(nm);
        }




        mPlayerUI.m_player.SeekTo((int)times);

    }

    public void SetTotalTime(long totalTime)
    {
        TotalTime = totalTime;

        int seconds = (int)(totalTime / 1000);
        txtTotalTime.text = SecondsToHMS(seconds);
    }



    public void SetCurrentTime(long currentTime)
    {
        int seconds = (int)(currentTime / 1000);
        txtCurrentTime.text = SecondsToHMS(seconds);

        float t = 0;
        if (TotalTime != 0)
            t = (float)currentTime / TotalTime;
        IsVoluntary = true;
        progressBar.value = t;
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
