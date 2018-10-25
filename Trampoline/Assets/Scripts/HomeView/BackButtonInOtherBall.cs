using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonInOtherBall : MonoBehaviour {

	public GameObject canvasHome;
	public GameObject canvasOtherBall;
	public GameObject lineManager;

	public void OnClick(){
		canvasHome.SetActive(true);
		canvasOtherBall.SetActive(false);
		lineManager.GetComponent<drawPhysicsLine>().gamefinish = false;
	}
}
