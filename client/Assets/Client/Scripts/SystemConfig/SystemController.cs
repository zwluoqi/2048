using UnityEngine;
using System.Collections;

using System;

public class SystemController : MonoSingleton<SystemController>{
	
	public bool isPause{
		get{
			
			return BoxManager.Instance.isPause ;
			
		}
		
	}
	
	public delegate void OnSharedCallBack();
	
	OnSharedCallBack onShareCallBack;
	
	void Start(){
		//ShareSDK.setCallbackObjectName("ShareSDK");
		//ShareSDK.open ("1bdf8d485308");
		
		//InitWeibo();
	}
	/*
	void InitWeibo(){
		Hashtable sinaWeiboConf = new Hashtable();
		sinaWeiboConf.Add("app_key", "1506608241");
		sinaWeiboConf.Add("app_secret", "a80ab88b88d1fbe2b59a4bf51e665da1");
		sinaWeiboConf.Add("redirect_uri", "http://www.sharesdk.cn");
		ShareSDK.setPlatformConfig (PlatformType.SinaWeibo, sinaWeiboConf);		
		
	}
	
	public void Share(string imagePath,int score, OnSharedCallBack _onShareCallBack){
		Hashtable content = new Hashtable();
		content["content"] = "well done.you get "+score.ToString()+",I do dare to challenge";
		content["image"] = imagePath;
		content["title"] = "2048萝莉";
		//content["description"] = "";
		content["url"] = "http://game.xiaomi.com/app-appdetail--app_id__25743.html";
		content["type"] = Convert.ToString((int)ContentType.Image);
		content["siteUrl"] = "http://game.xiaomi.com/app-appdetail--app_id__25743.html";
		//content["site"] = "weibo";
		//content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
		
		Debug.Log(MiniJSON.jsonEncode(content));
		
		onShareCallBack = _onShareCallBack;
		ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
		ShareSDK.showShareMenu (null, content, 100, 100, MenuArrowDirection.Up, evt);
	}
	
	void ShareResultHandler (ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
	{
	        if (state == ResponseState.Success)
	        {
	                print ("share result :");
	                print (MiniJSON.jsonEncode(shareInfo));
	        }
	        else if (state == ResponseState.Fail)
	        {
	                print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
	        }
		if(onShareCallBack != null){
			onShareCallBack();
			onShareCallBack = null;
		}
	}	
	*/
	void Update(){
	
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(BoxManager.Instance.isPause){
				BoxManager.Instance.RemoveMessageBox(null);
			}else{
				BoxManager.Instance .ShowQuitBox();
				UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnOkClick;
			}
		}

		
	}
	
	public void OnOkClick(GameObject go){
		StartCoroutine(QuitApp());
	}
	
	IEnumerator QuitApp(){
		yield return new WaitForSeconds(0.4f);
		Application.Quit();
	}
	
	public readonly float standard_width = 640;
    public readonly float standard_height = 960;
	public readonly int maxHeight = 2240;
	public readonly int minHeight = 320;
	public readonly int manualHeight = 1280;
	public float standard_aspect{
		get{
			return standard_width/standard_height;
		}
	}
}
