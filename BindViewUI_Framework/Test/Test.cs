using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
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
            ChangeUIEventArgs args = new ChangeUIEventArgs(new object[1] {12800 });
            App.Notifier.SendNotification(this,GlobalUIVariables.SendMsgToTestView, args);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            ChangeUIEventArgs args = new ChangeUIEventArgs(new object[1] { 9999 });
            App.Notifier.SendNotification(this,GlobalUIVariables.SendMsgHaha, args);
        }
    }
}
