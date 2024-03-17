//
//  NativeCallProxy.m
//  GameStake
//
//  Created by Alejandro Perez on 20/01/2022.
//

#import <Foundation/Foundation.h>
#import "NativeCallProxy.h"

@implementation FrameworkLibAPI

id<NativeCallsProtocol> api = NULL;
+(void) registerAPIforNativeCalls:(id<NativeCallsProtocol>) aApi
{
    api = aApi;
}

@end

@implementation AdCreationAPI

id<AdCreatorProtocol> adApi = NULL;
+(void) registerAPIforNativeCalls:(id<AdCreatorProtocol>) aApi
{
    adApi = aApi;
}

@end

extern "C"
{
    void sendMessageToMobileApp(const char* message)
    {
        return [api sendMessageToMobileApp:[NSString stringWithUTF8String:message]];
    }

    void createRewardedAd(const char* rewardedVideo_id)
    {
        return [adApi createRewardedAd:[NSString stringWithUTF8String:rewardedVideo_id]];
    }

    void createBannerAd(const char* banner_id)
    {
        return [adApi createBannerAd:[NSString stringWithUTF8String:banner_id]];
    }

    void createInterstitialAd(const char* inter_id)
    {
        return [adApi createInterstitialAd:[NSString stringWithUTF8String:inter_id]];
    }
}
