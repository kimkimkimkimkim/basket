using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour {

	public int myNum;

	private GameObject canvasOtherball;
	private GameObject detailView;
	private GameObject text;
	private GameObject textProgress;
	private GameObject ballField;
	private GameObject canvasHome;
	private GameObject camera;

	private void Start(){
		camera = GameObject.Find("Main Camera");
		canvasHome = GameObject.Find("CanvasHome");
		canvasOtherball = GameObject.Find("CanvasOtherball");
		detailView = canvasOtherball.transform.Find("DetailView").gameObject;
		text = detailView.transform.Find("PopView").Find("Text").gameObject;
		textProgress = detailView.transform.Find("PopView").Find("TextProgress").gameObject;
		ballField = canvasOtherball.transform.Find("BallField").gameObject;
	}

	public void OnClick(){

		string key = string.Format("skin{0}",myNum);
		if(PlayerPrefs.GetInt(key) == 0){
			//解放されてない
			detailView.SetActive(true);
			string str = ballField.GetComponent<BallFieldManager>().strCondition[myNum];
			text.GetComponent<Text>().text = str;
			if(myNum>=1 && myNum<4){
				//プレイ回数
				int now = PlayerPrefs.GetInt("playcount");
				int condition = camera.GetComponent<GameManager>().playCountCondition[myNum-1];
				string strProgress = string.Format("{0}/{1}",now,condition);
				textProgress.GetComponent<Text>().text = strProgress;
			}else if(myNum>=4 && myNum<7){
				//スコア
				int now = PlayerPrefs.GetInt("best");
				int condition = camera.GetComponent<GameManager>().scoreCondition[myNum-4];
				string strProgress = string.Format("{0}/{1}",now,condition);
				textProgress.GetComponent<Text>().text = strProgress;
			}else if(myNum>=7 && myNum<10){
				//ログイン日数
				int now = PlayerPrefs.GetInt("logindays");
				int condition = camera.GetComponent<GameManager>().logindayseCondition[myNum-7];
				string strProgress = string.Format("{0}/{1}",now,condition);
				textProgress.GetComponent<Text>().text = strProgress;
			}else if(myNum == 10){
				//動画報酬
				int now = PlayerPrefs.GetInt("fevercount");
				int condition = camera.GetComponent<GameManager>().rewordCondition;
				string strProgress = string.Format("{0}/{1}",now,condition);
				textProgress.GetComponent<Text>().text = strProgress;
			}else{
				//スキン全開放
				int now = PlayerPrefs.GetInt("skinNum");
				int condition = 11;
				string strProgress = string.Format("{0}/{1}",now,condition);
				textProgress.GetComponent<Text>().text = strProgress;
			}
		}else{
			//解放されている
			ballField.GetComponent<BallFieldManager>().Refresh();
			transform.Find("select").gameObject.SetActive(true);
			PlayerPrefs.SetInt("selectedBall",myNum);
			canvasHome.GetComponent<CanvasHomeManager>().RefreshBallImage();
		}

	}

}
