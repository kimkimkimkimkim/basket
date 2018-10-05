using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PPKey {
	best,
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
	public Vector3[] goalPosition = new Vector3[5];
	public bool isFever = false;
	public int[] goalposCheck = {0,0,0,0,0};
	public int combo = 0;

	private GameObject drawLine;
	private float timeSpan = 5.0f;
	private float goalTimeSpan = 20.0f;
	private float itemTimeSpan = 5.0f;
	private bool gamefinish = false;
	private int point = 0;
	private int ballCount = 0;

	// Use this for initialization
	void Start () {
		drawLine = GameObject.Find ("drawPhysicsLine");
		//Invoke("CreateItem",5);
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

		CreateAddBall();

		Invoke("GoalSpan",goalTimeSpan);


	}

	private void CreateAddBall(){

		ballCount++;
		if(ballCount % 5 == 0)timeSpan = (timeSpan-0.2f > 2) ? timeSpan - 0.2f : 2;

		GameObject addball = (GameObject)Instantiate(ballPrefab);
		addball.transform.SetParent(ballField.transform);
		//addball.transform.localPosition = new Vector3(Random.Range(-2.0f,2.3f),5.71f,0);
		addball.transform.localPosition = new Vector3(-2.08f,-4.18f,0);
		addball.GetComponent<Rigidbody2D>().velocity = new Vector2(1,4);

		if(!gamefinish){
			Invoke("CreateAddBall",timeSpan);
		}
	}

	private void GoalSpan(){
		CreateGoal();

		int count = basketGoal.transform.childCount - 1;
		int target = Random.Range(0,count);
		Destroy(basketGoal.transform.GetChild(target).gameObject);

		if(!gamefinish){
			Invoke("GoalSpan",goalTimeSpan);
		}
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
		textScore.GetComponent<Text> ().text = point.ToString ();
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
