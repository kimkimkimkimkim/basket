using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboText : MonoBehaviour {

	void OnEnable(){
		Invoke("ComboTextFalse",1.0f);
	}

	private void ComboTextFalse(){
		this.gameObject.SetActive(false);
	}

}
