using UnityEngine;
using System.Collections;

public class TestView : BaseView
{

    void Awake()
    {
    }


    public void ShowLog(int id)
    {
        Debug.Log("view ShowLog : " + id);
    }

    public void Show(string str)
    {
        Debug.Log( "view show : "+str);
    }



    public override void ViewBind()
    {
        RegisterAction(typeof(TestViewAction).Name);
    }

}
