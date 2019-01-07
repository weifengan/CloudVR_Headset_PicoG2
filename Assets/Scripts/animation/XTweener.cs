using System;
using UnityEngine;
using UnityEngine.Events;


public enum XTweenerStyle
{
	Once,
	Loop,
	PingPong,
}

public abstract class XTweener : MonoBehaviour
{
	public XTweenerStyle style = XTweenerStyle.Once;
    public bool ignoreTimescale = true;
	public bool playOnEnable = false;
	public bool disableOnFinished = false;
	public float duration;
	public float delay;
	public AnimationCurve animCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
	public XOnTweenerFinished onFinished = new XOnTweenerFinished();
    public Action onFinishedEvent;  // GC friendly callback.
	protected float _transition;
	protected float _startTimestamp;
	protected bool _started = false;
	protected bool _reverse = false;

    protected float minTime = 0;
	protected float maxTime = 1;

    protected float _currentTime
    {
        get { return ignoreTimescale ? Time.realtimeSinceStartup : Time.time; }
    }

	public bool IsSrart()
    {
		return _started;
	}

	protected virtual void CheckTweenTarget()
	{

	}

    public virtual void GetReady(bool forward)
    {

    }

    protected virtual void SetFinishedValue()
    {

    }

	public virtual void PlayForward()
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		_started = true;
		_startTimestamp = _currentTime;
		_reverse = false;
		enabled = true;
		_transition = 0;
		OnUpdate();
	}

	public virtual void PlayReverse()
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		_started = true;
		_startTimestamp = _currentTime;
		_reverse = true;
		enabled = true;
		_transition = 1;
		OnUpdate();
	}

	protected virtual void Update()
	{
		if (!_started)
		{
            if (playOnEnable)
            {
                _started = true;
                _startTimestamp = Time.time;
            }
            else
            {
                enabled = false;
                return;
            }
		}

        float timeElaspe = _currentTime - _startTimestamp - delay;
		if (timeElaspe <= 0)
		{
			return;
		}

		float percent = timeElaspe / duration;
        percent = Mathf.Min(percent, maxTime);
		_transition = animCurve.Evaluate(Mathf.Clamp(_reverse ? maxTime - percent : percent, minTime, maxTime));

		OnUpdate();

		if (Mathf.Approximately(percent, maxTime))
		{
			if (style == XTweenerStyle.Once)
			{
				enabled = false;
				_started = false;
                ProfilerSample.BeginSample("Tweener.onFinished.InVoke");
				onFinished.Invoke();
                if (onFinishedEvent != null)
                {
                    onFinishedEvent();
                    onFinishedEvent = null;
                }
                ProfilerSample.EndSample();
                SetFinishedValue();
				if (disableOnFinished)
				{
					gameObject.SetActive(false);
				}
			}
			else if (style == XTweenerStyle.Loop)
			{
				_startTimestamp = _currentTime;
			}
			else
			{
				_startTimestamp = _currentTime;
                _reverse = !_reverse;
			}
		}
	}

	protected virtual void OnUpdate()
	{

	}

	[Serializable]
	public class XOnTweenerFinished : UnityEvent
	{
	}


}


