﻿using System;
using UnityEngine;
using TMPro;

public class SwipeDetector : MonoBehaviour
{
	public static SwipeDetector Instance
	{
		get
		{
			if (instance != null) return instance;
			instance = FindObjectOfType<SwipeDetector>();
			return instance;
		}
	}
	private static SwipeDetector instance;

	private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

	public Vector3 touchPos;

    private void Update()
    {
		foreach (Touch touch in Input.touches)
        {
			touchPos = touch.position;

			if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
				tmpro.text = "Is Down";
				isSwiping = true;
			}

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
				tmpro.text = "Is Moveing!";
				isSwiping = true;
			}

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
				tmpro.text = "Is Up";
				isSwiping = false;
			}
        }
    }

	public bool isSwiping = false;
	public TextMeshProUGUI tmpro;
	public SwipeDirection swipeDir;

    private void DetectSwipe()
    {
		if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
				tmpro.text = "Vertical Swap!";
			}
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
				tmpro.text = "Horizontal Swap!";
			}
            fingerUpPosition = fingerDownPosition;
        }
	}

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}