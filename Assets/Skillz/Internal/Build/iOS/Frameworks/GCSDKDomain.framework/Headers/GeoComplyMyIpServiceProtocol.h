//
//  GeoComplyMyIpServiceProtocol.h
//  GCClient
//
//  Created by Sang Nguyen on 19/05/2021.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

/**
 This handler is called when MyIP service detects IP change.
 
 @param ip  The public IP of device.
 */
typedef void (^MyIpServiceSuccessHandler)(NSString *ip);

/**
 This handler is called when MyIP service gets an error.
 
 @param error  Indicates the error code, description, and timestamp.
 */
typedef void (^MyIpServiceFailureHandler)(GCMyIpError *error);


@protocol GeoComplyMyIpServiceProtocol <NSObject>
@required

/**
 Returns a boolean to indicate whether MyIP service is running or not.
 */
- (BOOL)isMyIpServiceRunning;

/**
 Starts MyIP service.
 
 @param onMyIpSuccess  It is called when MyIP service detects IP change. @see MyIpServiceSuccessHandler.
 @param onMyIpFailure  It is called when MyIP service gets an error. @see MyIpServiceFailureHandler.
 */
- (void)startMyIpServiceWithSuccess:(nonnull MyIpServiceSuccessHandler)onMyIpSuccess
                            failure:(nonnull MyIpServiceFailureHandler)onMyIpFailure;

/**
 Stops MyIP service.
 
 @warning Calls this method will invoke failureHandler from -startMyIpServiceWithSuccess:failure: with error code 700.
 */
- (void)stopMyIpService;


/**
 Calls this method to acknowledge receipt of an IP address change.
 */
- (void)ackMyIpSuccess;

@end


NS_ASSUME_NONNULL_END
