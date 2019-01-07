/*************************************
			
		NetMsgId 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetMessageId
{
    //同步头盔旋转角度
    AsyncHeadSetRotation,
    //心跳
    HeartBeat,
    //切换场景
    SwitchScene,
    //页码变化
    PageChanged,
    //播放视频
    PlayVideo,
    //快进视频
    SeekVideo,
    //
    MediaPause,

    //播放
    MediaPlay,

    /// <summary>
    /// 黑屏处理
    /// </summary>
    AppPause,

    //退出程序
    QuitApp,

    //切换语言
    SwitchLanguage,

    //视频缓冲
    OnVideoBufferStart,

    //视频缓冲结束
    OnVideoBufferFinish,

    //机顶盒视频Ready
    STBReady,

    //头盔Ready
    HeadSetReady

}
