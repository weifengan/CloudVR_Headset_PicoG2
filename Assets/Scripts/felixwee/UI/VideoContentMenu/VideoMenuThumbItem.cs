/*************************************
			
		VideoMenuThumbItem 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class VideoMenuThumbItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
 
    public Text txtType;
    public Text txtTitle;
    public Image imgThumb;
    private Text txtScroll;
    public MultiLngThumbTitle multiTitle;

    private VideoItemConfig m_Data;

    private float targetScale = 1;

    private bool _scrollTitle = false;

    public void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnThumbClickHandler);
    }


    private void OnThumbClickHandler()
    {
        Global.Instance.currentVideoData = m_Data;
        Global.LogInfo("you clicked video thumb", JsonUtility.ToJson(m_Data));

        Global.Instance.LastPageIndex = Global.Instance.CurrentPageIndex;
        Global.Instance.Switch2VideoPlayScene();
    }

    public void SetData(VideoItemConfig data)
    {
        m_Data = data;
       multiTitle = this.GetComponentInChildren<MultiLngThumbTitle>();
       multiTitle.Init(data);

        //根据类型显示

        txtType.text =  VideoConfigManager.Instance.GetScreenTypeTitle(data.ScreenType);

        //重置滚动组件
        VideoTitleScroll scrollT = this.GetComponentInChildren<VideoTitleScroll>();
        if (scrollT != null)
        {
            scrollT.DoPointerEnterHandler();
            scrollT.DoPointerExitHandler(this.GetComponent<RectTransform>().sizeDelta.x);
        }

        //检测缓存中是否有此图片
        Sprite sp = ThumbCacheManager.Instance.GetSprite(m_Data.PictureUrl);
        if (sp != null)
        {
            imgThumb.sprite = ThumbCacheManager.Instance.GetSprite(m_Data.PictureUrl);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(doLoadThumb());
        }     
        
          
    }



    private IEnumerator doLoadThumb()
    {
        Global.LogInfo("start dowload image"+m_Data.PictureUrl);
        WWW www = new WWW(m_Data.PictureUrl);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D tex = www.texture as Texture2D;
            Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            imgThumb.sprite = sp;
            ThumbCacheManager.Instance.AddCache(m_Data.PictureUrl, sp);
        }
        else
        {

        }
    }


    public void ClearData()
    {
        txtType.text = "";
        txtTitle.text = "";
        imgThumb.sprite = Resources.Load<Sprite>("Background/Notfound/notfound_16x9");

    }
 

    // Update is called once per frame
    void Update()
    {
        if (imgThumb.transform.localScale.x != targetScale)
        {
            imgThumb.transform.localScale = Vector3.Lerp(imgThumb.transform.localScale, Vector3.one * targetScale, Time.deltaTime * 10);
        }
        
    }

    //光标移上
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = 1.3f;
        _scrollTitle = true;

        VideoTitleScroll scrollT=this.GetComponentInChildren<VideoTitleScroll>();
        if (scrollT != null)
        {
            scrollT.DoPointerEnterHandler();
        }
 

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = 1;
        _scrollTitle = false;

        VideoTitleScroll scrollT = this.GetComponentInChildren<VideoTitleScroll>();
        if (scrollT != null)
        {
            RectTransform rtf = this.transform as RectTransform;
            scrollT.DoPointerExitHandler(rtf.sizeDelta.x);
        }
    }
}

