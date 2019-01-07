/*************************************
			
		VideoPlayerUIController 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum VideoPlayerUIAction
{
    //播放，暂停，下一个，前一个，退出
    Play,Pause,Next,Prev,Exit
}


public class VideoPlayerUIController : MonoBehaviour {

    public Text txtTitle;
    public Button btnPlay;
    public Button btnPause;
    public Button btnNext;
    public Button btnPrev;
    public Button btnExit;
    public Button btnVolume;

    public VideoPlayerUIVolumeBar m_VolumeSlider;
    public VideoPlayerProgressBar m_ProgressSlider;
    [Header("MediaPlayerCtrl对象，未指定时自动从父级获取")]
    public MediaPlayerCtrl m_player;


    public Action<VideoPlayerUIAction> OnActionHandler;


    // Use this for initialization
    void Start() {

        if (m_player == null)
        {
            m_player = GameObject.FindObjectOfType<MediaPlayerCtrl>();
        }
        btnPlay.onClick.AddListener(ClickPlayBtn);
        btnPause.onClick.AddListener(ClickPauseBtn);
        btnVolume.onClick.AddListener(ClickVolumeBtn);
        btnExit.onClick.AddListener(ClickExitBtn);
        SetPlayControlButtonStatus(true);
        
    }

    private void ClickExitBtn()
    {
        if (OnActionHandler!=null)
        {
            OnActionHandler(VideoPlayerUIAction.Exit);
        }
    }

    // Update is called once per frame
    void Update() {

        if(m_player.GetCurrentState()== MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        {
            SetVideoCurrentTime(m_player.GetSeekPosition());
        }

    }

    public void ClickVolumeBtn()
    {
        m_VolumeSlider.ShowOrHideUI();
    }
    public void ClickPauseBtn()
    {
        SetPlayControlButtonStatus(false);
        if (OnActionHandler != null)
        {
            OnActionHandler(VideoPlayerUIAction.Pause);
        }
    }

    public void ClickPlayBtn()
    {
        SetPlayControlButtonStatus(true);
        if (OnActionHandler != null)
        {
            OnActionHandler(VideoPlayerUIAction.Play);
        }

    }
    public void SetVideoTitle(string title)
    {
        txtTitle.text = title;
    }

    public void SetPlayControlButtonStatus(bool isPlay)
    {
        if (isPlay)
        {
            btnPlay.gameObject.SetActive(false);
            btnPause.gameObject.SetActive(true);
        }
        else
        {
            btnPlay.gameObject.SetActive(true);
            btnPause.gameObject.SetActive(false);
        }
    }

    public void SetVideoCurrentTime(int time)
    {
        m_ProgressSlider.SetCurrentTime(time);
    }
    public void SetVideoTotalTime(int time)
    {
        m_ProgressSlider.SetTotalTime(time);
    }
}
