/* AdGeneration UnityPlugin Ver.2.2.5 */

using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class ADGUnitySDK2 : ADGMonoBehaviour {

	// == SDK objects and constants ======================
	private static ADGUnitySDK2 myInstance;
#if UNITY_IPHONE
	private static IntPtr adgni = IntPtr.Zero;
#elif UNITY_ANDROID
	private static AndroidJavaObject androidPlugin = null;
#endif
	private const string ADG_NATIVE_MANAGER = "com.socdm.d.adgeneration.plugin.unity.ADGNativeManager";
	private const string ANDROID_BANNER_AD_PARAMS = "com.socdm.d.adgeneration.plugin.unity.BannerAdParams";
	private const string ANDROID_INTERSTITIAL_AD_PARAMS = "com.socdm.d.adgeneration.plugin.unity.InterstitialAdParams";
	private static bool isEditor {
		get {
			return Application.isEditor;
		}
	}
	private static bool noInstance {
		get {
			return myInstance == null || isEditor;
		}
	}

	// == Common parameters ======================
	private static bool isEnableTest = true;
	public static bool IsEnableTest {
		set { isEnableTest = value; }
		get { return isEnableTest; }
	}

	private static bool isGeolocationEnabled = true;
	public static bool IsGeolocationEnabled {
		set {
			isGeolocationEnabled = value;
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				_setGeolocationEnabledADG (value);
			}
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android) {
				AndroidJavaClass manager = new AndroidJavaClass (ADG_NATIVE_MANAGER);
				manager.CallStatic ("setGeolocationEnabledADG", value);
			}
#endif
		}
		get { return isGeolocationEnabled; }
	}

	private static string messageObjName = "Main Camera";
	public static string MessageObjName {
		set { messageObjName = value; }
		get { return messageObjName; }
	}

	// == Banner Ads parameters ======================
	private static string iosLocationID = "48547";
	public static string IOSLocationID {
		set { iosLocationID = value; }
		get { return iosLocationID; }
	}

	private static string androidLocationID = "48547";
	public static string AndroidLocationID {
		set { androidLocationID = value; }
		get { return androidLocationID; }
	}

	private static string adType = "SP";
	public static string AdType {
		set { adType = value; }
		get { return adType; }
	}

	private static bool isIOSEasyPosition = false;
	public static bool IsIOSEasyPosition {
		set { isIOSEasyPosition = value; }
		get { return isIOSEasyPosition; }
	}

	private static float x = 0;
	public static float X {
		set { x = value; }
		get { return x; }
	}

	private static float y = 0;
	public static float Y {
		set { y = value; }
		get { return y; }
	}

	private static string horizontal = "CENTER";
	public static string Horizontal {
		set { horizontal = value; }
		get { return horizontal; }
	}

	private static string vertical = "TOP";
	public static string Vertical {
		set { vertical = value; }
		get { return vertical; }
	}

	private static bool hidden = false;
	public static bool Hidden {
		set { hidden = value; }
		get { return hidden; }
	}

	private static int width = 0;
	public static int Width {
		set { width = value; }
		get { return width; }
	}

	private static int height = 0;
	public static int Height {
		set { height = value; }
		get { return height; }
	}

	private static int[] margin = { 0 };
	public static int[] Margin {
		set { margin = value; }
		get { return margin; }
	}

	private static bool isDpMargin = false;
	public static bool IsDpMargin {
		set { isDpMargin = value; }
		get { return isDpMargin; }
	}

	private static double scale = 1;
	public static double Scale {
		set { scale = value; }
		get { return scale; }
	}

	private static bool isExpandFrame = false;
	public static bool IsExpandFrame {
		set { isExpandFrame = value; }
		get { return isExpandFrame; }
	}

	// == Interstitial Ads parameters ======================
	private static string iosInterLocationID = "";
	public static string IOSInterLocationID {
		set { iosInterLocationID = value; }
		get { return iosInterLocationID; }
	}

	private static string androidInterLocationID = "";
	public static string AndroidInterLocationID {
		set { androidInterLocationID = value; }
		get { return androidInterLocationID; }
	}

	private static int backgroundType = 0;
	public static int BackgroundType {
		set { backgroundType = value; }
		get { return backgroundType; }
	}

	private static int closeButtonType = 0;
	public static int CloseButtonType {
		set { closeButtonType = value; }
		get { return closeButtonType; }
	}

	private static int span = 0;
	public static int Span {
		set { span = value; }
		get { return span; }
	}

	private static bool isPercentage = false;
	public static bool IsPercentage {
		set { isPercentage = value; }
		get { return isPercentage; }
	}

	private static bool isPreventAccidentClick = false;
	public static bool IsPreventAccidentClick {
		set { isPreventAccidentClick = value; }
		get { return isPreventAccidentClick; }
	}

	private static bool isInterstitialFullScreen = false;
	public static bool IsInterstitialFullScreen {
		set { isInterstitialFullScreen = value; }
		get { return isInterstitialFullScreen; }
	}

	public static void initADG () {
		initADGCommon ();
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			if (!isIOSEasyPosition) {
				horizontal = "";
				vertical = "";
			}
			var adgniParams = _initParamsADG();
			_setAdIDADG(adgniParams, iosLocationID);
			_setAdTypeADG(adgniParams, adType);
			_setPointADG(adgniParams, x, y);
			_setObjNameADG(adgniParams, messageObjName);
			_setSizeADG(adgniParams, width, height, (float) scale);
			_setAlignADG(adgniParams, horizontal, vertical);
			_setHiddenADG(adgniParams, hidden);
			_setEnableTestADG(adgniParams, isEnableTest);
			_setExpandFrameADG(adgniParams, isExpandFrame);
			adgni = _initADG (adgni, adgniParams);
			_releaseParamsADG(adgniParams);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			makeJavaInstance ();
			AndroidJavaClass bannerAdParamsClass = new AndroidJavaClass (ANDROID_BANNER_AD_PARAMS);
			AndroidJavaObject bannerAdParams = bannerAdParamsClass.CallStatic<AndroidJavaObject> ("instance");
			bannerAdParams.Call("setAdId", androidLocationID);
			bannerAdParams.Call("setAdType", adType);
			bannerAdParams.Call("setHorizontal", horizontal);
			bannerAdParams.Call("setVertical", vertical);
			bannerAdParams.Call("setGameObjName", messageObjName);
			bannerAdParams.Call("setWidth", width);
			bannerAdParams.Call("setHight", height);
			bannerAdParams.Call("setScale", (float) scale);
			bannerAdParams.Call("setMargin", margin);
			bannerAdParams.Call("setDpMarginEnable", isDpMargin);
			bannerAdParams.Call("setHidden", hidden);
			bannerAdParams.Call("setExpandFrame", isExpandFrame);
			bannerAdParams.Call("setEnableTest", isEnableTest);
			androidPlugin.Call ("initADG", bannerAdParams);
		}
