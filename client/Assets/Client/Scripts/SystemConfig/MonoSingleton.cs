using UnityEngine;
using System.Collections;

public class MonoSingleton<T> :MonoBehaviour where T:MonoBehaviour

{
	public static T Instance{
		get{
			if (instance == null) {
				var go = new GameObject ();
				instance = go.AddComponent < T >();
				GameObject.DontDestroyOnLoad (go);
			}
			return instance;
		}
	}
	public static T instance;

}

