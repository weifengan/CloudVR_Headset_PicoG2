using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class XBezierTool : MonoBehaviour 
{
	public LineRenderer lineRenderer;
	public Transform startPoint;
	public Transform endPoint;
	public Transform point1;
	public Transform point2;
	public int pointCount = 100;

	public Transform gizmo;

	void Start()
	{
		lineRenderer.SetVertexCount(pointCount);
		lineRenderer.sortingLayerName = "UI";
		lineRenderer.sortingOrder = 100;
		//StartCoroutine(MoveGizmo());
	}

	void Update () 
	{
		XBezier bezier = new XBezier(startPoint.position, point1.position, point2.position, endPoint.position);
		for (int i = 0; i < pointCount; i++) 
		{
			Vector3 position = bezier.GetPointAtTime(i / (float)pointCount);
			lineRenderer.SetPosition(i, position);
		}
	}

	[ContextMenu("Move")]
	void ShowGizmo()
	{
		StopAllCoroutines();
		StartCoroutine(MoveGizmo());
	}

	IEnumerator MoveGizmo()
	{
		int i = 0;
		while (true) 
		{
			i = i % pointCount;
			XBezier bezier = new XBezier(startPoint.position, point1.position, point2.position, endPoint.position);
			Vector3 position = bezier.GetPointAtTime(i / (float)pointCount);
			gizmo.transform.position = position;
			i++;
			yield return null;
		}
	}
}