#endif
	}

	private static void initADGCommon () {
		if (myInstance == null) {
			GameObject gameObject = new GameObject ("ADGForUnity");
			DontDestroyOnLoad (gameObject); //Makes the object target not be destroyed automatically when loading a new scene.
			gameObject.hideFlags = HideFlags.HideAndDontSave; //A combination of not shown in the hierarchy and not saved to to scenes.
			myInstance = gameObject.AddComponent<ADGUnitySDK2> ();
		}
	}

#if UNITY_ANDROID
	private static void makeJavaInstance () {
		if (androidPlugin == null) {
			AndroidJavaClass manager = new AndroidJavaClass (ADG_NATIVE_MANAGER);
			androidPlugin = manager.CallStatic<AndroidJavaObject> ("instance");
		}
	}
#endif

	public static bool canCallADG () {
		if (noInstance) return false;
#if UNITY_ANDROID
		else if (Application.platform == RuntimePlatform.Android && androidPlugin.Call<bool> ("canCallADG") == false) {
			return false;
		}
#endif
		else return true;
	}

	public static void finishADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_finishADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("finishADG");
		}
#endif
		Destroy (myInstance);
		myInstance = null;
	}

	public static void loadADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_loadADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("loadADG");
		}
#endif
	}

	public static void resumeADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_resumeADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("resumeADG");
		}
#endif
	}

	public static void pauseADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_pauseADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("pauseADG");
		}
#endif
	}

	public static void showADG () {
		if (noInstance) return;
		hidden = false;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_resumeADG (adgni);
			_showADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("resumeADG");
			androidPlugin.Call ("showADG");
		}
#endif
	}

	public static void hideADG () {
		if (noInstance) return;
		hidden = true;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_hideADG (adgni);
			_pauseADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("hideADG");
			androidPlugin.Call ("pauseADG");
		}
#endif
	}

	public static void changeLocationADG (float tempx, float tempy) {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_changeLocationADG (adgni, tempx, tempy);
		}
