using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {

	//画像
	public Sprite[] mySp = new Sprite[2];
	public GameObject camera;

	private int state = 1;

	void Start(){
		state = PlayerPrefs.GetInt("sound",1);
		this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = mySp[state]; //画像を変更
	}

	public void OnClick(){
		state = state * (-1) + 1; //０だったら１に、１だったら０に
		this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = mySp[state]; //画像を変更
		PlayerPrefs.SetInt("sound",state); //ppに保存
		if(state==1){
			camera.GetComponent<GameManager>().PointSE();
		}
	}
}
