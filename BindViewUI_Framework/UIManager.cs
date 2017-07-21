using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///完成打开和关闭View功能，打开View会从Dic中查找自己的数据，然后加载，关闭的时候会把数据存到Dic中
/// </summary>
public class UIManager : MonoBehaviour, IBaseEventAction
{
    /// <summary>
    /// 当前打开的View（打开）
    /// </summary>
    //private Dictionary<string, BaseView> _views = new Dictionary<string, BaseView>();
    private Stack<GameObject> viewHistory = new Stack<GameObject>();
    
    /// <summary>
    /// The List will store Instantiate view GameObject 
    /// </summary>
    private List<GameObject> _closeViewObjCachePoolList = new List<GameObject>();
    // 通用view根节点;
    private GameObject canvasRoot;
    public GameObject CanvasRoot
    {
        get
        {
            if(canvasRoot == null)
                canvasRoot = GameObject.Find("MainCanvas/Root");
            return canvasRoot;
        }
    }
    /// <summary>
    /// 获取当前打开的View
    /// </summary>
    public GameObject CurrentView
    {
        get { return viewHistory.Count == 0 ? null : viewHistory.Peek(); }
    }

    private static UIManager _instance;
    /// <summary>
    /// Singleton,方便各模块访问
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var app = FindObjectOfType(typeof(UIManager)) as UIManager;
                if (app == null)
                {
                    var appObject = new GameObject("UIManager");
                    app = appObject.AddComponent<UIManager>();
                }
                _instance = app;
            }

            return _instance;
        }
    }


    public void HandleNotification(object sender,ChangeUIEventArgs args)
    {
    }

    public List<string> ListNotificationString()
    {
        return null;
    }

    public BaseView OpenView(string name,bool keepShowPrevious = false)
    {
        GameObject currentView = CurrentView;
        if(!keepShowPrevious&& currentView != null)
            CloseView(currentView);

        BaseView viewObj = CreateView(name);
        viewHistory.Push(viewObj.gameObject);
        return viewObj;
    }

    public void CloseView(GameObject _view )
    {
        Debug.Log("UIManager.OnCloseView");
        if(!_closeViewObjCachePoolList.Contains(_view))
            _closeViewObjCachePoolList.Add(_view);
        _view.SetActive(false);
    }
    /// <summary>
    /// 返回历史中上一个Panel
    /// </summary>
    /// <param name="fade">显示时间</param>
    /// <param name="callback">显示完成回调</param>
    public void GoBack()
    {
        if (viewHistory.Count == 0)
            return;

        GameObject currentView = viewHistory.Pop();
        GameObject prevView = viewHistory.Count == 0 ? null : viewHistory.Peek();

        CloseView(currentView);
        if (prevView != null)
        {
            prevView.SetActive(true);
        }
    }
    
    public void ExitAll()
    {
        if (viewHistory.Count == 0)
            return;
        // hide the current menu screen
        GameObject currentView = viewHistory.Pop();
        CloseView(currentView);
        viewHistory.Clear();
    }

    /// <summary>
    /// Finds the view. if not exist in cache dict, it will Add / Load from Resource
    /// // 1. Check the root child
    /// // 2. Load from poll Dict
    /// // 3. Load from Cache View
    /// </summary>
    public BaseView CreateView(string name,bool isSetActive = true)
    {
        if (name == null)
            return null;
        if (CanvasRoot != null)
        {
            // 1. Check the root child
            // 2. Load from poll Dict
            // 3. Load from Cache View
            GameObject viewObj = null;
            #region 1. Check the root child
            
            string prefabName = name;
            if (name.Contains("/"))
            {
                string[] subName = name.Split('/');
                prefabName = subName[subName.Length - 1];
                viewObj = GetObjectExactMatch(CanvasRoot, prefabName);
            }
            else
            {
                BaseView[] views = CanvasRoot.GetComponentsInChildren<BaseView>(true);
                for (int i = 0; i < views.Length; i++)
                {
                    if (views[i].name.Equals(name))
                    {
                        viewObj = views[i].gameObject;
                        break;
                    }
                }
            }
            #endregion

            #region 2. Load from poll Dict
            for (int i = 0; i < _closeViewObjCachePoolList.Count; i++)
            {
                if (!_closeViewObjCachePoolList[i].activeInHierarchy && _closeViewObjCachePoolList[i].name == name)
                {
                    viewObj = _closeViewObjCachePoolList[i];
                    viewObj.SetActive(true);
                    _closeViewObjCachePoolList.RemoveAt(i);
                    break;
                }
            }
            #endregion

            #region 3. Load from Resources
            
            if (viewObj == null)
            {
                {
                    //这里从AssetBundle加载资源
                    //var assetRef = App.ResMgr.GetUIAsset(name);
                    //if (assetRef != null)
                    //    viewObj = assetRef.Asset as GameObject;
                    //else
                        viewObj = Instantiate(Resources.Load("UI/" + name) as GameObject);
                    if (viewObj == null)
                    {
                        UnityEngine.Debug.LogError("Create ViewName : " + name + "is not exist in Hierarchy or Bundle, or Resources/UI !");
                        return null;
                    }
                }
            }
            #endregion
            
            viewObj.name = prefabName;
            if (!viewObj.activeInHierarchy && isSetActive)
                viewObj.SetActive(true);

            if (viewObj.GetComponent<RectTransform>().parent == null)
                viewObj.GetComponent<RectTransform>().SetParent(CanvasRoot.GetComponent<RectTransform>(), false);
            viewObj.transform.SetAsLastSibling();
            BaseView baseView = viewObj.GetComponent<BaseView>();

            if (baseView != null)
            {
                baseView.OnCreateView();
                baseView.ViewBind();
            }
            else
                UnityEngine.Debug.LogError("viewObj : '" + name + "' not contain BaseView Script");
            return baseView;
        }
        return null;
    }
    
    GameObject GetObjectExactMatch(GameObject obj, string name)
    {
        string[] tNames = name.Split('/');

        GameObject tarObj = obj;
        for (int i = 0; i < tNames.Length; i++)
        {
            tarObj = GetObject(tarObj, tNames[i], true);
            if (tarObj == null)
                return null;
        }
        return tarObj;
    }
    public static GameObject GetObject(GameObject obj, string name, bool bExactMatch)
    {
        if (obj == null || string.IsNullOrEmpty(name))
        {
            return null;
        }

        if (!bExactMatch)
        {
            if (obj.name.Contains(name))
            {
                return obj;
            }
        }
        else if (obj.name == name)
        {
            return obj;
        }

        if (obj.transform)
        {
            foreach (Transform t in obj.transform)
            {
                if (!t) continue;

                GameObject result = GetObject(t.gameObject, name, bExactMatch);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return null;
    }
}
