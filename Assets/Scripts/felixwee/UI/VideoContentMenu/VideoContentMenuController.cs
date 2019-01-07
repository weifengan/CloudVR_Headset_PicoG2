/*************************************
			
		VideoContentMenuController 
		
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

public class VideoContentMenuController : MonoBehaviour
{

    public PaginationController2 pageController;

    //固定值，不翻页时不改变
    public VideoContentMenuPage m_P0; //Home页
    public VideoContentMenuPage m_P1;
    public VideoContentMenuPage m_P2;

    private VideoContentMenuPage curPage = null;
    private VideoContentMenuPage tmpPage = null;

    private bool isMoving = false;
    private PageDir curMoveDir = PageDir.NONE;

    public float m_MovingSpeed = 5;


    /// <summary>
    /// 当前页码，从0开始为代表第一页
    /// </summary>
    public int curPageIndex;
    private bool isFirstRun = true;
    // Use this for initialization
    void Start()
    {
        InitBgSphere();
        pageController.OnValueChanged += PageController_OnValueChanged;
        InitContent();
    }
    public void InitBgSphere()
    {
        MeshRenderer mr = GameObject.Find("360BGsphere").GetComponent<MeshRenderer>();
        string mtUrl = "";
        switch (Global.Instance.currentContentSubjectType)
        {
            case ContentSubjectType.BigScreen:
                mtUrl = "Materials/SceneBackground/BGBigScreen";
                break;
            case ContentSubjectType.LivingVideo:
                mtUrl = "Materials/SceneBackground/BGLivingStreaming";
                break;
            case ContentSubjectType.Education:
                mtUrl = "Materials/SceneBackground/BGEducation";
                break;
            case ContentSubjectType.FunnyVideo:
                mtUrl = "Materials/SceneBackground/BGFunny";
                break;

        }
        if (mr != null)
        {
            //根据类型替换全景背景图
            mr.material = Resources.Load<Material>(mtUrl);
        }
    }

    public void InitContent() {

        ///根据上一次存在的页码进行显示内容 
        
        int maxPage = VideoConfigManager.Instance.GetTotalPageBySubject(Global.Instance.currentContentSubjectType);
        if (Global.Instance.LastPageIndex >= maxPage - 1)
        {
            Global.Instance.LastPageIndex = maxPage - 1;
        }
        curPageIndex = Global.Instance.LastPageIndex;
       
        Global.LogInfo("进入" + Global.Instance.currentContentSubjectType.ToString() + "菜单界面   内容页数:" + maxPage);

        if (curPageIndex == 0)
        {
            //一进来，默认为首页时处理
            curPage = m_P0;
            tmpPage = m_P1;
        }else
        {
            //如果一进来，不是初始化首页
            curPage = m_P1;
            tmpPage = m_P2;
        }
        curPage.gameObject.SetActive(true);

        //初始化页码器
       
        pageController.Init(maxPage, curPageIndex);

        RefreshPageView();
    }
    /// <summary>
    /// 刷新页面显示
    /// </summary>
    public void RefreshPageView()
    {
        //根据页码获取列表数据
        List<VideoItemConfig> listData = VideoConfigManager.Instance.GetListByPage(Global.Instance.currentContentSubjectType,6,this.curPageIndex);
        VideoContentMenuPage page = curPage.GetComponent<VideoContentMenuPage>();
        page.SetPageData(listData);
    }



    public void PreLoadingPageData(VideoContentMenuPage curPage,VideoContentMenuPage tmpPage)
    {
        //根据页码获取列表数据
        List<VideoItemConfig> listData = VideoConfigManager.Instance.GetListByPage(Global.Instance.currentContentSubjectType, 6, this.curPageIndex);
        VideoContentMenuPage page = curPage.GetComponent<VideoContentMenuPage>();
        page.SetPageData(listData);

        
    }

    /// <summary>
    /// 页码发生改变回调
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="curPageIndex"></param>
    private void PageController_OnValueChanged(PageDir dir, int curPageIndex)
    {
        m_P0.gameObject.SetActive(false);
        m_P1.gameObject.SetActive(false);
        m_P2.gameObject.SetActive(false);
        this.curMoveDir = dir;
        this.curPageIndex = curPageIndex;
        //默认页面
        if (curMoveDir == PageDir.NONE)
        {
            //默认第一页
            if (this.curPageIndex == 0)
            {
                curPage = m_P0;
                tmpPage = m_P1;
            } else if (this.curPageIndex == 1)
            {
                curPage = m_P1;
                tmpPage = m_P2;
            }
        }

        if (curMoveDir == PageDir.NEXT)
        {
            if (curPage.name.Equals("P0"))
            {
                tmpPage = m_P1;
            }else if(curPage.name.Equals("P1"))
            {
                tmpPage = m_P2;
            }
            else
            {
                tmpPage = m_P1;
            }
        }

        if (curMoveDir == PageDir.PREV)
        {
            if (this.curPageIndex == 1) {
                if (curPage.name.Equals("P1"))
                {
                    tmpPage = m_P2;
                }
                else
                {
                    tmpPage = m_P1;
                }
            }
            if (this.curPageIndex == 0)
            {
                tmpPage = m_P0;
            }
        }
        RectTransform curRtf = curPage.transform as RectTransform;
        RectTransform tmpRtf = tmpPage.transform as RectTransform;

        if (curMoveDir == PageDir.NEXT)
        {
            //调整位置
            tmpRtf.anchoredPosition = new Vector2(0, -1090);
        }
        else
        {    
            tmpRtf.anchoredPosition = new Vector2(0, 1090);
        }

        curPage.gameObject.SetActive(true);
        tmpPage.gameObject.SetActive(true);

        //开始移动，并锁定页码器不能点击
        isMoving = true;
        if (isFirstRun)
        {
            pageController.isLock = false;
            isFirstRun = false;
            return;
        }
        pageController.isLock = true;

        PreLoadingPageData(tmpPage, curPage);

        ///同步全局当前页码
        Global.Instance.CurrentPageIndex = curPageIndex;


    }



    // Update is called once per frame
    void Update()
    {
        RectTransform curRtf = curPage.transform as RectTransform;
        RectTransform tmpRtf = tmpPage.transform as RectTransform;

        if (isMoving)
        {
            
            switch (curMoveDir)
            {
                case PageDir.NEXT:
                    curRtf.anchoredPosition = Vector2.Lerp(curRtf.anchoredPosition, new Vector2(0, 1090), Time.deltaTime * m_MovingSpeed);
                    tmpRtf.anchoredPosition = Vector2.Lerp(tmpRtf.anchoredPosition, Vector2.zero, Time.deltaTime * m_MovingSpeed);

                    if (Vector2.Distance(tmpRtf.anchoredPosition, Vector2.zero) < 2)
                    {
                        tmpRtf.anchoredPosition = Vector2.zero;

                        //变化当前页
                        curPage = tmpRtf.GetComponent<VideoContentMenuPage>();
                        tmpPage = curRtf.GetComponent<VideoContentMenuPage>();
                        tmpPage.gameObject.SetActive(false);
                        isMoving = false;
                        pageController.isLock = false;
                      
                    }
                    break;
                case PageDir.PREV:
                    curRtf.anchoredPosition = Vector2.Lerp(curRtf.anchoredPosition, new Vector2(0, -1090), Time.deltaTime * m_MovingSpeed);
                    tmpRtf.anchoredPosition = Vector2.Lerp(tmpRtf.anchoredPosition, Vector2.zero, Time.deltaTime * m_MovingSpeed);

                    if (Vector2.Distance(tmpRtf.anchoredPosition, Vector2.zero) < 2)
                    {
                        tmpRtf.anchoredPosition = Vector2.zero;

                        //变化当前页
                        RectTransform tt = curRtf;
                        curPage = tmpRtf.GetComponent<VideoContentMenuPage>();
                        tmpPage = curRtf.GetComponent<VideoContentMenuPage>();
                        isMoving = false;
                        tmpPage.gameObject.SetActive(false);
                        pageController.isLock = false;
                    }

                    break;
            }
        }
    }

  

    void OnDestroy()
    {
        pageController.OnValueChanged -= PageController_OnValueChanged;
    }


    public void Quit2HomeLauncher()
    {
        Global.Instance.SwitchScene("LetinvrLauncher");
    }
}
