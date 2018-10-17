#import <Foundation/Foundation.h>
#import <AudioToolBox/AudioToolBox.h>

extern "C" void Touch3D (int n)
{
    AudioServicesPlaySystemSound(n);
}
