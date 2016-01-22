﻿using UnityEngine;
using System.Collections;

//USED DATASTRUCTURES
//TREE SET
public class TreeSet
{
	public int[] coordinates;
	public TreeSet son;
	public float time;
	public TreeSet(int[] coor, TreeSet s, float t){
		coordinates = coor;
		son = s;
		time = t;
	}

	// checks if tail intersects
	public bool contains(int[] coor){
		if (coor [0] == coordinates [0] && coor [1] == coordinates [1])
			return true;
		if (son == null)
			return false;
		return son.contains (coor);
	}
	public string print(string text){
		text += coordinates [0] + " : " + coordinates [1] + " - " + time + " || ";
		if (son == null)
			return text;
		else
			return son.print (text);
		
	}
	public TreeSet getBorder(TreeSet ancestor = null, TreeSet finnish = null){
		TreeSet border = new TreeSet (coordinates, ancestor, 0);
		if (finnish != null && coordinates [0] == finnish.coordinates [0] && coordinates [1] == finnish.coordinates [1]) {
			return border.son;
		}else
			if (finnish == null)
				return son.getBorder (border, border);
			else
				return son.getBorder (border, finnish);

	}
	public int length(){
		if (son == null)
			return 1;
		return son.length() + 1;
	}
	public int[,] toArray(){
		int length = this.length ();
		int [,] array = new int [length, 2];
		TreeSet curr = this;
		for (int i = 0; i < length; i++) {
			array[i,0] = curr.coordinates[0];
			array[i,1] = curr.coordinates[1];
			curr = curr.next();
		}
		//printArray (array);
		return array;
	}
	public int[,] toOrderedArray(bool horiz){
		//Debug.Log ("HEREEEEEE1!!!");
		int length = this.length ();
		int [,] array = new int [length, 2];
		TreeSet curr = this.cloneCoord();
//		Debug.Log (this.print (""));
//		Debug.Log (curr.print (""));
		for (int i = 0; i < length; i++) {
			TreeSet minAncestor = curr.findMinAncestor(horiz);
			array[i,0] = minAncestor.coordinates[0];
			array[i,1] = minAncestor.coordinates[1];
			curr = curr.eraseNode(minAncestor);
		}
		//printArray (array);
		//print (this.findMinAncestor().coordinates[0] + " || " + this.findMinAncestor().coordinates[1]);
		return array;
	}
	private void printArray(int[,] arr){
		string etext = "";
		for (int i = 0; i < arr.GetLength(0); i++) {
			etext += arr[i,0] + " : " + arr [i,1] + " || ";
		}
		Debug.Log (etext);
	}
	private TreeSet cloneCoord(){
		TreeSet clone = new TreeSet (new int[]{ this.coordinates[0], this.coordinates[1] }, null, 0);
		TreeSet curr = this;
		TreeSet temp = clone;
		int length = this.length ();
		for (int i = 0; i < length; i++) {
			if (curr.son != null)
				temp.son = new TreeSet (new int[]{ curr.son.coordinates[0], curr.son.coordinates[1] }, null, 0);
			temp = temp.next();
			curr = curr.next();
		}
		return clone;
	}

	private TreeSet eraseNode(TreeSet node){
		if (this == node)
			return this.son;

		TreeSet curr = this;
		int length = this.length ();
		for (int i = 0; i < length; i++) {
			if (curr.son == node){
				curr.son = curr.son.son;
				break;
			}
			curr = curr.next();
		}
		return this;

	}
	private TreeSet findMinAncestor(bool horiz){
		TreeSet minNode = new TreeSet (new int[]{ 100, 100 }, null, 0);
		TreeSet curr = this;
		int length = this.length ();
		for (int i = 0; i < length; i++) {
			if (horiz) {
				if (curr.coordinates [0] < minNode.coordinates [0] || (curr.coordinates [0] == minNode.coordinates [0] && curr.coordinates [1] < minNode.coordinates [1]))
					minNode = curr;
			} else {
				if (curr.coordinates [1] < minNode.coordinates [1] || (curr.coordinates [1] == minNode.coordinates [1] && curr.coordinates [0] < minNode.coordinates [0]))
					minNode = curr;
			}
			curr = curr.next();
		}
		return minNode;
	}
//	private TreeSet eraseNode(TreeSet node){
//
//	}
	private TreeSet next(){
		return this.son;
	}

