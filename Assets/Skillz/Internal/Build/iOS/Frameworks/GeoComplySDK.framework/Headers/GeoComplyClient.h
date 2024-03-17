//
//  GeoComplyCient.h
//  GCClient
//
//  Created by Logan on 12/24/19.
//

#import <Foundation/Foundation.h>
#import <GCSDKDomain/GCSDKDomain.h>

@interface GeoComplyClient : NSObject <GeoComplyClientProtocol, GeoComplyMultipleSDKProtocol, GeoComplyMyIpServiceProtocol>

#pragma mark - GeoComplyClientProtocol

/**
 Sets or return current user id for geolocation request.
 */
@property (nonatomic, copy, nullable) NSString* userId;

/**
 Sets or return geolocation reason for geolocation request.
 */
@property (nonatomic, copy, nullable) NSString* geolocationReason;

/**
 Sets or return user phone number for geolocation request.
 */
@property (nonatomic, copy, nullable) NSString* userPhoneNumber;

/**
 Sets or return GCCustomFields for geolocation request.
 
 @see GCCustomFields.
 */
@property (nonatomic, readonly, nullable) GCCustomFields* customFields;

/**
 Sets or returns protocol for GCClientDelegate.
 
 @see GCClientDelegate.
 */
@property (nonatomic, weak, nullable) id<GCClientDelegate> delegate;


/**
 Please use the +instance method to create an instance for GeoComplyClient instead of the -init method.
 
 @see The +instance method.
 */
- (instancetype _Nullable)init NS_UNAVAILABLE;

/**
 Sets license string to GeoComplyClient instance.
 
 @param license The license string retrived from GeoComply Server.
 @param errorPtr An error pointer for error diagnostic.
 
 @return A boolean to indicate whether the license has been set successfully.
 */
- (BOOL)setLicense:(nonnull NSString*)license error:(NSError* _Nullable __autoreleasing* _Nullable)errorPtr;

/**
 Calls a geolocation request.
 
 @param error An error pointer for error diagnostic.
 
 @return A boolean to indicate whether the geolocation request is called successfully.
 */
- (BOOL)requestGeolocation:(NSError* _Nullable __autoreleasing* _Nullable)error;

/**
 Calls a geolocation request with timeout.
 
 @param error An error pointer for error diagnostic.
 @param timeout A timeout value for geolocation request.
 
 @return A boolean to indicate whether the geolocation request is called successfully.
 
 @warning If timeout value is equal to 0, GeoComply iOS SDK will use timeout value configured on GeoComply Server.
 This is equivalent to -requestGeolocation:
  */
- (BOOL)requestGeolocation:(NSError* _Nullable __autoreleasing* _Nullable)error timeout:(NSInteger)timeout;

/**
 Creates and returns a GeoComplyClient object.
 
 @warning The returned object is a singleton.
 */
+ (id<GeoComplyClientProtocol, GeoComplyMultipleSDKProtocol, GeoComplyMyIpServiceProtocol> _Nonnull)instance;

/**
 Handles memory warning notification.
 
 @warning This will cancel current geolocation request and returns error GCCErrorLowMemory via GCClientDelegate protocol -didGeolocationFailed:.
 
 @see -didGeolocationFailed:
 */
- (void)handleMemoryWarning;

/**
 Calls indoor geolocation updates.
 
 @warning Calls this method will auto reschedule next indoor geolocation updates. No further scheduling is required.
 */
- (BOOL)startUpdating:(NSError* _Nullable __autoreleasing* _Nullable)error;

/**
 Stops indoor geolocation updates.
 
 @warning Calls this method will invoke GCClientDelegate protocol -didStopUpdating:.
 
 @see -didStopUpdating:.
 */
- (void)stopUpdating;

/**
 Returns a boolean to indicate whether GeoComplyClient iOS SDK is updating indoor geolocation.
 */
- (BOOL)isUpdating;

/**
 Returns UUID of current geolocation request.
 
 @warning This method only returns valid UUID string when triggering -requestGeolocation:.
 
 @see -requestGeolocation:.
 */
- (nullable NSString*)currentRequestUUID;

