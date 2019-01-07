/*************************************
			
		UIMenuPage 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoContentMenuPage : MonoBehaviour {

    /// <summary>
    /// 页面数据
    /// </summary>
    private List<VideoItemConfig> _pageData;

    public VideoMenuThumbItem[] thumbItems;
    public void Start()
    {
        
    }


    private IEnumerator SetItemData()
    {
        int tmpcount = _pageData.Count;
        for (int i = 0; i < thumbItems.Length; i++)
        {
            VideoMenuThumbItem item = thumbItems[i];
            if (i <= tmpcount - 1)
            {
                item.gameObject.SetActive(true);
                item.SetData(_pageData[i]);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.2f*i);
        }
    }

    //设定页面数据
    public void SetPageData(List<VideoItemConfig> pageData)
    {
        
        _pageData = pageData;
        int tmpcount = _pageData.Count;
        for (int i = 0; i < thumbItems.Length; i++)
        {
            VideoMenuThumbItem item = thumbItems[i];
            item.ClearData();
            if (i <= tmpcount - 1)
            {
                item.gameObject.SetActive(true);
            }else
            {
                item.gameObject.SetActive(false);
            }
        }
        StartCoroutine(DelayToSetData());
    }

    IEnumerator DelayToSetData()
    {
        int tmpcount = _pageData.Count;

        for (int i = 0; i < thumbItems.Length; i++)
        {
            VideoMenuThumbItem item = thumbItems[i];
            if (i <= tmpcount - 1)
            {
                item.SetData(_pageData[i]);
                item.gameObject.SetActive(true);
            }
            else
            {
                item.ClearData();
                item.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EmptyPage()
    {

        for (int i = 0; i < thumbItems.Length; i++)
        {
            VideoMenuThumbItem item = thumbItems[i];
            item.gameObject.SetActive(true);
            item.ClearData();
        }
    }
}
