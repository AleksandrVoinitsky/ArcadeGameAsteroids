using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
	public delegate void OnTimerFinished();
	private OnTimerFinished onTimerFinished;

	public delegate void OnTimerTicked();
	public OnTimerTicked onTimerTicked;

	private float elapsedTime;
	private float waitTime;
	private bool paused;
	private bool finished;

	internal Timer(float waitTime, Timer.OnTimerFinished onTimerFinished = null, Timer.OnTimerTicked onTimerTicked = null)
	{
		SetWaitTime(waitTime);
		this.onTimerFinished = onTimerFinished;
		this.onTimerTicked = onTimerTicked;
	}

	public void Reset()
	{
		finished = false;
		SetElapsedTime(0.0f);
	}

	internal void UpdateTimer()
	{
		if (!paused && !finished)
		{
			elapsedTime += Time.deltaTime;
			InvokeTickedDelegate();
		}

		if (!finished && elapsedTime >= waitTime) { Finish(); }
	}

	private void Finish()
	{
		finished = true;
		InvokeFinishedDelegate();
	}

	private void InvokeFinishedDelegate()
	{
		if (onTimerFinished != null) { onTimerFinished.Invoke(); }
	}

	private void InvokeTickedDelegate()
	{
		if (onTimerTicked != null) { onTimerTicked.Invoke(); }
	}

	public void Pause() { paused = true; }

	public void Unpause() { paused = false; }

	public void SetElapsedTime(float newTime) { elapsedTime = newTime; }

	public float GetElapsedTime() { return elapsedTime; }

	public void SetWaitTime(float newTime) { waitTime = newTime; }

	public float GetWaitTime() { return waitTime; }

	public void AddListener(OnTimerFinished delgt) { onTimerFinished += delgt; }

	public void AddListener(OnTimerTicked delgt) { onTimerTicked += delgt; }

	public void RemoveListener(OnTimerFinished delgt) { onTimerFinished -= delgt; }

	public void RemoveListener(OnTimerTicked delgt) { onTimerTicked -= delgt; }

	public bool IsPaused() { return paused; }

	public bool IsFinished() { return finished; }

}
