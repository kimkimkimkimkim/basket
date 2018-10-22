#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <ADG/ADGManagerViewController.h>
#import <ADG/ADGInterstitial.h>
#import <ADG/ADGSettings.h>

extern "C" void UnitySendMessage(const char *, const char *, const char *);
extern UIViewController *UnityGetGLViewController();

#pragma mark - Plugin core

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

@interface ADGNIParams : NSObject

@property (nonatomic, copy) NSString *adid;
@property (nonatomic, copy) NSString *adtype;
@property (nonatomic, assign) float x;
@property (nonatomic, assign) float y;
@property (nonatomic, copy) NSString *objName;
@property (nonatomic, assign) int width;
@property (nonatomic, assign) int height;
@property (nonatomic, assign) float scale;
@property (nonatomic, copy) NSString *horizontal;
@property (nonatomic, copy) NSString *vertical;
@property (nonatomic, assign) BOOL hidden;
@property (nonatomic, assign) BOOL enableTest;
@property (nonatomic, assign) BOOL expandFrame;

@end

@implementation ADGNIParams
@end

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

@interface ADGNIInterParams : NSObject

@property (nonatomic, copy) NSString *adid;
@property (nonatomic, copy) NSString *objName;
@property (nonatomic, assign) int backgroundType;
@property (nonatomic, assign) int closeButtonType;
@property (nonatomic, assign) int span;
@property (nonatomic, assign) BOOL isPercentage;
@property (nonatomic, assign) BOOL isPreventAccidentClick;
@property (nonatomic, assign) BOOL isFullScreen;
@property (nonatomic, assign) BOOL enableTest;

@end

@implementation ADGNIInterParams
@end

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

@interface ADGNI : NSObject <ADGManagerViewControllerDelegate, ADGInterstitialDelegate> {
    NSDictionary *adgParam_;
    NSDictionary *adgInterParam_;
    NSString *gameObjName_;
    NSString *adgInterId_;
    CGRect frame_;
    int width_;
    int height_;
}

@property (nonatomic, retain) ADGManagerViewController *adg;
@property (nonatomic, retain) ADGInterstitial *adgInter;

@end

@implementation ADGNI

@synthesize adg = adg_;
@synthesize adgInter = adgInter_;

- (void)setParams:(UIViewController *)viewCon adgniParams:(ADGNIParams *)adgniParams {
    NSInteger adTypeInt = 0;
    if ([adgniParams.adtype isEqualToString:@"SP"]) {
        adTypeInt = kADG_AdType_Sp;
        width_ = kADGAdSize_Sp_Width;
        height_ = kADGAdSize_Sp_Height;
    } else if ([adgniParams.adtype isEqualToString:@"LARGE"]) {
        adTypeInt = kADG_AdType_Large;
        width_ = kADGAdSize_Large_Width;
        height_ = kADGAdSize_Large_Height;
    } else if ([adgniParams.adtype isEqualToString:@"RECT"]) {
        adTypeInt = kADG_AdType_Rect;
        width_ = kADGAdSize_Rect_Width;
        height_ = kADGAdSize_Rect_Height;
    } else if ([adgniParams.adtype isEqualToString:@"TABLET"]) {
        adTypeInt = kADG_AdType_Tablet;
        width_ = kADGAdSize_Tablet_Width;
        height_ = kADGAdSize_Tablet_Height;
    } else if ([adgniParams.adtype isEqualToString:@"FREE"]) {
        width_ = adgniParams.width;
        height_ = adgniParams.height;
    }

    frame_ = viewCon.view.frame;

    if (adgniParams.horizontal.length > 0) {
        adgniParams.x = [self getXFromHorizontal:adgniParams.horizontal];
    }

    if (adgniParams.vertical.length > 0) {
        adgniParams.y = [self getYFromtVertical:adgniParams.vertical];
    }

    if ([adgniParams.adtype isEqualToString:@"FREE"] && adgniParams.width > 0 && adgniParams.height > 0) {
        adgParam_ = @{
            @"locationid" : adgniParams.adid,
            @"adtype" : @(kADG_AdType_Free),
            @"originx" : @(adgniParams.x),
            @"originy" : @(adgniParams.y),
            @"w" : @(width_),
            @"h" : @(height_),
            @"expandframe" : @(adgniParams.expandFrame)
        };
    } else {
        adgParam_ = @{
            @"locationid" : adgniParams.adid,
            @"adtype" : @(adTypeInt),
            @"originx" : @(adgniParams.x),
            @"originy" : @(adgniParams.y),
            @"expandframe" : @(adgniParams.expandFrame)
        };
    }

    [adgParam_ retain];

    gameObjName_ = [[NSString stringWithString:adgniParams.objName] retain];

    adg_ = [[ADGManagerViewController alloc] initWithAdParams:adgParam_ adView:viewCon.view];
    [adg_.view setHidden:adgniParams.hidden];
    adg_.delegate = self;
    adg_.rootViewController = viewCon;

    if (fabs(adgniParams.scale - 1.0) > 0.01) {
        [adg_ setAdScale:adgniParams.scale];
    }

    [adg_ setEnableTestMode:adgniParams.enableTest];

    [adg_ loadRequest];
}

