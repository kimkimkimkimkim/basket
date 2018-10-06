using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OKButtonInSkin : MonoBehaviour {

	private GameObject detailView;

	private void Start(){
		detailView = transform.parent.gameObject;
	}

	public void OnClick(){
		detailView.SetActive(false);
	}

}
