using System;
using System.Collections;
using System.Collections.Generic;

public class App
{
    private static App app = null;
    public static App Instance { get { return app; } }
    
    private NotificationManager _notificationManager = new NotificationManager();

    public static NotificationManager Notifier { get { return app._notificationManager; } }

    //private ResMgr resMgr;
    //public static ResMgr ResMgr
    //{
    //    get
    //    {

    //        if (app == null)
    //            return null;

    //        return app.resMgr;
    //    }
    //}


    public static void Create()
    {
        app = new App();
    }

    public void Start()
    {
        _notificationManager.Init();
    }
}
