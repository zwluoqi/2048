using UnityEngine;
using System.Collections;
using System.IO;

public class UIGameResult : MonoBehaviour {

	public UILabel meizhiCountLabel;
	public UILabel scoreLabel;
	public CardXiongMao[] cardXiongMaos;
	private Vector3 startPos = new Vector3(-150,-150,-5);
	public float scrollLength = 100;
	public UIPanel meizhiPanle;
	private int score;
	
	private GamePlayManager gameManager;
	
	private AudioClip gameClip;
	private GameObject xiongmaoCard;
	private string CardXiongMaoName = "WindowsItem/CardXiongMao";
	public void Awake(){
		gameClip = Resources.Load(ResourcesManager.tiantianaixiaochu+"bgm_game",typeof(AudioClip)) as AudioClip;
		AudioManger.Instance.PlayAudio(gameClip);
		
		cardXiongMaos = new CardXiongMao[12];
		for(int i = 0;i<12;i++){
			GameObject temp = ResourcesManager.Instance.CreateGameObj(CardXiongMaoName);;
			
			cardXiongMaos[i] = temp.GetComponent<CardXiongMao>();
			
			temp.transform.parent = meizhiPanle.transform;
			temp.transform.localPosition = startPos + new Vector3((i%4)*scrollLength,(i/4)*scrollLength,0);
			temp.transform.localScale = Vector3.one;
			temp.transform.localRotation = Quaternion.identity;
		}
	}
	
	public void SetResult(int meinvCount,int _score,GamePlayManager _gameManager){
		meizhiCountLabel.text = meinvCount.ToString();
		scoreLabel.text = _score.ToString();
		this.score = _score;
		this.gameManager = _gameManager;
		
		for(int i = 0; i<cardXiongMaos.Length ;i++){
			if(i <= meinvCount){
				cardXiongMaos[i].xiongmao.SetActive(false);
				cardXiongMaos[i].meizhi.GetComponent<UISprite>().spriteName = (i+1).ToString();
			}else{
				cardXiongMaos[i].meizhi.SetActive(false);
			}
		}
	}
	
	public void OnShare(){
		string imageName = "scoreImage" + System.DateTime.Now.ToString("yyyy.MM.d HH.mm.ss") + ".png";
		ScreenCapture.CaptureScreenshot(imageName);
		imageName = Path.Combine(Application.persistentDataPath,imageName);

#if UNITY_ANDROID		
		//SystemController.Instance.Share(imageName,score,null);
#endif
		
		Debug.Log("share");
		
	}
	
	
	public void OnBack(){
		
		gameManager.AnimationStart(null);
		TweenPosition twPos = gameObject.GetComponent<TweenPosition>();
		twPos.from = new Vector3(0,0,0);
		twPos.to = new Vector3(-500,0,0);
		twPos.AddOnFinished(OnPlayFinished);
		twPos.ResetToBeginning();
		twPos.Play(true);		
		
	}
	
	private void OnPlayFinished(){
		Destroy(this.gameObject);
	}	
	
	void OnDestroy(){
		AudioManger.Instance.StopAudio();
		
	}
}
