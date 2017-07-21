// Copyright 2015 Baofeng Mojing Inc. All rights reserved.
//
// Author: KEN
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// 工具类
/// </summary>
public class DelayQuit : MonoBehaviour
{
	public float DelayTime;

	void OnEnable()
	{
        StopCoroutine("DelayQuitCoroutine");
        StartCoroutine("DelayQuitCoroutine");
	}

    public void Reset()
    {
        StopCoroutine("DelayQuitCoroutine");
        StartCoroutine("DelayQuitCoroutine");
    }
	

	IEnumerator DelayQuitCoroutine()
	{
		yield return new WaitForSeconds(DelayTime);

		transform.gameObject.SetActive(false);
	}
}


