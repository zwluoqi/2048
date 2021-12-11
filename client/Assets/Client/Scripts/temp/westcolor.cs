using UnityEngine;
using System.Collections;

public class westcolor : MonoBehaviour {
	
	public UISprite ui;
	int i = 0;
	// Use this for initialization
	void Start () {
		InvokeRepeating("bianhua",1,0.3f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void bianhua(){
		ui.color = ResourcesManager.backColors[i++%11];
		Debug.Log(ResourcesManager.backColors[i%11]);
	}
}
