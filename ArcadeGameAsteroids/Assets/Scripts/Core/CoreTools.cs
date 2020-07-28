using UnityEngine;
using System;
using System.Collections.Generic;
namespace Core
{
	public class CoreTools : Singleton<CoreTools>
	{ 
		private Dictionary<Type, object> data = new Dictionary<Type, object>();

		public static void Add(object obj)
		{
			var add = obj;
			var manager = obj as ManagerBase;

			if (manager != null)
				add = Instantiate(manager);
			else return;

			Instance.data.Add(obj.GetType(), add);

			if (add is IAwake)
			{
				(add as IAwake).OnAwake();
			}

			if (add is ITick)
			{
				GetManager<Updater>().AddTo(add);
			}

			if (add is ITickFixed)
			{
				GetManager<Updater>().AddTo(add);
			}

			if (add is ITickLate)
			{
				GetManager<Updater>().AddTo(add);
			}
		}

		public static T GetManager<T>()
		{
			object resolve;
			Instance.data.TryGetValue(typeof(T), out resolve);
			return (T)resolve;
		}

		public void ClearScene()
		{

		}
	}
}