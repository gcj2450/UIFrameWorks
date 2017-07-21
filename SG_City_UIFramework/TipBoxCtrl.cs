using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipBoxCtrl : AbstractBasePanelCtrl
{

    private static TipBoxCtrl mInstance;

    public static TipBoxCtrl Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(TipBoxCtrl)) as TipBoxCtrl;
            }
            return mInstance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mInstance = this;
        DontDestroyOnLoad(this);
    }


    protected override void OnDestroy()
    {
        OnTipBoxClose = null;
        base.OnDestroy();
    }

    public override void Init()
    {
        OnPanelCompleteDismiss += OnCompleteClosePanel;
        confirmBtn.onClick.AddListener(OnButtonPressed);
    }

    private void OnCompleteClosePanel()
    {
        OnTipBoxClose = null;
        //		Destroy (gameObject);
    }

    public Text message;
    public Text titleText;
    public Button confirmBtn;
    protected ChangeUIEventArgs.UIEventHandler OnTipBoxClose;


    protected System.Object sender;
    protected ChangeUIEventArgs changeUIEventArgs;

    public virtual void Show(string msg)
    {
        Show(msg, null, null, null, null, ChangeUIEventArgs.Empty);
    }

    public virtual void Show(string msg, ChangeUIEventArgs.UIEventHandler callback, System.Object sender, ChangeUIEventArgs args)
    {

        Show(msg, null, null, callback, sender, args);
    }

    public virtual void Show(string msg, string title, string btnText, ChangeUIEventArgs.UIEventHandler callback, System.Object sender, ChangeUIEventArgs args)
    {
        if (titleText != null)
        {
            if (title == null)
            {
                titleText.text = "";
                //			titleText.Text = I18NManager.Instance.GetProp ("tipbox.default.title.text");
            }
            else
            {
                titleText.text = title;
            }
        }

        if (btnText == null)
        {
            confirmBtn.GetComponentInChildren<Text>().text = "È·¶¨";
        }
        else
        {
            confirmBtn.GetComponentInChildren<Text>().text = btnText;
        }
        if(message!=null)
            message.text = msg == null ? "" : msg;

        OnTipBoxClose = callback;
        this.sender = sender;
        this.changeUIEventArgs = args;

        ShowPanel();

    }

    void OnButtonPressed()
    {

        if (OnTipBoxClose != null)
        {
            OnTipBoxClose(sender, changeUIEventArgs);
            OnTipBoxClose = null;
            sender = null;
            changeUIEventArgs = null;
        }

        HidePanel();
    }

    protected override GameObject GetBasePanel()
    {
        return transform.GetChild(0).gameObject;
    }
}
