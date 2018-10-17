using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallFieldManager : MonoBehaviour {

	public GameObject camera;
	public GameObject rowPrefab;
	public GameObject skinButtonPrefab;
	public Sprite[] ballSkin = new Sprite[12];
	public Color noColor;
	public string[] strCondition = new string[12];

	private int rowNum = 4;
	private int columnNum = 3;
	private int buttonCount = -1;
	private int selectedBall;

	// Use this for initialization
	void Start () {
		CreateBallField();
		//プレイ回数
		for(int i=1;i<4;i++){
			int num = camera.GetComponent<GameManager>().playCountCondition[i-1];
			string str = string.Format("プレイ回数\n{0}回突破",num);
			strCondition[i] = str;
		}
		//スコア
		for(int i=4;i<7;i++){
			int num = camera.GetComponent<GameManager>().scoreCondition[i-4];
			string str = string.Format("ベストスコア\n{0}以上",num);
			strCondition[i] = str;
		}
		//ログイン日数
		for(int i=7;i<10;i++){
			int num = camera.GetComponent<GameManager>().logindayseCondition[i-7];
			string str = string.Format("ログイン日数\n{0}日以上",num);
			strCondition[i] = str;
		}
		//動画報酬
		int rnum = camera.GetComponent<GameManager>().rewordCondition;
		string rstr = string.Format("動画視聴回数\n{0}回以上",rnum);
		strCondition[10] = rstr;
		//全スキン開放
		strCondition[11] = "全部のスキン開放";

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

				//ボタンの画像変更
				button.transform.GetChild(1).GetComponent<Image>().sprite = ballSkin[buttonCount];
				button.transform.GetChild(1).GetComponent<Image>().color = noColor;

				//選択しているかどうかで分岐
				selectedBall = PlayerPrefs.GetInt("selectedBall");
				if(buttonCount == selectedBall){
					button.transform.Find("select").gameObject.SetActive(true);
				}

				//解放しているかどうかで分岐
				string key = string.Format("skin{0}",buttonCount);
				if(PlayerPrefs.GetInt(key) == 1){
					//解放していたら
					button.transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,1);
				}
			}
		}
	}

	public void Refresh(){
		GameObject row;
		for(int i=0;i<rowNum;i++){
			row = transform.GetChild(i).gameObject;
			for(int j=0;j<columnNum;j++){
				GameObject button = row.transform.GetChild(j).gameObject;
				button.transform.Find("select").gameObject.SetActive(false);
			}
		}
	}

}
