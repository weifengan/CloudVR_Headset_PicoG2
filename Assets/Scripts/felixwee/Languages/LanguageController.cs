/*************************************
			
		LanguageController 
		
   @desction:用来通过界面控制语言切换,每点一次更换一次语言
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{

    private Text txt;
    // Use this for initialization
    void Start()
    {
        txt = this.transform.Find("Text").GetComponent<Text>();
        LanguageManager.Instance.OnLanguageChangeHandler += Instance_OnLanguageChangeHandler;
        UpdateView();
    }


    public void UpdateView()
    {
        switch (LanguageManager.Instance.CurLanguage)
        {
            case "English":
                txt.text = "EN";
                break;
            case "简体中文":
                txt.text = "ZH";
                break;
        }
    }

    private float t = 0.5f;
    public void ChangeLanguage()
    {
        if (t < 0)
        {
            LanguageManager.Instance.SwitchLanguage();
            UpdateView();
            t = 0.5f;
        }
    }

    private void Instance_OnLanguageChangeHandler(string obj)
    {
        UpdateView();
    }

    // Update is called once per frame
    void Update()
    {
        //屏蔽点击调用两次问题
        t -= Time.deltaTime;
        if (t < 0)
        {
            t = -2;
        }
    }


    private void OnDestroy()
    {
        LanguageManager.Instance.OnLanguageChangeHandler -= Instance_OnLanguageChangeHandler;
    }


}
