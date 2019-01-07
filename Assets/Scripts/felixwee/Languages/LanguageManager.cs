/*************************************

       LanguageManager

  @desction:应用多语言管理类
  @author:felixwee
  @email:felixwee@163.com
  @website:www.felixwee.com

***************************************/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class LanguageManager : MonoBehaviour
{

    /// <summary>
    /// 初始化配置完成
    /// </summary>
    private System.Action onInitCallback = null;

    private string configUrl = "";

    private static LanguageManager _instance = null;
    public static LanguageManager Instance
    {

        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("_" + typeof(LanguageManager).Name);
                _instance = go.AddComponent<LanguageManager>();

                if (Global.Instance != null)
                {
                    _instance.transform.SetParent(Global.Instance.transform);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private string[] _mLangList;

    /// <summary>
    /// 配置文件中语言列表
    /// </summary>
    public string[] LanguageList
    {
        get
        {
            return _mLangList;
        }
    }
    private string _curLanguage="English";

    public string CurLanguage
    {
        get
        {
            return _curLanguage;
        }
    }

    //当前语言索引
    private int curLanguageIndex = 0;

    //多语言配置文件
    public Dictionary<string, Dictionary<string, string>> mDic = new Dictionary<string, Dictionary<string, string>>();

    /// <summary>
    /// 语言发生更改时回调触发
    /// </summary>
    public event Action<string> OnLanguageChangeHandler = null;

    private string configFileURL = "";


    public void Init(Action callback = null)
    {

    }

    /// <summary>
    /// 初始化多语言管理器，加载多语言配置文件
    /// </summary>
    public void Init(string txt, Action callback = null)
    {
        //按行解析，并使用|替换
        string[] lineArray = txt.Replace("\r\n", "|").Split(new char[1] { '|' });
        _mLangList = new string[lineArray.Length - 1];

        string[] keys = lineArray[0].Split(new char[1] { ',' });
        //添加语言空字典
        for (int i = 1; i < lineArray.Length; i++)
        {
            string[] values = lineArray[i].Split(new char[1] { ',' });
            _mLangList[i - 1] = values[0];
            mDic.Add(values[0], new Dictionary<string, string>());
            Dictionary<string, string> _dic = mDic[values[0]];
            for (int j = 1; j < values.Length; j++)
            {
                _dic.Add(keys[j], values[j]);
            }
        }

        // PlayerPrefs.DeleteAll();
        //读取上次配置语言id
        if (PlayerPrefs.HasKey("CUR_LANGUAGE_ID"))
        {
            //获取上次保存语言ID
            curLanguageIndex = PlayerPrefs.GetInt("CUR_LANGUAGE_ID");
            _curLanguage = _mLangList[curLanguageIndex];
        }
        else
        {
            curLanguageIndex = 0;
            _curLanguage = _mLangList[curLanguageIndex];
            PlayerPrefs.SetInt("CUR_LANGUAGE_ID", curLanguageIndex);
        }
        //解析完成后的回调
        if (onInitCallback != null)
        {
            onInitCallback();
        }
        if (callback != null)
        {
            callback();
        }
        DispatchChange();
    }

    /// <summary>
    /// 通过key获取相应的语言
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetValueByKey(string key)
    {
        if (!mDic.ContainsKey(_curLanguage))
        {
            Debug.LogWarning("无此语言配置");
            curLanguageIndex = 0;
            _curLanguage = _mLangList[curLanguageIndex];
            PlayerPrefs.SetInt("CUR_LANGUAGE_ID", curLanguageIndex);
            Debug.LogWarning(string.Format("无{0}语言", _curLanguage));
            return "无此语言:" + _curLanguage;
        }
        if (!mDic[_curLanguage].ContainsKey(key))
        {
            return "未找到Key:" + key;
        }
        return mDic[_curLanguage][key];
    }


    /// <summary>
    /// 切换语言
    /// </summary>
    /// <param name="language"></param>
    public void SwitchLanguage(string language)
    {
        if (!_curLanguage.Equals(language))
        {
            for (int i = 0; i < _mLangList.Length; i++)
            {
                if (_mLangList[i].Equals(language))
                {
                    curLanguageIndex = i;
                    _curLanguage = _mLangList[curLanguageIndex];
                    PlayerPrefs.SetInt("CUR_LANGUAGE_ID", curLanguageIndex);
                }
            }
            DispatchChange();
            Debug.Log("Switch Language: " + _curLanguage);
        }
    }


    public void SwitchLanguage()
    {

        if (curLanguageIndex + 1 > _mLangList.Length - 1)
        {
            curLanguageIndex = 0;
        }
        else
        {
            curLanguageIndex += 1;
        }

        _curLanguage = _mLangList[curLanguageIndex];
        PlayerPrefs.SetInt("CUR_LANGUAGE_ID", curLanguageIndex);
        Debug.Log("语言索引" + curLanguageIndex + _curLanguage);
        DispatchChange();

    }

    public void DispatchChange()
    {
        if (NetworkManager.Instance.TCPConnected)
        {
            NetMessage nm = new NetMessage(NetMessageId.SwitchLanguage);
            SwitchLanguageVo slv = new SwitchLanguageVo();
            slv.currentLanguage = _curLanguage;
            nm.WriteJson(JsonConvert.SerializeObject(slv));

            NetworkManager.Instance.SendNetMessage(nm);
        }


        if (OnLanguageChangeHandler != null)
        {
            OnLanguageChangeHandler(_curLanguage);
        }
    }
}
