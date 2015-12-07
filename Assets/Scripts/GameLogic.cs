using UnityEngine;
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
		if (finnish != null && coordinates [0] == finnish.coordinates [0] && coordinates [1] == finnish.coordinates [1])
			return border.son;
		else
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
	public int[,] toOrderedArray(){
		//Debug.Log ("HEREEEEEE1!!!");
		int length = this.length ();
		int [,] array = new int [length, 2];
		TreeSet curr = this.cloneCoord();
//		Debug.Log (this.print (""));
//		Debug.Log (curr.print (""));
		for (int i = 0; i < length; i++) {
			TreeSet minAncestor = curr.findMinAncestor();
			array[i,0] = minAncestor.coordinates[0];
			array[i,1] = minAncestor.coordinates[1];
			curr = curr.eraseNode(minAncestor);
		}
		printArray (array);
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
	private TreeSet findMinAncestor(){
		TreeSet minNode = new TreeSet (new int[]{ 100, 100 }, null, 0);
		TreeSet curr = this;
		int length = this.length ();
		for (int i = 0; i < length; i++) {
			if (curr.coordinates[0]<minNode.coordinates[0] || (curr.coordinates[0] == minNode.coordinates[0] && curr.coordinates[1]<minNode.coordinates[1])){
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
	//public TreeSet get(){
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
	float tailTime;

	int[,] coordinates;
	TreeSet[] activePath;
	
	static int fieldSize = 100;
	static int fieldStartPosX = 205;
	static int fieldStartPosZ = 205;
	//float fieldStartPosX = fieldStart.transform.position.x;
	//float fieldStartPosZ = fieldStart.transform.position.z;
	//26.1;
	static int fieldEndPosX = 305;// = 48.71;
	static int fieldEndPosZ = 305;// = 123.19;

	int[,] pos;

	private float[,,] alphaData;
	private TerrainData tData;
	//private float percentage;


	// Use this for initialization
	void Start () {
		tailTime = players[0].GetComponent<TrailRenderer>().time;
//		fieldStartPosX = (int) fieldStart.transform.position.x;
//		fieldStartPosZ = (int) fieldStart.transform.position.z;
//
//		fieldEndPosX = (int) fieldEnd.transform.position.x;
//		fieldEndPosZ = (int) fieldEnd.transform.position.z;

		// INITIALIZE COORDINATES
		coordinates = new int[fieldSize, fieldSize];

		// INITIALIZE TERRAIN
		initializeTerrainTexture ();

		// SET PREVIOUS POSITION
		int k = 0;

		activePath = new TreeSet[players.GetLength(0)];
		pos = new int[players.GetLength (0), 2];
		foreach (GameObject player in players){
			pos[k,0] = getPosition(player)[0];
			pos[k,1] = getPosition(player)[1];
//			print (pos[k,0]);
//			print (pos[k,1]);
			int[] posCoor = {pos[k,0], pos[k,0]};
			activePath[k] = new TreeSet(posCoor, null, Time.time);
			k++;
		}



		// PRINT POSITIONS
//		foreach (GameObject player in players){
//			print (getPosition(player)[0] + " || " + getPosition(player)[1]);
//		}
	}

	void initializeTerrainTexture (){
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
//		foreach (GameObject player in players){
//			print (getPosition(player)[0] + " || " + getPosition(player)[1]);
//		}

		// SET POSITION
		int k = 0;
		foreach (GameObject player in players){
			int[] nPos = getPosition(player);
//			print (pos[k,0] + "==" + nPos[0]);
//			print (pos[k,1] + "==" + nPos[1]);
			if(pos[k,0] != nPos[0] || pos[k,1] != nPos[1]){
				activePath[k].deleteOld(tailTime);
				int[] posCoor = {pos[k,0], pos[k,1]};
				bool contains = activePath[k].contains(posCoor);
				activePath[k] = new TreeSet(posCoor, activePath[k], Time.time);
				if (contains){
					TreeSet res = activePath[k].getBorder();
					//print(res.print (""));
					//print(res.toArray()[0,0] + " : " + res.toArray()[0,1] + " || " + res.toArray()[res.length() - 1,0] + " : " + res.toArray()[res.length() - 1,1]);
					//print(res.toOrderedArray()[0,0] + " : " + res.toOrderedArray()[0,1] + " || " + res.toOrderedArray()[res.length() - 1,0] + " : " + res.toOrderedArray()[res.length() - 1,1]);
					//res.toArray();
					//res.toOrderedArray();
					setCoordinatesOptimized(res.toOrderedArray(), 3);
					//print(res.toOrderedArray()[0,0]);
				}
				pos[k,0] = nPos[0];
				pos[k,1] = nPos[1];
				//print (activePath[k].print(""));
			}
				
			k++;
		}
		applyPercantage ();
		applyCoordinates ();

	}

	void applyPercantage(){

	}

	void applyCoordinates ()
	{
		float area1 = 0;
		float area2 = 0;
		for (int i = 0; i < coordinates.GetLength(0); i++)
			for (int j = 0; j < coordinates.GetLength(1); j++) 
		{
			if ( coordinates[i,j] != 0)
			{			
				alphaData[fieldStartPosX + i , fieldStartPosZ + j ,1] = 0;
				alphaData[fieldStartPosX + i , fieldStartPosZ + j ,coordinates[i,j]] = 1;
				if (coordinates[i,j] == 2)
					area1++;
				else if (coordinates[i,j] == 3)
					area2++;
			}
			
		}	

		if (GameManager.instance != null) {
			GameManager.instance.area1 = area1;
			GameManager.instance.area2 = area2;
		}

		
		tData.SetAlphamaps (0, 0, alphaData);
	}

	void setCoordinates(int[,] border, int mark){
		// THIS IS UGLY!!!
		for (int i = 0; i < fieldSize; i++) {
			bool flag = false;
			for (int j = 0; j < fieldSize; j++) {
				for (int k = 0; k < border.GetLength(0); k++) {
					//print ("HERE!!!");
					if (border [k, 0]+50 == i && border [k, 1]+50 == j) {
						int borderPoints = 0;
						int prev = j;
						for (int l = j+1; l < fieldSize; l++) {
							for (int m = k+1; m < border.GetLength(0); m++) {
								if (border [m, 0]+50 == i && border [m, 1]+50 == l && prev != l-1) {
									borderPoints++;
									prev=l;
								}
							}
						}
						if (borderPoints % 2 == 0) {
							coordinates [i, j] = mark;
							flag = false;
//							print ("HERE");
						} else {
							coordinates [i, j] = mark;
							flag = true;
//							print ("NOOO");
						}
					}
				}
				if (flag) {
					coordinates [i, j] = mark;
				}
			}
		}

	}

	void setCoordinatesOptimized(int[,] border, int mark){
		print("i: "+ (border [border.GetLength(0)-1, 0]+50));
		for (int i = border [0, 0]+50; i <= border [border.GetLength(0)-1, 0]+50; i++) {
			bool flag = false;
			for (int j = 0; j < fieldSize; j++) {
				for (int k = 0; k < border.GetLength(0); k++) {
					if (border [k, 0]+50 == i && border [k, 1]+50 == j) {
						print ((border [k, 0]+50) + " : " + (border [k, 1]+50));
						int borderPoints = 0;
						int prev = border [k, 1]+50;
						for (int m = k+1; m < border.GetLength(0); m++) {
							if (border [m, 0]+50 == i && prev != border [m, 1]+50-1) {
								borderPoints++;
							}
							prev=border [m, 1]+50;
						}
						if (borderPoints % 2 == 0) {
							coordinates [i, j] = mark;
							flag = false;
							//							print ("HERE");
						} else {
							coordinates [i, j] = mark;
							flag = true;
							//							print ("NOOO");
						}
					}
				}
				if (flag) {
					coordinates [i, j] = mark;
				}
			}
		}
		
	}

	int [] getPosition (GameObject player)
	{
		Transform playerTransform = player.transform;
		// get player position
		Vector3 position = playerTransform.position;
		
		float [] pos = new float[2];
		pos [0] = position.z / tData.size.x * tData.alphamapWidth;
		pos [1] = position.x / tData.size.z * tData.alphamapHeight;
		
		int [] pos1 = new int[2];
		
		pos1 [0] = (int) Mathf.Round (pos [0]);
		pos1 [1] = (int) Mathf.Round (pos [1]);


		return pos1;
		
	}
}