- (void)loadRequest {
    [adg_ loadRequest];
}

- (void)pause {
    [adg_ pauseRefresh];
}

- (void)resume {
    [adg_ resumeRefresh];
}

- (void)hide {
    [adg_.view setHidden:YES];
}

- (void)show {
    [adg_.view setHidden:NO];
}

- (void)changeLocation:(float)x y:(float)y {
    CGPoint point = CGPointMake(x, y);
    [adg_ setAdOrigin:point];
}

- (void)changeLocationEasy:(NSString *)horizontal vertical:(NSString *)vertical {
    float x = [self getXFromHorizontal:horizontal];
    float y = [self getYFromtVertical:vertical];
    CGPoint point = CGPointMake(x, y);
    [adg_ setAdOrigin:point];
}

- (void)setBackgroundColorWithRed:(int)red green:(int)green blue:(int)blue alpha:(int)alpha {
    if (!adg_) return;

    if (alpha > 0.0) {
        float f_red = (float)red / 255.0;
        float f_green = (float)green / 255.0;
        float f_blue = (float)blue / 255.0;
        float f_alpha = (float)alpha / 255.0;
        UIColor *color = [[UIColor colorWithRed:f_red green:f_green blue:f_blue alpha:f_alpha] retain];

        [adg_.view setBackgroundColor:color];
        [adg_ setBackGround:color opaque:NO];
    }
}

- (void)finish {
    if (!adg_) return;

    [self clearGameObjName];

    [adgParam_ release];
    adgParam_ = nil;

    [adg_ pauseRefresh];
    [adg_.view setHidden:YES];
}

- (void)setInterParams:(UIViewController *)viewCon params:(ADGNIInterParams *)params {
    gameObjName_ = [[NSString stringWithString:params.objName] retain];
    adgInterId_ = params.adid;

    adgInter_ = [[ADGInterstitial alloc] init];
    adgInter_.delegate = self;
    adgInter_.rootViewController = viewCon;
    [adgInter_ setSpan:params.span isPercentage:params.isPercentage];
    [adgInter_ setBackgroundType:params.backgroundType];
    [adgInter_ setCloseButtonType:params.closeButtonType];
    [adgInter_ setPreventAccidentClick:params.isPreventAccidentClick];
    [adgInter_ setLocationId:adgInterId_];
    adgInter_.isFullscreen = params.isFullScreen;
    [adgInter_ setEnableTestMode:params.enableTest];
}

- (void)loadInter {
    [adgInter_ preload];
}

- (void)showInter {
    BOOL isShow = [adgInter_ show];
    if ([self canUseGameObj] && !isShow) {
        NSString *str = [NSString stringWithFormat:@"ADGInterstitialClose from iOS %@", adgInterId_];
        UnitySendMessage([gameObjName_ UTF8String], "ADGInterstitialClose", [str UTF8String]);
    }
}

- (void)dismissInter {
    [adgInter_ dismiss];
}

