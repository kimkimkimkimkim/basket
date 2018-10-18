using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingButton : MonoBehaviour {

	public GameObject canvasRanking;
	public GameObject lineManager;

	public void OnClick(){
		canvasRanking.SetActive(true);
		lineManager.GetComponent<drawPhysicsLine>().gamefinish = true;
	}
}
