/*************************************
			
		PaginationController 
		
   @desction: 页码展示器
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 翻页时的方向 
/// </summary>
public enum PageDir
{
    NONE,NEXT,PREV
}
public class PaginationController2 : MonoBehaviour {

    /// <summary>
    /// 页面发生变化时事件
    /// </summary>
    public event Action<PageDir, int> OnValueChanged = null;
    public Button m_BtnNext;
    public Button m_BtnPrev;
    public Text m_TxtPage;

    private int _currentPageIndex = 0;
    private int _totalPageCount = 0;

    private int _lastPageIndex = 0;
    // Use this for initialization
	void Start () {
        m_BtnNext.onClick.AddListener(NextPage);
        m_BtnPrev.onClick.AddListener(PrevPage);
    }

    public bool _isLock = false;
    public bool isLock
    {
        get
        {
            return _isLock;
        }
        set
        {
            _isLock = value;
        }
    }
    public void NextPage()
    {
        if (_isLock) return;
        if (this._currentPageIndex + 1 >= this._totalPageCount)
        {
            this._currentPageIndex = this._totalPageCount - 1;
        }else
        {
            this._currentPageIndex++;
        }

        if (_lastPageIndex != this._currentPageIndex)
        {
            _lastPageIndex = this._currentPageIndex;

            if (OnValueChanged != null)
            {
                OnValueChanged(PageDir.NEXT, this._currentPageIndex);
            }
        }


        ///同步页码
        if (NetworkManager.Instance.TCPConnected)
        {
            NetMessage nm = new NetMessage(NetMessageId.PageChanged);

            PageChangeVo pcv = new PageChangeVo();
            pcv.currentContentType = Global.Instance.currentContentSubjectType;
            pcv.PageChangDir = PageDir.NEXT;
            pcv.currentPageIndex = this._currentPageIndex;
            pcv.lastPageIndex = Global.Instance.LastPageIndex;
           

            nm.WriteJson(JsonConvert.SerializeObject(pcv));

            NetworkManager.Instance.SendNetMessage(nm);
        }

        FreshPageView();
    }

    public void PrevPage()
    {
        if (_isLock) return;
        if (this._currentPageIndex-1<=0)
        {
            this._currentPageIndex = 0;
        }
        else
        {
            this._currentPageIndex--;
        }
        if (_lastPageIndex != this._currentPageIndex)
        {
            _lastPageIndex = this._currentPageIndex;

            if (OnValueChanged != null)
            {
                OnValueChanged(PageDir.PREV, this._currentPageIndex );
            }
           
        }


        ///同步页码
        if (NetworkManager.Instance.TCPConnected)
        {
            NetMessage nm = new NetMessage(NetMessageId.PageChanged);

            PageChangeVo pcv = new PageChangeVo();
            pcv.currentContentType = Global.Instance.currentContentSubjectType;
            pcv.PageChangDir = PageDir.PREV;
            pcv.lastPageIndex = Global.Instance.LastPageIndex;
            pcv.currentPageIndex = this._currentPageIndex;
            nm.WriteJson(JsonConvert.SerializeObject(pcv));

            NetworkManager.Instance.SendNetMessage(nm);
        }

        FreshPageView();
    }

    /// <summary>
    /// 初始化页码器
    /// </summary>
    /// <param name="totalPage">实际的页码数量</param>
    /// <param name="curIndex">实际的显示的页码，1为默认页码</param>
    public void Init(int totalPage,int curIndex = 0)
    {
        _totalPageCount = totalPage;
        _currentPageIndex = curIndex;
        _lastPageIndex = _currentPageIndex;
        if (OnValueChanged != null)
        {
            OnValueChanged(PageDir.NONE, this._currentPageIndex);
        }
        FreshPageView();
    }
   
    /// <summary>
    /// 跳转到指定页面
    /// </summary>
    /// <param name="pageIndex"></param>
    public void GotoPage(int pageIndex)
    {
        //异常处理
        if (pageIndex < 0)
        {
            _currentPageIndex = 0;
        }
        if (pageIndex > _totalPageCount-1)
        {
            pageIndex = _totalPageCount - 1;
        }
        FreshPageView();
    }
    
    public void FreshPageView()
    {
        m_TxtPage.text =(this._currentPageIndex +1)+ "/" + this._totalPageCount;

        
    }
 
	// Update is called once per frame
	void Update () {
		
	}
}
