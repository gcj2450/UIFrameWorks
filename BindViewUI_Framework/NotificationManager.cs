using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Store Notification information, the notification variables include "notification name", 
/// the data that send to receiver, the script of sender, and a callback delegate,
/// The "m_name" memeber variable of this class must set, the other variables can choose if need.
/// </summary>
//public class NotificationData
//{
//    /// <summary>
//    /// The name of the notification.
//    /// </summary>
//    private string m_name;

//    /// <summary>
//    /// The data send to receiver.
//    /// </summary>
//    private object[] m_data;

//    /// <summary>
//    /// the notification sender
//    /// </summary>
//    private object m_sender;

//    /// <summary>
//    /// The callback call by View if need
//    /// </summary>
//    private System.Action<object> m_callBack;


//    /// <summary>
//    /// The name of the <c>Notification</c> instance
//    /// </summary>
//    public string Name
//    {
//        get { return m_name; }
//    }

//    /// <summary>
//    /// The data of the <c>Notification</c> instance
//    /// </summary>
//    public object[] Data
//    {
//        get
//        {
//            // Setting and getting of reference types is atomic, no need to lock here
//            return m_data;
//        }
//        set
//        {
//            // Setting and getting of reference types is atomic, no need to lock here
//            m_data = value;
//        }
//    }


//    /// <summary>
//    /// The sender of the <c>Notification</c> instance
//    /// </summary>
//    public object Sender
//    {
//        get
//        {
//            return m_sender;
//        }
//    }

//    /// <summary>
//    /// Call by view if need, it set by sender
//    /// </summary>
//    /// <value>The callback.</value>
//    public System.Action<object> Callback
//    {
//        get
//        {
//            return this.m_callBack;
//        }
//    }

//    /// <summary>
//    /// Initializes a notification by notification name
//    /// </summary>
//    public NotificationData(string name) : this(name, null, null)
//    { }

//    /// <summary>
//    /// Initializes a notification by notification name and data
//    /// </summary>
//    public NotificationData(string name, object[] data) : this(name, data, null)
//    { }

//    /// <summary>
//    /// Initializes a notification by notification name and data and sender object
//    /// </summary>
//    public NotificationData(string name, object[] data, object sender) : this(name, data, sender, null)
//    { }


//    /// <summary>
//    /// Initializes a notification by notification name and data and callBack delegate
//    /// </summary>
//    public NotificationData(string name, object[] data, System.Action<object> callBack) : this(name, data, null, callBack)
//    { }

//    /// <summary>
//    /// Initializes a notification by notification name and data and callBack delegate, and sender script object
//    /// </summary>
//    public NotificationData(string name, object[] data, object sender, System.Action<object> callBack)
//    {
//        m_name = name;
//        m_data = data;
//        m_sender = sender;
//        m_callBack = callBack;
//    }


//    public bool IsDataNullOrEmpty()
//    {
//        if (this.Data == null || this.Data.Length == 0)
//            return true;

//        return false;
//    }
//    /// <summary>
//    /// Get the string representation of the <c>Notification instance</c>
//    /// </summary>
//    /// <returns>The string representation of the <c>Notification</c> instance</returns>
//    public override string ToString()
//    {
//        string msg = "Notification Name: " + Name;
//        msg += "\nData:" + ((Data == null) ? "null" : Data.ToString());
//        msg += "\nSender:" + ((Sender == null) ? "null" : Sender);
//        return msg;
//    }
//}

/// <summary>
/// A notification center manages the sending and receiving of notifications.
/// </summary>
public class NotificationManager
{

    /// <summary>
    /// The m_view observers dict. The key is constant message variable, the value is the view action class.
    /// </summary>
    public Dictionary<string, List<IBaseEventAction>> m_viewObserversDict = new Dictionary<string, List<IBaseEventAction>>();

    /// <summary>
    /// m_nameActionPairDict will use name to get Action
    /// </summary>
    private Dictionary<string, IBaseEventAction> m_nameActionPairDict = new Dictionary<string, IBaseEventAction>();
    public Dictionary<string, IBaseEventAction> NameActionPairDict
    {
        get
        {
            return m_nameActionPairDict;
        }
    }
    private UIActionRegister _uiRegister;

    public UIActionRegister UIRegister { get { return _uiRegister; } }
    public virtual void Init()
    {
        _uiRegister = new UIActionRegister();
        _uiRegister.RegisterViewActions();
    }

    // Register a observer into NotificationCenter
    public void RegisterObserver(string name, IBaseEventAction observer)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Null name specified for notification in AddObserver.");
            return;
        }


        if (!m_viewObserversDict.ContainsKey(name))
        {
            m_viewObserversDict.Add(name, new List<IBaseEventAction>());
        }
        List<IBaseEventAction> baseViewList = m_viewObserversDict[name];
        if (!baseViewList.Contains(observer))
            baseViewList.Add(observer);
    }

    /// <summary>
    /// Sends the notification with name 
    /// </summary>
    /// <param name="name">Name.</param>
    public void SendNotification(object sender, string name )
    {
        SendNotification(sender, new ChangeUIEventArgs(name));
    }
    /// <summary>
    /// Sends the notification whit data & callBack delegate with has one arg
    /// </summary>
    public void SendNotification(object sender, string name, ChangeUIEventArgs args)
    {
        args.Name = name;
        SendNotification(sender, args);
    }

    // Post notification to observers 
    public void SendNotification(object sender, ChangeUIEventArgs args)
    {
        List<IBaseEventAction> observers = null;

        if (m_viewObserversDict.ContainsKey(args.Name))
        {
            observers = m_viewObserversDict[args.Name];
        }
        else
        {
            Debug.Log("GlobalUIVariable : '" + args.Name + "' is not exist in GlobalUIVariable.cs or ListNotificationString or you not regiester Action On UIActionRegister!");
        }

        if (observers != null)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].HandleNotification(sender,args);
            }
        }
    }


    // Remove a observer from NotificationCenter
    public void RemoveObserver(string name, IBaseEventAction observer)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Null name specified for notification in RemoveObserver.");
            return;
        }

        if (!m_viewObserversDict.ContainsKey(name))
        {
            return;
        }

        List<IBaseEventAction> baseViewList = m_viewObserversDict[name];
        if (baseViewList.Contains(observer))
        {
            baseViewList.Remove(observer);
            if (baseViewList.Count == 0)
                m_viewObserversDict.Remove(name);
        }
    }

}
