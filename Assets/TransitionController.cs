﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
	public static TransitionController Instance
	{
		get
		{
			if (instance != null) return instance;
			instance = FindObjectOfType<TransitionController>();
			return instance;
		}
	}
	private static TransitionController instance;

	Animator animator;
	LogShaderControl[] logShaderControl;
	public bool isEnd;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		if (isEnd) SetColor();
	}

	public void StartTransition(bool _start)
	{
		string _trigger = _start ? "Start" : "End";
		animator.SetTrigger(_trigger);
		Invoke("GetColor", 1f);
	}

	void GetColor()
	{
		logShaderControl = new LogShaderControl[transform.childCount];
		UserManager.Instance.transitionMatColor = new float[transform.childCount];

		for (int i = 0; i < this.transform.childCount; i++)
		{
			logShaderControl[i] = this.transform.GetChild(i).GetComponent<LogShaderControl>();
			UserManager.Instance.transitionMatColor[i] = logShaderControl[i].GetLogColor();
		}
	}

	void SetColor()
	{
		logShaderControl = new LogShaderControl[transform.childCount];

		for (int i = 0; i < this.transform.childCount; i++)
		{
			logShaderControl[i] = this.transform.GetChild(i).GetComponent<LogShaderControl>();
			logShaderControl[i].SetLogColor(UserManager.Instance.transitionMatColor[i]);
			//Debug.Log(logShaderControl[i].GetLogColor());
		}
	}
}