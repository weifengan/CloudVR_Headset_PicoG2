/*************************************
			
		NetMessageVo 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//切换场景时的VO对象 
public struct SwitchSceneVo
{
    public string previousSceneName;
    public string newSceneName;
    public ContentSubjectType currentContentType;
    public int lastPageIndex;
    public int currentPageIndex;
}

/// <summary>
/// 页码改变Vo
/// </summary>
 public struct PageChangeVo
{
    public ContentSubjectType currentContentType;
    //翻页方向
    public PageDir PageChangDir;

    public int currentPageIndex;

    public int lastPageIndex;
}

/// <summary>
/// 播放视频vo
/// </summary>
public struct PlayVideoVo
{
    public string previousSceneName;
    public int lastPageIndex;
    public VideoItemConfig videoData;
    
}

//搜寻视频
public struct SeekVideoVo
{ 
    public long targetTime;
}


public struct SwitchLanguageVo
{
    public string currentLanguage;
}


public enum BufferFinishState
{
    None,Pause,Play
}
public struct VideoBufferVo
{
    public BufferFinishState playerstate;
    //缓冲完成后，开始播放时间点
    public long targetTime;
}
