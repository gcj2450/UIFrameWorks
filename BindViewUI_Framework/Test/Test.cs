using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public GameObject TestViewPrefab;
    GameObject viewObj;
    // Use this for initialization
    void Start()
    {
        App.Create();
        App.Instance.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (viewObj == null)
                viewObj = Instantiate(TestViewPrefab);
            Debug.Log("goood");
            BaseView baseView = viewObj.GetComponent<BaseView>();

            if (baseView != null)
            {
                baseView.ViewBind();
            }

            ChangeUIEventArgs args = new ChangeUIEventArgs(new object[1] {12800 });
            App.Notifier.SendNotification(this,GlobalUIVariables.SendMsgToTestView, args);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            if (viewObj == null)
                viewObj = Instantiate(TestViewPrefab);
            Debug.Log("Hahaha");
            BaseView baseView = viewObj.GetComponent<BaseView>();

            if (baseView != null)
            {
                baseView.ViewBind();
            }
            ChangeUIEventArgs args = new ChangeUIEventArgs(new object[1] { 9999 });
            App.Notifier.SendNotification(this,GlobalUIVariables.SendMsgHaha, args);
        }
    }
}
