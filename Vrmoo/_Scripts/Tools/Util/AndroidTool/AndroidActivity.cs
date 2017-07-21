using UnityEngine;
using System.Collections;

public class AndroidActivity 
{
    public static AndroidJavaObject Current()
    {
        if (Application.platform == RuntimePlatform.Android)
			//return new AndroidJavaClass("com.google.vrtoolkit.cardboard.plugins.unity.UnityCardboardActivity").GetStatic<AndroidJavaObject>("Instance");
			return new AndroidJavaClass("com.mojing.u3d.pubblico.activity.UnityActivity").GetStatic<AndroidJavaObject>("Instance");
        else
            return null;
    }

    public static AndroidJavaObject GetActivity(string package_name,string activity_name)
    {
        return new AndroidJavaClass(package_name).GetStatic<AndroidJavaObject>(activity_name);
    }

}
