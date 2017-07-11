using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//ConfirmBoxCtrl.Instance.OpenSelf (OnVipArmyListFullCallback, "这是确认框的内容”", new ChangeUIEventArgs());
//public void OnVipArmyListFullCallback(int callBackCMD, object sender, ChangeUIEventArgs args1, out bool destoryCallBack, out bool closePanel, ChangeUIEventArgs attr)
//{
//    closePanel = destoryCallBack = false;
//    switch (callBackCMD)
//    {
//        case ConfirmBoxCtrl.BTN_CMD_COMPLETE_BRING_IN:
//            break;
//        case ConfirmBoxCtrl.BTN_CMD_LEFT_PRESSED:
//            ShopManagerPanelCtrl shopManager = ResourceLoader.Instance.GetObjectOf<ShopManagerPanelCtrl>();
//            shopManager.OpenSelf(ShopManagerPanelCtrl.TAB_VIP);
//            HidePanel();
//            closePanel = destoryCallBack = true;
//            break;
//        case ConfirmBoxCtrl.BTN_CMD_RIGHT_PRESSED:
//            closePanel = destoryCallBack = true;
//            break;
//    }
//}
//==============================================================
//ConfirmBoxCtrl boxCtrl = ConfirmBoxCtrl.Instance;
//ChangeUIEventArgs args = ConfirmBoxCtrl.CreateSelfArgs(null, I18NManager.Instance.GetProp("disp.queue.cancel.comfirm"), null, null);
//boxCtrl.OpenSelf (this, OnConfirmBoxCallBack, new ChangeUIEventArgs(ctrl.DispInfo), args);
//public void OnConfirmBoxCallBack(int callBackCMD, object sender, ChangeUIEventArgs args, out bool destoryCallBack, out bool closePanel, ChangeUIEventArgs attr)
//{
//    closePanel = destoryCallBack = false;
//    switch (callBackCMD)
//    {
//        case ConfirmBoxCtrl.BTN_CMD_COMPLETE_BRING_IN:

//            break;
//        case ConfirmBoxCtrl.BTN_CMD_LEFT_PRESSED:
//            CityCampArmy data = attr.Data as CityCampArmy;
//            armyManagerCtrl.SendLoadCancelFunction(SpeedUpData.ACCELERATE_TYPE_ARMY, data.Id, Const.PROTOCOL_1401_CAMP);
//            closePanel = destoryCallBack = true;
//            break;
//        case ConfirmBoxCtrl.BTN_CMD_RIGHT_PRESSED:

//            closePanel = destoryCallBack = true;
//            break;
//    }
//}

//ChangeUIEventArgs args = new ChangeUIEventArgs();
//args.Id = cmd;
//		args.Attachment = new System.Collections.Generic.Dictionary<string, object> ();
//		args.Attachment["ctid"] = param.GetLong ("ctid");
//		args.Attachment["finishTime"] = param.GetLong ("finishTime");
//		args.Attachment["tid"] = tid;  
//		args.Attachment["cbLevel"] = armyBuildingCtrl.SelfCityBuilding.Level;

//        ReloadContent(this, args);
//public override void ReloadContent(object sender, ChangeUIEventArgs args)
//{
//    switch (GetCurrentTabIndex())
//    {
//        case TAB_ARMY_MGR:
//            if (args.Id == CMD_LOAD_PRIC_LIST)
//            {
//                if (args.Boolean)
//                {
//                    TabChangeValue(TAB_IN_TRAINING, true);
//                    allTabCtrls[TAB_IN_TRAINING].FlushContent(this, args);
//                }
//            }
//            else
//            {
//                allTabCtrls[TAB_ARMY_MGR].FlushContent(this, args);
//            }
//            break;
//        case TAB_IN_TRAINING:
//            allTabCtrls[TAB_IN_TRAINING].FlushContent(this, args);
//            break;
//        case TAB_ILL:
//            allTabCtrls[TAB_ILL].FlushContent(this, args);
//            break;
//        case TAB_IMPROVE:
//            allTabCtrls[TAB_IMPROVE].FlushContent(this, args);
//            break;
//    }
//}

