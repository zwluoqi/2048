using UnityEngine;
using System.Collections;

public class BoxManager  :Singleton<BoxManager> {
	string uiMessageBoxName = "Windows/MessageBox";
	
	public GameObject buttonOk;
	public GameObject buttonCancle;
	GameObject topWindow;
	public bool isPause{get{
			return topWindow != null;
		}
	}
	
	public void ShowWinBox(){
		ShowMessageBox();
		MessageBox messageBox = topWindow.GetComponent<MessageBox>();
		messageBox.textInfo.text = "Congratulation,You Win!";
		messageBox.okButton.GetComponentInChildren<UILabel>().text = "Show Results";
		messageBox.cancleButton.GetComponentInChildren<UILabel>().text = "Restart";
	}
	
	public void ShowQuitBox(){
		ShowMessageBox();
		MessageBox messageBox = topWindow.GetComponent<MessageBox>();
		messageBox.textInfo.text = "Exit Game?";
	}
	
	public void ShowRefreshBox(){
		ShowMessageBox();
		MessageBox messageBox = topWindow.GetComponent<MessageBox>();
		messageBox.textInfo.text = "Restart?";
	}
	
	public void ShowFailedRefreshBox(){
		ShowMessageBox();
		MessageBox messageBox = topWindow.GetComponent<MessageBox>();
		messageBox.textInfo.text = "Sorry,You Are Lose";
		messageBox.okButton.GetComponentInChildren<UILabel>().text = "Show Results";
		messageBox.cancleButton.GetComponentInChildren<UILabel>().text = "Restart";
	}	
	
	public void ShowMessageBox(){
		RemoveMessageboxDirectly();
		CreateMessageBox();
	}
	
	public void RemoveMessageBox(GameObject go){
		if(topWindow != null){
			TweenPosition twPos = topWindow.GetComponent<TweenPosition>();
			twPos.from = new Vector3(0,0,0);
			twPos.to = new Vector3(-500,0,0);
			twPos.AddOnFinished(OnPlayFinished);
			twPos.ResetToBeginning();
			twPos.Play(true);
		}
		buttonOk = null;
		buttonCancle = null;
	}
	
	private void OnPlayFinished(){
		GameObject.Destroy(topWindow);
		topWindow = null;
	}
	
	private void RemoveMessageboxDirectly(){
		if(topWindow != null){
			OnPlayFinished();
		}
		buttonOk = null;
		buttonCancle = null;
		
	}
	
	private void CreateMessageBox(){
		topWindow = ResourcesManager.Instance.CreateGameObj(uiMessageBoxName);
	
		MessageBox messageBox = topWindow.GetComponent<MessageBox>();
		buttonOk = messageBox.okButton;
		buttonCancle = messageBox.cancleButton;
		
		UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += RemoveMessageBox;
		UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += RemoveMessageBox;
	}
}
