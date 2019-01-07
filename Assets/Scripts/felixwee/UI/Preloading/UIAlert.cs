/*************************************
			
		UIAlert 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAlert : MonoBehaviour {

    public RectTransform _mBody;
    public Text _mTxt;
    public CanvasGroup cg;
	// Use this for initialization
	void Start () {
   
	}

    /// <summary>
    /// 显示提示文字
    /// </summary>
    /// <param name="msg"></param>
    public void Show(string msg="",bool quitApp=false)
    {
        cg.alpha = 1;
        _mBody.gameObject.SetActive(true);
        _mTxt.text = msg;

        if (quitApp)
        {
            StopAllCoroutines();
            StartCoroutine(wait2QuitApp());
        }

    }

    private IEnumerator wait2QuitApp()
    {
        yield return new WaitForSeconds(3);
        int seconds = 5;
        while (seconds >=0)
        {
            _mTxt.text = seconds + "秒后将自动退出程序";
            yield return new WaitForSeconds(1);
            seconds--;
        }

        cg.alpha = 0;
        //退出App
        Global.Instance.QuitApp();
    }


    public void HideAlert()
    {
        cg.alpha = 0;
        _mBody.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
