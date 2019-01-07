/*************************************
			
		SplashLogoController 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SplashLogoController : MonoBehaviour
{

    private string splashLogoURL = "";
    public Image imgLogo;
    public static SplashLogoData data;
    public static Sprite logoSprite;
    public int splashTime = 3;
    // Use this for initialization
    void Start()
    {
 
        string filePath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = "/sdcard/letinstreaming/splash.json";
        }
        else
        {
            filePath = Application.dataPath + "/letinstreaming/splash.json";
        }

        if (!File.Exists(filePath))
        {
            Debug.Log("not found splash.json flie! display letinvr logo!");
            data = new SplashLogoData();
            data.SplashWidth = 600;
            data.SplashHeight = 140;
            data.LauncherWidth = 500;
            data.LauncherHeight = 160;
            data.SplashTime = splashTime;
            logoSprite = Resources.Load<Sprite>("Textures/letinvr_logo");
            imgLogo.rectTransform.sizeDelta = new Vector2(600, 140);
            imgLogo.sprite = logoSprite;// Sprite.Create(tex, new Rect(0, 0, data.SplashWidth, data.SplashHeight), Vector2.zero);
            StartCoroutine(LogoAlphaGo());
            return;
        }
        //读取配置
        string config = File.ReadAllText(filePath);
        data = JsonConvert.DeserializeObject<SplashLogoData>(config);

        Debug.Log(config);
        StartCoroutine(loadLogoImage());


    }

    private IEnumerator loadLogoImage()
    {
        Debug.Log("start loading splash logo from " + data.logoURL);
        WWW www = new WWW(data.logoURL);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("display splash logo from config");
            Texture2D tx = www.texture;
            logoSprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
            imgLogo.rectTransform.sizeDelta = new Vector2(data.SplashWidth, data.SplashHeight);
            imgLogo.sprite = logoSprite;
        }
        else
        {
            Debug.Log("display letinvr splash logo!");
            data = new SplashLogoData();
            data.SplashWidth = 600;
            data.SplashHeight = 140;
            data.LauncherWidth = 500;
            data.LauncherHeight = 160;
            data.SplashTime = splashTime;
            logoSprite = Resources.Load<Sprite>("Textures/letinvr_logo");
            imgLogo.rectTransform.sizeDelta = new Vector2(600, 140);
            imgLogo.sprite = logoSprite;
        }
        StartCoroutine(LogoAlphaGo());
    }


    IEnumerator LogoAlphaGo()
    {

        while (imgLogo.color.a < 1)
        {
            Color c = imgLogo.color;
            c.a += 0.02f;
            imgLogo.color = c;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(data.SplashTime);

        SceneManager.LoadScene(SceneList.Preloading);

    }


    void OnDestroy()
    {
        StopAllCoroutines();
    }


    // Update is called once per frame
    void Update()
    {

    }



}

public struct SplashLogoData
{
    public string logoURL;
    public int SplashWidth;
    public int SplashHeight;
    public int LauncherWidth;
    public int LauncherHeight;
    public int SplashTime;
}