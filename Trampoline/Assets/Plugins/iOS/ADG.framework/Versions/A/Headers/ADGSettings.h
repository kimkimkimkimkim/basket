//
//  ADGSettings.h
//  ADG
//
//  Copyright © 2017年 adgeneration. All rights reserved.
//


/**
 Video Audio Type

 - ADGVideoAudioTypeMix: Enabling audio playing while background audio is being played.
 - ADGVideoAudioTypeSolo: Enabling audio playing with stopping background audio.
 - ADGVideoAudioTypeSoloForce: Enabling audio playing with stopping background audio, and it is played even in silent mode.
 */
typedef NS_ENUM(NSUInteger, ADGVideoAudioType) {
    ADGVideoAudioTypeMix,
    ADGVideoAudioTypeSolo,
    ADGVideoAudioTypeSoloForce,
};

@interface ADGSettings : NSObject

/**
 * Get the setting whether this SDK automatically get Geolocation.
 * @return YES: enable / NO: disable
 */
+ (BOOL)isGeolocationEnabled;

/**
 * Determines whether this SDK automatically get Geolocation.
 * @param enable YES: enable / NO: disable
 */
+ (void)setGeolocationEnabled:(BOOL)enable;

/**
 * Determines whether check filler ad.
 */
@property (class, nonatomic, assign) BOOL fillerAdCheckEnable;

/**
 * Determines whether video ads playback sound enable in silent mode.
 */
@property (class, nonatomic) BOOL videoAdsPlaybackSoundInSilentModeEnabled DEPRECATED_MSG_ATTRIBUTE("Use +videoAudioType instead");

/**
 * Set video audio type
 */
@property (class, nonatomic) ADGVideoAudioType videoAudioType;

@end
