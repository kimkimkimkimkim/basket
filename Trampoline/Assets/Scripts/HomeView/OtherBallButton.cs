﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherBallButton : MonoBehaviour {

	public GameObject canvasHome;
	public GameObject canvasOtherBall;
	public GameObject lineManager;

	public void OnClick(){
		canvasHome.SetActive(false);
		canvasOtherBall.SetActive(true);
		lineManager.GetComponent<drawPhysicsLine>().gamefinish = true;
	}
}
