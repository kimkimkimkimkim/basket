using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour {

	DatabaseReference reference;

	public GameObject rankingSpace;

	void Start() {

		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://trampolinebasket.firebaseio.com/");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
  		var dependencyStatus = task.Result;
  		if (dependencyStatus == Firebase.DependencyStatus.Available) {
		    // Create and hold a reference to your FirebaseApp, i.e.
		    //   app = Firebase.FirebaseApp.DefaultInstance;
		    // where app is a Firebase.FirebaseApp property of your application class.

		    // Set a flag here indicating that Firebase is ready to use by your
		    // application.
		  } else {
		    UnityEngine.Debug.LogError(System.String.Format(
		      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
		    // Firebase Unity SDK is not safe to use here.
		  }
		});

	}

	public class User {
    public string username;
    public string score;

    public User() {
    }

    public User(string username, string score) {
        this.username = username;
        this.score = score;
    }
	}

	public void writeNewScore(string userId, string name, string score) {
    User user = new User(name,score);
    string json = JsonUtility.ToJson(user);
		Debug.Log(json);
    reference.Child("ranking").Child(userId).SetRawJsonValueAsync(json);
	}

	public void getRanking(){
		//RealtileDataBaseから現在のランキングを取得
		//bossNoのノードからtimeで昇順ソートして最大10件を取る（非同期)
		reference.Child("ranking").OrderByChild("score").LimitToFirst(10).GetValueAsync().ContinueWith(task =>{
  		if(task.IsFaulted){ //取得失敗
    	//Handle the Error
			Debug.Log("error");
	  	}else if(task.IsCompleted){ //取得成功
		    DataSnapshot snapshot = task.Result; //結果取得
				Debug.Log(snapshot.ToString());
		    IEnumerator<DataSnapshot> en = snapshot.Children.GetEnumerator(); //結果リストをenumeratorで処理
		    int rank = 1;
		    while(en.MoveNext()){ //１件ずつ処理
		      DataSnapshot data = en.Current; //データ取る
		      string name = (string)data.Child("username").GetValue(true); //名前取る
		      string score = (string)data.Child("score").GetValue(true); //スコアを取る
					Debug.Log("name:" + name + " score:" + score);
					/*
					//Textに反映
					GameObject row = rankingSpace.transform.GetChild(rank).gameObject;
					row.transform.GetChild(0).gameObject.GetComponent<Text>().text = rank.ToString(); //順位
					row.transform.GetChild(1).gameObject.GetComponent<Text>().text = score; //スコア
					row.transform.GetChild(2).gameObject.GetComponent<Text>().text = name; //名前*/
		      rank++;
		    }

			}
		});
	}


}
