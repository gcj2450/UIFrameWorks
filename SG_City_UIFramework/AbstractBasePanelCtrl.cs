using UnityEngine;
using System.Collections;
using System;

public abstract class AbstractBasePanelCtrl : MonoBehaviour
{
	
	private  bool isAwake = false;
	private  bool isStart = false;
	
	//this variable to tell us the first run the panel  (show <----> and not show)
	private bool isShowing = false;
	private bool startAnyTransition = false;

	//using these method to noifty panel event
	public delegate void PanelStartBringDelegate ();

	public delegate void PanelCompleteBringDelegate ();

	public delegate void PanelStartDismissDelegate ();

	public delegate void PanelCompleteDismissDelegate ();

	public PanelStartBringDelegate OnPanelStartBring;
	public PanelCompleteBringDelegate OnPanelCompleteBring;
	public PanelStartDismissDelegate OnPanelStartDismiss;
	public PanelCompleteDismissDelegate OnPanelCompleteDismiss;
	
	protected virtual void Awake ()
	{
		if (isAwake) {
			return;
		}
		isAwake = true;	
	}
	
	private void OnTransitionComplete (GameObject panel, bool isBringForward)
	{
        if (isBringForward)
        {
            startAnyTransition = false;
            StartCoroutine(DelayOnPanelCompleteBring());
        }
        else
        {
            startAnyTransition = false;
            if (OnPanelCompleteDismiss != null)
            {
                OnPanelCompleteDismiss();
            }
        }
	}

	private IEnumerator DelayOnPanelCompleteBring ()
	{
		yield return 0;
		if (OnPanelCompleteBring != null) {
			OnPanelCompleteBring ();
		}
	}

	private IEnumerator DelayOnPanelCompleteDismiss ()
	{
		yield return 0;
		isShowing = false;
		if (OnPanelCompleteDismiss != null) {
			OnPanelCompleteDismiss ();
		}
		startAnyTransition = false;
	}

	protected void Start ()
	{
		if (isStart) {
			return;
		}
		isStart = true;
		InitPanel ();
	}

	protected virtual void OnDestroy ()
	{
		OnPanelCompleteBring = null;
		OnPanelCompleteDismiss = null;
		OnPanelStartBring = null;
		OnPanelStartDismiss = null;
	}

	// use Init() method to initialize like Start() method
	public virtual void Init ()
	{
		
	}

	protected void InitPanel ()
	{
		Init ();
	}

	public bool IsShowing {
		get { return this.isShowing; }
	}

	public bool StartAnyTransition {
		get { return startAnyTransition; }
	}

	//how can the AbstractBasePanelCtrl get Transition panel
	//tip: we must override it
	protected abstract GameObject GetBasePanel ();

	//use this to show panel
	public virtual void ShowPanel ()
	{
		
		if (startAnyTransition) {
			Debug.LogWarning ("the panel at " + gameObject.name + " is startAnyTransition !");
			return;
		}
		
		if (isShowing) {
			Debug.LogWarning ("the panel at " + gameObject.name + " is Showing, don,t run showPanel twice~ ");
			return;
		}
		
		
		if (!isAwake) {
			Debug.LogWarning ("the panel at " + gameObject.name + " is not Init isAwake !");
			Awake ();
		}
		
			
		if (!isStart) {
			Debug.LogWarning ("the panel at " + gameObject.name + " is not Init isStart !");
			Start ();
		}
		
		
		//because when invoke ShowPanel method , we have never used this method twice		
		isShowing = true;
		startAnyTransition = true;
		if (OnPanelStartBring != null) {
			OnPanelStartBring ();
		}
        StartTransPanel(true,OnTransitionComplete);
		//GetBasePanel ().AddTempTransitionDelegate (OnTransitionComplete);
		//GetBasePanel ().StartTransition (UIPanelManager.SHOW_MODE.BringInForward);
	}



	//use this to hide panel
	public virtual void HidePanel ()
	{
		
		if (startAnyTransition) {
			Debug.LogWarning ("the panel HidePanel at " + gameObject.name + " is startAnyTransition !");
//			return;
		}
		
		startAnyTransition = true;
		isShowing = false;
		
		if (!isAwake) {
			Start ();
		}
		
		if (!isStart) {
			Start ();
		}
		
		
		if (OnPanelStartDismiss != null) {
			OnPanelStartDismiss ();
		}
        StartTransPanel(false, OnTransitionComplete);
        //GetBasePanel ().AddTempTransitionDelegate (OnTransitionComplete);
        //GetBasePanel ().StartTransition (UIPanelManager.SHOW_MODE.DismissBack);
    }

    /// <summary>
    /// 简单的实现，也可以参考MenuBase的show，hide方法重新实现
    /// </summary>
    /// <param name="isBringForward"></param>
    /// <param name="onTransitionComplete"></param>
    public virtual void StartTransPanel(bool isBringForward, Action<GameObject, bool> onTransitionComplete)
    {
        if (isBringForward)
        {
            GetBasePanel().gameObject.SetActive(true);
            if (onTransitionComplete != null)
                onTransitionComplete(GetBasePanel(), true);
        }
        else
        {
            GetBasePanel().gameObject.SetActive(false);
            if (onTransitionComplete != null)
                onTransitionComplete(GetBasePanel(), false);
        }
    }

 //   public bool IsHideTransform (Transform trans)
	//{
	//	return Vector3.Distance (trans.localScale, Vector3.zero) < 0.0001;
	//}

	//public void HideTransform (Transform trans, bool hide)
	//{
	//	trans.localScale = hide ? Vector3.zero : Vector3.one;
	//}
}