public class ConfirmBoxCtrl : AbstractBasePanelCtrl
{
	public Button btnOk;
	public Button btnCancel;
	public Text content;
	public Text title;
	private  ChangeUIEventArgs.ChangeUIEventCallBack   callback;
	private	object sender;
	private ChangeUIEventArgs attribute;
	public const int BTN_CMD_RIGHT_PRESSED = 1;
	public const int BTN_CMD_LEFT_PRESSED = 2;
	public const int BTN_CMD_COMPLETE_BRING_IN = 3;
	private static ConfirmBoxCtrl mInstance;

	public static ConfirmBoxCtrl Instance {
		get {
			if (mInstance == null) {
				mInstance = FindObjectOfType (typeof(ConfirmBoxCtrl)) as ConfirmBoxCtrl;
			}
			return mInstance;
		}
	}
	
	public override void Init ()
	{
		base.Init ();
		base.OnPanelCompleteDismiss = OnPanelCompleteDismiss1;
		base.OnPanelCompleteBring = OnPanelCompleteBring1;
		btnOk.onClick.AddListener (OnLeftBtnPressed);
		btnCancel.onClick.AddListener (OnRightBtnPressed);
	}
	
	private void OnLeftBtnPressed ()
	{
		QuickCallBack (BTN_CMD_LEFT_PRESSED, sender, attribute);
	}
	
	private void OnRightBtnPressed ()
	{
		QuickCallBack (BTN_CMD_RIGHT_PRESSED, sender, attribute);
	}
	
	private void QuickCallBack (int callBackCMD, object sender, ChangeUIEventArgs args)
	{
		if (callback == null) {
			return;
		}
		
		bool hidePanel, destoryCallback;
		callback (callBackCMD, sender, args, out destoryCallback, out hidePanel, attribute);
		if (destoryCallback) {
			callback = null;
			sender = null;
			attribute = null;
		}
		if (hidePanel) {
			HidePanel ();	
		}
	}
	
	protected override void Awake ()
	{
		base.Awake ();
		mInstance = this;
		DontDestroyOnLoad (gameObject);
	}

	private void ShowBox (object sender, ChangeUIEventArgs args1)
	{
		this.title.text = args1.Data as string;
		this.content.text = args1.Name;
		btnOk.GetComponentInChildren<Text>().text = args1.Attachment ["ok"] as string;
		btnCancel.GetComponentInChildren<Text>().text = args1.Attachment ["cancel"] as string;
		
		if (!IsShowing)
			ShowPanel ();
	}
	
	public static ChangeUIEventArgs CreateSelfArgs (string con)
	{
		return CreateSelfArgs (null, con, null, null);
	}

	public static ChangeUIEventArgs CreateSelfArgs (string title, string con, string ok, string cancelText)
	{
		ChangeUIEventArgs args1 = new ChangeUIEventArgs ();
		if (title == null) {
			title = "";
		}
		
		if (ok == null) {
			ok = "确认";
		}
		
		if (cancelText == null) {
			cancelText ="取消";
		}
		
		
		if (con == null) {
			con = "";
		}
		
		args1.Attachment = new Dictionary<string, object> ();
		args1.Name = con;
		args1.Data = title;
		args1.Attachment ["ok"] = ok;
		args1.Attachment ["cancel"] = cancelText;
		return args1;
	}
		
	public void OpenSelf (ChangeUIEventArgs.ChangeUIEventCallBack callback, string content, ChangeUIEventArgs att)
	{
		OpenSelf (null, callback, att, CreateSelfArgs (content));
	}
	
	public void OpenSelf (object sender, ChangeUIEventArgs.ChangeUIEventCallBack callback, ChangeUIEventArgs attribute, ChangeUIEventArgs args)
	{
		this.callback = callback;
		this.sender = sender;
		this.attribute = attribute;
		ShowBox (sender, args);
	}
	
	private void OnPanelCompleteDismiss1 ()
	{
		callback = null;
		sender = null;
		attribute = null;
	}
	
	private void OnPanelCompleteBring1 ()
	{
		QuickCallBack (BTN_CMD_COMPLETE_BRING_IN, sender, attribute);
	}
    protected override GameObject GetBasePanel()
    {
        return transform.GetChild(0).gameObject;
    }
}
