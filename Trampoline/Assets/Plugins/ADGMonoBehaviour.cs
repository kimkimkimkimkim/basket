using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class ADGMonoBehaviour : MonoBehaviour {
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	protected static extern IntPtr _initADG (IntPtr adgni, IntPtr adgniParams);
	[DllImport ("__Internal")]
	protected static extern void _finishADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _loadADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _resumeADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _pauseADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _showADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _hideADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _changeLocationADG (IntPtr adgni, float x, float y);
	[DllImport ("__Internal")]
	protected static extern void _changeLocationEasyADG (IntPtr adgni, string vertical, string horizontal);
	[DllImport ("__Internal")]
	protected static extern void _setBackgroundColorADG (IntPtr adgni, int red, int green, int blue, int alpha);
	[DllImport ("__Internal")]
	protected static extern IntPtr _initParamsADG ();
	[DllImport ("__Internal")]
	protected static extern void _setAdIDADG (IntPtr adgniParams, string adid);
	[DllImport ("__Internal")]
	protected static extern void _setAdTypeADG (IntPtr adgniParams, string adtype);
	[DllImport ("__Internal")]
	protected static extern void _setPointADG (IntPtr adgniParams, float x, float y);
	[DllImport ("__Internal")]
	protected static extern void _setObjNameADG (IntPtr adgniParams, string objName);
	[DllImport ("__Internal")]
	protected static extern void _setSizeADG (IntPtr adgniParams, int width, int height, float scale);
	[DllImport ("__Internal")]
	protected static extern void _setAlignADG (IntPtr adgniParams, string horizontal, string vertical);
	[DllImport ("__Internal")]
	protected static extern void _setHiddenADG (IntPtr adgniParams, bool hidden);
	[DllImport ("__Internal")]
	protected static extern void _setEnableTestADG (IntPtr adgniParams, bool enableTest);
	[DllImport ("__Internal")]
	protected static extern void _setExpandFrameADG (IntPtr adgniParams, bool expandFrame);
	[DllImport ("__Internal")]
	protected static extern void _releaseParamsADG (IntPtr adgniParams);
	[DllImport ("__Internal")]
	protected static extern IntPtr _initInterADG (IntPtr adgni, IntPtr adgniInterParams);
	[DllImport ("__Internal")]
	protected static extern void _loadInterADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _showInterADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _dismissInterADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _finishInterADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern IntPtr _initParamsInterADG ();
	[DllImport ("__Internal")]
	protected static extern void _setAdIdInterADG(IntPtr adgniInterParams, string adid);
	[DllImport ("__Internal")]
	protected static extern void _setObjNameInterADG(IntPtr adgniInterParams, string objName);
	[DllImport ("__Internal")]
	protected static extern void _setBackgroundTypeInterADG(IntPtr adgniInterParams, int backgroundType);
	[DllImport ("__Internal")]
	protected static extern void _setCloseButtonTypeInterADG(IntPtr adgniInterParams, int closeButtonType);
	[DllImport ("__Internal")]
	protected static extern void _setSpanInterADG(IntPtr adgniInterParams, int span);
	[DllImport ("__Internal")]
	protected static extern void _setIsPercentageInterADG(IntPtr adgniInterParams, bool isPercentage);
	[DllImport ("__Internal")]
	protected static extern void _setIsPreventAccidentClickInterADG(IntPtr adgniInterParams, bool isPreventAccidentClick);
	[DllImport ("__Internal")]
	protected static extern void _setIsInterstitialFullScreenInterADG(IntPtr adgniInterParams, bool isFullScreen);
	[DllImport ("__Internal")]
	protected static extern void _setEnableTestInterADG(IntPtr adgniInterParams, bool enableTest);
	[DllImport ("__Internal")]
	protected static extern float _getNativeWidthADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern float _getNativeHeightADG (IntPtr adgni);
	[DllImport ("__Internal")]
	protected static extern void _addFANTestDeviceADG (string deviceHash);
	[DllImport ("__Internal")]
	protected static extern void _setGeolocationEnabledADG (bool enable);
#endif
}