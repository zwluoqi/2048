using UnityEngine;
using System.Collections;

public class Singleton<T> where T:new()
{
	public static T Instance{
		get{
			if (instance == null) {
				instance = new T();
			}
			return instance;
		}
	}
	public static T instance;
}

