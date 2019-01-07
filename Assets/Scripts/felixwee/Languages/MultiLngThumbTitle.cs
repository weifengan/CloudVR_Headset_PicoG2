/*************************************
			
		MultiLngThumbTitle 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLngThumbTitle : MonoBehaviour
{

    private VideoItemConfig mCfgData;
   
    private Text mTxt;
    // Use this for initialization
    void OnEnable()
    {
        mTxt = this.GetComponent<Text>();
        LanguageManager.Instance.OnLanguageChangeHandler += Instance_OnLanguageChangeHandler;
    }

    private void Instance_OnLanguageChangeHandler(string obj)
    {
        UpdateLangugeView();
    }


    public void Init(VideoItemConfig data)
    {
        mCfgData = data;
        UpdateLangugeView();
    }

    public void UpdateLangugeView()
    {
        if (mTxt == null)
        {
            mTxt = this.GetComponent<Text>();
        }
        if (mTxt != null)
        {
            //多语言
            string title = "Empty";
            //根据语言显示文本
            switch (LanguageManager.Instance.CurLanguage)
            {
                case "English":
                    title = mCfgData.TitleEn;
                    break;
                case "简体中文":
                    title = mCfgData.TitleZh;
                    break;
            }
            mTxt.text = title;
        }
    }

    private void OnDisable()
    {
        LanguageManager.Instance.OnLanguageChangeHandler -= Instance_OnLanguageChangeHandler;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
