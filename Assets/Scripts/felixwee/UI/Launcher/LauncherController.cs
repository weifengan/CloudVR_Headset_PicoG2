/*************************************
			
		UIMainLauncherController 
		
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
using UnityEngine.UI;

public class LauncherController : MonoBehaviour {



    public Text txtVersion;
    public Image LogoImg;
    public Button btnLanguage;
    public Button btnExit;
    public LauncherThumbItem[] thumbs;


    public Button[] btnMenus;

    private Dictionary<string, ContentSubjectType> mSubjects = new Dictionary<string, ContentSubjectType>();

	// Use this for initialization
	void Start () {

        //显示版本
        txtVersion.text ="  V" + Global.Instance.MajorVersion + "." + Global.Instance.Minor_Version + "." + Global.Instance.Revision_Version;

        if (SplashLogoController.logoSprite!=null)
        {
            Global.LogInfo("Splash 配置:" + JsonConvert.SerializeObject(SplashLogoController.data));
            LogoImg.rectTransform.sizeDelta = new Vector2(SplashLogoController.data.LauncherWidth, SplashLogoController.data.LauncherHeight);
            LogoImg.sprite = SplashLogoController.logoSprite;
        }else
        {
            Sprite logoSprite = Resources.Load<Sprite>("Textures/letinvr_logo");
            LogoImg.rectTransform.sizeDelta = new Vector2(600, 140);
            LogoImg.sprite = logoSprite;
        }

        foreach(ContentSubjectType type in Enum.GetValues(typeof(ContentSubjectType)))
        {
            Debug.Log("======" + type);
        }
        //用于处理菜单对应主题
        mSubjects.Add("LiveVideo", ContentSubjectType.LivingVideo);
        mSubjects.Add("FunnyVideo", ContentSubjectType.FunnyVideo);
        mSubjects.Add("BigScreen", ContentSubjectType.BigScreen);
        mSubjects.Add("Education", ContentSubjectType.Education);
        mSubjects.Add("Game", ContentSubjectType.Game);


        ///加载主页推荐内容
        List<VideoItemConfig> thumbdatalist = VideoConfigManager.Instance.GetLauncherList();

        for (int i = 0; i < thumbs.Length; i++)
        {
            thumbs[i].SetData(thumbdatalist[i], thumbdatalist[i].Subject);
        }


        foreach (Button  btn in btnMenus)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnMenuClicked(btn.name);
            });
        }

        //切换语言
        btnLanguage.onClick.AddListener(delegate () {

            LanguageController lc = btnLanguage.GetComponent<LanguageController>();
            lc.ChangeLanguage();
            Global.LogInfo("切换语言" + LanguageManager.Instance.CurLanguage);
        });

        btnExit.onClick.AddListener(delegate () {
            Global.Instance.QuitApp();
        });

     }

    private void OnMenuClicked(string name)
    {
        ContentSubjectType type = mSubjects[name.Replace("btn", "")];

        if (type == ContentSubjectType.Game)
        {
            Global.LogInfo("暂未开通云游戏功能!");
            return;
        }
        Global.Instance.LastPageIndex = 0;
        Global.Instance.currentContentSubjectType = type;
        Global.Instance.SwitchScene(SceneList.VideoContentMenu);
    }


    public void QuitApp()
    {
        Global.Instance.QuitApp();
    }

    // Update is called once per frame
    void Update () {
		
	}


     
}
