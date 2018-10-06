using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFieldManager : MonoBehaviour {

	public GameObject rowPrefab;
	public GameObject skinButtonPrefab;

	private int rowNum = 4;
	private int columnNum = 3;
	private int buttonCount = 0;

	// Use this for initialization
	void Start () {
		CreateBallField();
	}

	private void CreateBallField(){
		CreateRow();
		CreateColumn();
	}

	private void CreateRow(){
		for(int i=0;i<rowNum;i++){
			GameObject row = (GameObject)Instantiate(rowPrefab);
			row.transform.SetParent(transform);
			row.transform.localScale = new Vector3(1,1,1);
			row.transform.localPosition = new Vector3(0,0,0);
		}
	}

	private void CreateColumn(){
		GameObject parent;
		for(int i=0;i<rowNum;i++){
			parent = transform.GetChild(i).gameObject;
			for(int j=0;j<columnNum;j++){
				buttonCount++;
				GameObject button = (GameObject)Instantiate(skinButtonPrefab);
				button.transform.SetParent(parent.transform);
				button.transform.localScale = new Vector3(1,1,1);
				button.transform.localPosition = new Vector3(0,0,0);
				button.GetComponent<SkinButton>().myNum = buttonCount;
			}
		}
	}

}
