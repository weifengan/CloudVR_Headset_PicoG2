using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoTitleScroll :MonoBehaviour
{
    ScrollRect rect;
    GameObject roll;
    Text rollText;
    float rollSpeed = 0.3f;
    public  bool isRoll;

    private void Awake()
    {
        roll = transform.GetChild(0).gameObject;
        rollText = roll.GetComponent<Text>();
        //获取 ScrollRect变量
        rect = this.GetComponent<ScrollRect>();
    }

    public void DoPointerEnterHandler()
    {
        string temp = rollText.text;
        int count = Regex.Matches(temp, @"[a-z]").Count;
        float rollWeith = (count * 0.5f + (rollText.text.Length - count)) * rollText.fontSize;
        rollSpeed =Mathf.Abs(roll.GetComponent<RectTransform>().sizeDelta.x / rollWeith)/2;
        //Debug.Log("跑马灯速度：" + rollSpeed + "宽度：" + roll.GetComponent<RectTransform>().sizeDelta.x);
        //Debug.Log("文本字符长度：" + rollText.text.Length);
        roll.GetComponent<RectTransform>().sizeDelta = new Vector2(rollWeith*1.2f, this.GetComponent<RectTransform>().sizeDelta.y);
        //Debug.Log("文本大小：" + roll.GetComponent<RectTransform>().sizeDelta);
        roll.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        roll.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        rect.horizontalNormalizedPosition = 0;
        isRoll = true;
    }


    public void DoPointerExitHandler(float width)
    {
        isRoll = false;
        roll.GetComponent<RectTransform>().sizeDelta = new Vector2(width, roll.GetComponent<RectTransform>().sizeDelta.y);
        rect.horizontalNormalizedPosition = 0;
    }
    void Update()
    {
        if (isRoll)
        {
            ScrollValue();
        }     

    }

    private void ScrollValue()
    {
        if (rect.horizontalNormalizedPosition > 1.0f)
        {

            rect.horizontalNormalizedPosition = 0;

        }
        rect.horizontalNormalizedPosition = rect.horizontalNormalizedPosition + rollSpeed * Time.deltaTime;

    }

    public void OnScrollPointerEnter()
    {
        isRoll=true;
    }

    public void OnScrollPointerExit()
    {
        
    }
}
