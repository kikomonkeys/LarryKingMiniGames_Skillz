//
//  NativeCallProxy.h
//  GameStake
//
//  Created by Alejandro Perez on 20/01/2022.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@protocol NativeCallsProtocol
@required
- (void) sendMessageToMobileApp:(NSString*)message;
// other methods
@end

@protocol AdCreatorProtocol
@required
- (void) createRewardedAd:(NSString*)rewardedVideo_id;
- (void) createBannerAd:(NSString*)banner_id;
- (void) createInterstitialAd:(NSString*)inter_id;
// other methods
@end

__attribute__ ((visibility("default")))
@interface FrameworkLibAPI : NSObject
+(void) registerAPIforNativeCalls:(id<NativeCallsProtocol>) aApi;
@end

__attribute__ ((visibility("default")))
@interface AdCreationAPI : NSObject
+(void) registerAPIforNativeCalls:(id<AdCreatorProtocol>) aApi;

@end
