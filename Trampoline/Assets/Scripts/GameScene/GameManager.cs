using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public enum PPKey {
	best, //ベストスコア
	playcount, //プレイ回数
	logindays, //ログイン日数
	movierewordcount, //動画報酬視聴回数
	skinNum, //スキン開放数
	latestloginday, //最新ログイン日時
	userid, //ユーザーID
	username, //ユーザー名
	vibration, //バイブの有無
	music, //音の有無
	/*
	skinは
	skin0,
	skin1, ・・・
	のようにkeyを設定
	*/
	selectedBall, //選択したボール（使用するボール）
}

public class GameManager : MonoBehaviour {

	public AudioClip jumpSE;
	public AudioClip pointSE;
	public AudioClip gameoverSE;
	public AudioSource audioSource;
	public GameObject[] itemPrefab = new GameObject[3];
	public GameObject addballPregab;
	public GameObject ballPrefab;
	public GameObject gameoverView;
	public GameObject basketGoal;
	public GameObject goalPrefab;
	public GameObject movegoalPrefab;
	public GameObject textScore;
	public GameObject sliderFever;
	public GameObject canvasHome;
	public GameObject wall;
	public GameObject ballField;
	public GameObject feverBlack;
	public GameObject neon;
	public GameObject ballskinField;
	public GameObject firebaseManager;
	public GameObject adBanner;
	public GameObject adRectangle;
	public Vector3[] goalPosition = new Vector3[5];
	public bool isFever = false;
	public int[] goalposCheck = {0,0,0,0,0};
	public int combo = 0;
	//スキン開放に必要なプレイ回数⬇︎
	public int[] playCountCondition = new int[]{10,50,100};
	//スキン開放に必要なスコア
	public int[] scoreCondition = new int[]{50,200,500};
	//スキン開放に必要なログイン日数
	public int[] logindayseCondition = new int[]{2,4,7};
	//スキン開放に必要な動画視聴回数
	public int rewordCondition = 10;

	private GameObject drawLine;
	private float timeSpan = 5.0f;
	private float goalTimeSpan = 20.0f;
	private float itemTimeSpan = 5.0f;
	private float feverBallSpan = 0.5f;
	private float feverTime = 10.0f;
	private bool gamefinish = false;
	private bool isLeft = true;
	private int point = 0;
	private int ballCount = 0;
	private int feverCount = 0;
	private int maxFeverCount = 10;

	[DllImport("__Internal")]
  private static extern void Touch3D(int n);

