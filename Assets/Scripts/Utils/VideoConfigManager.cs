/*************************************

       VideoConfigManager 

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

public class VideoConfigManager
{

    private Dictionary<string, List<VideoItemConfig>> mDic = new Dictionary<string, List<VideoItemConfig>>();
    private static VideoConfigManager _instance;

    private Dictionary<string, string> ScreenTypeDic_Zh = new Dictionary<string, string>();
    private Dictionary<string, string> ScreenTypeDic_En = new Dictionary<string, string>();

    public VideoConfig m_config;
    public static VideoConfigManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new VideoConfigManager();
            }
            return _instance;
        }
    }

    public void Init(string url, Action complete = null)
    {
        //添加视频类型对应关系
        ScreenTypeDic_Zh.Add("Normal2D", "巨幕2D");
        ScreenTypeDic_Zh.Add("Normal3DLR", "巨幕3D");
        ScreenTypeDic_Zh.Add("Normal3DUD", "巨幕3D");
        ScreenTypeDic_Zh.Add("1802D", "180°2D");
        ScreenTypeDic_Zh.Add("1803DLR", "180°3D");
        ScreenTypeDic_Zh.Add("1803DUD", "180°3D");
        ScreenTypeDic_Zh.Add("3602D", "360°2D");
        ScreenTypeDic_Zh.Add("3603DUD", "360°3D");
        ScreenTypeDic_Zh.Add("3603DLR", "360°3D");

        //英文对应关系
        ScreenTypeDic_En.Add("Normal2D", "BigScreen 2D");
        ScreenTypeDic_En.Add("Normal3DLR", "BigScreen 3D");
        ScreenTypeDic_En.Add("Normal3DUD", "BigScreen 3D");
        ScreenTypeDic_En.Add("1802D", "180°2D");
        ScreenTypeDic_En.Add("1803DLR", "180°3D");
        ScreenTypeDic_En.Add("1803DUD", "180°3D");
        ScreenTypeDic_En.Add("3602D", "360°2D");
        ScreenTypeDic_En.Add("3603DUD", "360°3D");
        ScreenTypeDic_En.Add("3603DLR", "360°3D");
        Global.Alert("获取内容列表......", false);

        HttpHelper.Instance.GET(url, (long responseCode, string content) => {

            switch (responseCode)
            {
                case 200:
                    try
                    {
                        //进行转化
                        m_config = JsonConvert.DeserializeObject<VideoConfig>(content);
                        mDic[ContentSubjectType.LivingVideo.ToString()] = m_config.LivingVideo;
                        mDic[ContentSubjectType.FunnyVideo.ToString()] = m_config.FunnyVideo;
                        mDic[ContentSubjectType.Education.ToString()] = m_config.Education;
                        mDic[ContentSubjectType.BigScreen.ToString()] = m_config.BigScreen;
                        mDic[ContentSubjectType.Picture.ToString()] = m_config.Picture;

                        Global.Alert("解析视频内容列表OK", false);
                        if (complete != null)
                        {
                            complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Alert("解析视频内容出错，请确保VideoConfig.json配置正确！" + ex.Message, true);
                    }
                    break;
                case 404:
                    Global.Alert("请确保视频内容配置VideoConfig.json存在!", true);
                    break;
                case 0:
                    Global.Alert("无法读取视频内容配置VideoConfig.json!", true);
                    break;
            }
        });
    }

    /// <summary>
    /// 通过主题获取内容
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<VideoItemConfig> GetVideoListBySubejct(ContentSubjectType type)
    {
        if (mDic.ContainsKey(type.ToString()))
        {
            return mDic[type.ToString()];
        }
        return new List<VideoItemConfig>();
    }


    /// <summary>
    /// 获取Launcher界面图
    /// </summary>
    /// <returns></returns>
    public List<VideoItemConfig> GetLauncherList()
    {
        List<VideoItemConfig> list = new List<VideoItemConfig>();
        mDic[ContentSubjectType.LivingVideo.ToString()][0].Subject = ContentSubjectType.LivingVideo;
        list.Add(mDic[ContentSubjectType.LivingVideo.ToString()][0]);
        mDic[ContentSubjectType.FunnyVideo.ToString()][0].Subject = ContentSubjectType.FunnyVideo;
        list.Add(mDic[ContentSubjectType.FunnyVideo.ToString()][0]);
        mDic[ContentSubjectType.BigScreen.ToString()][0].Subject = ContentSubjectType.BigScreen;
        list.Add(mDic[ContentSubjectType.BigScreen.ToString()][0]);
        return list;
    }


    /// <summary>
    /// 从主题中从start
    /// </summary>
    /// <param name="type"></param>
    /// <param name="from"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public List<VideoItemConfig> GetListBetween(ContentSubjectType type, int start = 0, int count = 0)
    {
        List<VideoItemConfig> list = mDic[type.ToString()];
        if (list.Count <= 0)
        {
            return new List<VideoItemConfig>();
        }
        else
        {
            return list.GetRange(start, count);
        }
    }

    
    /// <summary>
    /// 获取单个主题页码数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="homeCount"></param>
    /// <param name="percount"></param>
    /// <returns></returns>
    public int GetTotalPageBySubject(ContentSubjectType type,int homeCount=6,int percount=9)
    {
        float Count = mDic[type.ToString()].Count;
        float totalPage = 1;
        if (Count <= homeCount)
        {
            totalPage = 1;
        }
        else
        {
            totalPage = 1 + Mathf.CeilToInt((Count - homeCount) / percount);
        }
        return (int)totalPage;
    }

    
    public List<VideoItemConfig> GetListByPage(ContentSubjectType type, int homeCount = 6, int pageIndex = 0, int perCount = 9)
    {
        int start = 0;
        int end = 0;
        List<VideoItemConfig> list = mDic[type.ToString()];
        List<VideoItemConfig> data = new List<VideoItemConfig>();
        //首页
        if (pageIndex == 0)
        {
            //起始索引
            start = 0;
            //计算终止索引
            if (list.Count <=homeCount)
            {
                end = list.Count - 1;
            }else
            {
                end = homeCount-1;
            }
        }else
        {
            start = 6 + (pageIndex - 1) * perCount;
            int tmpEnd = homeCount + (pageIndex * perCount)-1;
            if (list.Count < tmpEnd)
            {
                end = list.Count - 1;
            }else
            {
                end = tmpEnd;
            }
        }
        return list.GetRange(start, end-start+1);
    }


    public string GetScreenTypeTitle(string screenType)
    {
        //中文
        if (LanguageManager.Instance != null)
        {
            switch (LanguageManager.Instance.CurLanguage)
            {
                case "English":
                    if (ScreenTypeDic_En.ContainsKey(screenType))
                    {
                        return ScreenTypeDic_En[screenType];
                    }
                    break;
                case "简体中文":
                    if (ScreenTypeDic_Zh.ContainsKey(screenType))
                    {
                        return ScreenTypeDic_Zh[screenType];
                    }
                    break;
            }
        }
        else
        {
            if (ScreenTypeDic_Zh.ContainsKey(screenType))
            {
                return ScreenTypeDic_Zh[screenType];
            }
        }

        return "";
    }
}

[System.Serializable]
public class VideoItemConfig
{
    public string TitleZh;
    public string TitleEn;
    public string ScreenType;
    public int    HotValue;
    public string Kbps;
    public string PictureUrl;
    public string VideoUrl;
    public string StbUrl;
    public string Duration;
    public string Width;
    public string Height;
    public ContentSubjectType Subject;
}

[System.Serializable]
public class VideoConfig
{
    public List<VideoItemConfig> LivingVideo;
    public List<VideoItemConfig> FunnyVideo;
    public List<VideoItemConfig> Education;
    public List<VideoItemConfig> BigScreen;
    public List<VideoItemConfig> Picture;
}


//内容主题类型
public enum ContentSubjectType
{
    LivingVideo,
    FunnyVideo,
    Education,
    BigScreen,
    Picture,
    Game
}
