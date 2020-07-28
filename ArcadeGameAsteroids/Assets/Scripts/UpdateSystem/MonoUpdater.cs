using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class MonoUpdater : MonoBehaviour
	{
		private Updater mng;

		public void Setup(Updater mng)
		{
			this.mng = mng;
		}

		private void Update()
		{
			mng.Tick();
		}

		private void FixedUpdate()
		{
			mng.TickFixed();
		}

		private void LateUpdate()
		{
			mng.TickLate();
		}
	}
}
