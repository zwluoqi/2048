using UnityEngine;
using System.Collections;

public enum MoveType{
	None,
	Left,
	Right,
	Up,
	Down
}

public class UITouchEvent : MonoSingleton<UITouchEvent> {
	
	// 手勢判斷
	TouchPhase touchPhase = TouchPhase.Began;

	Vector2 startPos = Vector2.zero;
	Vector2 endPos = Vector2.zero;
	float minSwipeLength = 50f;
	Vector2 GesturesCtrl (Vector2 startPoint,Vector2 endPoint){
		Vector2 gesturesDir = Vector2.zero;
	    float xDiff = endPoint.x - startPoint.x;
	    float yDiff = endPoint.y - startPoint.y;
	    float slope = Mathf.Abs(yDiff / xDiff);
		
	    // 宣告 垂直 水平 布林，上、右為真
	    int verticalBool;    // 上 1 下 -1
	    int horizontal;        // 右 1 左 -1
	    // 宣告 方向
	    
	    // 判斷水平方向，大於0則於上方1、小於0則為下方-1，否則可能水平移動0
	    if (yDiff>0){
	        verticalBool = 1;
	    }else if(yDiff<0){
	        verticalBool = -1;
	    }else{
	        verticalBool = 0;
	    }
	    // 判斷水平方向，大於0則於右方1、小於0則為左方-1，否則可能垂直移動0
	    if (xDiff>0){
	        horizontal = 1;
	    }else if(xDiff<0){
	        horizontal = -1;
	    }else{
	        horizontal = 0;
	    }
	    // 假使 斜率 大於 2 則為 純 垂直方向
	    if (slope>2){
	        gesturesDir.y = verticalBool;
	        gesturesDir.x = 0;
	    // 假使 斜率 小於 0.5 則為 純 水平方向
	    }else if(slope<0.5){
	        gesturesDir.x = horizontal;
	        gesturesDir.y = 0;
	    // 假使 [斜率 落於 2,0.5之間 則為斜向] 或 [傾斜角非90度(x相減不為零) 則為水平(機率低但發生則因yDiff判斷為0而成立為純水平方向，又已於xDiff判斷水平方向)]
	    }else{
	        gesturesDir.x = horizontal;
	        gesturesDir.y = verticalBool;
	    }
	    print(gesturesDir);
		return gesturesDir;
	}	
	
	public MoveType GetMoveType(){
		MoveType moveType = MoveType.None;
		
		if(Input.touchCount == 1){
			if(Input.GetTouch(0).phase == TouchPhase.Moved
				&& touchPhase == TouchPhase.Began){
			    endPos = Input.GetTouch(0).position;
				if((startPos - endPos).magnitude > minSwipeLength){
				    Vector2 gesturesDir = GesturesCtrl(startPos,endPos);
					
					if(Mathf.Abs(gesturesDir.y) > Mathf.Abs(gesturesDir.x)){
						if(gesturesDir.y >0 ){
							moveType = MoveType.Up;
							Debug.Log("moveUp");
						}else{
							moveType = MoveType.Down;
							Debug.Log("moveDown");
						}
					}else{
						if(gesturesDir.x >0 ){
							moveType = MoveType.Right;
							Debug.Log("moveRight");
						}else{
							moveType = MoveType.Left;
							Debug.Log("moveLeft");
						}
					}
					touchPhase = TouchPhase.Ended;
				}
			}
		}

		return moveType;
		
	}	
	
	void Update(){
		if(Input.touchCount == 1){
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
			    startPos = Input.GetTouch(0).position;
				Debug.Log("Began");
			}			
			
			if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				Debug.Log("End");
				touchPhase = TouchPhase.Began;
			}
		}
		
	}
}
