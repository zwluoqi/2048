using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {

	public GameObject okButton;
	public GameObject cancleButton;
	public UILabel titleText;
	public UILabel textInfo;
	
	
	public void OnFinishedOpen(){
		TweenPosition twPos = this.GetComponent<TweenPosition>();
		Debug.Log(twPos);
	}
}
