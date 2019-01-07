using System;
using UnityEngine;


public class XTweenPosition : XTweener
{
	public Vector3 from;
	public Vector3 to;

    public override void GetReady(bool forward)
    {
        transform.localPosition = forward ? from : to;
    }

    protected override void SetFinishedValue()
    {
        transform.localPosition = _reverse ? from : to;
    }

    protected override void OnUpdate()
	{
		transform.localPosition = Vector3.LerpUnclamped(from, to, _transition);
	}

    public void Play(Vector3 fromPos, Vector3 toPos, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		transform.localPosition = fromPos;
		from = fromPos;
		to = toPos;
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

	[ContextMenu("Play")]
	public void PlayTest()
	{
		_reverse = false;
		enabled = true;
		_started = true;
		_startTimestamp = _currentTime;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
	}

	public static XTweenPosition Tween(GameObject go, Vector3 fromPos, Vector3 toPos, float duraton,
                                       float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenPosition tweener = go.GetComponent<XTweenPosition>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenPosition>();
		}
		tweener.Play(fromPos, toPos, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}


