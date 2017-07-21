using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 简单的多Panel管理系统
/// Simple Panel Manager for managing multiple panels and transitioning between them.
/// </summary>
public class BasePanelManager : MonoBehaviour
{
    // 当前管理的Panel，通过名称字典管理
    private Dictionary<string, PanelBase> childPanels = new Dictionary<string, PanelBase>();
    // The traversal history, so we can return through previous menus
    private Stack<PanelBase> panelHistory = new Stack<PanelBase>();
    // The menu system also manages the HUD
    private HUD hud;

    /// <summary>
    /// Gets the currently displayed panel (or null if none is open)
    /// </summary>
    /// <value>The current panel.</value>
    public PanelBase CurrentPanel
    {
        get { return panelHistory.Count == 0 ? null : panelHistory.Peek(); }
    }

    protected virtual void Awake()
    {
        ScanChildern();
    }

    protected virtual void Start()
    {
    }
    /// <summary>
    /// 扫描子物体中的PanelBase
    /// </summary>
    public void ScanChildern(bool setCurPanelActive = true)
    {
        childPanels.Clear();
        PanelBase curPanel = CurrentPanel;
        PanelBase[] panels = transform.GetComponentsInChildren<PanelBase>(true);
        for (int i = 0; i < panels.Length; i++)
        {
            childPanels[panels[i].name] = panels[i];
            if (setCurPanelActive)
            {
                if (panels[i] != curPanel)
                    panels[i].gameObject.SetActive(false);
            }
        }
    }

    public HUD HUD
    {
        get { return hud; }
    }

    /// <summary>
    /// 通过物体名称返回指定Panel
    /// </summary>
    /// <returns>返回的Panel</returns>
    /// <param name="name">需要查找的Panel名称</param>
    public PanelBase GetPanel(string name)
    {
        //如果字典中不包含指定名称的Panel，重新扫描，再次查找，如果没有，返回null
        if (childPanels.ContainsKey(name))
            return childPanels[name];
        else
        {
            ScanChildern();
            if (childPanels.ContainsKey(name))
                return childPanels[name];
            else
            {
                Debug.Log("Did not find panel : " + name);
                return null;
            }
        }
    }

    /// <summary>
    /// 显示指定的Panel
    /// </summary>
    /// <param name="screen">将要显示的Panel</param>
    /// <param name="fade">显示时间</param>
    /// <param name="callback">显示完成回调</param>
    public void ShowPanel(PanelBase panel, float fade = 0.5f, SimpleTween.Callback callback = null)
    {
        if (panel == null)
        {
            Debug.LogWarning("Attempting to show null Menu Screen");
            return;
        }

        PanelBase currentScreen = CurrentPanel;

        // 将新Panel添加进历史
        panelHistory.Push(panel);

        // 在显示新Panel之前隐藏当前的
        if (currentScreen != null)
        {
            currentScreen.Hide(fade, () =>
            {
                panel.Show(fade, callback);
            });
        }
        else
        {
            panel.Show(fade, callback);
        }
    }

    /// <summary>
    /// 返回历史中上一个Panel
    /// </summary>
    /// <param name="fade">显示时间</param>
    /// <param name="callback">显示完成回调</param>
    public void GoBack(float fade = 0.5f, SimpleTween.Callback callback = null)
    {
        if (panelHistory.Count == 0)
            return;

        PanelBase currentPanel = panelHistory.Pop();
        PanelBase prevPanel = panelHistory.Count == 0 ? null : panelHistory.Peek();

        // hide the current screen and then show the previous screen (if there is one).
        if (prevPanel != null)
        {
            currentPanel.Hide(fade, () =>
            {
                prevPanel.Show(fade, callback);
            });
        }
        else
        {
            currentPanel.Hide(fade, callback);
        }
    }

    /// <summary>
    /// Exits all of the panels without going back through the history
    /// </summary>
    /// <param name="fade">Transition time</param>
    /// <param name="callback">Callback function to call when the transition is complete</param>
    public void ExitAll(float fade = 0.5f, SimpleTween.Callback callback = null)
    {
        if (panelHistory.Count == 0)
            return;

        // hide the current menu screen
        PanelBase currentScreen = panelHistory.Pop();
        currentScreen.Hide(fade, callback);

        // clear the history, since we have now completely exited the menus.
        panelHistory.Clear();
    }
}