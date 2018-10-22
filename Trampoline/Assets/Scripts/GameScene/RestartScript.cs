using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour {

	public GameObject adRectangle;

	public void OnClick(){
		adRectangle.GetComponent<AdRectangle>().HideAd();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
