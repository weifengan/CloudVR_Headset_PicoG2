using System;
using UnityEngine;


public class XTweenRotation : XTweener
{
	public Vector3 from;
	public Vector3 to;

    public override void GetReady(bool forward)
    {
        transform.localRotation = forward ? Quaternion.Euler(from) : Quaternion.Euler(to);
    }

    protected override void SetFinishedValue()
    {
        transform.localRotation = _reverse ? Quaternion.Euler(from) : Quaternion.Euler(to);
    }

    protected override void OnUpdate()
	{
		transform.localRotation = Quaternion.Euler(new Vector3(
													Mathf.Lerp(from.x, to.x, _transition),
													Mathf.Lerp(from.y, to.y, _transition),
													Mathf.Lerp(from.z, to.z, _transition)));
	}

	public void Play(Vector3 fromAngle, Vector3 toAngle, float theDuration,
                     float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		transform.localRotation = Quaternion.Euler(fromAngle);
		from = fromAngle;
		to = toAngle;
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

	public static XTweenRotation Tween(GameObject go, Vector3 fromAngle, Vector3 toAngle, float duraton,
                                       float theDelay = 0f, bool theIgnoreTimescale = true)
	{
		XTweenRotation tweener = go.GetComponent<XTweenRotation>();
		if (tweener == null)
		{
			tweener = go.AddComponent<XTweenRotation>();
		}
		tweener.Play(fromAngle, toAngle, duraton, theDelay, theIgnoreTimescale);
		return tweener;
	}
}


