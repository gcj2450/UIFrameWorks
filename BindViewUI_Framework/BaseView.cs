using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public abstract class BaseView : MonoBehaviour
{
    protected System.Object sender;
    protected ChangeUIEventArgs changeUIEventArgs;

    public virtual void Init(object sender,ChangeUIEventArgs args)
	{
        this.sender = sender;
        this.changeUIEventArgs = args;
	}

    /// <summary>
    /// 这个方法必须实现，用于绑定与这个view相关的Action，Views the bind.
    /// 而且这个实现类里必须调用  RegisterAction方法
    /// </summary>
    public abstract void ViewBind();

    public virtual void OnCreateView()
    {
    }

    public virtual void OnOpen()
    {
    }

    // This is call When is already open, then open another time. This time will not call ReOpen instead of OnOpen
    public virtual void ReOpen()
    {
    }
    
    public virtual void OnClose()
    {
    }

    public virtual void OnRemove()
    {
    }

    public void RegisterAction(string actionName)
    {
        if (App.Notifier == null)
            return;
        App.Notifier.UIRegister.RegisterViewComponent(actionName, this);
    }
}
