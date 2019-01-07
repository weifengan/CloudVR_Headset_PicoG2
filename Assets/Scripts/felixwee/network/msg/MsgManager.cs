/*************************************
			
		MsgManager 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NetEventJsonCallBackHandler(NetMessageId id, string json);
public class MsgManager {

    private Dictionary<NetMessageId, List<NetEventJsonCallBackHandler>> mDic = new Dictionary<NetMessageId, List<NetEventJsonCallBackHandler>>();

    private static MsgManager _instance = null;
    private static readonly object _lock = new object();
    public static MsgManager Instance
    {
        get
        {
            lock (_lock)
            {

                if (_instance == null)
                {
                    _instance = new MsgManager();
                }
                return _instance;
            } 
        }
    }

    public bool Add(NetMessageId id, NetEventJsonCallBackHandler handler)
    {
        if (!mDic.ContainsKey(id))
        {
            mDic.Add(id, new List<NetEventJsonCallBackHandler>());
            mDic[id].Add(handler);
            return true;
        }else
        {
            List<NetEventJsonCallBackHandler> list = mDic[id];

            if (!list.Contains(handler))
            {
                list.Add(handler);
                return true;
            }else
            {
                Debug.Log("添加消息回调" + id.ToString() + "失败:此回调已经存在");
            }
            return false;
        }
    }

    /// <summary>
    /// 删除某个消息的单个回调
    /// </summary>
    /// <param name="id"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public bool Remove(NetMessageId id, NetEventJsonCallBackHandler handler)
    {
        if (!mDic.ContainsKey(id))
        {
            return false;
        }

        List<NetEventJsonCallBackHandler> list = mDic[id];

        if (list.Contains(handler))
        {
            list.Remove(handler);
            return true;
        }
        return false;  
    }


    /// <summary>
    /// 删除某个消息的所有回调
    /// </summary>
    /// <param name="id"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public bool Remove(NetMessageId id)
    {
        if (!mDic.ContainsKey(id))
        {
            return false;
        }
         mDic[id].Clear();
         return false;
    }

   public void Dispatch(NetMessageId msgId,string json)
    {
        if (!mDic.ContainsKey(msgId))
        {
            Debug.Log("未监听" + msgId + "消息");
            return;
        }
        foreach (var item in mDic[msgId])
        {
            item(msgId, json);
        }
    }
    //派发消息
    public void Dispatch(NetMessage msg)
    {
        if (!mDic.ContainsKey(msg.Id))
        {
            return;
        }
        foreach (var item in mDic[msg.Id])
        {
            item(msg.Id, msg.Json);
        }
    }
}
