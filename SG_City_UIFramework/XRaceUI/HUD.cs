using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleTween;

/// <summary>
/// Simple HUD that displays the current distance and the current record.
/// This expects to find some child Text elements with particular names, specifically:
/// 	DistanceMeters - this should be a Text element that is updated every frame with the current disance value
/// 	RecordMeters - this should be a Text element that is updated to show the current record distance
/// </summary>
public class HUD : MonoBehaviour 
{
	private CanvasGroup canvasGroup;

	void Start () 
	{
		canvasGroup = GetComponent<CanvasGroup>();
		// start with HUD off.
		canvasGroup.alpha = 0.0f;
	}
	
	void Update () 
	{
	}

	public void Show()
	{
		// slide on
		RectTransform rect = canvasGroup.GetComponent<RectTransform>();
		SimpleTweener.AddTween(()=>new Vector2(-80, 0), x=>rect.anchoredPosition=x, Vector2.zero, 0.5f);
		SimpleTweener.AddTween(()=>canvasGroup.alpha, x=>canvasGroup.alpha=x, 1.0f, 0.5f);
	}

	public void Hide()
	{
		// slide off
		RectTransform rect = canvasGroup.GetComponent<RectTransform>();
		SimpleTweener.AddTween(()=>Vector2.zero, x=>rect.anchoredPosition=x, new Vector2(-80, 0), 0.5f).Ease(Easing.EaseIn);
		SimpleTweener.AddTween(()=>canvasGroup.alpha, x=>canvasGroup.alpha=x, 0.0f, 0.5f).Ease (Easing.EaseIn);
	}
    
}