- (void)finishInter {
    if (!adgInter_) return;
    [adgInter_ dismiss];

    [self clearGameObjName];

    [adgInter_ setDelegate:nil];
    [adgInter_ setRootViewController:nil];
    [adgInter_ release];
    adgInter_ = nil;
}

- (float)getNativeWidthADG {
    return (float)[self getViewWidth];
}

- (float)getNativeHeightADG {
    return (float)[self getViewHeight];
}

- (BOOL)canUseGameObj {
    if (!gameObjName_) return NO;
    return [gameObjName_ length] > 0;
}

- (void)clearGameObjName {
    if (!(adg_ && adgInter_)) {
        [gameObjName_ release];
        gameObjName_ = nil;
    }
}

- (float)getXFromHorizontal:(NSString *)horizontal {
    float x = 0.0;
    if ([horizontal isEqualToString:@"CENTER"]) {
        x = ([self getViewWidth] - width_) / 2;
    } else if ([horizontal isEqualToString:@"RIGHT"]) {
        x = [self getViewWidth] - width_;
    }
    return x;
}

- (float)getYFromtVertical:(NSString *)vertical {
    float y = 0.0;
    if ([vertical isEqualToString:@"BOTTOM"]) {
        y = [self getViewHeight] - height_;
    } else if ([vertical isEqualToString:@"CENTER"]) {
        y = ([self getViewHeight] - height_) / 2;
    }
    return y;
}

- (int)getViewWidth {
    int width = frame_.size.width;
    switch ([UIApplication sharedApplication].statusBarOrientation) {
        case UIInterfaceOrientationPortrait:
        case UIInterfaceOrientationPortraitUpsideDown:
            width = [self getMinWithVal1:frame_.size.width val2:frame_.size.height];
            break;
        case UIInterfaceOrientationLandscapeLeft:
        case UIInterfaceOrientationLandscapeRight:
            width = [self getMaxWithVal1:frame_.size.width val2:frame_.size.height];
            break;
        case UIInterfaceOrientationUnknown:
            break;
    }
    return width;
}

- (int)getViewHeight {
    int height = frame_.size.height;
    switch ([UIApplication sharedApplication].statusBarOrientation) {
        case UIInterfaceOrientationPortrait:
        case UIInterfaceOrientationPortraitUpsideDown:
            height = [self getMaxWithVal1:frame_.size.width val2:frame_.size.height];
            break;
        case UIInterfaceOrientationLandscapeLeft:
        case UIInterfaceOrientationLandscapeRight:
            height = [self getMinWithVal1:frame_.size.width val2:frame_.size.height];
            break;
        case UIInterfaceOrientationUnknown:
            break;
    }
    return height;
}

- (int)getMaxWithVal1:(int)val1 val2:(int)val2 {
    return val1 > val2 ? val1 : val2;
}

- (int)getMinWithVal1:(int)val1 val2:(int)val2 {
    return val1 < val2 ? val1 : val2;
}

+ (void)addFANTestDevice:(NSString *)deviceHash {
    Class class_FBAdSettings = NSClassFromString(@"FBAdSettings");

    if (!class_FBAdSettings) return;
    if (![class_FBAdSettings respondsToSelector:sel_registerName([@"addTestDevice:" UTF8String])]) return;

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wobjc-method-access"
    [class_FBAdSettings addTestDevice:deviceHash];
#pragma clang diagnostic pop
}

+ (void)setGeolocationEnabled:(BOOL)enable {
    [ADGSettings setGeolocationEnabled:enable];
}

#pragma mark ADGManagerViewControllerDelegate

- (void)ADGManagerViewControllerReceiveAd:(ADGManagerViewController *)adgManagerViewController {
    if ([self canUseGameObj]) {
        NSString *str = [NSString stringWithFormat:@"ADGReceiveAd from iOS %@", adgManagerViewController.locationid];
        UnitySendMessage([gameObjName_ UTF8String], "ADGReceiveAd", [str UTF8String]);
    }
}

