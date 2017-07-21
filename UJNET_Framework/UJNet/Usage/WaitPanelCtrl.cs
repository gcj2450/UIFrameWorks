using UnityEngine;
using System.Collections;
using UJNet.Data;
using UJNet;

public class WaitPanelCtrl : AbstractBasePanelCtrl
{

	private static WaitPanelCtrl mInstance;
	public static WaitPanelCtrl Instance {
		get {
			if (mInstance == null) {
				mInstance = GameObject.FindObjectOfType (typeof(WaitPanelCtrl)) as WaitPanelCtrl;
			}
			return mInstance;
		}
	}
	
	public override void Init ()
	{
		base.Init ();
	}
	
	protected override GameObject GetBasePanel ()
	{
		return transform.GetChild (0).gameObject;
	}
	
	protected override void Awake ()
	{
		base.Awake ();
		mInstance = this;
		DontDestroyOnLoad (gameObject);
	}
	
	public override void HidePanel ()
	{
		base.HidePanel ();
	}
	
	public override void ShowPanel ()
	{
		base.ShowPanel ();
	}
}
