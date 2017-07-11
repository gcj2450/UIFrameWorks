//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//public class InputDialogCtrl : AbstractBasePanelCtrl
//{


//	private static InputDialogCtrl mInstance;

//	public static InputDialogCtrl Instance {
//		get {
//			if (mInstance == null) {
//				mInstance = FindObjectOfType (typeof(InputDialogCtrl)) as InputDialogCtrl;
//			}
//			return mInstance;
//		}
//	}
	
//	protected override void Awake ()
//	{
//		base.Awake ();
//		mInstance = this;
//		DontDestroyOnLoad (this);
//	}

//	public Text title;
//	public Text inputText;
//	public Button leftBtn;
//	public Button rightBtn;
//	public InputField textField;
//	private int maxLength = 0;

//	public delegate void InputDialogDelegate (System.Object sender,ChangeUIEventArgs args);

//	private InputDialogDelegate OnLeftBtnPressed;
//	private InputDialogDelegate OnRightBtnPressed;
//	private System.Object sender;
////	private ChangeUIEventArgs changeUIEventArgs;


//	private bool IsHideLeftBtn ()
//	{
//		return Vector3.Distance (leftBtn.transform.localScale, Vector3.zero) < 0.0001;
//	}

//	private void HideLeftBtn (bool hide)
//	{
//		leftBtn.transform.localScale = hide ? Vector3.zero : Vector3.one;
//	}

//	private bool IsHideRightBtn ()
//	{
//		return Vector3.Distance (rightBtn.transform.localScale, Vector3.zero) < 0.0001;
//	}

//	private void HideRightBtn (bool hide)
//	{
//		rightBtn.transform.localScale = hide ? Vector3.zero : Vector3.one;
//	}

//	public override void Init ()
//	{
//		OnPanelCompleteDismiss += OnCompleteClosePanel;
//		textField.SetCommitDelegate (OnInputCommit);
//		leftBtn.SetValueChangedDelegate (OnLeftBtn);
//		rightBtn.SetValueChangedDelegate (OnRightBtn);
//	}

//	private void OnCompleteClosePanel ()
//	{
////		Destroy (gameObject);
//		OnLeftBtnPressed = null;
//		OnRightBtnPressed = null;
//		sender = null;
//		maxLength = 0;
//	}
	
//	public void ShowInputDialog (string leftBtnName, string rightBtnName, string tipMessage, InputDialogDelegate onLeftBtnPressed, InputDialogDelegate onRightBtnPressed, int maxLength)
//	{
		
//		ShowInputDialog (null, null, leftBtnName, rightBtnName, tipMessage, onLeftBtnPressed, onRightBtnPressed, maxLength, null);
//	}
	
//	public void ShowInputDialog (string title, string inputContent, string leftBtnName, string rightBtnName, string tipMessage, InputDialogDelegate onLeftBtnPressed, InputDialogDelegate onRightBtnPressed, int maxLength, object sender)
//	{
		
//		this.title.Text = title != null ? title : "";
//		this.textField.Text = inputContent != null ? inputContent : "";
//		this.inputText.Text = tipMessage != null ? tipMessage : "";
//		this.maxLength = maxLength;
//		this.sender = sender;
		
//		if (leftBtnName == null) {
//			HideLeftBtn (true);
//		} else {
//			HideLeftBtn (false);
//			leftBtn.Text = leftBtnName;
//			OnLeftBtnPressed += onLeftBtnPressed;
//		}
		
//		if (rightBtnName == null) {
//			HideRightBtn (true);
//		} else {
//			HideRightBtn (false);
//			rightBtn.Text = rightBtnName;
//			OnRightBtnPressed += onRightBtnPressed;
//		}
		
//		ShowPanel ();
//	}

//	private void OnInputCommit (IKeyFocusable control)
//	{
		
//		string content = control.Content;
//		if (content.Length > maxLength) {
//			textField.Text = content.Substring (0, maxLength);
//		}
//	}

//	private void OnLeftBtn (IUIObject uiObj)
//	{
//		if (OnLeftBtnPressed != null) {
//			ChangeUIEventArgs args = new ChangeUIEventArgs ();
//			args.Name = textField.Text;
//			args.Boolean = true;
//			HidePanel ();
//			OnLeftBtnPressed (this, args);
//		}
//	}

//	private void OnRightBtn (IUIObject uiObj)
//	{
//		if (OnRightBtnPressed != null) {
//			ChangeUIEventArgs args = new ChangeUIEventArgs ();
//			args.Name = textField.Text;
//			args.Boolean = false;
//			HidePanel ();
//			OnRightBtnPressed (this, ChangeUIEventArgs.Empty);
//		}
//	}

//	protected override UIPanelBase GetBasePanel ()
//	{
//		return transform.GetChild (0).GetComponent<UIPanelBase> ();
//	}
	
//}
