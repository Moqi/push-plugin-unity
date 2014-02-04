using System.Collections.Generic;
using UnityEngine;

public class InfobipPushNotification 
{
    public string NotificationId
    { 
        get; 
        set; 
    }
    
    public string Sound
    {
        get;
        set;
    }
    
    public string Url
    {
        get;
        set;
    }
    
    public object AdditionalInfo
    {
        get;
        set;
    }
    
    public string MediaData
    {
        get;
        set;
    }
    
    public bool isMediaNotification()
    {
        if (MediaData != null)
        {
            return true;
        } 
        return false;
    }
    
    public string Title
    {
        get;
        set;
    }
    
    public string Message
    {
        get;
        set;
    }
    
    public string MimeType
    {
        get;
        set;
    }
    
    public int Badge
    {
        get;
        set;
    }
    
    public override string ToString()
    {
		IDictionary<string, object> notif = new Dictionary<string, object>(9);
		notif ["notificationId"] = NotificationId;
		notif ["sound"] = Sound; 
		notif ["url"] = Url;
		notif ["additionalInfo"] = AdditionalInfo;
		notif ["mediaData"] = MediaData;
		notif ["title"] = Title;
		notif ["message"] = Message; 
		notif ["mimeType"] = MimeType;
	    notif ["badge"] = Badge; 
		return MiniJSON.Json.Serialize(notif);
    }
    
    public InfobipPushNotification(string notif)
    {
        IDictionary<string, object> dictNotif = MiniJSON.Json.Deserialize(notif) as Dictionary<string,object>;
        object varObj = null;
        int varInt;
        if (dictNotif.TryGetValue("notificationId", out varObj))
        {
            NotificationId = (string)varObj;
        }
        if (dictNotif.TryGetValue("title", out varObj))
        {
            Title = (string)varObj;
        }
        //IDictionary<string, int> dictNotifInt = dictNotif as Dictionary<string, int>;
        if (dictNotif.TryGetValue("badge", out varObj))
        {
            // TODO: fix 'badge' (string "" if it is 0, and int if not)
            if (varObj as string != null)
            {
                Badge = 0;
            } else
            {
                varInt = (int)varObj;
                Badge = varInt;
            }
        }
        if (dictNotif.TryGetValue("sound", out varObj))
        {
            Sound = (string)varObj;
        }
        if (dictNotif.TryGetValue("mimeType", out varObj))
        {
            MimeType = (string)varObj;
        }
        if (dictNotif.TryGetValue("url", out varObj))
        {
            Url = (string)varObj;
        }
        if (dictNotif.TryGetValue("aditionalInfo", out varObj))
        {
//            print("additionalInfo real: " + varObj as string);
//            print("additionalInfo " + MiniJSON.Json.Serialize(AdditionalInfo));
            // TODO: store this value in this.AdditionalInfo
        }
        if (dictNotif.TryGetValue("mediaData", out varObj))
        {
			MediaData = (string)varObj;
//			print("MediaData: " + MediaData);
        }
        if (dictNotif.TryGetValue("message", out varObj))
        {
            Message = (string)varObj;
        }
    }
    
    public InfobipPushNotification()
    {
    }
}
