using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasHomeManager : MonoBehaviour {

	public GameObject textDraw;

	// Use this for initialization
	void Start () {
		FadeOutText();
	}

	// Update is called once per frame
	void Update () {

	}

	private void UpdateTextAlfa(float a){
		Color c = textDraw.GetComponent<TextMeshProUGUI>().color;
		textDraw.GetComponent<TextMeshProUGUI>().color = new Color(c.r,c.b,c.g,a);
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

}
