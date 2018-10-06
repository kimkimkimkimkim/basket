using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour {

	public int myNum;

	private GameObject canvasOtherball;
	private GameObject detailView;
	private GameObject text;

	private void Start(){
		canvasOtherball = GameObject.Find("CanvasOtherball");
		detailView = canvasOtherball.transform.Find("DetailView").gameObject;
		text = detailView.transform.Find("PopView").Find("Text").gameObject;
	}

	public void OnClick(){
		detailView.SetActive(true);

		string str = string.Format("requirement（{0}）",myNum);
		text.GetComponent<Text>().text = str;
	}

}
