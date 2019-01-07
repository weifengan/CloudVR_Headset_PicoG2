using System;
using UnityEngine;

public class XGravityAnim : MonoBehaviour
{
    public XTweenerStyle style = XTweenerStyle.Once;
    public bool disableOnFinished;
    public bool ignoreTimescale;
    public float xVelocityMin;
    public float xVelocityMax;
    public float yVelocityMin;
    public float yVelocityMax;
    public float startPosXMin;
    public float startPosXMax;
    public float startPosY;
    public float gravity = -10;
    private float _xVelocity;
    private float _yVelocity;

    private bool _started;
    private float _startTimestamp;
    private Vector3 _startPos;

    protected float _currentTime
    {
        get { return ignoreTimescale ? Time.realtimeSinceStartup : Time.time; }
    }

    private void Update()
    {
        if (!_started)
        {
            enabled = false;
            return;
        }


        float timeElaspe = _currentTime - _startTimestamp;

        float xDistance = _xVelocity * timeElaspe;
        float yDistance = _yVelocity * timeElaspe + 0.5f * gravity * timeElaspe * timeElaspe;
        transform.localPosition = _startPos + new Vector3(xDistance, yDistance, 0);

        if (transform.localPosition.y <= _startPos.y)
        {
            if (style == XTweenerStyle.Once)
            {
                enabled = false;
                _started = false;
                if (disableOnFinished)
                {
                    gameObject.SetActive(false);
                }
            }
            else if (style == XTweenerStyle.Loop)
            {
                Play();
            }
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        _startPos = new Vector3(UnityEngine.Random.Range(startPosXMin, startPosXMax), startPosY, 0);
        if (_startPos.x < 0)
        {
            _xVelocity = UnityEngine.Random.Range(0, xVelocityMax);
        }
        else
        {
            _xVelocity = UnityEngine.Random.Range(xVelocityMin , 0);
        }
        _yVelocity = UnityEngine.Random.Range(yVelocityMin, yVelocityMax);
        transform.localPosition = _startPos;
        _startTimestamp = _currentTime;
        _started = true;
        enabled = true;
        gameObject.SetActive(true);
    }
}

