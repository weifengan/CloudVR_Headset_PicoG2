using System;
using UnityEngine;


public class XTweenScale : XTweener
{
	public Vector3 from;
	public Vector3 to;

    public override void GetReady(bool forward)
    {
        transform.localScale = forward ? from : to;
    }

    protected override void SetFinishedValue()
    {
        transform.localScale = _reverse ? from : to;
    }

    protected override void OnUpdate()
	{
		transform.localScale = Vector3.LerpUnclamped(from, to, _transition);
	}

    public void Play(Vector3 fromScale, Vector3 toScale, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		transform.localScale = fromScale;
		from = fromScale;
		to = toScale;
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

	public static XTweenScale Tween(GameObject go, Vector3 fromScale, Vector3 toScale, float duraton,
                                    float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenScale tweener = go.GetComponent<XTweenScale>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenScale>();
		}
		tweener.Play(fromScale, toScale, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}


