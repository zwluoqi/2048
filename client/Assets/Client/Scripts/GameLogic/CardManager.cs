using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager {

	int maxGrid = 16;
	public List<CardModel> cards;
	public List<CardModel> lastCards;
	
	int maxLists = 10;
	private List<List<int>> cardPosLists; 
	
	public int currentScore {get;set;}
	
	public int largePower{
		get{
			int power = 0;
			foreach(CardModel card in cards){
				if(card._power > power){
					power = card._power;
				}
			}
			return power;
		}
		
	}
	
	public bool isWon{
		get{
			foreach(CardModel card in cards){
				if(card._value == 1<<(ResourcesManager.maxCardNum-1)){
					return true;
				}
				
			}
			return false;
		}
		
	}
	
	bool failed = false;
	public bool isFailed{get{
			return failed;
		}
		}
	
	public void init(int maxCount){
		cardPosLists = new List<List<int>>(maxLists);

		maxGrid = maxCount;
		cards = new List<CardModel>(maxGrid);
		lastCards = new List<CardModel>(maxGrid);
		for(int i = 0;i<maxGrid;i++){
			cards.Add(new CardModel(0));
			lastCards.Add(new CardModel(0));
		}

		
		int first = Random.Range(0,maxGrid);
		int second = -1;
		do{
			second = Random.Range(0,maxGrid);
		}while(first == second);
		
		cards[first]._value = 1<<Random.Range(1,3);
		cards[second]._value = 1<<Random.Range(1,3);
		lastCards[first]._value = cards[first]._value;
		lastCards[second]._value = cards[second]._value;
		
		LogCardInfo();
	}
	
	public void UndoProcessCard(){
		
		List<int> firstList = null;
		if(cardPosLists.Count > 0){
			firstList = cardPosLists[cardPosLists.Count-1];
			cardPosLists.RemoveAt(cardPosLists.Count-1);
		}
		
		if(firstList != null){
			for(int i = 0;i<maxGrid;i++){
				cards[i]._value = firstList[i];
	
				cards[i]._changed = false;
				
				lastCards[i]._moveSteps = 0;
			}
		}
		
		
		
	}
	
	public bool AddCardRandom(){
		int[] temp = new int[maxGrid];
		int index = 0;
		int i = 0;
		foreach(CardModel card in cards){
			if(card._value == 0){
				temp[index++] = i;
			}
			i++;
		}
		
		if(index == 0){
			Debug.LogWarning("do not have space");
			failed = true;
			return false;
		}else{
			int randomValue = Random.Range(0,index);
			cards[temp[randomValue]] = new CardModel(2);
			failed = false;
			return true;
		}
	}
	
	public void LogCardInfo(){
		string append = "";
		string[] temp = new string[4];
		for(int i=0;i <maxGrid;i++){
			append += string.Format("\t{0}", cards[i]._value.ToString());

			if((i+1)%4 == 0){
				temp[i/4] = append;
				append = "";
			}
		}
		
		append = "";
		for(int i = 3 ; i>=0;i--){
			append += temp[i]+'\n';
		}
		Debug.Log(append);
	}
	
	public bool ProcessCards(MoveType moveType){
		
		//存储当前的位置信息;
		List<int> posValue = new List<int>(maxGrid);
		
		for(int i = 0;i<maxGrid;i++){
			posValue.Add(cards[i]._value);
			
			lastCards[i]._value = cards[i]._value;
			lastCards[i]._moveSteps = 0;

			
			cards[i]._changed = false;
		}
		
		if(cardPosLists.Count >= maxLists){
			cardPosLists.RemoveAt(0);
		}
		cardPosLists.Add(posValue);
		
		//处理方块格子信息;
		switch(moveType){
		case MoveType.Left:
			ProcessRow(0,1,3);
			ProcessRow(4,1,7);
			ProcessRow(8,1,11);
			ProcessRow(12,1,15);
			break;
		case MoveType.Right:
			ProcessRow(3,-1,0);
			ProcessRow(7,-1,4);
			ProcessRow(11,-1,8);
			ProcessRow(15,-1,12);
			break;
		case MoveType.Up:
			ProcessRow(12,-4,0);
			ProcessRow(13,-4,1);
			ProcessRow(14,-4,2);
			ProcessRow(15,-4,3);
			break;
		case MoveType.Down:
			ProcessRow(0,4,12);
			ProcessRow(1,4,13);
			ProcessRow(2,4,14);
			ProcessRow(3,4,15);			
			break;
		default:
			break;
		}
		//合成完后随即产生2,4;
		if(AddCardRandom()){
			//调试信息
			LogCardInfo();
			return true;
		}else{
			return false;
		}
		

		
	}
	
	public void ProcessRow(int start,int increase,int end){
		int index = 0;
		List<CardModel> temp = new List<CardModel>();
		
		//抽取一排或一列中非空项
		for(int i = start; i != end; i += increase){
			if(cards[i]._value != 0){
				//return;
				temp.Add(cards[i]);
			}else{
				for(int j = i + increase;j != end;j += increase){
					if(lastCards[j]._value != 0){
						lastCards[j]._moveSteps++;
					}
				}
				if(lastCards[end]._value != 0)
					lastCards[end]._moveSteps++; 
			}
		}
		if(cards[end]._value != 0)
			temp.Add(cards[end]);
		
		
		//将一排或一列的非空项挤压到顶端
		for(int i = start; i != end; i += increase){
			if(temp.Count > index){
				cards[i]._value = temp[index++]._value;
			}else{
				cards[i]._value = 0;
			}
		}
		if(temp.Count > index){
			cards[end]._value = temp[index++]._value;
		}else{
			cards[end]._value = 0;
		}
		
		
		//对一排或一列的非空项处理
		//合成数字
		for(int i = start; i != end; i += increase){
			if(cards[i]._value == 0 || cards[i+increase]._value == 0)
				break;
			
			if(cards[i]._value == cards[i+increase]._value){
				cards[i]._value <<= 1;
				cards[i]._changed = true;
				currentScore += cards[i]._value;
				
				int j = i+increase;
				while(j != end){
					cards[j]._value = cards[j+increase]._value;
					if(lastCards[j]._value != 0){
						lastCards[j]._moveSteps++;
					}
					 j += increase;
				}
				cards[end]._value = 0;
				if(lastCards[end]._value != 0){
					lastCards[end]._moveSteps++;
				}
			}
		}
		
	}
	
}
