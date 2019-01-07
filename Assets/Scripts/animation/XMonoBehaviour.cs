/************************************************
 * Used for caching some frequent used components.
 ************************************************
*/
using UnityEngine;


public class XMonoBehaviour : MonoBehaviour
{
    Transform __transform;
    public new Transform transform
    {
        get 
        {
            if (__transform == null) 
            {
                __transform = base.transform;
            }
            return __transform;
        }
    }

    GameObject __gameObject;
    public new GameObject gameObject
    {
        get
        {
            if (__gameObject == null) 
            {
                __gameObject = base.gameObject;
            }
            return __gameObject;
        }
    }

    Camera __camera;
    public Camera cachedCamera
    {
        get
        {
            if (__camera == null) 
            {
				__camera = base.GetComponent<Camera>();
            }
            return __camera;
        }
    }

    ParticleSystem __particalSystem;
    public ParticleSystem cachedParticleSystem
    {
        get
        {
            if (__particalSystem == null) 
            {
                __particalSystem = base.GetComponent<ParticleSystem>();
            }
            return __particalSystem;
        }
    }

    ParticleEmitter __particalEmitter;
    public ParticleEmitter cachedParticleEmitter
    {
        get
        {
            if (__particalEmitter == null) 
            {
                __particalEmitter = base.GetComponent<ParticleEmitter>();
            }
            return __particalEmitter;
        }
    }

    AudioSource __audio;
    public AudioSource cachedAudio
    {
        get
        {
            if (__audio == null)
            {
                __audio = base.GetComponent<AudioSource>();
            }
            return __audio;
        }
    }

    Animation __animation;
    public Animation cachedAnimation
    {
        get
        {
            if (__animation == null)
            {
                __animation = base.GetComponent<Animation>();
            }
            return __animation;
        }
    }

    Collider __collider;
    public Collider cachedCollider
    {
        get
        {
            if (__collider == null) 
            {
                __collider = base.GetComponent<Collider>();
            }
            return __collider;
        }
    }

    Collider2D __collider2D;
    public Collider2D cachedCollider2D
    {
        get
        {
            if (__collider2D == null)
            {
                __collider2D = base.GetComponent<Collider2D>();
            }
            return __collider2D;
        }
    }

    Light __light;
    public Light cachedLight
    {
        get
        {
            if (__light == null) 
            {
                __light = base.GetComponent<Light>();
            }            
            return __light;
        }
    }

    Renderer __renderer;
    public Renderer cachedRenderer
    {
        get
        {
            if (__renderer == null) 
            {
                __renderer = base.GetComponent<Renderer>();
            }
            return __renderer;
        }
    }

    Rigidbody __rigidbody;
    public Rigidbody cachedRigidbody
    {
        get
        {
            if (__rigidbody == null) 
            {
                __rigidbody = base.GetComponent<Rigidbody>();
            }
            return __rigidbody;
        }
    }

    Rigidbody2D __rigidbody2D;
    public Rigidbody2D cachedRigidbody2D
    {
        get
        {
            if (__rigidbody2D == null) 
            {
                __rigidbody2D = base.GetComponent<Rigidbody2D>();
            }
            return __rigidbody2D;
        }
    }

    Transform[] __cachedChildren;
    public Transform[] cachedChildren
    {
        get
        {
            if (__cachedChildren == null) 
            {
                __cachedChildren = GetComponentsInChildren<Transform>(true);
            }
            return __cachedChildren;
        }
    }

    public Transform FindCachedChild(string childName)
    {
        foreach (var child in cachedChildren)
        {
            if (child.name == childName) 
            {
                return child;
            }
        }
        return null;
    }

    public Transform FindCachedChildPrecisely(string childName, string parentName)
    {
        foreach (var child in cachedChildren)
        {
            Transform parent = child.parent;
            if (child.name == childName && parent!= null && parent.name == parentName) 
            {
                return child;
            }
        }
        return null;
    }
}