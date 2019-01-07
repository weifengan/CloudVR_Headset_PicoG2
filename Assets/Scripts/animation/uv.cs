using UnityEngine;
using System.Collections;


public class uv : XMonoBehaviour
{
    public bool bAnimationForever = true;
    public Vector2 beginOffset = Vector2.zero;
    public Vector2 endOffset = Vector2.one;
    public Vector2 calEndMotionOffset = Vector2.one;
    public float intervalTime = 3.0f;
    public float delayTime = 0.0f;
    
    public delegate void VoidDelegate();

    public VoidDelegate OnIntervalMotionEnded;
    public float x = 0.01f;
    public float y = 0.01f;
    private Vector2 offset;
    private bool bStopAnimator = false;
    private float nextUVTime;
    private bool bActionOnce = false;

    public void BeginUVAnimation()
    {
        bStopAnimator = false;
                
        if (!bAnimationForever)
        {
            nextUVTime = Time.time + delayTime;
        }
        
        cachedRenderer.enabled = false;   
        cachedRenderer.material.SetTextureOffset("_MainTex", beginOffset);
    }
    
    public void EndUVAnimation()
    {
        bStopAnimator = true;
        cachedRenderer.enabled = false;
    }
    
    void IntervalAnimation()
    {
        bActionOnce = true;
        nextUVTime = Time.time + intervalTime;

        cachedRenderer.enabled = true;
        cachedRenderer.material.SetTextureOffset("_MainTex", beginOffset);
    }
    
    void Start()
    {
        if (!bAnimationForever)
        {
            BeginUVAnimation();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (bStopAnimator)
        {
            return;
        }

        if (bAnimationForever)
        {
            offset = cachedRenderer.material.GetTextureOffset("_MainTex") + new Vector2(x, y);
            cachedRenderer.material.SetTextureOffset("_MainTex", offset);
        }
        else
        {
            if (nextUVTime < Time.time)
            {
                IntervalAnimation();
            }
            
            if (bActionOnce)
            {
                offset = cachedRenderer.material.GetTextureOffset("_MainTex") + new Vector2(x, y);
                cachedRenderer.material.SetTextureOffset("_MainTex", offset);
                
                if ((y > 0 && offset.y > endOffset.y) 
                    || (y < 0 && offset.y < endOffset.y)
                    || (x > 0 && offset.x > endOffset.x)
                    || (x < 0 && offset.x < endOffset.x))
                {
                    bActionOnce = false;
                    cachedRenderer.enabled = false;
                }
                
                if ((y > 0 && offset.y > calEndMotionOffset.y) 
                    || (y < 0 && offset.y < calEndMotionOffset.y)
                    || (x > 0 && offset.x > calEndMotionOffset.x)
                    || (x < 0 && offset.x < calEndMotionOffset.x))
                {
                    if (OnIntervalMotionEnded != null)
                    {
                        OnIntervalMotionEnded();
                    }
                }
            }
        }
    }
}
