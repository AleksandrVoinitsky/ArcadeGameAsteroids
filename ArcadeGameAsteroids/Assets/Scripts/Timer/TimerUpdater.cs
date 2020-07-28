using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class TimerUpdater : MonoBehaviour, ITick
	{
		private TimerManager mng;

		public void Setup(TimerManager mng)
		{
			this.mng = mng;
		}

		void Start()
		{
			CoreTools.GetManager<Updater>().AddTo(this);
		}

		public void Tick()
		{
			mng.Update();
		}
	}
}
