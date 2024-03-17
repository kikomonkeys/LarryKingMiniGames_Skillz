//
//  GCCancelReason.h
//  GCClient
//
//  Created by geotech on 17/05/2022.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSUInteger, GCCancelReason) {
  GCCancelReasonNoReason,
  GCCancelReasonUserCancels,
  GCCancelReasonApplicationEntersBackground,
  GCCancelReasonRequestNotNeeded,
  GCCancelReasonLongRequest,
  GCCancelReasonOther
};



