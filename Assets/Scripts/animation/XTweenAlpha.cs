using UnityEngine;
using System.Collections;

public class XTweenAlpha : XTweener
{
	[Range(0, 1)]
	public float from;
	[Range(0, 1)]
	public float to;

	SpriteRenderer _spriteRenderer;

	protected override void CheckTweenTarget()
	{
		if (_spriteRenderer == null)
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}

    public override void GetReady(bool forward)
    {
        CheckTweenTarget();
        Color col = _spriteRenderer.color;
        col.a = forward? from : to;
        _spriteRenderer.color = col;
    }

    protected override void SetFinishedValue()
    {
        Color col = _spriteRenderer.color;
        col.a = _reverse ? from : to;
        _spriteRenderer.color = col;
    }

    protected override void OnUpdate()
	{
		CheckTweenTarget();
        Color col = _spriteRenderer.color;
		col.a = Mathf.LerpUnclamped(from, to, _transition);
		_spriteRenderer.color = col;
	}

	public override void PlayForward ()
	{
		CheckTweenTarget();
		base.PlayForward ();
	}

	public override void PlayReverse ()
	{
		CheckTweenTarget();
		base.PlayReverse ();
	}

	public void Play(float fromAlpha, float toAlpha, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		CheckTweenTarget();

        Color col = _spriteRenderer.color;
        col.a = fromAlpha;
		_spriteRenderer.color = col;

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

	public static XTweenAlpha Tween(GameObject go, float fromAlpha, float toAlpha, float duraton,
                                    float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenAlpha tweener = go.GetComponent<XTweenAlpha>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenAlpha>();
		}
		tweener.Play(fromAlpha, toAlpha, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}
