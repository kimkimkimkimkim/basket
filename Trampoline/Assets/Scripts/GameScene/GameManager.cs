using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PPKey {
	best,
	/*
	skinは
	skin1,
	skin2, ・・・
	のようにkeyを設定
	*/
	selectedBall,
}

public class GameManager : MonoBehaviour {

	public AudioClip jumpSE;
	public AudioClip pointSE;
	public AudioSource audioSource;
	public GameObject[] itemPrefab = new GameObject[3];
	public GameObject addballPregab;
	public GameObject ballPrefab;
	public GameObject gameoverView;
	public GameObject basketGoal;
	public GameObject goalPrefab;
	public GameObject movegoalPrefab;
	public GameObject textScore;
	public GameObject canvasHome;
	public GameObject wall;
	public GameObject ballField;
	public GameObject feverBlack;
	public GameObject neon;
	public GameObject ballskinField;
	public Vector3[] goalPosition = new Vector3[5];
	public bool isFever = false;
	public int[] goalposCheck = {0,0,0,0,0};
	public int combo = 0;

	private GameObject drawLine;
	private float timeSpan = 5.0f;
	private float goalTimeSpan = 20.0f;
	private float itemTimeSpan = 5.0f;
	private float feverBallSpan = 0.5f;
	private float feverTime = 10.0f;
	private bool gamefinish = false;
	private int point = 0;
	private int ballCount = 0;
	private int feverCount = 0;
	private int maxFeverCount = 10;

	// Use this for initialization
	void Start () {
		drawLine = GameObject.Find ("drawPhysicsLine");
		//Invoke("CreateItem",5);
		PlayerPrefs.SetInt("skin0",1);
		PlayerPrefs.SetInt("skin1",1);
		PlayerPrefs.SetInt("skin2",1);
		PlayerPrefs.SetInt("skin6",1);
	}

	// Update is called once per frame
	void Update () {

	}

	public void GameStart(){

		canvasHome.SetActive(false);
		Vector3 wallPos = wall.transform.localPosition;
		wall.transform.localPosition = new Vector3(wallPos.x,wallPos.y,0);

		textScore.SetActive(true);

		CreateGoal();
		CreateGoal();

		CreateBall();

		Invoke("GoalSpan",goalTimeSpan);

	}

	private void CreateBall(){

		ballCount++;
		if(ballCount % 5 == 0)timeSpan = (timeSpan-0.2f > 2) ? timeSpan - 0.2f : 2;

		GameObject ball = (GameObject)Instantiate(ballPrefab);
		ball.transform.SetParent(ballField.transform);
		//addball.transform.localPosition = new Vector3(Random.Range(-2.0f,2.3f),5.71f,0);
		ball.transform.localPosition = new Vector3(-2.08f,-4.18f,0);
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2(1,4);
		ball.GetComponent<SpriteRenderer>().sprite = ballskinField.GetComponent<BallFieldManager>().ballSkin[PlayerPrefs.GetInt("selectedBall")];

		if(!gamefinish){
			Invoke("CreateBall",timeSpan);
		}
	}

	private void CreateAddBall(){

		GameObject addball = (GameObject)Instantiate(addballPregab);
		addball.transform.SetParent(ballField.transform);
		//addball.transform.localPosition = new Vector3(Random.Range(-2.0f,2.3f),5.71f,0);
		addball.transform.localPosition = new Vector3(-2.08f,-4.18f,0);
		addball.GetComponent<Rigidbody2D>().velocity = new Vector2(1,4);

		if(!gamefinish){
			Invoke("CreateAddBall",feverBallSpan);
		}
	}

	private void GoalSpan(){
		CreateGoal();

		DeleteGoal();

		if(!gamefinish){
			Invoke("GoalSpan",goalTimeSpan);
		}
	}

	private void DeleteGoal(){
		int count = basketGoal.transform.childCount - 1;
		int target = Random.Range(0,count);
		Destroy(basketGoal.transform.GetChild(target).gameObject);
	}

	private void CreateItem(){
		int target = Random.Range(0,itemPrefab.Length);
		GameObject item = (GameObject)Instantiate(itemPrefab[target]);
		item.transform.localPosition = new Vector3(Random.Range(-2.0f,2.3f),5.71f,0);

		if(!gamefinish){
			Invoke("CreateItem",itemTimeSpan);
		}
	}

	public void GameOver(){
		//線を引けなくする
		drawLine.GetComponent<drawPhysicsLine> ().gamefinish = true;
		gamefinish = true;


		//GameOverUIを表示
		int score = int.Parse(textScore.GetComponent<Text>().text);
		int best = PlayerPrefs.GetInt(PPKey.best.ToString());
		if(score > best)best = score;
		PlayerPrefs.SetInt(PPKey.best.ToString(), best);

		gameoverView.SetActive(true);
		gameoverView.transform.Find("TextScore").GetChild(0).GetComponent<TextMeshProUGUI>().text
			= score.ToString();
		gameoverView.transform.Find("TextHighScore").GetChild(0).GetComponent<TextMeshProUGUI>().text
			= best.ToString();
		iTween.MoveFrom(gameoverView,iTween.Hash("x",-2));

		CancelInvoke();
		textScore.SetActive(false);
	}

	public void UpdateScore(int p){
		point += p;
		feverCount++;
		if(feverCount==maxFeverCount){
			FeverStart();
		}
		textScore.GetComponent<Text> ().text = point.ToString ();
	}

	private void FeverStart(){
		CancelInvoke();

		int childCount = ballField.transform.childCount;
		for(int i=0;i<childCount;i++){
			GameObject ball = ballField.transform.GetChild(i).gameObject;
			Destroy(ball);
		}
		Invoke("FeverFinish",feverTime);

		isFever = true;

		feverBlack.SetActive(true);
		neon.SetActive(true);

		CreateGoal();
		Invoke("GoalSpan",goalTimeSpan);
		CreateAddBall();
	}

	private void FeverFinish(){
		CancelInvoke();
		DeleteGoal();

		feverCount = 0;

		feverBlack.SetActive(false);
		neon.SetActive(false);

		Invoke("CreateBall",5.0f);
		Invoke("GoalSpan",goalTimeSpan);
	}

	public void CreateGoal(){
		//Invoke ("Create",1);
		Create();
	}

	private void Create(){
		bool isOK = false;
		int posCount = goalPosition.Length;
		int target = 0;

		while(!isOK){
			target = Random.Range (0, posCount);
			if(goalposCheck[target] == 0) isOK = true;
		}

		goalposCheck[target] = 1;

		GameObject goal;
		if(Random.Range(0,5) == 1){
			goal = (GameObject)Instantiate(movegoalPrefab);
		}else{
			goal = (GameObject)Instantiate(goalPrefab);
		}
		goal.transform.SetParent(basketGoal.transform);
		goal.transform.localPosition = goalPosition[target];
		goal.GetComponent<GoalManager>().posNum = target;
		//basketGoal.transform.GetChild (target).gameObject.SetActive (true);
	}

	//オーディオ
	public void JumpSE(){
		audioSource.PlayOneShot(jumpSE);
	}

	public void PointSE(){
		audioSource.PlayOneShot(pointSE);
	}

}
