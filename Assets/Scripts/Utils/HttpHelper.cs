/*************************************
			
		HttpHelper 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public delegate void HttpRequestCallBack(long responseCode, string content);
public class HttpHelper : MonoBehaviour
{

    private static HttpHelper _instance = null;
    private static readonly object _lock = new object();

    public static HttpHelper Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_HttpHelper");
                    _instance = go.AddComponent<HttpHelper>();
                    if (Global.Instance != null)
                    {
                        _instance.transform.SetParent(Global.Instance.transform);
                    }
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }



    public void POST(string url, WWWForm form, HttpRequestCallBack callback)
    {
        StartCoroutine(doPostRequest(url, form, callback));
    }

    private IEnumerator doPostRequest(string url,WWWForm form=null, HttpRequestCallBack callback=null,int timeout=5)
    {
        UnityWebRequest wr = UnityWebRequest.Post(url, form);
        wr.timeout = timeout;
       
        yield return wr.SendWebRequest();

       
        if (callback != null)
        {
            callback(wr.responseCode, wr.downloadHandler.text);
        }
    }

    public void GET(string url, HttpRequestCallBack callback)
    {
        StartCoroutine(doGetRequest(url, callback));
    }

    private IEnumerator doGetRequest(string url, HttpRequestCallBack callback = null, int timeout = 5)
    {
        UnityWebRequest wr = UnityWebRequest.Get(url);
        wr.timeout = timeout;
        yield return wr.SendWebRequest();
        if (callback != null)
        {
            callback(wr.responseCode, wr.downloadHandler.text);
        }
    }

}
