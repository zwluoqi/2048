using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoSingleton<ResourcesManager>  {

	public const int maxCardNum = 32;

	public static string[] digital2 = new string[maxCardNum];

	public static void InitData(){
		for (int i=1; i<maxCardNum; i++) {
			digital2[i] = (1<<i).ToString();
		}
	}
	
	public static string[] stylejxpve = new string[]{
		"",
		"雁虞",
		"剑鸣",
		"荻魂",
		"蜀风",
		"蚩灵",
		"南皇",
		"烛天",
		"破军",
		"狼烟",
		"靖世",
		"定国",
		"破虏",
		"秦风",
		"朔雪",
		"儒风",
		"校服1",
		"校服2",
		"校服3",
		"校服4",
		"校服5",
		"校服6",
		"校服7",
		"校服8",
		"校服9",
		"校服10",
		"校服11",
		"校服12",
		"校服13",
		"校服14",
		"校服15",
		"校服16",
		"校服17",
		"校服18",
		"校服19",
		"校服20",
	};
	
	public static Color[] backColors = new Color[]{
		new Color(1,1,1),//l baise		
		new Color(173/255f,216/255f,230/255f),//l blue
		new Color(240/255f,128/255f,128/255f),//l coral
		new Color(224/255f,255f/255f,255f/255f),//l cyan
		new Color(144/255f,238/255f,144/255f),//l green
		new Color(211/255f,211/255f,211/255f),//l grey
		new Color(255f/255f,182/255f,193/255f),//l pink
		new Color(255f/255f,160/255f,122/255f),//l salmon
		new Color(32/255f,178/255f,170/255f),//l seagreen
		new Color(135/255f,206/255f,250/255f),//l skyblue
		new Color(255f/255f,69/255f,0/255f),//l Orangered
		new Color(173/255f,216/255f,230/255f),//l blue
		new Color(240/255f,128/255f,128/255f),//l coral
		new Color(224/255f,255f/255f,255f/255f),//l cyan
		new Color(144/255f,238/255f,144/255f),//l green
		new Color(211/255f,211/255f,211/255f),//l grey
		new Color(255f/255f,182/255f,193/255f),//l pink
		new Color(255f/255f,160/255f,122/255f),//l salmon
		new Color(32/255f,178/255f,170/255f),//l seagreen
		new Color(135/255f,206/255f,250/255f),//l skyblue
		new Color(255f/255f,69/255f,0/255f),//l Orangered
		new Color(173/255f,216/255f,230/255f),//l blue
		new Color(240/255f,128/255f,128/255f),//l coral
		new Color(224/255f,255f/255f,255f/255f),//l cyan
		new Color(144/255f,238/255f,144/255f),//l green
		new Color(211/255f,211/255f,211/255f),//l grey
		new Color(255f/255f,182/255f,193/255f),//l pink
		new Color(255f/255f,160/255f,122/255f),//l salmon
		new Color(32/255f,178/255f,170/255f),//l seagreen
		new Color(135/255f,206/255f,250/255f),//l skyblue
		new Color(255f/255f,69/255f,0/255f),//l Orangered
		new Color(173/255f,216/255f,230/255f),//l blue
		new Color(240/255f,128/255f,128/255f),//l coral
		new Color(224/255f,255f/255f,255f/255f),//l cyan
		new Color(144/255f,238/255f,144/255f),//l green
		new Color(211/255f,211/255f,211/255f),//l grey
		new Color(255f/255f,182/255f,193/255f),//l pink
		new Color(255f/255f,160/255f,122/255f),//l salmon
		new Color(32/255f,178/255f,170/255f),//l seagreen
		new Color(135/255f,206/255f,250/255f),//l skyblue
		new Color(255f/255f,69/255f,0/255f),//l Orangered
	};
	
	
	public GameObject CreateGameObj(string objName){
		Object quitapp = Resources.Load(objName);
		GameObject topWindow = GameObject.Instantiate(quitapp) as GameObject;
		topWindow.transform.parent = Camera.main.transform;
		topWindow.transform.localScale = Vector3.one;
		topWindow.transform.localPosition = Vector3.zero;
		topWindow.transform.localRotation = Quaternion.identity;
		
		Resources.UnloadUnusedAssets();
		return topWindow;
	}
	
	public static string tiantianaixiaochu = "Sounds/tiantianaixiaochu/";
	public static string meinvjiaosheng = "Sounds/meinvjiaosheng/";
}
