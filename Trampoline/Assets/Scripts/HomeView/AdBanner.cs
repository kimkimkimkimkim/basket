using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// バナー広告
/// </summary>
public class AdBanner : MonoBehaviour {

    private bool AdBannerShowFlag;

	void Awake() {
        ADGUnitySDK.IOSLocationID = "72698";
        ADGUnitySDK.AndroidLocationID = "72700";
        ADGUnitySDK.AdType = "FREE";
        if (Screen.width != 1125 && Screen.height != 2436) {
            ADGUnitySDK.IsIOSEasyPosition = true;
            ADGUnitySDK.Vertical = "BOTTOM";
            ADGUnitySDK.Horizontal = "CENTER";
        }
        float width = 320;
        float height = 50;
        float scale = 1 + height / width;
        ADGUnitySDK.Width = (int)(width * scale);
        ADGUnitySDK.Height = (int)(height * scale);
        ADGUnitySDK.Scale = scale;
        ADGUnitySDK.MessageObjName = "";
        AdBannerShowFlag = true;
	}

    void Update(){
      /*
    	if(AdBannerShowFlag&&(gameManager.gameState == GameState.CLEARED||gameManager.gameState == GameState.GAMEOVER)){
			HideAd();
            AdBannerShowFlag = false;
		}*/
	}

    void Start(){
        InitAd();
        LoadAd();
        ShowAd();
    }

    void OnApplicationQuit(){
        FinishAd();
    }

    void ADGReceiveAd(string msg){
        Debug.Log(msg);
    }

	public void InitAd() {
        //Debug.Log("Init");
		ADGUnitySDK.initADG();
        // iPhoneX対応
        if (Screen.width == 1125 && Screen.height == 2436) {
            ADGUnitySDK.X = ADGUnitySDK.getNativeWidth() / 2 - 320 * (float)ADGUnitySDK.Scale / 2;
            ADGUnitySDK.Y = ADGUnitySDK.getNativeHeight() - ADGUnitySDK.Height - 35;
            ADGUnitySDK.changeLocationADG(ADGUnitySDK.X, ADGUnitySDK.Y);
        }
	}

    public void LoadAd() {
        ADGUnitySDK.loadADG();
    }

    public void ShowAd() {
        ADGUnitySDK.showADG();
    }

    public void HideAd() {
        ADGUnitySDK.hideADG();
    }

	public void FinishAd() {
		ADGUnitySDK.finishADG();
	}
}
