using UnityEngine;
using System;


public class XBezierTweener : MonoBehaviour
{
	XBezier _bezier;

	public float duration;
	bool _start = false;
	Action _onFinished;

	//float _speed;
	//float _ratio;
	float _percentage;
	float _startTimestamp;

	void OnEnable()
	{
		_startTimestamp = Time.time;
	}

	void Update()
	{
		if (_bezier == null || !_start)
		{
			enabled = false;
			return;
		}

		_percentage = (Time.time - _startTimestamp) / duration;
		_percentage = Mathf.Clamp01(_percentage);
//		_speed += XGameSettings.instance.bezierAcc * (1 + _ratio);
//		_speed = Mathf.Min(_speed, XGameSettings.instance.bezierMaxSpeed);
//
//		_percentage += _speed;
//		_percentage = Mathf.Min(1, _percentage);

		transform.position = _bezier.GetPointAtTime(_percentage);
		if (_percentage == 1)
		{
			if (_onFinished != null) 
			{
				_onFinished();
			}
			enabled = false;
		}
	}

	public void Play(Vector3 theFrom, Vector3 theTo, Vector3 theHandlePoint1, Vector3 theHandlePoint2, float theDuration, Action onFinished = null)
	{
		//_ratio = (200 - (theTo - theFrom).sqrMagnitude) * 0.005f;
		//_speed = 0;
		_bezier = new XBezier(theFrom, theHandlePoint1, theHandlePoint2, theTo);
		duration = theDuration * (1 + (theTo - theFrom).sqrMagnitude * 0.005f);
		_onFinished = onFinished;
		_start = true;
		_percentage = 0;
		enabled = true;
	}
}

