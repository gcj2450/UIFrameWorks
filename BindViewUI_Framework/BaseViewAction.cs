using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// The base class of action script view, the child class maintains the sent notification to control the linked view script.
/// All view need an action script to receive notification message except no notification view.
/// </summary>


public abstract class BaseViewAction : IBaseEventAction
{
    public ChangeUIEventArgs ArgsData;
    /// <summary>
    /// The view component which this action bind
    /// </summary>
    public object ViewComponent { get; set; }


    public virtual void Init()
    {
        //Horizon.Debug.Log("Init a new Action");
        OnNotificationRegister();

        if (App.Notifier.NameActionPairDict.ContainsKey(typeof(BaseViewAction).Name))
            App.Notifier.NameActionPairDict.Add(typeof(BaseViewAction).Name, this);
    }


    public abstract void HandleNotification(object sender, ChangeUIEventArgs args);
    /// <summary>
    /// List the <c>NotificationData</c> names this <c>Action</c> is interested in being notified of
    /// </summary>
    /// <returns>The list of <c>Notification</c> names </returns>
    public abstract List<string> ListNotificationString();

    public void OnNotificationRegister()
    {
        List<string> strs = this.ListNotificationString();
        for (int i = 0; i < strs.Count; i++)
        {
            //Horizon.Debug.Log(this);
            App.Notifier.RegisterObserver(strs[i], this);
        }
    }

    public void OnNotificationRemove()
    {

    }


}
