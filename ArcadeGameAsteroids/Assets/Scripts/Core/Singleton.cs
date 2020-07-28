﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		public static T Instance { get; protected set; }

		public static bool instanceExists
		{
			get { return Instance != null; }
		}

		protected virtual void Awake()
		{
			if (instanceExists)
			{
				Destroy(gameObject);
			}
			else
			{
				Instance = (T)this;
			}
		}

		protected virtual void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}