	// DELETES TOO OLD NODES
	public void deleteOld(float tailTime){
		if (this.son == null || Time.time - this.son.time > tailTime)
			this.son = null;
		else
			this.son.deleteOld (tailTime);
	}

}

public class GameLogic : MonoBehaviour {


	public GameObject fieldStart ;
	public GameObject fieldEnd ;
	public GameObject [] players ;
	public bool eatArea = true;
	float tailTime;

	static int maxPlayers = 4;

	int[,] coordinates;
	TreeSet[] activePath;

	// IF IT IS 60 --> 60*60
	static int fieldSize = 60;
	// ABSOLUTE FIELD START POSITION
	static int fieldStartPosX = 225;
	static int fieldStartPosZ = 225;

	//26.1;
	static int fieldEndPosX = 285;// = 48.71;
	static int fieldEndPosZ = 285;// = 123.19;

	int[,] pos;

	private float[,,] alphaData;
	private TerrainData tData;
	//private float percentage;


	// Use this for initialization
	void Start () {
		// TIME IN WHICH TAIL VANISHES
		tailTime = players[0].GetComponent<TrailRenderer>().time;

		// INITIALIZE COORDINATES
		coordinates = new int[fieldSize, fieldSize];

		// INITIALIZE TERRAIN
		initializeTerrainTexture ();

		// SET PREVIOUS POSITION
		int k = 0;

		// CREATES A DATA STRUCTURE WITH THE TAIL
		activePath = new TreeSet[players.GetLength(0)];
		pos = new int[players.GetLength (0), 2];
		foreach (GameObject player in players){
			pos[k,0] = getPosition(player)[0];
			pos[k,1] = getPosition(player)[1];

			// INITIAL POSITION MUST BE DIFFERENT FROM REAL, SO THAT WE DONT GET INITIAL POS COLORED
			int[] posCoor = {0, 0};
			activePath[k] = new TreeSet(posCoor, null, Time.time);
			k++;
		}



		// PRINT POSITIONS
//		foreach (GameObject player in players){
//			print (getPosition(player)[0] + " || " + getPosition(player)[1]);
//		}
	}

	// SELECTS A FIELD TEXTURE DESERT AND APPLIES IT TO WASTELAND
	public void initializeTerrainTexture (){
		tData = Terrain.activeTerrain.terrainData;
		
		alphaData = tData.GetAlphamaps (0, 0, tData.alphamapWidth, tData.alphamapHeight);

		for (int i = fieldStartPosX; i < fieldEndPosX; i++)
			for (int j = fieldStartPosZ; j < fieldEndPosZ; j++) {
			
			
				alphaData [i, j, 0] = 0;
				alphaData [i, j, 1] = 1;
				alphaData [i, j, 2] = 0;
				alphaData [i, j, 3] = 0;
				alphaData [i, j, 4] = 0;
				alphaData [i, j, 5] = 0;
				
			}	
		
		tData.SetAlphamaps (0, 0, alphaData);
		Debug.Log ("GameLogic: Initialized Terrain" );
		
//		print (tData.alphamapHeight);
//		print (tData.alphamapWidth);


	}

//	public void SetPercentage(double perc){
//		percentage = (float) perc /100f;
//		
//		for(int y=0; y<tData.alphamapHeight; y++){
//			for(int x = 0; x < tData.alphamapWidth; x++){
//				alphaData[x, y, 0] = 1 - percentage;
//				alphaData[x, y, 2] = percentage;
//			}
//		}
//		
//		tData.SetAlphamaps(0, 0, alphaData);
//	}
	// Update is called once per frame
	void Update () {

		// SET POSITION
		int k = 0;
		foreach (GameObject player in players){
			// NEW POSITION
			int[] nPos = getPosition(player);
//			print (pos[k,0] + "==" + nPos[0]);
//			print (pos[k,1] + "==" + nPos[1]);
			if(pos[k,0] != nPos[0] || pos[k,1] != nPos[1]){
				// deletes too old nodes
				activePath[k].deleteOld(tailTime);
				int[] posCoor = {pos[k,0], pos[k,1]};
				// checks if tail intersects
				bool contains = activePath[k].contains(posCoor);
				// adds the latest element to our path
				activePath[k] = new TreeSet(posCoor, activePath[k], Time.time);
				if (contains){
					TreeSet res = activePath[k].getBorder();

					//int[] posCoor = {0, 0};
					// erases tail
					activePath[k] = new TreeSet(posCoor, null, Time.time);
					player.GetComponent<TrailRenderer> ().Clear ();
					//rend.Clear;
					//print(res.print (""));
					//print(res.toArray()[0,0] + " : " + res.toArray()[0,1] + " || " + res.toArray()[res.length() - 1,0] + " : " + res.toArray()[res.length() - 1,1]);
					//print(res.toOrderedArray()[0,0] + " : " + res.toOrderedArray()[0,1] + " || " + res.toOrderedArray()[res.length() - 1,0] + " : " + res.toOrderedArray()[res.length() - 1,1]);
					//res.toArray();
					//res.toOrderedArray();
					bool horiz = true;

					if (GameManager.instance != null) {
						if (player.name == "Player0")
							//setCoordinatesOptimized (res.toOrderedArray (), GameManager.instance.color1);
							//getMark(res.toOrderedArray (), GameManager.instance.color1)
							if (eatArea)
								setCoordinatesOptimized2(res.toOrderedArray (horiz), GameManager.instance.players[0].colorID);
							else
								setCoordinatesOptimized2(res.toOrderedArray (horiz), getMark(res.toOrderedArray (horiz), GameManager.instance.players[0].colorID));
						else if (player.name == "Player1")
							//setCoordinatesOptimized (res.toOrderedArray (), GameManager.instance.color2);
							if (eatArea)
								setCoordinatesOptimized2(res.toOrderedArray (horiz), GameManager.instance.players[1].colorID);
							else
								setCoordinatesOptimized2(res.toOrderedArray (horiz), getMark(res.toOrderedArray (horiz), GameManager.instance.players[1].colorID));
					} else
						if (eatArea)
							setCoordinatesOptimized2 (res.toOrderedArray (horiz), 3);
						else
							setCoordinatesOptimized2(res.toOrderedArray (horiz), getMark(res.toOrderedArray (horiz), 3));
					//print(res.toOrderedArray()[0,0]);
				}
				pos[k,0] = nPos[0];
				pos[k,1] = nPos[1];
				//print (activePath[k].print(""));
			}
				
			k++;
		}
		//applyPercantage ();
		applyCoordinates ();

	}

//	void applyPercantage(){
//
//	}