#endif
	}

	public static void changeLocationADG (string temphorizontal, string tempvertical) {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			if (isIOSEasyPosition) {
				_changeLocationEasyADG (adgni, temphorizontal, tempvertical);
			}
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("changeLocationADG", temphorizontal, tempvertical);
		}
#endif
	}

	public static void setDefaultLocationADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_changeLocationADG (adgni, x, y);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("changeLocationADG", horizontal, vertical);
		}
#endif
	}

	public static void changeMarginADG (int[] tempmargin) {
		if (noInstance) return;
#if UNITY_IPHONE
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("changeMarginADG", tempmargin);
		}
#endif
	}

	public static void setBackgroundColorADG (int red, int green, int blue, int alpha) {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_setBackgroundColorADG (adgni, red, green, blue, alpha);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("setBackgroundColorADG", red, green, blue, alpha);
		}
#endif
	}

	public static void initInterADG () {
		initADGCommon ();
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			var adgniInterParams = _initParamsInterADG();
			_setAdIdInterADG(adgniInterParams, iosInterLocationID);
			_setObjNameInterADG(adgniInterParams, messageObjName);
			_setBackgroundTypeInterADG(adgniInterParams, backgroundType);
			_setCloseButtonTypeInterADG(adgniInterParams, closeButtonType);
			_setSpanInterADG(adgniInterParams, span);
			_setIsPercentageInterADG(adgniInterParams, isPercentage);
			_setIsPreventAccidentClickInterADG(adgniInterParams, isPreventAccidentClick);
			_setIsInterstitialFullScreenInterADG(adgniInterParams, isInterstitialFullScreen);
			_setEnableTestInterADG(adgniInterParams, isEnableTest);
			adgni = _initInterADG (adgni, adgniInterParams);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			makeJavaInstance ();
			AndroidJavaClass interstitialAdParamsClass = new AndroidJavaClass(ANDROID_INTERSTITIAL_AD_PARAMS);
			AndroidJavaObject interstitialAdParams = interstitialAdParamsClass.CallStatic<AndroidJavaObject>("instance");
			interstitialAdParams.Call("setAdid", androidInterLocationID);
			interstitialAdParams.Call("setGameName", messageObjName);
			interstitialAdParams.Call("setBackgroundType", changeDesignNum (backgroundType));
			interstitialAdParams.Call("setCloseButtonType", changeDesignNum (closeButtonType));
			interstitialAdParams.Call("setSpan", span);
			interstitialAdParams.Call("setIsPercentage", isPercentage);
			interstitialAdParams.Call("setIsPreventAccidentClick", isPreventAccidentClick);
			interstitialAdParams.Call("setIsFullScreen", isInterstitialFullScreen);
			interstitialAdParams.Call("setEnableTest", isEnableTest);
			androidPlugin.Call ("initInterADG", interstitialAdParams);
		}
#endif
	}

	public static void loadInterADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_loadInterADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("loadInterADG");
		}
#endif
	}

	public static void showInterADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_showInterADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("showInterADG");
		}
#endif
	}

	public static void dismissInterADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_dismissInterADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("dismissInterADG");
		}
#endif
	}

	public static void finishInterADG () {
		if (noInstance) return;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_finishInterADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin.Call ("finishInterADG");
		}
#endif
	}

	public static float getNativeWidth () {
		if (noInstance) return (float) 0.0;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return _getNativeWidthADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			return androidPlugin.Call<float> ("getNativeWidthADG");
		}
#endif
		return (float) 0.0;
	}

	public static float getNativeHeight () {
		if (noInstance) return (float) 0.0;
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return _getNativeHeightADG (adgni);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			return androidPlugin.Call<float> ("getNativeHeightADG");
		}
#endif
		return (float) 0.0;
	}

	public static void addFANTestDevice (string deviceHash) {
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_addFANTestDeviceADG (deviceHash);
		}
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass manager = new AndroidJavaClass (ADG_NATIVE_MANAGER);
			manager.CallStatic ("addFANTestDeviceADG", deviceHash);
		}
#endif
	}

	private const int androidDesignShift = 100;
	private static int changeDesignNum (int num) {
		if (num < androidDesignShift && Application.platform == RuntimePlatform.Android) {
			return num + androidDesignShift;
		} else {
			return num;
		}
	}

	// Use this for initialization
	void Start () {

	}

	void Update(){

	}

}
