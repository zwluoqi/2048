using UnityEngine;
using System.Collections;

public class CardModel  {
	
	public bool _changed{get;set;}//表示该卡牌的值是否变化;
	public int 	_value{get;set;}//该位置当前的值2-2048;
	public int 	_currentPos{get;set;}//当前的位置;
	public int 	_lastPos{get;set;}//上次的位置;
	public int 	_moveSteps{get;set;}//卡牌到下一阶段的步数;
	
	public int _power{get{
			int q = _value;
			int p = 0;
			while(q > 1){
				p++;
				q >>= 1;
			}
			return p;
		}
	}//幂次方
	
	public CardModel(int t_value){
		this._value = t_value;

		
	}
	
	public void StoreCard(CardModel cardModel){
		if(cardModel != null){
			this._changed =     cardModel._changed;
			this._value =       cardModel._value;
			this._currentPos =  cardModel._currentPos;
			this._lastPos =     cardModel._lastPos;
			this._moveSteps =   cardModel._moveSteps;
		}else{
			Debug.LogError("CardModel Store Error");
		}
	}
	
}
