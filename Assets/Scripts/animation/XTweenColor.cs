using UnityEngine;
using System.Collections;

public class XTweenColor : XTweener
{
	public Color from;
	public Color to;

	SpriteRenderer _spriteRenderer;
	Color _spriteColor;

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
        _spriteRenderer.color = forward ? from : to;
    }

    protected override void SetFinishedValue()
    {
        _spriteRenderer.color = _reverse ? from : to;
    }

    protected override void OnUpdate()
	{
		_spriteColor = Color.Lerp(from, to, _transition);
		_spriteRenderer.color = _spriteColor;
	}

	public void Play(Color fromColor, Color toColor, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		if (_spriteRenderer == null)
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		_spriteColor = _spriteRenderer.color;
		_spriteRenderer.color = _spriteColor;

		from = fromColor;
		to = toColor;
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

	public static XTweenColor Tween(GameObject go, Color fromColor, Color toColor, float duration,
                                    float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenColor tweener = go.GetComponent<XTweenColor>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenColor>();
		}
		tweener.Play(fromColor, toColor, duration, theDelay, theIgnoreTimescale);
		return tweener;
	}
}
