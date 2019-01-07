/*************************************
			
		VideoPlayerUIVolumenBar 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoPlayerUIVolumeBar : MonoBehaviour {

    public Slider VolumeSlider;
    public Text VolumePercentText;

    private bool VolEnable = false;
    private int maxVolNum = 1;
    private bool isShow = false;
    //判断音量是否可用
    public void Awake()
    {
        //显示整数
        VolumeSlider.wholeNumbers = true;
        //初始化Pico音量和电量类
        bool initOK=Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_InitBatteryVolClass();
        Debug.Log("UPvr_InitBatteryVolClass" + initOK);

        VolEnable = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_StartAudioReceiver(this.gameObject.name);
        maxVolNum = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_GetMaxVolumeNumber();
        VolumeSlider.maxValue = maxVolNum;

        Debug.Log("最大音量值为" + maxVolNum);
    }


    public void OnDisable()
    {
        if (VolEnable)
        {
            Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_StopAudioReceiver();
        }
    }

    public void Start()
    {
        VolumeSlider.onValueChanged.AddListener(ValueChanged);
    }
 

    private void ValueChanged(float f)
    {
        SetCurrentVolume(f);
    }

    /// <summary>
    /// Update the volume in the UI.
    /// </summary>
    /// <param name="volume">current volume percentage</param>
    private void SetCurrentVolume(float volume)
    {
        if (volume < 0)
            volume = 0;
        else if (volume > maxVolNum)
            volume = maxVolNum;

        VolumeSlider.value = volume;
        VolumePercentText.text = VolumeSlider.value.ToString();
    }

    public void ShowOrHideUI()
    {
        if (this.transform.localScale.x == 0)
        {
            isShow = true;
            this.transform.localScale = Vector3.one;
            SetCurrentVolume(Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_GetCurrentVolumeNumber());
            Debug.Log("系統音量="+Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_GetCurrentVolumeNumber());
        }
        else
        {
            this.transform.localScale = Vector3.zero;
            isShow = false;
        }
    }


    
    public void Update()
    {
        if (isShow)
        {
            if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(0, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(0, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetMouseButtonUp(0))
            {
                Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_SetVolumeNum((int)(VolumeSlider.value));
            }
        }
    }
 }
