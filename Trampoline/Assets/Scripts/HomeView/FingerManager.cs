using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerManager : MonoBehaviour {

	private Vector3 pos;

	// Use this for initialization
	void Start () {
		pos = this.gameObject.GetComponent<RectTransform>().localPosition;
		StartMovePos();
	}
	
	void OnEnable(){
		CancelInvoke();
		//StartMovePos();
	}

	void OnDisable(){
		Debug.Log("cancelinvoke");
		CancelInvoke();
	}

	void UpdatePos(float x){
		this.gameObject.GetComponent<RectTransform>().localPosition
		 = new Vector3(x,pos.y,pos.z);
	}

	void StartMovePos(){
		iTween.ValueTo(gameObject,iTween.Hash("from",-163.5,"delay",0.3f
			,"to",210,"onupdate","UpdatePos","onupdatetarget",gameObject,"time",1.5
			,"oncomplete","StartMovePos","oncompletetarget",gameObject));
		
		//Invoke("StartMovePos",1.8f);
	}


}
