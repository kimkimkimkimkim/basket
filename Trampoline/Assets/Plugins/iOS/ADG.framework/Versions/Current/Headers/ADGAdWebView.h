#import <UIKit/UIKit.h>

@interface ADGAdWebView : UIView <UIWebViewDelegate>

@property (nonatomic, weak) id delegate;
@property (nonatomic, strong, readonly) UIWebView *adWebView;
@property (nonatomic, assign, setter=setScrollEnabled:, getter=isScrollEnabled) BOOL scrollEnabled;
@property (nonatomic, assign) BOOL mraidEnabled;
@property (nonatomic, assign) BOOL isInterstitial;
@property (nonatomic, assign) BOOL checkingWebViewFillerAd;

- (void)loadHTMLString:(NSString *)HTML baseURL:(NSURL *)baseURL;
- (void)setFrame:(CGRect)rect;
- (void)stopLoading;
- (BOOL)isClicked;
- (BOOL)isNoAd:(BOOL)callDelegate;
- (void)resetNoAdCount;
- (void)setWebViewBackgroundColor:(UIColor *)color;
- (void)setWebViewOpaque:(BOOL)opaque;
- (void)setAdScale:(float)scale;

@end

@protocol ADGAdWebViewDelegate

@optional
- (BOOL)adgAdWebView:(UIWebView *)webView
    shouldStartLoadWithRequest:(NSURLRequest *)request
                navigationType:(UIWebViewNavigationType)navigationType;
- (void)adgAdWebViewDidFinishLoad:(UIWebView *)webView;
- (void)adgAdWebView:(UIWebView *)webView didFailLoadWithError:(NSError *)error;
- (void)adgNoAd;
@end
