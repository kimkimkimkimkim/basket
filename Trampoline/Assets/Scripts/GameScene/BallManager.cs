using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour {

	//public GameObject goalField;

	private GameObject canvasScore;
	private GameObject textCombo;
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


	// Use this for initialization
	void Start () {
		canvasScore = GameObject.Find("CanvasScore");
		textCombo = canvasScore.transform.GetChild(1).gameObject;
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

		if(col.gameObject.tag == "GoalBig"){
			Destroy(col.gameObject);
		}

		if(col.gameObject.tag == "GoalSmall"){
			Destroy(col.gameObject);
		}

		if(col.gameObject.tag == "Double"){
			Destroy(col.gameObject);
		}

		if(col.gameObject.tag == "Wall"){
			this.GetComponent<CircleCollider2D>().isTrigger = false;
			canChangeCol = false;
		}

	}

	void OnTriggerStay2D(Collider2D col){
		if(col.gameObject.tag == "Wall"){
			this.GetComponent<CircleCollider2D>().isTrigger = false;
			canChangeCol = false;
		}
	}

	void Point(Collider2D col){
		//Debug.Log ("point");
		//ゴールを削除
		/*
		if(col.transform.parent.parent.gameObject.tag == "MoveGoal"){
			Destroy(col.transform.parent.parent.gameObject);
		}else{
			Destroy(col.transform.parent.gameObject);
		}
		*/
		//ポイント反映
		camera.GetComponent<GameManager> ().UpdateScore (1);
		//camera.GetComponent<GameManager> ().CreateGoal ();

		//コンボテキスト関係
		textCombo.GetComponent<Text>().text = "+1";
		textCombo.GetComponent<Text>().color = new Color(0.95f,1,0,1);
		textCombo.SetActive(true);

		camera.GetComponent<GameManager>().PointSE();

    	camera.GetComponent<GameManager>().Vibration(1519); //振動

		//ボールをDestroy
		Destroy(this.gameObject);
	}

	void GameClear(){
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;

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
		//bは下降でtrue、上昇でfalse
		/*
		for (int j = 0; j < goalField.transform.childCount; j++) {
			BoxCollider2D[] bCol;
			EdgeCollider2D[] eCol;
			if(goalField.transform.GetChild(j).gameObject.tag == "Goal"){
				bCol = goalField.transform.GetChild(j).gameObject.GetComponents<BoxCollider2D> ();
				eCol = goalField.transform.GetChild(j).gameObject.GetComponents<EdgeCollider2D> ();
			}else{
				//movegoalの時
				bCol = goalField.transform.GetChild(j).GetChild(0).gameObject.GetComponents<BoxCollider2D> ();
				eCol = goalField.transform.GetChild(j).GetChild(0).gameObject.GetComponents<EdgeCollider2D> ();
			}
			for (int i = 0; i < bCol.Length; i++) {
				bCol [i].enabled = b;
			}
			for (int i = 0; i < eCol.Length; i++) {
				eCol [i].enabled = b;
			}
		}
		*/
		//this.GetComponent<CircleCollider2D>().enabled = b;
		this.GetComponent<CircleCollider2D>().isTrigger = !b;
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

		camera.GetComponent<GameManager>().JumpSE();

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
