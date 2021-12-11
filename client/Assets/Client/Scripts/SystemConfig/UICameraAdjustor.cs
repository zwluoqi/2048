using UnityEngine;
using System.Collections;

/// <summary>
/// 根据设备的宽高比，调整camera.orthographicSize. 以保证UI在不同分辨率(宽高比)下的自适应
/// 须与UIAnchor配合使用
/// 将该脚本添加到UICamera同一节点上
/// </summary>

[RequireComponent(typeof(UICamera))]
public class UICameraAdjustor : MonoBehaviour
{

//#if !UNITY_IPHONE	

    float device_width = 0f;
    float device_height = 0f;

    void Awake()
    {

		foreach(UIRoot root in UIRoot.list)
		{
			if (root != null) {
				root.manualHeight = SystemController.Instance.manualHeight;
				root.minimumHeight = SystemController.Instance.minHeight;
				root.maximumHeight = SystemController.Instance.maxHeight;
			}
		}
		
		
        SetCameraSize();
    }
	
	void Update(){
		SetCameraSize();
	}
	
    private void SetCameraSize()
    {
        device_width = Screen.width;
        device_height = Screen.height;		
		float rw = device_width/SystemController.Instance.standard_width;
		float rh = device_height/SystemController.Instance.standard_height;
		
		float r = 1;
		r = rw > rh ? rh:rw;

		GetComponent<Camera>().orthographicSize =  1/r;
    }
//#endif	
}
