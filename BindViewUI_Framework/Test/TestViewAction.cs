using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestViewAction : BaseViewAction
{
    private TestView _view;
    public override void HandleNotification(object sender,ChangeUIEventArgs args)
    {
        if (ViewComponent == null)
        {
            Debug.Log("ViewComponent=null show one");
            _view = (TestView)UIManager.Instance.OpenView("TestView");
        }
        switch (args.Name)
        {
            case GlobalUIVariables.OpenTestView:

                //App.UIMgr.CloseViewWithoutOpen();
                //HeadNavItemData data = (HeadNavItemData)notification.Data[0];
                //ViewInfo viewInfo = App.UIMgr.OpenView("HomeLocalView");
                //if (null == viewInfo)
                //{
                //    // TODO:
                //}
                break;
            case GlobalUIVariables.SendMsgToTestView:
                Debug.Log("sender : " + sender.GetType().ToString());
                object[] obj = args.Data as object[];
                int _data= (int)(obj[0]);

                Debug.Log(_data);
                _view.ShowLog(_data);
                break;
            case GlobalUIVariables.SendMsgHaha:
                Debug.Log("sender : " + sender.GetType().ToString());
                object[] obj2 = args.Data as object[];
                int _data2 = (int)(obj2[0]);

                Debug.Log(_data2);
                _view.ShowLog(_data2);
                break;
        }
    }

    public override List<string> ListNotificationString()
    {
        List<string> notificationList = new List<string>();
        notificationList.Add(GlobalUIVariables.OpenTestView);
        notificationList.Add(GlobalUIVariables.SendMsgToTestView);
        notificationList.Add(GlobalUIVariables.SendMsgHaha);
        return notificationList;
    }
}
