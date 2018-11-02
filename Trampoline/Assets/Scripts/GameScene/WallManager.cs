using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour {

	private bool isIphoneX = false;

	// Use this for initialization
	void Start () {
		if (Screen.width == 1125 && Screen.height == 2436) isIphoneX = true;
		if (Screen.width == 828 && Screen.height == 1792) isIphoneX = true;
		if (Screen.width == 1242 && Screen.height == 2688) isIphoneX = true;
		if (Screen.width == 1440 && Screen.height == 2960) isIphoneX = true;

		if(isIphoneX){
			//Wallを狭く
			Vector3 posL = this.transform.GetChild(2).localPosition;
			Vector3 posR = this.transform.GetChild(3).localPosition;
			this.transform.GetChild(2).localPosition = new Vector3(-2.7f,posL.y,posL.z);
			this.transform.GetChild(3).localPosition = new Vector3(2.7f,posR.y,posR.z);
		}else{
			//Wallを広く
			Vector3 posL = this.transform.GetChild(2).localPosition;
			Vector3 posR = this.transform.GetChild(3).localPosition;
			this.transform.GetChild(2).localPosition = new Vector3(-3.15f,posL.y,posL.z);
			this.transform.GetChild(3).localPosition = new Vector3(3.15f,posR.y,posR.z);
		}
	}

	
}
