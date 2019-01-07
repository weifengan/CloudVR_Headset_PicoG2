/*************************************
			
		AsyncCVRAdminManager 
		
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

public class AsyncCVRAdminManager : MonoBehaviour
{

    private static AsyncCVRAdminManager _instance;
    private string SERVER;
 

    public void Start()
    {
        _instance = this;
    }
    public static AsyncCVRAdminManager Instance
    {
        get
        {
            return _instance;
        }
    }


    /// <summary>
    /// 初始化后台管理器路径
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public void Init(string serverHost)
    {
        SERVER = serverHost;
        Global.LogInfo("【DB】初始化AsyncCVRAdminManager管理器OK 服务器地址:"+ SERVER);
       
     }

    public void AsyncQuitVideo()
    {

        string url = SERVER + "/api/quit.php";
        Global.LogInfo("向管理后台同步视频数据" + url);
        HttpHelper.Instance.GET(url, delegate (long code, string content)
        {
            if (code != 200)
            {
                Global.LogError("【ADMIN】管理后台同步退出视频播放失败  code" + code + "  原因:" + content);
                return;
            }
            try
            {
                HttpResponseStruct hr = JsonConvert.DeserializeObject<HttpResponseStruct>(content);
                if (hr.result == 1)
                {
                    Global.LogInfo("【ADMIN】管理后台同步退出视频播放OK");
                }
                else
                {
                    Global.LogInfo("【ADMIN】管理后台同步退出视频播放失败     原因:" + content);
                }
            }
            catch (Exception ex)
            {
                Global.LogInfo("【ADMIN】管理后台同步退出视频播放失败     原因:" + ex.Message);
            }
        });
    }

    public void AsyncVideoInfo(ContentSubjectType subject, VideoItemConfig videoData)
    {

        videoData.Subject = subject;
        string json = JsonConvert.SerializeObject(videoData);
        Global.LogInfo("向管理后台同步视频数据" + json);

        string url = SERVER + "/api/update.php?data=" + json;
        Global.LogInfo(url);
        HttpHelper.Instance.GET(url, delegate (long code, string content) {
            if (code != 200)
            {
                Global.LogError("【ADMIN】" + content);
                return;
            }
            try
            {
                HttpResponseStruct hr = JsonConvert.DeserializeObject<HttpResponseStruct>(content);
                if (hr.result == 1)
                {
                    Global.LogInfo("【ADMIN】管理系统同步视频播放信息OK");
                }
                else
                {
                    Global.LogInfo("【ADMIN】管理系统同步视频播放信息失败     原因:" + content);
                }
            }
            catch (Exception ex)
            {
                Global.LogInfo("【ADMIN】同步管理系统失败     原因:" + ex.Message);
            }
         });
 
    }
  
}
public struct HttpResponseStruct
{
    public int result;
    public string msg;
}