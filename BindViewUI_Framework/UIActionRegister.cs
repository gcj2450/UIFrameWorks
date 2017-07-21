using System.Collections;
using System.Collections.Generic;

public class UIActionRegister
{
    public Dictionary<string, BaseViewAction> baseActionList = new Dictionary<string, BaseViewAction>();

    public void Init()
    {
        RegisterViewActions();
    }

    public void RegisterViewActions()
    {
        this.BaseActionListAdd(new TestViewAction(), typeof(TestViewAction));
        //this.BaseActionListAdd(new RecommandViewAction(), typeof(RecommandViewAction));

    }

    public void BaseActionListAdd(BaseViewAction action, System.Type type)
    {
        action.Init();
        //HDebug.LogError(action +","+type.Name);
        baseActionList.Add(type.Name, action);
    }

    /// <summary>
    /// 用于View 与 Action的自动绑定。
    /// </summary>
    /// <param name="actionName">Action name.</param>
    /// <param name="viewScript">View script.</param>
    public void RegisterViewComponent(string actionName, object viewScript)
    {
        if (baseActionList.ContainsKey(actionName))
            baseActionList[actionName].ViewComponent = viewScript;
        else
            UnityEngine.Debug.LogError("The BaseAction is not exist! ---------baseActionList.Count: " + baseActionList.Count);
    }
}