	// Use this for initialization
	void Start () {
		drawLine = GameObject.Find ("drawPhysicsLine");
		//Invoke("CreateItem",5);
		PlayerPrefs.SetInt("skin0",1);

		//ログイン機能
		string day = DateTime.Now.ToString("yyyy/MM/dd/hh");
		string latestday = PlayerPrefs.GetString(PPKey.latestloginday.ToString());
		if(day != latestday){
			//最終ログイン日時と現在の日時が違えばログイン日数更新
			int n = PlayerPrefs.GetInt(PPKey.logindays.ToString()) + 1;
			PlayerPrefs.SetInt(PPKey.logindays.ToString(), n);
		}
		PlayerPrefs.SetString(PPKey.latestloginday.ToString(),day);

		RefreshFeverSlider();

		//初回起動時useridを設定
		if(PlayerPrefs.GetString(PPKey.userid.ToString()) == ""){
			System.Guid guid = System.Guid.NewGuid ();
			string uuid = guid.ToString ();
			PlayerPrefs.SetString(PPKey.userid.ToString(),uuid);
		}else{
			//Debug.Log(PlayerPrefs.GetString(PPKey.userid.ToString()));
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void GameStart(){

		//ボトムバナー広告を非表示
		adBanner.GetComponent<AdBanner>().HideAd();

		canvasHome.SetActive(false);
		Vector3 wallPos = wall.transform.localPosition;
		wall.transform.localPosition = new Vector3(wallPos.x,wallPos.y,0);

		textScore.SetActive(true);
		sliderFever.SetActive(true);

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

		float angle = UnityEngine.Random.Range(60.0f,80.0f);

		float k = 4.2f;
		float sin = k * Mathf.Sin(angle * (Mathf.PI / 180));
		float cos = k * Mathf.Cos(angle * (Mathf.PI / 180));
		//Debug.Log("cos :" + cos + ",sin :" + sin);
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2(cos,sin);

		ball.GetComponent<SpriteRenderer>().sprite
			= ballskinField.GetComponent<BallFieldManager>().ballSkin[PlayerPrefs.GetInt("selectedBall")];

		if(!gamefinish){
			Invoke("CreateBall",timeSpan);
		}
	}

	private void CreateAddBall(){

		isLeft = !isLeft;

		GameObject addball = (GameObject)Instantiate(addballPregab);
		addball.transform.SetParent(ballField.transform);
		//addball.transform.localPosition = new Vector3(Random.Range(-2.0f,2.3f),5.71f,0);
		addball.transform.localPosition = new Vector3(-2.08f,-4.18f,0);
		if(isLeft)addball.transform.localPosition = new Vector3(2.08f,-4.18f,0);

		float angle = UnityEngine.Random.Range(75.0f,80.0f);
		if(isLeft)angle = 180.0f - angle;

		float k = 4.2f;
		float sin = k * Mathf.Sin(angle * (Mathf.PI / 180));
		float cos = k * Mathf.Cos(angle * (Mathf.PI / 180));
		//Debug.Log("cos :" + cos + ",sin :" + sin);
		addball.GetComponent<Rigidbody2D>().velocity = new Vector2(cos,sin);

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
		int target = UnityEngine.Random.Range(0,count);
		Destroy(basketGoal.transform.GetChild(target).gameObject);
	}

	private void CreateItem(){
		int target = UnityEngine.Random.Range(0,itemPrefab.Length);
		GameObject item = (GameObject)Instantiate(itemPrefab[target]);
		item.transform.localPosition = new Vector3(UnityEngine.Random.Range(-2.0f,2.3f),5.71f,0);

		if(!gamefinish){
			Invoke("CreateItem",itemTimeSpan);
		}
	}

	public void GameOver(){

		//SE
		audioSource.PlayOneShot(gameoverSE);

		//線を引けなくする
		drawLine.GetComponent<drawPhysicsLine> ().gamefinish = true;
		gamefinish = true;

		//プレイ回数上昇
		int count = PlayerPrefs.GetInt(PPKey.playcount.ToString());
		count++;
		PlayerPrefs.SetInt(PPKey.playcount.ToString(),count);


		//GameOverUIを表示
		int score = int.Parse(textScore.GetComponent<Text>().text);
		int best = PlayerPrefs.GetInt(PPKey.best.ToString());
		if(score > best)best = score;
		PlayerPrefs.SetInt(PPKey.best.ToString(), best);

		//レクタングル広告表示
		adRectangle.GetComponent<AdRectangle>().ShowAd();

		//RealtimeDataBaseにscoreを保存
		string id = PlayerPrefs.GetString(PPKey.userid.ToString());
		string name = PlayerPrefs.GetString(PPKey.username.ToString(),"No Name");
		int bestscore = PlayerPrefs.GetInt(PPKey.best.ToString());
		firebaseManager.GetComponent<FirebaseManager>().writeNewScore(id, name, bestscore);

		gameoverView.SetActive(true);
		gameoverView.transform.Find("TextScore").GetChild(0).GetComponent<TextMeshProUGUI>().text
			= score.ToString();
		gameoverView.transform.Find("TextHighScore").GetChild(0).GetComponent<TextMeshProUGUI>().text
			= best.ToString();
		iTween.MoveFrom(gameoverView,iTween.Hash("x",-2));

		CancelInvoke();
		textScore.SetActive(false);

		JudgeSkinRelease();
	}

	public void UpdateScore(int p){
		point += p;
		feverCount++;
		UpdateFeverSlider();
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
		isFever = false;

		CancelInvoke();
		DeleteGoal();

		feverCount = 0;

		maxFeverCount++;
		RefreshFeverSlider();

		isLeft = true;

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
			target = UnityEngine.Random.Range (0, posCount);
			if(goalposCheck[target] == 0) isOK = true;
		}

		goalposCheck[target] = 1;

		GameObject goal;
		if(UnityEngine.Random.Range(0,5) == 1){
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

	//バイブレーション
	//n -> 1519~1521
	public void Vibration(int n){
		Touch3D(n);
	}

	public void JudgeSkinRelease(){

		//プレイ回数
		int playC = PlayerPrefs.GetInt(PPKey.playcount.ToString());
		if(playC >= playCountCondition[0]){
			PlayerPrefs.SetInt("skin1",1);
		}else if(playC >= playCountCondition[1]){
			PlayerPrefs.SetInt("skin2",1);
		}else if(playC >= playCountCondition[2]){
			PlayerPrefs.SetInt("skin3",1);
		}

		//スコア
		int score = PlayerPrefs.GetInt(PPKey.best.ToString());
		if(score >= scoreCondition[0]){
			PlayerPrefs.SetInt("skin4",1);
		}else if(score >= scoreCondition[1]){
			PlayerPrefs.SetInt("skin5",1);
		}else if(score >= scoreCondition[2]){
			PlayerPrefs.SetInt("skin6",1);
		}

		//ログイン日数
		int days = PlayerPrefs.GetInt(PPKey.logindays.ToString());
		if(days >= logindayseCondition[0]){
			PlayerPrefs.SetInt("skin7",1);
		}else if(days >= logindayseCondition[1]){
			PlayerPrefs.SetInt("skin8",1);
		}else if(days >= logindayseCondition[2]){
			PlayerPrefs.SetInt("skin9",1);
		}

		//動画報酬
		int movie = PlayerPrefs.GetInt(PPKey.movierewordcount.ToString());
		if(movie == rewordCondition){
			PlayerPrefs.SetInt("skin10",1);
		}

		//スキン全開放
		int num = 0;
		for(int i=0;i<11;i++){
			string str = string.Format("skin{0}",i);
			if(PlayerPrefs.GetInt(str) == 1)num++;
		}
		PlayerPrefs.SetInt(PPKey.skinNum.ToString(),num);
		if(num == 11)PlayerPrefs.SetInt("skin11",1);

	}

	private void RefreshFeverSlider(){
		sliderFever.GetComponent<Slider>().maxValue = maxFeverCount;
		sliderFever.GetComponent<Slider>().value = 0;
		string strText = string.Format("0/{0}",maxFeverCount);
		sliderFever.transform.Find("Text").gameObject.GetComponent<Text>().text = strText;
	}

	private void UpdateFeverSlider(){
		sliderFever.GetComponent<Slider>().value = feverCount;
		string strText = string.Format("{0}/{1}",feverCount,maxFeverCount);
		if(!isFever)sliderFever.transform.Find("Text").gameObject.GetComponent<Text>().text = strText;
	}

}
