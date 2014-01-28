#import "CWrappedInfobipPush.h"



void IBSetLogModeEnabled(bool isEnabled, int lLevel) {
    NSLog(@"IBSetLogModeEnabled method");
    InfobipPushLogLevel logLevel = IPPushLogLevelDebug;
    switch (lLevel) {
        case 0: break;
        case 1: logLevel = IPPushLogLevelInfo; break;
        case 2: logLevel = IPPushLogLevelWarn; break;
        case 3: logLevel = IPPushLogLevelError; break;
        default: NSLog(@"IBSetLogModeEnabled-> lLevel > 3");
    }
    
    [InfobipPush setLogModeEnabled:isEnabled withLogLevel:logLevel];
};

bool IBIsLogModeEnabled() {
    return [InfobipPush isLogModeEnabled];
};

void IBSetTimezoneOffsetInMinutes(int offsetMinutes){
    NSLog(@"IBSetTimezoneOffsetInMinutes method");
    [InfobipPush setTimezoneOffsetInMinutes:offsetMinutes];
};

void IBSetTimezoneOffsetAutomaticUpdateEnabled (bool isEnabled){
    NSLog(@"IBSetTimezoneOffsetAutomaticUpdateEnabled method");
    [InfobipPush setTimezoneOffsetAutomaticUpdateEnabled:isEnabled];
};

void IBInitialization(char * appId, char * appSecret){
    NSLog(@"IBInitialization");
    NSString * applicationId = [NSString stringWithFormat:@"%s",appId];
    NSString * applicationSecret = [NSString stringWithFormat:@"%s",appSecret];
    
    [InfobipPush initializeWithAppID:applicationId appSecret:applicationSecret];
	[[UIApplication sharedApplication] registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge |
                                                                           UIRemoteNotificationTypeSound |
                                                                           UIRemoteNotificationTypeAlert)];
}

void IBInitializationWithRegistrationData(char * appId, char * appSecret, char * registrationData) {
    IBInitialization(appId, appSecret);
    //    NSLog(@"RegistrationData: %@", registrationData);
    
    NSError *e;
    NSString * regDataString = [NSString stringWithFormat:@"%s", registrationData];
    NSDictionary * regDictionary = [NSJSONSerialization JSONObjectWithData:[regDataString  dataUsingEncoding:NSUTF8StringEncoding]
                                                                   options:NSJSONReadingMutableContainers error:&e];
    
    NSString * userId = [regDictionary objectForKey:@"userId"];
    NSArray * channels = [regDictionary objectForKey:@"channels"];
    
    // prepare channels for AppDelegate
    [IBPushUtil setChannels:channels];
    
    // set UserId
    [InfobipPush setUserID:userId usingBlock:^(BOOL succeeded, NSError *error) {
        if (!succeeded) {
            NSString * errorCode = [NSString stringWithFormat:@"%u", [error code]];
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_ERROR_HANDLER UTF8String], [errorCode UTF8String]);
        }
    }];
}

bool IBIsRegistered(){
    return [InfobipPush isRegistered];
};

void IBInitializationWithRegistrationData(char * appId, char * appSecret, char * registrationData) {
    IBInitialization(appId, appSecret);
    NSLog(@"RegistrationData: %s", registrationData);
};

char* cStringCopy(const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
};

char* IBDeviceId(){
    NSString* devId=[InfobipPush deviceID];
    return cStringCopy([devId UTF8String]);
};
