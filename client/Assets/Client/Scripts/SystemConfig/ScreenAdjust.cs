using UnityEngine;
using System.Collections;


public enum RatioType
{
    R9_16,
    R3_5,
    R10_16,
    R2_3,
    R3_4
}
public enum SceneType
{
	NONE,
	UI,
	BATTLE,
    SELECT_ACTOR,
    GROUP,
}
public class ScreenAdjust : MonoBehaviour {
    private UIRoot uroot;
    //private SceneType sceneType = SceneType.NONE;
	void Awake() 
    {
        uroot = GetComponent<UIRoot>();
        ResetSceneRatio(SceneType.UI);
	}

    public void ResetSceneRatio(SceneType sceneType)
    {
        RatioType rt = GetRatio();
        //sceneType = StaticData.sceneType;
        switch (rt)
        {
            case RatioType.R9_16:
            case RatioType.R3_5:
                uroot.manualHeight = 640;
                break;
            case RatioType.R10_16:
                uroot.manualHeight = 640;
                break;
            case RatioType.R2_3:
                uroot.manualHeight = 640;
                break;
            case RatioType.R3_4:
                uroot.manualHeight = 810;
                break;
        }
    }

    public static float Scale
    {
        get { return ((float)ManualHeight * 1.0f) / 640f; }
    }

    public static int ManualHeight
    {
        get
        {
            RatioType ratioType = GetRatio();
            switch (ratioType)
            {
                case RatioType.R9_16:
                case RatioType.R3_5:
                    return 640;
                case RatioType.R10_16:
                    return 640;
                case RatioType.R2_3:
                    return 640;
                case RatioType.R3_4:
                    return 810;
                default:
                    return 640;
            }
            //if (StaticData.sceneType == SceneType.UI)
            //{
            //    switch (ratioType)
            //    {
            //        case RatioType.R9_16:
            //        case RatioType.R3_5:
            //            return 640;
            //        case RatioType.R10_16:
            //            return 640;
            //        case RatioType.R2_3:
            //            return 640;
            //        case RatioType.R3_4:
            //            return 810;
            //        default:
            //            return 640;
            //    }
            //}
            //else
            //{
            //    switch (ratioType)
            //    {
            //        case RatioType.R9_16:
            //        case RatioType.R3_5:
            //            return 720;
            //        case RatioType.R10_16:
            //        case RatioType.R2_3:
            //            return 800;
            //        case RatioType.R3_4:
            //            return 900;
            //        default:
            //            return 720;
            //    }
            //}
        }
    }
    
    public static Vector2 ScreenSize{
    	get{
            float height = ManualHeight;
            float width = height * Screen.width / (float)Screen.height;
            return new Vector2(width,height);
    	}
    }
    

    public static RatioType GetRatio()
    { 
		float fHeight = (float)Mathf.Min(Screen.height, Screen.width);
		float fWidth = (float)Mathf.Max(Screen.height, Screen.width);
		float r = fHeight / fWidth;
		//Debug.Log("height: " +fHeight+ "  width: "+fWidth);
        if (r <= 9.02f / 16f )
        {
            return RatioType.R9_16;
        }
        else if (r < 3.01f / 5f)
        {
            return RatioType.R3_5;
        }
        else if (r <= 10.01f / 16f)
        {
            return RatioType.R10_16;
        }
        else if (r <= 2.01f / 3f)
        {
            return RatioType.R2_3;
        }
        else
        {
            return RatioType.R3_4;
        }
    }

    public static float[] R16_9_BackPar = new float[] { 2.875f, 2.3f, 0.66f, 1.725f, 1.15f, 1.15f, 1.44f };
    public static float[] R4_3_BackPar = new float[] { 2.3f, 2f, 0.5f, 1.5f, 1f, 1f, 1.44f };
    public static string[] BackDoorParChild = new string[] { "a_1", "a_1/a_1_1", "a_1/a_1_2", "a_1/a_1_3", "a_1/a_3", "a_1/a_3/a_3", "a_1/a_2" };
}
