/*************************************
			
		TTTT 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTT : MonoBehaviour {


    public Camera m_MainCamera;
	// Use this for initialization
	void Start () {
        
	}
	

    void OnGUI()
    {
        //普通3DLR
        if (GUILayout.Button("Normal3DLR"))
        {
            SetPlayMode3DLR();
           
        }
        if (GUILayout.Button("3D UD"))
        {
            SetPlayMode3DUD();

        }
        //普通3DLR
        if (GUILayout.Button("普通2D"))
        {
            SetPlayMode2D();

        }
    }

    public enum MediaScreenType
    {
        Screen2D,Screen3DLR,Screen3DUP
    }

    public void SetPlayMode3DLR()
    {
        if (m_MainCamera != null)
        {
            Camera leftCamera = m_MainCamera.transform.Find("LeftEye").GetComponent<Camera>();
            leftCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenRight"));
            Camera RightCamera = m_MainCamera.transform.Find("RightEye").GetComponent<Camera>();
            RightCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenLeft"));
         }
     }

    public void SetPlayMode3DUD()
    {
        if (m_MainCamera != null)
        {
            Camera leftCamera = m_MainCamera.transform.Find("LeftEye").GetComponent<Camera>();
            leftCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenDown"));
            Camera RightCamera = m_MainCamera.transform.Find("RightEye").GetComponent<Camera>();
            RightCamera.cullingMask = ~(1 << LayerMask.NameToLayer("ScreenTop"));
        }
    }

    public void SetPlayMode2D()
    {
        if (m_MainCamera != null)
        {
            Camera leftCamera = m_MainCamera.transform.Find("LeftEye").GetComponent<Camera>();
            leftCamera.cullingMask = -1;
            Camera RightCamera = m_MainCamera.transform.Find("RightEye").GetComponent<Camera>();
            RightCamera.cullingMask = -1;
        }
    }







    // Update is called once per frame
    void Update () {
		
	}
}
