using UnityEngine;
using System.Collections;

public class XTweenBrightness : XTweener
{
#if UNITY_EDITOR
    [Range(0, 1)]
#endif
    public float from;
#if UNITY_EDITOR
    [Range(0, 1)]
#endif
    public float to;

	SpriteRenderer _spriteRenderer;
	Color _spriteColor;

	protected override void CheckTweenTarget ()
	{
		if (_spriteRenderer == null)
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}

    public override void GetReady(bool forward)
    {
        CheckTweenTarget();

        _spriteColor = _spriteRenderer.color;
        _spriteColor.b = forward? from : to;
        _spriteRenderer.color = _spriteColor;
    }

    protected override void SetFinishedValue()
    {
        Color col = _spriteRenderer.color;
        col.b = _reverse ? from : to;
        _spriteRenderer.color = col;
    }

    protected override void OnUpdate()
	{
		CheckTweenTarget();
		_spriteColor.b = Mathf.LerpUnclamped(from, to, _transition);
		_spriteRenderer.color = _spriteColor;
	}

	public void Play(float fromBrightness, float toBrightness, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		CheckTweenTarget();

		_spriteColor = _spriteRenderer.color;
		_spriteColor.b = fromBrightness;
		_spriteRenderer.color = _spriteColor;

		from = fromBrightness;
		to = toBrightness;
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

	public override void PlayForward ()
	{
		CheckTweenTarget();
		_spriteColor = _spriteRenderer.color;
		base.PlayForward();
	}

	public override void PlayReverse ()
	{
		CheckTweenTarget();
		_spriteColor = _spriteRenderer.color;
		base.PlayReverse();
	}

	public static XTweenBrightness Tween(GameObject go, float fromBrightness, float toBrightness,
                                         float duraton, float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenBrightness tweener = go.GetComponent<XTweenBrightness>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenBrightness>();
		}
		tweener.Play(fromBrightness, toBrightness, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}
