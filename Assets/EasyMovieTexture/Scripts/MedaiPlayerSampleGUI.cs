using UnityEngine;
using System.Collections;

public class MedaiPlayerSampleGUI : MonoBehaviour {


	public MediaPlayerCtrl scrMedia;
	
	public bool m_bFinish = false;
	// Use this for initialization
	void Start () {
		scrMedia.OnEnd += OnEnd;

	}

	
	// Update is called once per frame
	void Update () {


	
	}
	#if !UNITY_WEBGL
	void OnGUI() {
		
	
		if (GUILayout.Button("Load"))
		{
			scrMedia.Load("EasyMovieTexture.mp4");
			m_bFinish = false;
		}
		
		if (GUILayout.Button("Play"))
		{
			scrMedia.Play();
			m_bFinish = false;
		}
	 	
		if( GUILayout.Button("stop"))
		{
			scrMedia.Stop();
		}
		
		if (GUILayout.Button("pause"))
		{
			scrMedia.Pause();
		}
		
		if (GUILayout.Button("Unload"))
		{
			scrMedia.UnLoad();
		}
		
		if( GUI.Button(new Rect(50,800,100,100), " " + m_bFinish))
		{
		
		}
		
		if (GUILayout.Button("SeekTo"))
		{
			scrMedia.SeekTo(10000);
		}


		
            GUILayout.Label("GetVideoWidth:" + scrMedia.GetVideoWidth().ToString());
            GUILayout.Label("GetVideoHeight:" + scrMedia.GetVideoHeight().ToString());
            GUILayout.Label("SeekPercent:" + scrMedia.GetCurrentSeekPercent().ToString());
            GUILayout.Label("GetCurrentState:" + scrMedia.GetCurrentState().ToString());

       




    }
	#endif


	
	void OnEnd()
	{
		m_bFinish = true;
	}
}
