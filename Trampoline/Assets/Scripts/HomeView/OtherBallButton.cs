using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherBallButton : MonoBehaviour {

	public GameObject canvasOtherBall;

	public void OnClick(){
		canvasOtherBall.SetActive(true);
	}
}