- (void)ADGManagerViewControllerFailedToReceiveAd:(ADGManagerViewController *)adgManagerViewController
                                             code:(kADGErrorCode)code {
    if ([self canUseGameObj]) {
        NSString *str = @"";
        switch (code) {
            case kADGErrorCodeExceedLimit:
                str =
                    [NSString stringWithFormat:@"ADGExceedErrorLimit from iOS %@", adgManagerViewController.locationid];
                UnitySendMessage([gameObjName_ UTF8String], "ADGExceedErrorLimit", [str UTF8String]);
                break;
            case kADGErrorCodeNeedConnection:
                str = [NSString stringWithFormat:@"ADGNeedConnection from iOS %@", adgManagerViewController.locationid];
                UnitySendMessage([gameObjName_ UTF8String], "ADGNeedConnection", [str UTF8String]);
                break;
            case kADGErrorCodeNoAd:
            case kADGErrorCodeCommunicationError:
            case kADGErrorCodeUnknown:
            case kADGErrorCodeTemplateFailed:
                str = [NSString
                       stringWithFormat:@"ADGFailedToReceiveAd from iOS %@", adgManagerViewController.locationid];
                UnitySendMessage([gameObjName_ UTF8String], "ADGFailedToReceiveAd", [str UTF8String]);
                break;
            case kADGErrorCodeReceivedFiller:
                str = [NSString stringWithFormat:@"ADGReceiveFiller from iOS %@", adgManagerViewController.locationid];
                UnitySendMessage([gameObjName_ UTF8String], "ADGReceiveFiller", [str UTF8String]);
                break;
            default:
                break;
        }
    }
}

- (void)ADGManagerViewControllerDidTapAd:(ADGManagerViewController *)adgManagerViewController {
    if ([self canUseGameObj]) {
        NSString *str = [NSString stringWithFormat:@"ADGOpenUrl from iOS %@", adgManagerViewController.locationid];
        UnitySendMessage([gameObjName_ UTF8String], "ADGOpenUrl", [str UTF8String]);
    }
}

- (void)ADGInterstitialClose {
    if ([self canUseGameObj]) {
        NSString *str = [NSString stringWithFormat:@"ADGInterstitialClose from iOS %@", adgInterId_];
        UnitySendMessage([gameObjName_ UTF8String], "ADGInterstitialClose", [str UTF8String]);
    }
}

- (void)ADGManagerViewControllerCompleteRewardAd {
    if ([self canUseGameObj]) {
        NSString *str = [NSString stringWithFormat:@"ADGCompleteRewardAd from iOS %@", adgInterId_];
        UnitySendMessage([gameObjName_ UTF8String], "ADGCompleteRewardAd", [str UTF8String]);
    }
}

@end

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma mark - C++ definition

extern "C" {
void *_initADG(void *adgni, ADGNIParams *adgniParams);
void _loadADG(void *adgni);
void _pauseADG(void *adgni);
void _resumeADG(void *adgni);
void _hideADG(void *adgni);
void _showADG(void *adgni);
void _finishADG(void *adgni);
void _changeLocationADG(void *adgni, float x, float y);
void _changeLocationEasyADG(void *adgni, const char *horizontal, const char *vertical);
void _setBackgroundColorADG(void *adgni, int red, int green, int blue, int alpha);
void _setAdScaleADG(void *adgni, float scale);

ADGNIParams *_initParamsADG();
void _setAdIDADG(ADGNIParams *adgniParams, const char *adid);
void _setAdTypeADG(ADGNIParams *adgniParams, const char *adtype);
void _setPointADG(ADGNIParams *adgniParams, float x, float y);
void _setObjNameADG(ADGNIParams *adgniParams, const char *objName);
void _setSizeADG(ADGNIParams *adgniParams, int width, int height, float scale);
void _setAlignADG(ADGNIParams *adgniParams, const char *horizontal, const char *vertical);
void _setHiddenADG(ADGNIParams *adgniParams, bool hidden);
void _setEnableTestADG(ADGNIParams *adgniParams, bool enableTest);
void _setExpandFrameADG(ADGNIParams *adgniParams, bool expandFrame);
void _releaseParamsADG(ADGNIParams *adgniParams);

void *_initInterADG(void *adgni, ADGNIInterParams *adgniInterParams);
void _loadInterADG(void *adgni);
void _showInterADG(void *adgni);
void _dismissInterADG(void *adgni);
void _finishInterADG(void *adgni);

ADGNIInterParams *_initParamsInterADG();
void _setAdIdInterADG(ADGNIInterParams *adgniInterParams, const char *adid);
void _setObjNameInterADG(ADGNIInterParams *adgniInterParams, const char *objName);
void _setBackgroundTypeInterADG(ADGNIInterParams *adgniInterParams, int backgroundType);
void _setCloseButtonTypeInterADG(ADGNIInterParams *adgniInterParams, int closeButtonType);
void _setSpanInterADG(ADGNIInterParams *adgniInterParams, int span);
void _setIsPercentageInterADG(ADGNIInterParams *adgniInterParams, bool isPercentage);
void _setIsPreventAccidentClickInterADG(ADGNIInterParams *adgniInterParams, bool isPreventAccidentClick);
void _setIsInterstitialFullScreenInterADG(ADGNIInterParams *adgniInterParams, bool isFullScreen);
void _setEnableTestInterADG(ADGNIInterParams *adgniInterParams, bool enableTest);

float _getNativeWidthADG(void *adgni);
float _getNativeHeightADG(void *adgni);
void _addFANTestDeviceADG(const char *deviceHash);
void _setGeolocationEnabledADG(bool enable);

}

