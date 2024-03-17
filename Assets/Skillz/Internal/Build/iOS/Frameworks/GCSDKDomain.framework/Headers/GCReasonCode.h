//
//  GCReasonCode.h
//  GCSDKDomain
//
//  Created by Huy Thai on 19/09/2022.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, GCReasonCode) {
    GCReasonCodeLogin = 1,
    GCReasonCodeInterval = 2,
    GCReasonCodeIPChange = 3,
    GCReasonCodeUserDrivenRetry = 4,
    GCReasonCodePostIntervalForeground = 5,
    GCReasonCodeGameLaunchOrSwitch =  6,
    GCReasonCodeRegistration = 7,
    GCReasonCodeDeposit = 8,
    GCReasonCodePurchase = 9,
    GCReasonCodeSell = 10,
};


typedef NS_ENUM(NSInteger, GCGameCode) {
    GCGameCodeSports = 1,
    GCGameCodeCasino = 2,
    GCGameCodeDFS = 3,
    GCGameCodeBingo = 4,
    GCGameCodeSkillGame = 5,
    GCGameCodeP2P = 6,
    GCGameCodeRoulette = 7,
    GCGameCodeSlot = 8,
    GCGameCodeOther = 9,
};
