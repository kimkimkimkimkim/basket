using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour {

	public int myNum;

	private GameObject canvasOtherball;
	private GameObject detailView;
	private GameObject text;
	private GameObject ballField;
	private GameObject canvasHome;

	private void Start(){
		canvasHome = GameObject.Find("CanvasHome");
		canvasOtherball = GameObject.Find("CanvasOtherball");
		detailView = canvasOtherball.transform.Find("DetailView").gameObject;
		text = detailView.transform.Find("PopView").Find("Text").gameObject;
		ballField = canvasOtherball.transform.Find("BallField").gameObject;
	}

	public void OnClick(){

		string key = string.Format("skin{0}",myNum);
		if(PlayerPrefs.GetInt(key) == 0){
			//解放されてない
			detailView.SetActive(true);
			string str = string.Format("requirement（{0}）",myNum);
			text.GetComponent<Text>().text = str;
		}else{
			//解放されている
			ballField.GetComponent<BallFieldManager>().Refresh();
			transform.Find("select").gameObject.SetActive(true);
			PlayerPrefs.SetInt("selectedBall",myNum);
			canvasHome.GetComponent<CanvasHomeManager>().RefreshBallImage();
		}

	}

}
