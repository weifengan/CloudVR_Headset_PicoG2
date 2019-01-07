/*************************************
			
		LetinButton 
		
   @desction:
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetinButton : Button {

    private float ___delayTime = 0.1f;
    private bool ___canClicked = true;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (___canClicked)
        {
            ___canClicked = false;
            StopAllCoroutines();
            base.OnPointerClick(eventData);
            GameObject go = new GameObject("click");
            LetinButtonDelayClick lbd = go.AddComponent<LetinButtonDelayClick>();
            lbd.StartCoroutine(_____clickdelay(go, ___delayTime));
        }
       
    }

    private IEnumerator _____clickdelay(GameObject go,float delayTime)
    {
        ___canClicked = false;
        yield return new WaitForSeconds(delayTime);
        ___canClicked = true;
        Destroy(go);
    }
}
