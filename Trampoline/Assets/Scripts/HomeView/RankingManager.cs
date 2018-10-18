using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour {

	public GameObject rankingSpace;
	public GameObject firebaseManager;

	void OnEnable(){
		Debug.Log("onenbale");
		firebaseManager.GetComponent<FirebaseManager>().getRanking();
  }
}