#pragma mark Native interface methods for banner ads

void *_initADG(void *adgni, ADGNIParams *adgniParams) {
    ADGNI *adgni_temp;

    if (adgni == NULL) {
        adgni_temp = [[ADGNI alloc] init];
    } else {
        adgni_temp = (ADGNI *)adgni;
    }

    [adgni_temp setParams:UnityGetGLViewController() adgniParams:(ADGNIParams *)adgniParams];

    return adgni_temp;
}

void _loadADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp loadRequest];
}

void _pauseADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp pause];
}

void _resumeADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp resume];
}

void _hideADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp hide];
}

void _showADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp show];
}

void _changeLocationADG(void *adgni, float x, float y) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp changeLocation:x y:y];
}

void _changeLocationEasyADG(void *adgni, const char *horizontal, const char *vertical) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    NSString *horizontalStr = [NSString stringWithCString:horizontal encoding:NSUTF8StringEncoding];
    NSString *verticalStr = [NSString stringWithCString:vertical encoding:NSUTF8StringEncoding];
    [adgni_temp changeLocationEasy:horizontalStr vertical:verticalStr];
}

void _setBackgroundColorADG(void *adgni, int red, int green, int blue, int alpha) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp setBackgroundColorWithRed:red green:green blue:blue alpha:alpha];
}

void _finishADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp finish];
}

#pragma mark Native interface methods for banner ads parameter

ADGNIParams *_initParamsADG() {
    ADGNIParams *adgniParams = [[ADGNIParams alloc] init];
    return adgniParams;
}

void _setAdIDADG(ADGNIParams *adgniParams, const char *adid) {
    NSString *adidStr = [NSString stringWithCString:adid encoding:NSUTF8StringEncoding];
    adgniParams.adid = adidStr;
}

void _setAdTypeADG(ADGNIParams *adgniParams, const char *adtype) {
    NSString *adtypeStr = [NSString stringWithCString:adtype encoding:NSUTF8StringEncoding];
    adgniParams.adtype = adtypeStr;
}

void _setPointADG(ADGNIParams *adgniParams, float x, float y) {
    adgniParams.x = x;
    adgniParams.y = y;
}

void _setObjNameADG(ADGNIParams *adgniParams, const char *objName) {
    NSString *objNameStr = [NSString stringWithCString:objName encoding:NSUTF8StringEncoding];
    adgniParams.objName = objNameStr;
}

void _setSizeADG(ADGNIParams *adgniParams, int width, int height, float scale) {
    adgniParams.width = width;
    adgniParams.height = height;
    adgniParams.scale = scale;
}

