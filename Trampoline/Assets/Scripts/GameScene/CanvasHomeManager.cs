using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasHomeManager : MonoBehaviour {

	public GameObject textDraw;
	public GameObject buttonOtherBall;
	public GameObject ballField;
	public GameObject inputField;

	// Use this for initialization
	void Start () {
		FadeOutText();
		RefreshBallImage();
		inputField.GetComponent<InputField>().text = PlayerPrefs.GetString("username","No Name");
	}

	// Update is called once per frame
	void Update () {

	}

	private void UpdateTextAlfa(float a){
		Color c = textDraw.GetComponent<TextMeshProUGUI>().color;
		textDraw.GetComponent<TextMeshProUGUI>().color = new Color(c.r,c.g,c.b,a);
	}

	private void FadeInText(){
		iTween.ValueTo(gameObject, iTween.Hash("from",0,"to",1,
		"onupdate","UpdateTextAlfa","onupdatetarget",gameObject,
		"oncomplete","FadeOutText","oncompletetarget",gameObject));
	}

	private void FadeOutText(){
		iTween.ValueTo(gameObject, iTween.Hash("from",1,"to",0,
		"onupdate","UpdateTextAlfa","onupdatetarget",gameObject,
		"oncomplete","FadeInText","oncompletetarget",gameObject));
	}

	public void RefreshBallImage(){
		int num = PlayerPrefs.GetInt("selectedBall");

		buttonOtherBall.transform.GetChild(0).gameObject.GetComponent<Image>().
			sprite = ballField.GetComponent<BallFieldManager>().ballSkin[num];
	}

	//InputField用のメソッド
	public void UpdateText(){
		PlayerPrefs.SetString("username",inputField.GetComponent<InputField>().text);
	}

}
