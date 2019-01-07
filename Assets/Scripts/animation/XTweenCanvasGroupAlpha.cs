using System;
using UnityEngine.UI;
using UnityEngine;


public class XTweenCanvasGroupAlpha : XTweener
{
	[Range(0, 1)]
	public float from;
	[Range(0, 1)]
	public float to;

	CanvasGroup _canvasGroup;

	protected override void CheckTweenTarget()
	{
		if (_canvasGroup == null)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}
	}

    public override void GetReady(bool forward)
    {
        CheckTweenTarget();
        _canvasGroup.alpha = forward? from : to;
    }

    protected override void SetFinishedValue()
    {
        _canvasGroup.alpha = _reverse ? from : to;
    }

    protected override void OnUpdate()
	{
		CheckTweenTarget();
		_canvasGroup.alpha = Mathf.LerpUnclamped(from, to, _transition);
	}

	public void Play(float fromAlpha, float toAlpha, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		CheckTweenTarget();

		_canvasGroup.alpha = fromAlpha;

		from = fromAlpha;
		to = toAlpha;
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

	public static XTweenCanvasGroupAlpha Tween(GameObject go, float fromAlpha, float toAlpha,
                                               float duraton, float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenCanvasGroupAlpha tweener = go.GetComponent<XTweenCanvasGroupAlpha>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenCanvasGroupAlpha>();
		}
		tweener.Play(fromAlpha, toAlpha, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}