void _setAlignADG(ADGNIParams *adgniParams, const char *horizontal, const char *vertical) {
    NSString *horizontalStr = [NSString stringWithCString:horizontal encoding:NSUTF8StringEncoding];
    NSString *verticalStr = [NSString stringWithCString:vertical encoding:NSUTF8StringEncoding];
    adgniParams.horizontal = horizontalStr;
    adgniParams.vertical = verticalStr;
}

void _setHiddenADG(ADGNIParams *adgniParams, bool hidden) {
    adgniParams.hidden = hidden;
}

void _setEnableTestADG(ADGNIParams *adgniParams, bool enableTest) {
    adgniParams.enableTest = enableTest;
}

void _setExpandFrameADG(ADGNIParams *adgniParams, bool expandFrame) {
    adgniParams.expandFrame = expandFrame;
}

void _releaseParamsADG(ADGNIParams *adgniParams) {
    [adgniParams release];
    adgniParams = nil;
}

#pragma mark Native interface methods for interstitial ads

void *_initInterADG(void *adgni, ADGNIInterParams *adgniInterParams) {
    ADGNI *adgni_temp;

    if (adgni == NULL) {
        adgni_temp = [[ADGNI alloc] init];
    } else {
        adgni_temp = (ADGNI *)adgni;
    }

    [adgni_temp setInterParams:UnityGetGLViewController() params:adgniInterParams];
    return adgni_temp;
}

void _loadInterADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp loadInter];
}

void _showInterADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp showInter];
}

void _dismissInterADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp dismissInter];
}

void _finishInterADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    [adgni_temp finishInter];
}

#pragma mark Native interface methods for interstitial ads parameter

ADGNIInterParams *_initParamsInterADG() {
    return [[ADGNIInterParams alloc] init];
}

void _setAdIdInterADG(ADGNIInterParams *adgniInterParams, const char *adid) {
    NSString *adidStr = [NSString stringWithCString:adid encoding:NSUTF8StringEncoding];
    adgniInterParams.adid = adidStr;
}

void _setObjNameInterADG(ADGNIInterParams *adgniInterParams, const char *objName) {
    NSString *objNameStr = [NSString stringWithCString:objName encoding:NSUTF8StringEncoding];
    adgniInterParams.objName = objNameStr;
}

void _setBackgroundTypeInterADG(ADGNIInterParams *adgniInterParams, int backgroundType) {
    adgniInterParams.backgroundType = backgroundType;
}

void _setCloseButtonTypeInterADG(ADGNIInterParams *adgniInterParams, int closeButtonType) {
    adgniInterParams.closeButtonType = closeButtonType;
}

void _setSpanInterADG(ADGNIInterParams *adgniInterParams, int span) {
    adgniInterParams.span = span;
}

void _setIsPercentageInterADG(ADGNIInterParams *adgniInterParams, bool isPercentage) {
    adgniInterParams.isPercentage = isPercentage;
}

void _setIsPreventAccidentClickInterADG(ADGNIInterParams *adgniInterParams, bool isPreventAccidentClick) {
    adgniInterParams.isPreventAccidentClick = isPreventAccidentClick;
}

void _setIsInterstitialFullScreenInterADG(ADGNIInterParams *adgniInterParams, bool isFullScreen) {
    adgniInterParams.isFullScreen = isFullScreen;
}

void _setEnableTestInterADG(ADGNIInterParams *adgniInterParams, bool enableTest) {
    adgniInterParams.enableTest = enableTest;
}

#pragma mark Native interface other methods

float _getNativeWidthADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    return [adgni_temp getNativeWidthADG];
}

float _getNativeHeightADG(void *adgni) {
    ADGNI *adgni_temp = (ADGNI *)adgni;
    return [adgni_temp getNativeHeightADG];
}

void _addFANTestDeviceADG(const char *deviceHash) {
    NSString *deviceHashStr = [NSString stringWithCString:deviceHash encoding:NSUTF8StringEncoding];
    [ADGNI addFANTestDevice:deviceHashStr];
}

void _setGeolocationEnabledADG(bool enable) {
    [ADGNI setGeolocationEnabled:enable];
}
