using UnityEngine;
using UnityEngine.UI;
using System;

public class XTweenFillAmount: XTweener
{
	[Range(0, 1)]
	public float from;
	[Range(0, 1)]
	public float to;

	Image _image;

    public Action<float> onValueChanged;

	protected override void CheckTweenTarget ()
	{
		if (_image == null)
		{
			_image = GetComponent<Image>();
		}
	}

    public override void GetReady(bool forward)
    {
        CheckTweenTarget();
        _image.fillAmount = forward ? from : to;
    }

    protected override void SetFinishedValue()
    {
        _image.fillAmount = _reverse ? from : to;
    }

    protected override void OnUpdate()
	{
		CheckTweenTarget();
        // OPTIMIZE@yangwei: GC Warning of change image fillamount.
		_image.fillAmount= Mathf.Lerp(from, to, _transition);
        if (onValueChanged != null)
        {
            onValueChanged(_image.fillAmount);
        }
	}

	public void Play(float fromValue, float toValue, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		CheckTweenTarget();

		_image.fillAmount = fromValue;

		from = fromValue;
		to = toValue;
		duration = theDuration;
		delay = theDelay;
        ignoreTimescale = theIgnoreTimescale;
		_reverse = false;
		enabled = true;
		_started = true;
		_startTimestamp = _currentTime;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
	}

	public static XTweenFillAmount Tween(GameObject go, float fromValue, float toValue, float duraton,
                                         float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenFillAmount tweener = go.GetComponent<XTweenFillAmount>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenFillAmount>();
		}
		tweener.Play(fromValue, toValue, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}
