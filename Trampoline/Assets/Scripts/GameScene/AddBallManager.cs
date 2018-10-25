using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddBallManager : MonoBehaviour {


	private GameObject canvasScore;
	private GameObject textCombo;
	private GameObject goalField;
	private GameObject camera;
	private float vel; //ballの速度
	private float boundPower = 8f;
	private float boundPowerSmall = 2f;
	private float maxBound = 9f;
	private float middleBound = 6f;
	private float minBound = 3f;
	private bool isRising = false;
	private bool isIn = false;
	private bool canChangeCol = true;
	private int point = 0;

	// Use this for initialization
	void Start () {
		canvasScore = GameObject.Find("CanvasScore");
		textCombo = canvasScore.transform.GetChild(1).gameObject;
		goalField = GameObject.Find("BasketGoal");
		camera = GameObject.Find ("Main Camera");
	}

	// Update is called once per frame
	void Update () {
		JudgeUpDown (); //上昇してるか下降してるか判定
	}


	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.tag == "Trampoline") {
			isIn = false;
			Bound (col);
		}

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Goal") {
			if(!isRising)isIn = true;
		}

		if (col.gameObject.tag == "Out") {
			if(isIn)Point (col);
		}

		if(col.gameObject.tag == "Wall"){
			this.GetComponent<CircleCollider2D>().isTrigger = false;
			canChangeCol = false;
		}

	}

	void Point(Collider2D col){
		//Debug.Log ("point");
		if(col.transform.parent.parent.gameObject.tag == "MoveGoal"){
			Destroy(col.transform.parent.parent.gameObject);
		}else{
			Destroy(col.transform.parent.gameObject);
		}

		//コンボの点数
		int combo = camera.GetComponent<GameManager>().combo;
		combo++;
		camera.GetComponent<GameManager>().combo = combo;
		//コンボテキスト関係
		textCombo.GetComponent<Text>().text = "+" + combo.ToString();
		textCombo.GetComponent<Text>().color = new Color(1,0,9f/255,147f/255);
		textCombo.SetActive(true);
		//Invoke("ComboTextFalse",1.0f);
		//ポイント反映
		camera.GetComponent<GameManager> ().UpdateScore (combo);
		camera.GetComponent<GameManager> ().CreateGoal ();

		camera.GetComponent<GameManager>().PointSE();

		camera.GetComponent<GameManager>().Vibration(1519); //振動

		//ボールをDestroy
		Destroy(this.gameObject);

	}

	private void ComboTextFalse(){
		textCombo.SetActive(false);
	}

	void JudgeUpDown(){
		vel = GetComponent<Rigidbody2D> ().velocity.y;

		if(vel >= 0)isRising = true;
		if (vel < 0)isRising = false;


		if(!isRising)canChangeCol = true;
		if(canChangeCol)ChangeColliderEnable (!isRising);


	}

	//goalに付いているコライダーのアクティブを変更
	void ChangeColliderEnable(bool b){

		this.GetComponent<CircleCollider2D>().isTrigger  = !b;
	}

	void GameClear(){
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;

	}

	void Bound(Collision2D col){
		Vector3 colAngle = col.transform.localEulerAngles;
		colAngle.z = TransformAngle(colAngle.z);

		gameObject.transform.localEulerAngles = colAngle;
		//長さによって速度変更
		float xScale = col.transform.localScale.x;
		if(xScale >= 3.0)xScale = 3.0f;
		float minLen = 0.2f;
		float maxLen = 3.0f;
		float minVel = 0.1f;
		float maxVel = 7.0f;
		float ratio = (xScale - minLen)/(maxLen - minLen);

		float speed = Mathf.Lerp(minVel,maxVel,ratio);

		GetComponent<Rigidbody2D>().velocity = transform.up * speed;

		//トランポリンを削除
		Destroy (col.gameObject);
	}

	float TransformAngle(float angle){
		if (angle >= -90 && angle <= 90) {
			return angle;
		} else if (angle > 90) {
			return TransformAngle(angle - 180);
		} else {
			return TransformAngle(angle + 180);
		}
	}
}
