using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GamePlayManager : MonoBehaviour{
	
	CardManager cardManager;
	int maxGrid = 16;
	
	int mBestScore = 0;
	int bestScore {get{
			return mBestScore;
		}set{
			mBestScore = value;
			PlayerPrefs.SetInt("bestScore",value);
			PlayerPrefs.Save();
		}
	}
	
	bool showLable{
		get{
			return currentAtlasName =="jxpve"||currentAtlasName=="digital2";
		}
		
	}
	
	int currentScore {get;set;}
	public UILabel currentScoreLable;
	public UILabel bestScoreLable;
	public UILabel popupInfoLable;
	
	string uiGameResultName = "Windows/UIGameResult";
	
	string cardName = "WindowsItem/Card2D";
	string hetiName = "Heti";
	string[] atlasNames = new string[]{
		"meinv",
		"digital2",
		"jxpve"
	};
	string currentAtlasName ;
	string[] currentStyle;
	

	
	AudioClip readyClip;
	AudioClip UI_touch;
		
	GameObject heti ;
	Dictionary<string,AudioClip> audioClips = new Dictionary<string, AudioClip>();
	Dictionary<string,AudioClip> comboClips = new Dictionary<string, AudioClip>();
	
	List<GameObject> cardGameObjects = new List<GameObject>();
	List<GameObject> animationCardGameObjs = new List<GameObject>();
	
	bool started = false;
	void Awake(){

		ResourcesManager.InitData ();
		bestScore = PlayerPrefs.GetInt("bestScore");
		
		//按钮点击音乐
		string path = "";
		for(int i = 0; i <11;i++){
			if(i == 0){
				path = ResourcesManager.meinvjiaosheng+"0";
			}else{
				path = ResourcesManager.meinvjiaosheng + (1<<i).ToString();
			}
			AudioClip audioClip = Resources.Load(path) as AudioClip;
			if(audioClip != null){
				audioClips.Add(audioClip.name,audioClip);
			}
		}
		
		//combo音乐
		for(int i = 0; i<4;i++){

			path = ResourcesManager.tiantianaixiaochu + i.ToString();
			AudioClip comboClip = Resources.Load(path) as AudioClip;
			if(comboClips != null){
				comboClips.Add(comboClip.name,comboClip);
			}						
		}
		
		
		readyClip = Resources.Load(ResourcesManager.tiantianaixiaochu+"V_ready",typeof(AudioClip)) as AudioClip;
		UI_touch = Resources.Load(ResourcesManager.tiantianaixiaochu+"UI_touch",typeof(AudioClip)) as AudioClip;
		currentAtlasName = atlasNames[0];
		
		/*
		GameObject hetiPre = Resources.Load(hetiName) as GameObject;
		heti = GameObject.Instantiate(hetiPre) as GameObject;
		heti.transform.parent = transform.parent;
		heti.transform.localPosition = new Vector3(0,0,-5);
		heti.transform.localScale = Vector3.one;
		heti.transform.localRotation = Quaternion.identity;
		*/
		
		GameObject card2D = Resources.Load(cardName) as GameObject;
		for(int i=0;i<maxGrid;i++){
			GameObject temp = GameObject.Instantiate(card2D) as GameObject;
			temp.name = cardName + i.ToString();
			temp.transform.parent = transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localRotation = Quaternion.identity;
			int horizontal = i%4;
			int vertical = i/4;
			temp.transform.localPosition = new Vector3((horizontal - 2)*100,(vertical-2)*100,0);
			cardGameObjects.Add(temp);
			
			//此预制为播放动画使用;
			GameObject animationTemp = GameObject.Instantiate(temp) as GameObject;
			animationTemp.transform.parent = transform;
			animationTemp.transform.localScale = Vector3.one;
			animationTemp.transform.localRotation = Quaternion.identity;
			animationCardGameObjs.Add(animationTemp);
		}
		
		Resources.UnloadUnusedAssets();
	}
	
	
	// Use this for initialization
	void Start () {
		cardManager = new CardManager();
		cardManager.init(maxGrid);
		
		if(PlayerPrefs.GetInt("store",-1) == 1){
			for(int i = 0 ;i<maxGrid;i++){
				cardManager.cards[i]._value = PlayerPrefs.GetInt("card"+i.ToString(),0);
			}
			cardManager.currentScore = PlayerPrefs.GetInt("storeCurrentScore",0);
			SaveCardGrids(false);
		}
		
		ResetSprite();
		
		UpdateLable();
		
		OnPlayAudioClip();
	}
	
	private void OnPlayAudioClip(){
		//heti.SetActive(false);
		started = true;
		animationPlayed = true;
		
		UpdateSprite();
	}
	
	public void OnRefreshClick(){
		
		BoxManager.Instance.ShowRefreshBox();
		UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += AnimationStart;
		
	}	
	

	
	#region FaileRefresh
	//游戏失败弹出框并选择
	//分享或继续;
	
	public void AnimationStart(GameObject go){
		ResetSprite();
		StartCoroutine(AnimationStartCor());
	}	
	
	IEnumerator AnimationStartCor(){
		//yield return new WaitForSeconds(1);
		GameObject three = ResourcesManager.Instance.CreateGameObj("TimeView");
		yield return new WaitForSeconds(3);
		BoxManager.Instance.RemoveMessageBox(null);
		Destroy(three);
		Start();
	}
	#endregion
	
	public void OnUndo(){
		
		cardManager.UndoProcessCard();
		UpdateSprite();
	}
	
	public void OnChangeAtlas(){
		int resoult = 0;
		for(int i = 0 ; i< atlasNames.Length;i++ ){
			if(currentAtlasName == atlasNames[i]){
				resoult = (i+1)%atlasNames.Length;
				break;
			}
		}
		currentAtlasName = atlasNames[resoult];
		if(currentAtlasName == "jxpve"){
			currentStyle = ResourcesManager.stylejxpve;
		}else if(currentAtlasName == "digital2"){
			currentStyle = ResourcesManager.digital2;
		}else{
			;
		}
		ChangeSprite();
	}
	
	
	
	// Update is called once per frame
	void Update () {
		if(SystemController.Instance.isPause || !started){
			return;
		}
		
		MoveType moveType = MoveType.None;
		
		if(animationPlayed){
#if UNITY_EDITOR		
		if(Input.GetKeyDown(KeyCode.W)){
			moveType = MoveType.Up;
		}else if(Input.GetKeyDown(KeyCode.S)){
			moveType = MoveType.Down;
		}else if(Input.GetKeyDown(KeyCode.A)){
			moveType = MoveType.Left;
		}else if(Input.GetKeyDown(KeyCode.D)){
			moveType = MoveType.Right;
		}else{
			;
		}
#else		
		moveType = UITouchEvent.Instance.GetMoveType();
#endif		
		}
		
		if(moveType != MoveType.None){
			cardManager.ProcessCards(moveType);
			if(cardManager.isFailed){
				started = false;
			}else{
				animationPlayed = false;
				StartCoroutine("PlayCardMove",moveType);
			}
			
			GameResult();
		}
	}
	

	bool animationPlayed = false;	
	IEnumerator PlayCardMove(MoveType moveType){
		//将格子全部设置为看不见;
		for(int i = 0;i < maxGrid ; i++){
			animationCardGameObjs[i].SetActive(true);
			UISprite uiSprite = cardGameObjects[i].GetComponentInChildren<UISprite>();
			UISprite uiSpriteAnimation = animationCardGameObjs[i].GetComponentInChildren<UISprite>();
			
			UILabel uiLable = cardGameObjects[i].GetComponentInChildren<UILabel>();
			UILabel uiLableAnimation = animationCardGameObjs[i].GetComponentInChildren<UILabel>();
			
			
			if(showLable){
				uiLableAnimation.text = uiLable.text;
				uiSpriteAnimation.spriteName = "0";
			}else{
				uiLableAnimation.text = "";
				uiSpriteAnimation.spriteName = uiSprite.spriteName;
			}
			if(uiSprite.color != blackAlpha){
				uiSpriteAnimation.color = uiSprite.color;
			}else{
				uiSpriteAnimation.color = alphaColor;
			}
			
			uiSprite.spriteName = "0";
			uiLable.text = "";
			
			if(uiSpriteAnimation.spriteName == "0"){
				animationCardGameObjs[i].transform.localPosition = cardGameObjects[i].transform.localPosition + new Vector3(0,0,5);
				uiSpriteAnimation.depth = -1;
			}else{
				animationCardGameObjs[i].transform.localPosition = cardGameObjects[i].transform.localPosition + new Vector3(0,0,-10);
				uiSpriteAnimation.depth = 1;
			}
			
		}
		yield return null;
		
		
		//开始移动格子
		float startTime = Time.time;
		float speed = 3600f;
		float playTime = 0.1f;
		while(Time.time - startTime < playTime){
			for(int i=0;i<maxGrid;i++){
				if(cardManager.lastCards[i]._moveSteps == 0
					||cardManager.lastCards[i]._value == 0)
					continue;
				UISprite uiSprite = cardGameObjects[i].GetComponentInChildren<UISprite>();
				if(uiSprite.color != blackAlpha){
					uiSprite.color = blackAlpha;
				}

				float moveSpeed = speed*cardManager.lastCards[i]._moveSteps/4f;
				float newpos = 0;
				switch(moveType){
				case MoveType.Left:
					newpos = Mathf.Clamp(animationCardGameObjs[i].transform.localPosition.x - moveSpeed*Time.deltaTime,
										-200,100);
					animationCardGameObjs[i].transform.localPosition = new Vector3(newpos,
						animationCardGameObjs[i].transform.localPosition.y,
						animationCardGameObjs[i].transform.localPosition.z);
					break;
				case MoveType.Right:
					newpos = Mathf.Clamp(animationCardGameObjs[i].transform.localPosition.x + moveSpeed*Time.deltaTime,
										-200,100);	
					animationCardGameObjs[i].transform.localPosition = new Vector3(newpos,
						animationCardGameObjs[i].transform.localPosition.y,
						animationCardGameObjs[i].transform.localPosition.z);
					break;
				case MoveType.Up:
					newpos = Mathf.Clamp(animationCardGameObjs[i].transform.localPosition.y + moveSpeed*Time.deltaTime,
										-200,100);
					animationCardGameObjs[i].transform.localPosition = new Vector3(
						animationCardGameObjs[i].transform.localPosition.x,
						newpos,
						animationCardGameObjs[i].transform.localPosition.z);					
					break;
				case MoveType.Down:
					newpos = Mathf.Clamp(animationCardGameObjs[i].transform.localPosition.y - moveSpeed*Time.deltaTime,
										-200,100);						
					animationCardGameObjs[i].transform.localPosition = new Vector3(
						animationCardGameObjs[i].transform.localPosition.x,
						newpos,
						animationCardGameObjs[i].transform.localPosition.z);	
					break;
				default:
					break;

				}
				//tempCardGameObjs[i].transform.localPosition += 
			}
			Debug.Log("Play Animation");
			yield return null;
		}
		
		UpdateSprite();
		yield return null;
		UpdateLable();
		animationPlayed = true;
		yield return null;
	}
	
	
	void GameResult(){


		if(cardManager.isWon){
			UpdateBestScore();
			BoxManager.Instance.ShowWinBox();
			UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnShowResult;
			UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += AnimationStart;

			
		}else if(cardManager.isFailed){
			UpdateBestScore();
			BoxManager.Instance.ShowFailedRefreshBox();
			UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnShowResult;
			UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += AnimationStart;	


		}else{
			;
		}

//		StartCoroutine("OpenYjfGG");

	}

//#if UNITY_IPHONE || UNITY_ANDROID
//	IEnumerator OpenYjfGG(){
//		UnityAds.ShowAds();
//		Debug.Log ("show ads");
//		yield return null;	
//		
//	}
//#endif
	void OnShowResult(GameObject go){
		
		GameObject resultObj = ResourcesManager.Instance.CreateGameObj(uiGameResultName);
		UIGameResult uiResultObj = resultObj.GetComponent<UIGameResult>();
		uiResultObj.SetResult(cardManager.largePower,currentScore,this);
		
	}
	Color blackAlpha = new Color (1, 1, 1, 0.2f);
	Color alphaColor = new Color(0,0,0,0);

	void ChangeSprite(){
		for(int i=0;i<maxGrid;i++){
			animationCardGameObjs[i].SetActive(false);
			UISprite uiSprite = cardGameObjects[i].GetComponentInChildren<UISprite>();
			UILabel uiLable = cardGameObjects[i].GetComponentInChildren<UILabel>();
			UIPlaySound uiPlaySound = cardGameObjects[i].GetComponentInChildren<UIPlaySound>();
			if(showLable){
				uiLable.text = currentStyle[cardManager.cards[i]._power];
				uiSprite.spriteName = "0";
				if(cardManager.cards[i]._power == 0){
					if(uiSprite.color != blackAlpha){
						uiSprite.color = blackAlpha;
					}
				}else{
					if(uiSprite.color !=  ResourcesManager.backColors[cardManager.cards[i]._power]){
						uiSprite.color = ResourcesManager.backColors[cardManager.cards[i]._power];
					}
				}
				uiPlaySound.audioClip = UI_touch;
			}else{
				uiLable.text = "";
				uiSprite.spriteName = cardManager.cards[i]._power.ToString();
				if(cardManager.cards[i]._power == 0){
					if(uiSprite.color != blackAlpha){
						uiSprite.color = blackAlpha;
					}
				}else{
					if(uiSprite.color != ResourcesManager.backColors[0]){
						uiSprite.color = ResourcesManager.backColors[0];
					}
				}

				//				audioClips.TryGetValue( cardManager.cards[i]._value.ToString(),out uiPlaySound.audioClip);
			}

		}
	}
	
	void ResetSprite(){
		for(int i=0;i<maxGrid;i++){
			animationCardGameObjs[i].SetActive(false);
			UISprite uiSprite = cardGameObjects[i].GetComponentInChildren<UISprite>();
			UILabel uiLable = cardGameObjects[i].GetComponentInChildren<UILabel>();
			UIPlaySound uiPlaySound = cardGameObjects[i].GetComponentInChildren<UIPlaySound>();
			
			uiSprite.spriteName = "0";
			uiLable.text = "";
			uiSprite.color = blackAlpha;
			uiPlaySound.audioClip = null;
		}
		currentScore = cardManager.currentScore;
	}
	
	void UpdateSprite(){
		//显示当前格子的最总效果;
		for(int i=0;i<maxGrid;i++){
			animationCardGameObjs[i].SetActive(false);
			
			UISprite uiSprite = cardGameObjects[i].GetComponentInChildren<UISprite>();
			UILabel uiLable = cardGameObjects[i].GetComponentInChildren<UILabel>();
			TweenScale uiTweenScale = cardGameObjects[i].GetComponentInChildren<TweenScale>();
			UIPlaySound uiPlaySound = cardGameObjects[i].GetComponentInChildren<UIPlaySound>();

			CardModel cardModel = cardManager.cards[i];
			
			if(uiSprite != null && cardModel != null){

				if(showLable){
					uiLable.text = currentStyle[cardManager.cards[i]._power];
					uiSprite.spriteName = "0";
					if(cardManager.cards[i]._power == 0){
						if(uiSprite.color != blackAlpha){
							uiSprite.color = blackAlpha;
						}
					}else{
						if(uiSprite.color !=  ResourcesManager.backColors[cardManager.cards[i]._power]){
							uiSprite.color = ResourcesManager.backColors[cardManager.cards[i]._power];
						}
					}
				}else{
					uiLable.text = "";
					uiSprite.spriteName = cardManager.cards[i]._power.ToString();
					if(cardManager.cards[i]._power == 0){
						if(uiSprite.color != blackAlpha){
							uiSprite.color = blackAlpha;
						}
					}else{
						if(uiSprite.color != ResourcesManager.backColors[0]){
							uiSprite.color = ResourcesManager.backColors[0];
						}
					}
				}

				
				if(cardManager.cards[i]._value != 0
					&&cardManager.cards[i]._changed){
					uiTweenScale.ResetToBeginning();
					uiTweenScale.Play(true);
					int p = cardManager.cards[i]._power%4;
					NGUITools.PlaySound(comboClips[p.ToString()]);
				}
			}
			
			if(uiPlaySound != null && cardModel != null){
				if(!showLable){
//					audioClips.TryGetValue( cardManager.cards[i]._value.ToString(),out uiPlaySound.audioClip);
				}
			}
//			Debug.Log(uiPlaySound.audioClip.name);
			
		}
		currentScore = cardManager.currentScore;
	}
	void UpdateLable(){
		currentScoreLable.text = currentScore.ToString();
		bestScoreLable.text = bestScore.ToString();
	}
	
	void OnApplicationPause(bool pauseStatus){
		UpdateBestScore();
		SaveCardGrids(pauseStatus);
	}
	
	void OnApplicationQuit(){
		//UpdateBestScore();
		//SaveCardGrids(true);
	}
	
	void UpdateBestScore(){
		if(currentScore > bestScore){
			bestScore = currentScore;
		}
	}
	
	void SaveCardGrids(bool pauseStatus){
		
		if(pauseStatus){
			PlayerPrefs.SetInt("store",1);
			PlayerPrefs.SetInt("storeCurrentScore",currentScore);
			for(int i = 0;i<maxGrid;i++){
				PlayerPrefs.SetInt("card"+i.ToString(),cardManager.cards[i]._value);
			}
			Debug.LogWarning("save game");
		}else{
			PlayerPrefs.SetInt("store",-1);
			Debug.LogWarning("restore game");
		}
		
	}
}
