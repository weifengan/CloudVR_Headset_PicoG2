/*************************************
			
		BasicLanguageText 
		
   @desction: 用于根据key关键了来显示对应语言文本基础组件
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicLanguageText : MonoBehaviour {

    [Header("语言KEY")]
    public string m_MultiKey = "";

    private Text mTxt;
	// Use this for initialization
	private void OnEnable () {
        //获取自身Text组件
        mTxt = this.GetComponent<Text>();

        //注册语言更改时的界面文本刷新回调
        LanguageManager.Instance.OnLanguageChangeHandler += Instance_OnLanguageChangeHandler;

        UpdateLanuageView();
    }

    private void Instance_OnLanguageChangeHandler(string obj)
    {
        UpdateLanuageView();
    }


    /// <summary>
    /// 刷新UI文本显示
    /// </summary>
    public void UpdateLanuageView()
    {
        if (mTxt == null)
        {
            mTxt = this.GetComponent<Text>();
        }
        if (mTxt != null)
        {
            mTxt.text = LanguageManager.Instance.GetValueByKey(m_MultiKey);
        }
    }


    private void OnDisable()
    {
        LanguageManager.Instance.OnLanguageChangeHandler -= Instance_OnLanguageChangeHandler;
    }

}
