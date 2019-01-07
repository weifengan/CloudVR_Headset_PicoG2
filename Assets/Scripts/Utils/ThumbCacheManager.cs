/*************************************
			
		ThumbCacheManager 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbCacheManager  {
    private static ThumbCacheManager _instance = null;
    public static ThumbCacheManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ThumbCacheManager();
            }
            return _instance;
        }
    }

    private Dictionary<string, Sprite> mDic = new Dictionary<string, Sprite>();

    /// <summary>
    /// 添加缓存
    /// </summary>
    /// <param name="picUrl"></param>
    /// <param name="sprite"></param>
    public void AddCache(string picUrl,Sprite sprite)
    {
        if (mDic.ContainsKey(picUrl))
        {
            return;
        }
         mDic.Add(picUrl, sprite);
    }
    /// <summary>
    /// 根据路径索取缩略图
    /// </summary>
    /// <param name="picUrl"></param>
    /// <returns></returns>
    public Sprite GetSprite(string picUrl)
    {
        if (mDic.ContainsKey(picUrl))
        {
            return mDic[picUrl];
        }
        return null;
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    public void ClearCache()
    {
        mDic.Clear();
    }
	
}
