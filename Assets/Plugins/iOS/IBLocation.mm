//
//  IBLocation.m
//  Unity-iPhone
//
//  Created by MMiroslav on 1/29/14.
//
//

#import "IBLocation.h"

NSString *const PUSH_SHARE_LOCATION = @"IBShareLocation_SUCCESS";

void IBEnableLocation() {
    [InfobipPush startLocationUpdate];
}

void IBDisableLocation() {
    [InfobipPush startLocationUpdate];
}

BOOL IBIsLocationEnabled() {
    return [InfobipPush locationUpdateActive];
}

void IBSetBackgroundLocationUpdateModeEnabled(const int enable) {
    NSNumber * locationEnabled = [NSNumber numberWithInt:enable];
    [InfobipPush setBackgroundLocationUpdateModeEnabled:[locationEnabled boolValue]];
}

BOOL IBBackgroundLocationUpdateModeEnabled() {
    return [InfobipPush backgroundLocationUpdateModeEnabled];
}

void IBSetLocationUpdateTimeInterval(const int minutes) {
    [InfobipPush setLocationUpdateTimeInterval:minutes];
}

int IBGetLocationUpdateTimeInterval() {
    return [InfobipPush locationUpdateTimeInterval];
}

void IBShareLocation(const char *locationCharArray) {
    NSError *e;
    NSString *locationString = [NSString stringWithFormat:@"%s", locationCharArray];
    NSDictionary *locationDict = [NSJSONSerialization JSONObjectWithData:[locationString  dataUsingEncoding:NSUTF8StringEncoding]
                                                                 options:NSJSONReadingMutableContainers error:&e];
    
    CLLocationCoordinate2D coords = CLLocationCoordinate2DMake([[locationDict objectForKey:@"latitude"] floatValue],
                                                               [[locationDict objectForKey:@"longitude"] floatValue]);
    NSDateFormatter *df = [[NSDateFormatter alloc] init];
    [df setDateFormat:@"yyyy-MM-dd hh:mm:ss a"];
    NSDate *myDate = [df dateFromString: [locationDict objectForKey:@"timestamp"]];
    
    CLLocation *location = [[CLLocation alloc] initWithCoordinate:coords altitude:[[locationDict objectForKey:@"altitude"] floatValue] horizontalAccuracy:[[locationDict objectForKey:@"horizontalAccuracy"] floatValue] verticalAccuracy:[[locationDict objectForKey:@"verticalAccuracy"] floatValue] timestamp:myDate];
    
    [InfobipPush shareLocation:location withBlock:^(BOOL succeeded, NSError *error) {
        if (succeeded) {
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_SHARE_LOCATION UTF8String], [@"" UTF8String]);
        } else {
            [IBPushUtil passErrorCodeToUnity:error];
        }
    }];
}