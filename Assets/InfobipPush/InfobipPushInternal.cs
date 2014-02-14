using UnityEngine;
using System.Collections;
using System;

public class InfobipPushInternal : MonoBehaviour
{
    #if UNITY_ANDROID
    private static AndroidJavaObject infobipPushJava = null;
    #endif

    #region singleton game object
    private const string SINGLETON_GAME_OBJECT_NAME = "InfobipPushNotifications";
   
    
    public static InfobipPushInternal Instance
    {
        get
        {
            return GetInstance();
        }
    }
    
    private static InfobipPushInternal _instance = null;
    
    internal static InfobipPushInternal GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType(typeof(InfobipPushInternal)) as InfobipPushInternal;
            if (_instance == null)
            {
                var gameObject = new GameObject(SINGLETON_GAME_OBJECT_NAME);
                _instance = gameObject.AddComponent<InfobipPushInternal>();
            }
        }
        return _instance;
    }
    #endregion

    internal static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    public void IBPushDidReceiveRemoteNotification(string notification)
    {
        InfobipPush.OnNotificationReceived(new InfobipPushNotification(notification));
    }

    public void IBPushDidOpenRemoteNotification(string notification)
    {
        InfobipPush.OnNotificationOpened(new InfobipPushNotification(notification));
    }

    public void IBPushDidRegisterForRemoteNotificationsWithDeviceToken()
    {
        InfobipPush.OnRegistered();
    }

    public void IBSetUserId_SUCCESS()
    {
        InfobipPush.OnUserDataSaved();
    }

    public void IBUnregister_SUCCESS()
    {
        InfobipPush.OnUnregistered();
    }

    public void IBPushErrorHandler(string errorCode)
    {
        InfobipPush.OnError(errorCode);
    }

    public void IBShareLocation_SUCCESS()
    {
        InfobipPush.OnLocationShared();
    }

    public void IBGetChannels_SUCCESS(string channels)
    {
        InfobipPush.OnGetChannelsFinished(channels);
    }

    public void IBSetChannels_SUCCESS()
    {
        InfobipPush.OnRegisteredToChannels();
    }

    public void IBGetUnreceivedNotifications_SUCCESS(string notificationsList)
    {   
        var list = MiniJSON.Json.DeserializeArrayNoRecursion(notificationsList);
        foreach (var notifJson in list)
        {
            InfobipPushNotification notification = new InfobipPushNotification(notifJson as String);
            InfobipPush.OnUnreceivedNotificationReceived(notification);
        }
    }
    
    #if UNITY_ANDROID
    internal void SetLogModeEnabled(bool enabled)
    {
        GetCurrentActivity().Call("setDebugMode", new object[] { enabled });
    }

    public bool GetLogModeEnabled()
    {
        return GetCurrentActivity().Call<bool>("getDebugMode", new object[] { });
    }

    internal void Initialize(string applicationId, string applicationSecret, InfobipPushRegistrationData registrationData)
    {
        GetCurrentActivity().Call("initialize", new object[] {
            applicationId, applicationSecret, (registrationData != null ? registrationData.ToString() : "null")
        });
    }

    internal bool IsRegistered()
    {
        return GetCurrentActivity().Call<bool>("isRegistered", new object[] {});
    }

    internal void Unregister()
    {
        GetCurrentActivity().Call("unregister", new object[] {});
    }

    public void SetTimeZoneOffset(int offsetMinutes)
    {
        GetCurrentActivity().Call("setTimeZoneOffset", new object[] {offsetMinutes});
       
    }

    public void SetTimeZoneAutomaticUpdateEnabled(bool isEnabled)
    { 
        GetCurrentActivity().Call("setTimeZoneAutomaticUpdateEnabled", new object[] {isEnabled});
    }

    internal string GetDeviceId()
    {
        return GetCurrentActivity().Call<string>("getDeviceId", new object[] {});
    }

    internal string GetUserId()
    {
        return GetCurrentActivity().Call<string>("getUserId", new object[] {});
    }

    internal void BeginSetUserId(string value)
    {
        GetCurrentActivity().Call("setUserId", new object[] { value });
    }

    internal void RegisterToChannels(string value)
    {
        GetCurrentActivity().Call("registerToChannels", new object[] { value });
    }

    internal static AndroidJavaObject GetCurrentActivity()
    {
        if (infobipPushJava == null)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                infobipPushJava = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            } 
        }
        return infobipPushJava;
    }
    #endif
   
}
