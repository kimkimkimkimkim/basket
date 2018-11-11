using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// レクタングル広告
/// </summary>
public class AdInterstitial : MonoBehaviour {

	private bool AdRectangleShowFlag = false;

	void Awake() {

		ADGUnitySDK3.IOSInterLocationID = "73204";
		ADGUnitySDK3.AndroidInterLocationID = "73203";
		ADGUnitySDK3.BackgroundType = 3;
		ADGUnitySDK3.CloseButtonType = 3;
		ADGUnitySDK3.Span = 70;
		ADGUnitySDK3.IsPercentage = true;
		ADGUnitySDK3.IsPreventAccidentClick = false;
		ADGUnitySDK3.MessageObjName = "";

	}

    void Start(){
        InitAd();
        LoadAd();
        HideAd();
        //ShowAd();
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
		ADGUnitySDK3.initInterADG();
				/*
        // iPhoneX対応
        if (Screen.width == 1125 && Screen.height == 2436) {
            ADGUnitySDK3.X = ADGUnitySDK3.getNativeWidth() / 2 - 320 * (float)ADGUnitySDK3.Scale / 2;
            ADGUnitySDK3.Y = Screen.height/2 - ADGUnitySDK3.getNativeHeight()/2;
            ADGUnitySDK3.changeLocationADG(ADGUnitySDK3.X, ADGUnitySDK3.Y);
        }*/
	}

    public void LoadAd() {
        ADGUnitySDK3.loadInterADG();
    }

    public void ShowAd() {
        ADGUnitySDK3.showInterADG();
    }

    public void HideAd() {
        ADGUnitySDK3.dismissInterADG();
    }

	public void FinishAd() {
		ADGUnitySDK3.finishADG();
	}
}