	void applyCoordinates ()
	{
		float area1 = 0;
		float area2 = 0;
		for (int i = 0; i < coordinates.GetLength(0); i++)
			for (int j = 0; j < coordinates.GetLength(1); j++) 
			{
				if ( coordinates[i,j] != 0)
				{			
					//alphaData[fieldStartPosX + i , fieldStartPosZ + j ,1] = 0;
					for (int k = 0; k < maxPlayers + 2; k++)
						alphaData [fieldStartPosX + i, fieldStartPosZ + j, k] = 0;
					alphaData[fieldStartPosX + i , fieldStartPosZ + j ,coordinates[i,j]] = 1;
					if (GameManager.instance != null) {
						if (coordinates [i, j] == GameManager.instance.players[0].colorID)
							area1++;
						else if (coordinates [i, j] == GameManager.instance.players[1].colorID)
							area2++;
					} else {
						if (coordinates [i, j] == 2)
							area1++;
						else if (coordinates [i, j] == 3)
							area2++;
					}
				}
			}	

		if (GameManager.instance != null) {
			GameManager.instance.players[0].areaColored = area1;
			GameManager.instance.players[1].areaColored = area2;
		}

		
		tData.SetAlphamaps (0, 0, alphaData);
	}

//	void setCoordinates(int[,] border, int mark){
//		// THIS IS UGLY!!!
//		for (int i = 0; i < fieldSize; i++) {
//			bool flag = false;
//			for (int j = 0; j < fieldSize; j++) {
//				for (int k = 0; k < border.GetLength(0); k++) {
//					//print ("HERE!!!");
//					if (border [k, 0]+(fieldSize/2) == i && border [k, 1]+(fieldSize/2) == j) {
//						int borderPoints = 0;
//						int prev = j;
//						for (int l = j+1; l < fieldSize; l++) {
//							for (int m = k+1; m < border.GetLength(0); m++) {
//								if (border [m, 0]+(fieldSize/2) == i && border [m, 1]+(fieldSize/2) == l && prev != l-1) {
//									borderPoints++;
//									prev=l;
//								}
//							}
//						}
//						if (borderPoints % 2 == 0) {
//							coordinates [i, j] = mark;
//							flag = false;
////							print ("HERE");
//						} else {
//							coordinates [i, j] = mark;
//							flag = true;
////							print ("NOOO");
//						}
//					}
//				}
//				if (flag) {
//					coordinates [i, j] = mark;
//				}
//			}
//		}
//	}
	int getMark (int[,] border, int initMark) {
		int borderLength = border.GetLength (0);
		//int finalMark = initMark;
		int[] leadingColor = new int[4];
		for (int i = 0; i < borderLength; i++) {
			// variable which tells us if next element of border is in the same column
			bool endsInSameLine = i + 1 < borderLength && border [i, 0] == border [i + 1, 0];
			// variable that tells us the last column to color
			int endCol = border [i, 1];
			if (endsInSameLine){
				endCol = border [i + 1, 1];
			}
			for (int j = border [i, 1]; j <= endCol; j++) {
				//LOGIC ------------
				int color = coordinates [border [i, 0]+(fieldSize/2), j+(fieldSize/2)];
				// checks if color is not desert or his color
				if (color > 1 && color != initMark)
					leadingColor [color - 2]++;
				//------------------
			}
			//if (endsInSameLine)
			//	i++;
		}
		int biggestArea = 0;
		int finalMark = initMark;
		for (int i = 0; i < leadingColor.GetLength (0); i++) {
			if (leadingColor [i] > biggestArea) {
				biggestArea = leadingColor [i];
				finalMark = i + 2;
			}
		}
		if (biggestArea == 0)
			return initMark;
		else
			return finalMark;
	}
	int setCoordinatesOptimized2 (int[,] border, int initMark) {
		int borderLength = border.GetLength (0);
		for (int i = 0; i < borderLength; i++) {
			// variable which tells us if next element of border is in the same column
			bool endsInSameLine = i + 1 < borderLength && border [i, 0] == border [i + 1, 0];
			// variable that tells us the last column to color
			int endCol = border [i, 1];
			if (endsInSameLine){
				endCol = border [i + 1, 1];
			}
			for (int j = border [i, 1]; j <= endCol; j++) {
				//LOGIC ------------
				coordinates [border [i, 0]+(fieldSize/2), j+(fieldSize/2)] = initMark;
				//------------------
			}
			//if (endsInSameLine)
			//	i++;
		}
		return 0;
	}
//	void setCoordinatesOptimized(int[,] border, int mark){
//		print("i: "+ (border [border.GetLength(0)-1, 0]+(fieldSize/2)));
//		for (int i = border [0, 0]+(fieldSize/2); i <= border [border.GetLength(0)-1, 0]+(fieldSize/2); i++) {
//			bool flag = false;
//			for (int j = 0; j < fieldSize; j++) {
//				for (int k = 0; k < border.GetLength(0); k++) {
//					if (border [k, 0]+(fieldSize/2) == i && border [k, 1]+(fieldSize/2) == j) {
//						print ((border [k, 0]+(fieldSize/2)) + " : " + (border [k, 1]+(fieldSize/2)));
//						int borderPoints = 0;
//						int prev = border [k, 1]+(fieldSize/2);
//						for (int m = k+1; m < border.GetLength(0); m++) {
//							if (border [m, 0]+(fieldSize/2) == i && prev != border [m, 1]+(fieldSize/2)-1) {
//								borderPoints++;
//							}
//							prev=border [m, 1]+(fieldSize/2);
//						}
//						if (borderPoints % 2 == 0) {
//							coordinates [i, j] = mark;
//							flag = false;
//							//							print ("HERE");
//						} else {
//							coordinates [i, j] = mark;
//							flag = true;
//							//							print ("NOOO");
//						}
//					}
//				}
//				if (flag) {
//					coordinates [i, j] = mark;
//				}
//			}
//		}
//		
//	}

	// RETURNS POSITION ON OUR MAP FROM PLAYER
	int [] getPosition (GameObject player)
	{
		Transform playerTransform = player.transform;
		// get player position
		Vector3 position = playerTransform.position;
		
		float [] pos = new float[2];
		// transforms absolute position to position on our map
		pos [0] = position.z / tData.size.x * tData.alphamapWidth;
		pos [1] = position.x / tData.size.z * tData.alphamapHeight;
		
		int [] pos1 = new int[2];

		// rounds position to int
		pos1 [0] = (int) Mathf.Round (pos [0]);
		pos1 [1] = (int) Mathf.Round (pos [1]);


		return pos1;
		
	}
}
