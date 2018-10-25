using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingButton : MonoBehaviour {

	public GameObject canvasHome;
	public GameObject canvasRanking;
	public GameObject lineManager;

	public void OnClick(){
		canvasHome.SetActive(false);
		canvasRanking.SetActive(true);
		lineManager.GetComponent<drawPhysicsLine>().gamefinish = true;
	}
}
