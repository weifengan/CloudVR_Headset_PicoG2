/*************************************
			
		LauncherThumbItem 
		
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


public class LauncherThumbItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text txtType;
    public Text txtTitle;
    public Image imgThumb;



    public MultiLngThumbTitle multiTitle;

    private VideoItemConfig m_Data;

    private Dictionary<string, string> mScreenTypeDic = new Dictionary<string, string>();

    public ContentSubjectType m_Subject;

    private float targetScale = 1;

    private bool _scrollTitle = false;


    public void Start()
    {
        //添加点击响应事件
        this.GetComponentInChildren<Button>().onClick.AddListener(OnThumbClickHandler);
        if (LanguageManager.Instance)
        {
            LanguageManager.Instance.OnLanguageChangeHandler += Instance_OnLanguageChangeHandler;
        }
    }

    private void Instance_OnLanguageChangeHandler(string obj)
    {
        if (txtType)
        {
            txtType.text = VideoConfigManager.Instance.GetScreenTypeTitle(m_Data.ScreenType);
        }
    }


    private void OnThumbClickHandler()
    {

        Global.LogInfo("点击了视频 ", m_Data.VideoUrl);
        Global.Instance.LastPageIndex = 0;
        Global.Instance.CurrentPageIndex = 0;

        Global.Instance.currentContentSubjectType = m_Subject;
        Global.Instance.currentVideoData = m_Data;
        Global.Instance.Switch2VideoPlayScene();
    }

    public void SetData(VideoItemConfig data, ContentSubjectType subject)
    {
        m_Subject = subject;
        m_Data = data;
        multiTitle = this.GetComponentInChildren<MultiLngThumbTitle>();
        multiTitle.Init(data);


        //根据类型显示

        txtType.text = VideoConfigManager.Instance.GetScreenTypeTitle(data.ScreenType);
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
        WWW www = new WWW(m_Data.PictureUrl);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D tex = www.texture as Texture2D;
            Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            imgThumb.sprite = sp;
            ThumbCacheManager.Instance.AddCache(m_Data.PictureUrl, sp);
        }

    }

    //光标移上
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = 1.3f;
        _scrollTitle = true;

        VideoTitleScroll scrollT = this.GetComponentInChildren<VideoTitleScroll>();
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

    public void OnDestroy()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChangeHandler -= Instance_OnLanguageChangeHandler;
        }
    }
}