/**
 Return the SDK version.
 */
- (NSString *_Nonnull)version;


#pragma mark - GeoComplyMultipleSDKProtocol

/**
 Select the sdk version for using.
 
 @param version A version string.
 
 @warning If version is not equal to any sdk version inside this sdk, GeoComply iOS sdk will return error. Otherwise, GeoComply iOS SDK will using  the version as expected. Unless User set the version, GeoComply iOS SDK will use the oldest version by default.
 */
- (void)useSDKVersion:(NSString* _Nonnull)version error:(NSError* _Nullable __autoreleasing* _Nullable)error;

/**
 Returns the current SDK that is using.
 */
- (NSString* _Nonnull)currentUsingSDKVersion;

/**
 Handle launch options when the app is woken up from suspended state
 
 @param launchOptions The launch options dictionary which is passed into app's delegate methods.
 @param sdkVersion The SDK version that handles the event.
 */
+ (void)handleLaunchOptions:(NSDictionary* _Nullable)launchOptions sdkVersion:(NSString* _Nonnull)sdkVersion;

/**
 Handle touch events to detect remote control transactions.
 
 To ensure the remote control detection works well, the app has to add the method
    -touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
 for UIControls that users have to touch on them to play betting.
 
 When the method -touchesBegan:withEvent: is called, the app has to call the SDK method as the following example:
 
    @implementation MyAppButton
 
    - (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event {
        [GeoComplyClient touchesBegan:touches withEvent:event onView:self sdkVersion:@"2.8.0"];
        [super touchesBegan:touches withEvent:event]
    }
 
    @end
 
 @param touches       Returned from -touchesBegan:withEvent:
 @param event           Returned from -touchesBegan:withEvent:
 @param view             The view that handles the method -touchesBegan:withEvent:
 @param sdkVersion The current SDK version that app is using
 */
+ (void)touchesBegan:(NSSet* _Nonnull)touches withEvent:(UIEvent* _Nonnull)event onView:(UIView* _Nonnull)view sdkVersion:(NSString * _Nonnull)sdkVersion;

/**
 Sets the user session id for geolocation request.
 */
- (void)setUserSessionID:(NSString* _Nullable)userSessionID;

/**
 Return the current user session id.
 */
- (NSString * _Nullable)currentUserSessionID;

/**
 Invalidate the current user session id.
 */
- (void)invalidateUserSession;

#pragma mark - GeoComplyMyIpServiceProtocol

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

#pragma mark - Cancel ongoing geolocation

/**
 Return boolean to indicate whether geolocation is in process or not
 */
- (BOOL)isGeolocationInProgress;

/**
 Cancel the ongoing geolocation
 
 @param reason  A reson enum for cancel.
 @param details The message explains why the apps cancels the geolocation request.
 @param completion The callback block after completing cancellation.
 
 @warning It  is only available in version 2.7.0+.
 */
- (void)cancelCurrentGeolocationReason:(GCCancelReason)reason
                              details:(NSString* _Nullable)details
                           completion:(void(^ _Nonnull)(BOOL success, NSString* _Nullable description))completion;

#pragma mark - Reason Code & Game Code

/**
 Set reason code value for geolocation request.
 
 @param reasonCode  A reason code enum for setting value.
 
 @warning It is only available in version 2.8.0+.
 */
- (void)setReasonCode:(GCReasonCode)reasonCode;

/**
 Set game code value for geolocation request.
 
 @param gameCode  A game code enum for setting value.
 
 @warning It is only available in version 2.8.0+.
 */
- (void)setGameCode:(GCGameCode)gameCode;

/**
 Get current reason code value.
 
 @return GCReasonCode, which is a reaon code enum is equal to current value if it is not nil. Otherwise, returned value is 0.
 
 @warning It is only available in version 2.8.0+.
 */
- (GCReasonCode)reasonCode;

/**
 Get current game code value.
 
 @return GCGameCode, which is a game code enum is equal to current value if it is not nil. Otherwise, returned value is GCGameCodeOther.
 
 @warning It is only available in version 2.8.0+.
 */
- (GCGameCode)gameCode;

@end
