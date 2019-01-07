using System;
using UnityEngine;
using UnityEngine.UI;

public class XTweenImageAlpha : XTweener
{
	[Range(0, 1)]
	public float from;
	[Range(0, 1)]
	public float to;

	Graphic _image;
	Color _spriteColor = Color.white;

	protected override void CheckTweenTarget ()
	{
		if (_image == null)
		{
			_image = GetComponent<Graphic>();
		}
	}

    public override void GetReady(bool forward)
    {
        CheckTweenTarget();
        Color col = _image.color;
        col.a = forward ? from : to;
        _image.color = col;
    }

    protected override void SetFinishedValue()
    {
        Color col = _image.color;
        col.a = _reverse ? from : to;
        _image.color = col;
    }

    protected override void OnUpdate()
	{
		CheckTweenTarget();
		_spriteColor.a = Mathf.LerpUnclamped(from, to, _transition);
        _image.color = _spriteColor;
	}

	public void Play(float fromAlpha, float toAlpha, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		CheckTweenTarget();

		_spriteColor = _image.color;
		_spriteColor.a = fromAlpha;
		_image.color = _spriteColor;

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

    [ContextMenu("PlayForward")]
    public override void PlayForward ()
	{
		CheckTweenTarget();
		_spriteColor = _image.color;
		base.PlayForward ();
	}

    [ContextMenu("PlayReverse")]
	public override void PlayReverse ()
	{
		CheckTweenTarget();
		_spriteColor = _image.color;
		base.PlayReverse ();
	}

	public static XTweenImageAlpha Tween(GameObject go, float fromAlpha, float toAlpha, float duraton,
                                         float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenImageAlpha tweener = go.GetComponent<XTweenImageAlpha>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenImageAlpha>();
		}
		tweener.Play(fromAlpha, toAlpha, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}


