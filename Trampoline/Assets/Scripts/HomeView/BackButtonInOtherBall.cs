using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonInOtherBall : MonoBehaviour {

	public GameObject canvasOtherBall;
	public GameObject lineManager;

	public void OnClick(){
		canvasOtherBall.SetActive(false);
		lineManager.GetComponent<drawPhysicsLine>().gamefinish = false;
	}
}
