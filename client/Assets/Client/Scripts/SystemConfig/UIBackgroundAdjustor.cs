using UnityEngine;
using System.Collections;

/// <summary>
/// 根据设备的宽高比，调整UISprite scale, 以保证全屏的背景图在不同分辨率(宽高比)下的自适应
/// 将该脚本添加到UISprite同一节点上
/// 须与UICameraAdjustor脚本配合使用
/// </summary>


public class UIBackgroundAdjustor : MonoBehaviour
{
    float device_width = 0f;
    float device_height = 0f;

    void Awake()
    {


        SetBackgroundSize();
    }
	
	void Update(){
		SetBackgroundSize();
	}	
	
    private void SetBackgroundSize()
    {
        UITexture m_back_sprite = GetComponent<UITexture>();
		
        device_width = Screen.width;
        device_height = Screen.height;		
		float standard_aspect = SystemController.Instance.standard_aspect;
		Debug.Log("standard_aspect:"+standard_aspect);
        float device_aspect = device_width / device_height;
		Debug.Log("device_aspect:"+device_aspect);
		
		float adust = device_aspect/standard_aspect;
        if (device_aspect < standard_aspect)
        {
			m_back_sprite.height = Mathf.CeilToInt( SystemController.Instance.standard_height / adust);
        }else{
			m_back_sprite.width = Mathf.CeilToInt( SystemController.Instance.standard_width * adust);
		}
		
		
    }
}