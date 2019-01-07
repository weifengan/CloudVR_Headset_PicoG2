using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XTweenSlider : XTweener
{
	[Range(0, 1)]
	public float from;
	[Range(0, 1)]
	public float to;

	Slider _slider;

	protected override void CheckTweenTarget ()
	{
		if (_slider == null)
		{
			_slider = GetComponent<Slider>();
		}
	}

    public override void GetReady(bool forward)
    {
        CheckTweenTarget();
        _slider.value = forward ? from : to;
    }

    protected override void SetFinishedValue()
    {
        _slider.value = _reverse ? from : to;
    }

    protected override void OnUpdate()
	{
		CheckTweenTarget();
		_slider.value = Mathf.Lerp(from, to, _transition);
	}

	public void Play(float fromValue, float toValue, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		CheckTweenTarget();

		_slider.value = fromValue;

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

	public static XTweenSlider Tween(GameObject go, float fromValue, float toValue, float duraton,
                                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenSlider tweener = go.GetComponent<XTweenSlider>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenSlider>();
		}
		tweener.Play(fromValue, toValue, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}
