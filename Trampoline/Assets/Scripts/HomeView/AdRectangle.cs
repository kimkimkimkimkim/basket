using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// レクタングル広告
/// </summary>
public class AdRectangle : MonoBehaviour {

	void Awake() {

        ADGUnitySDK2.IOSLocationID = "72699";
        ADGUnitySDK2.AndroidLocationID = "72701";
        ADGUnitySDK2.AdType = "RECT";
        if (Screen.width != 1125 && Screen.height != 2436) {
            ADGUnitySDK2.IsIOSEasyPosition = true;
            ADGUnitySDK2.Vertical = "CENTER";
            ADGUnitySDK2.Horizontal = "CENTER";
        }
        float width = 300;
        float height = 250;
        float scale = 1 + height / width;
        ADGUnitySDK2.Width = (int)(width);
        ADGUnitySDK2.Height = (int)(height);
        //ADGUnitySDK2.Scale = scale;
        ADGUnitySDK2.MessageObjName = "";
	}

    void Start(){
        InitAd();
        LoadAd();
        HideAd();
    }

	void Update(){
	}

    void OnApplicationQuit(){
        FinishAd();
    }

    void ADGReceiveAd(string msg){
        Debug.Log(msg);
    }

	public void InitAd() {
        Debug.Log("Init");
		ADGUnitySDK2.initADG();
        // iPhoneX対応
        if (Screen.width == 1125 && Screen.height == 2436) {
            ADGUnitySDK2.X = ADGUnitySDK2.getNativeWidth() / 2 - 320 * (float)ADGUnitySDK2.Scale / 2;
            ADGUnitySDK2.Y = ADGUnitySDK2.getNativeHeight() / 2 - 250 * (float)ADGUnitySDK2.Scale / 2;
            ADGUnitySDK2.changeLocationADG(ADGUnitySDK2.X, ADGUnitySDK2.Y);
        }
	}

    public void LoadAd() {
        ADGUnitySDK2.loadADG();
    }

    public void ShowAd() {
        Debug.Log("show rectangle");
        ADGUnitySDK2.showADG();
    }

    public void HideAd() {
        ADGUnitySDK2.hideADG();
    }

	public void FinishAd() {
		ADGUnitySDK2.finishADG();
	}
}
