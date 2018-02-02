using UnityEngine;
using System.Collections;

//USED DATASTRUCTURES
//TREE SET
using System.Collections.Generic;

public class LinkedTreeSet
{
	public TreeSet treeSet;
	public LinkedTreeSet left;
	public LinkedTreeSet right;

	public LinkedTreeSet(LinkedTreeSet p, TreeSet m, LinkedTreeSet n){
		treeSet = m;
		left = p;
		right = n;
	}
	public void create (){
		right = new LinkedTreeSet (this, treeSet.son, null);
		LinkedTreeSet t = this;
		while (t.treeSet.son != null) {
			t.right = new LinkedTreeSet (t, t.treeSet.son, null);
			t = t.right;
		}
		t.right = this;
		this.left = t.right;
	}
	public int size(){
		int i = 0;
		LinkedTreeSet t = this;
		while (t.right != this) {
			i++;
			t = t.right;
		}
		return i + 1;
	}
	public string print(string text){
		LinkedTreeSet t = this;
		for (int i = 0; i < size (); i++) {
			text += t.treeSet.coordinates [0] + " : " + t.treeSet.coordinates [1] + " || ";
			t = t.right;
		}
		return text;
	}
	// O(n)
	public LinkedTreeSet find(int x, int y){
		LinkedTreeSet t = this;
		for (int i = 0; i < size (); i++) {
			if (t.treeSet.coordinates [0] == x && t.treeSet.coordinates [1] == y)
				return t;
			t = t.right;
		}
		return null;
	}
}

public class TreeSet
{
	public int[] coordinates;
	public TreeSet son;
	public float timeorcolor;
	public TreeSet(int[] coor, TreeSet s, float tc){
		coordinates = coor;
		son = s;
		timeorcolor = tc;
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
		//text += coordinates [0] + " : " + coordinates [1] + " - " + timeorcolor + " || ";
		text += coordinates [0] + " : " + coordinates [1] + " || ";
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

	public TreeSet get(int location){
		if (location == 0)
			return this;
		return son.get (location - 1);
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
		return array;
	}

	public int[,] toOrderedArray(){
		int length = this.length ();
		int [,] array = new int [length, 2];
		TreeSet curr = this.cloneCoord();
		for (int i = 0; i < length; i++) {
			TreeSet minAncestor = curr.findMinAncestor();
			array[i,0] = minAncestor.coordinates[0];
			array[i,1] = minAncestor.coordinates[1];
			curr = curr.eraseNode(minAncestor);
		}
		return array;
	}

	public TreeSet toOrderedTreeSet(){
		int length = this.length ();
		TreeSet ts = null;
		TreeSet curr = this.cloneCoord();
		for (int i = 0; i < length; i++) {
			TreeSet minAncestor = curr.findMaxAncestor();
			ts = new TreeSet (new int[] { minAncestor.coordinates [0], minAncestor.coordinates [1] }, ts, 0);
			curr = curr.eraseNode(minAncestor);
		}
		return ts;
	}

	public TreeSet extremesTreeSet(){
		int length = this.length ();
		TreeSet extremes = null;// new TreeSet (new int[]{ 100, 100 }, null, 0);
		TreeSet curr = this;
		TreeSet currBeginner = this; // for handling elements on the same height
		int location = this.length () - 1;
		while (location > 0 && this.get (location).coordinates [0] == this.coordinates [0]) {
			currBeginner = this.get (location);
			location = location - 1;
		}
		TreeSet prevTreeSet = this.get (location);
		bool asc = prevTreeSet.coordinates [0] < curr.coordinates [0];

		for (int i = 0; i <= length; i++) {
			//Debug.Log ("prevTreeSet " + prevTreeSet + " curr " + curr);
			if (prevTreeSet.coordinates [0] > curr.coordinates [0] && asc) {
				TreeSet firstExtreme;
				if (currBeginner.coordinates [1] < prevTreeSet.coordinates [1])
					firstExtreme = currBeginner;
				else
					firstExtreme = prevTreeSet;
				extremes = new TreeSet (new int[] { firstExtreme.coordinates [0], firstExtreme.coordinates [1] }, extremes, 0);
				asc = false;
			} else if (prevTreeSet.coordinates [0] < curr.coordinates [0] && !asc) {
				TreeSet firstExtreme;
				if (currBeginner.coordinates [1] < prevTreeSet.coordinates [1])
					firstExtreme = currBeginner;
				else
					firstExtreme = prevTreeSet;
				extremes = new TreeSet (new int[] { firstExtreme.coordinates [0], firstExtreme.coordinates [1] }, extremes, 0);
				asc = true;
			}

			if (prevTreeSet.coordinates [0] != curr.coordinates [0]) {
				currBeginner = curr;
			}
			//Debug.Log (i + " - " + length);
			prevTreeSet = curr;
			if (i == length - 1) {
				//Debug.Log ("HERE!!!");
				curr = this;
			}else
				curr = curr.next();
		}
		//if there are only horizontal lines
		if (extremes == null)
			return this;
		return extremes;
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
			if (curr.coordinates [0] < minNode.coordinates [0] || (curr.coordinates [0] == minNode.coordinates [0] && curr.coordinates [1] < minNode.coordinates [1]))
				minNode = curr;

			curr = curr.next();
		}
		return minNode;
	}

	private TreeSet findMaxAncestor(){
		TreeSet maxNode = new TreeSet (new int[]{ -100, -100 }, null, 0);
		TreeSet curr = this;
		int length = this.length ();
		for (int i = 0; i < length; i++) {
			if (curr.coordinates [0] > maxNode.coordinates [0] || (curr.coordinates [0] == maxNode.coordinates [0] && curr.coordinates [1] > maxNode.coordinates [1]))
				maxNode = curr;

			curr = curr.next();
		}
		return maxNode;
	}

	private TreeSet next(){
		return this.son;
	}

	// DELETES TOO OLD NODES
	public void deleteOld(float tailTime){
		if (this.son == null || Time.time - this.son.timeorcolor > tailTime)
			this.son = null;
		else
			this.son.deleteOld (tailTime);
	}

}

public class GameLogic : MonoBehaviour {
	public GameObject fieldStart ;
	public GameObject fieldEnd ;
	public GameObject [] players ;
	public GameObject[] pickups;
	public float treeDensity;
	public bool eatArea = true;
	public int pickupSpawnTime;
	public float pickupProbability;
	float tailTime;
	GameObject pickupParent;
	List <TreeInstance> treeList;
	float wtfNumber = 5.5f;
	bool pickUpAble = false;

	static int maxPlayers = 4;

	int[,] coordinates;
	TreeSet[] activePath;

	// IF IT IS 60 --> 60*60
	static int fieldSize = 60;

	// ABSOLUTE FIELD START POSITION
	static int fieldStartPosX = 225;
	static int fieldStartPosZ = 225;

	static int fieldEndPosX = 285;
	static int fieldEndPosZ = 285;

	static float terrainSize = 500f;

	int[,] pos;

	private float[,,] alphaData;
	private TerrainData tData;

	public void printArray(int[,] arr){
		string etext = "";
		for (int i = 0; i < arr.GetLength(0); i++) {
			etext += arr[i,0] + " : " + arr [i,1] + " || ";
		}
		Debug.Log (etext);
	}

	// Use this for initialization
	void Start () {
		// GET A PLACE TO DROP ON PICKUPS
		pickupParent = GameObject.Find("Pickups");

		// TIME IN WHICH TAIL VANISHES
		tailTime = players[0].GetComponent<TrailRenderer>().time;

		// INITIALIZE COORDINATES
		coordinates = new int[fieldSize, fieldSize];

		// INITIALIZE TERRAIN
		initializeTerrainTexture ();

		// INITIALIZE TREES
		treeList = new List <TreeInstance> ();
		Terrain.activeTerrain.terrainData.treeInstances = treeList.ToArray();

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
			activePath[k] = null;
			k++;
		}
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
	}

	public void restartTextures() {
		for (int i = 0; i < fieldSize; i++) {
			for (int j = 0; j < fieldSize; j++) {
				coordinates [i, j] = 1;
			}
		}
		// erases tail
		int k = 0;
		foreach (GameObject player in players) {
			int[] posCoor = { 1000, 1000 };
			activePath [k] = null;
			player.GetComponent<TrailRenderer> ().Clear ();
			pos [k, 0] = -1000;
			pos [k, 1] = -1000;
			k++;
		}


		var children = new List<GameObject>();
		foreach (Transform child in pickupParent.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));

		// erases trees
		List <TreeInstance> emptyForest = new List <TreeInstance> ();
		Terrain.activeTerrain.terrainData.treeInstances = emptyForest.ToArray();
		treeList = emptyForest;

		applyCoordinates ();
	}

	// Update is called once per frame
	void Update () {
		addPickups ();

		// SET POSITION
		int k = 0;
		foreach (GameObject player in players){
			// NEW POSITION
			int[] nPos = getPosition(player);
			if(pos[k,0] != nPos[0] || pos[k,1] != nPos[1]){
				// deletes too old nodes
				if (activePath[k] != null)
					activePath[k].deleteOld(tailTime);
				int[] posCoor = {pos[k,0], pos[k,1]};


				//bool contains = activePath[k].contains(posCoor);

				// adds the latest element to our path
				//activePath[k] = addTreeSet(posCoor, activePath[k], Time.time);//new TreeSet(posCoor, activePath[k], Time.time);

				bool contains = false;
				TreeSet intersect = null;


				if (activePath[k] != null) {
					// while x or z coordinates are not connected
					while (activePath[k].coordinates [0] < posCoor [0] - 1 || activePath[k].coordinates [0] > posCoor [0] + 1 ||
						activePath[k].coordinates [1] < posCoor [1] - 1 || activePath[k].coordinates [1] > posCoor [1] + 1) {


						int nx = activePath[k].coordinates [0];
						int nz = activePath[k].coordinates [1];

						// if x coordinate is not connected
						if (activePath[k].coordinates [0] < posCoor [0] - 1) {
							nx = activePath[k].coordinates [0] + 1;
						} else if (activePath[k].coordinates [0] > posCoor [0] + 1){
							nx = activePath[k].coordinates [0] - 1;
						}

						// if y coordinate is not connected
						if (activePath[k].coordinates [1] < posCoor [1] - 1) {
							nz = activePath[k].coordinates [1] + 1;
						} else if (activePath[k].coordinates [1] > posCoor [1] + 1){
							nz = activePath[k].coordinates [1] - 1;
						}

						int[] newCoors = { nx, nz };

						// add the missing coordinate
						activePath[k] = new TreeSet (newCoors, activePath[k], activePath[k].timeorcolor);
						if (activePath [k].son.contains (activePath [k].coordinates)) {
							contains = true;
							intersect = activePath [k];
							break;
						}
					}
				}
				// add the new coordinate
				activePath[k] = new TreeSet(posCoor, activePath[k], Time.time);



				if (!contains && activePath [k].son != null) {
					// checks if tail intersects with last element
					contains = activePath [k].son.contains (activePath [k].coordinates);
					if (contains)
						intersect = activePath [k];
				}

				if (contains){
					TreeSet borderTree = intersect.getBorder();

					// erases tail
					//activePath[k] = new TreeSet(posCoor, null, Time.time);
					intersect.son = null;
					player.GetComponent<TrailRenderer> ().Clear ();
					if (GameManager.instance != null) {
						if (player.name == "Player0") {
							setCoordinates(borderTree, 0);
						} else if (player.name == "Player1") {
							setCoordinates(borderTree, 1);
						}
					}
				}
				pos[k,0] = nPos[0];
				pos[k,1] = nPos[1];
			}
			k++;
		}
		applyCoordinates ();
	}

	TreeSet addTreeSet(int [] coord, TreeSet ancestor, float time) {
		if (ancestor != null) {
			// while x or z coordinates are not connected
			while (ancestor.coordinates [0] < coord [0] - 1 || ancestor.coordinates [0] > coord [0] + 1 ||
				ancestor.coordinates [1] < coord [1] - 1 || ancestor.coordinates [1] > coord [1] + 1) {
				int nx = ancestor.coordinates [0];
				int nz = ancestor.coordinates [1];

				// if x coordinate is not connected
				if (ancestor.coordinates [0] < coord [0] - 1) {
					nx = ancestor.coordinates [0] + 1;
				} else if (ancestor.coordinates [0] > coord [0] + 1){
					nx = ancestor.coordinates [0] - 1;
				}

				// if y coordinate is not connected
				if (ancestor.coordinates [1] < coord [1] - 1) {
					nz = ancestor.coordinates [1] + 1;
				} else if (ancestor.coordinates [1] > coord [1] + 1){
					nz = ancestor.coordinates [1] - 1;
				}

				int[] newCoors = { nx, nz };

				// add the missing coordinate
				ancestor = new TreeSet (newCoors, ancestor, ancestor.timeorcolor);
			}
		}
		// add the new coordinate
		return new TreeSet(coord, ancestor, Time.time);
	}

	void addPickups () {
		if ((int)Time.time % pickupSpawnTime != 0) {
			pickUpAble = true;
		} else if (pickUpAble) {
			pickUpAble = false;
			for (int i = 0; i < pickups.GetLength (0); i++) {
				if (Random.value < pickupProbability) {
					float x = (Random.value * 45) - 24;
					float z = (Random.value * 45) - 24;
					Vector3 newVec = new Vector3 (x, Terrain.activeTerrain.terrainData.GetInterpolatedHeight ((x + terrainSize / 2) / terrainSize, (z + terrainSize / 2) / terrainSize), z);
					Quaternion q = Quaternion.Euler (new Vector3 (0, 0, 0));
					GameObject newPickup = (GameObject)Instantiate (pickups [i], newVec, q);
					newPickup.transform.SetParent (pickupParent.transform, false);
				}
			}
		}
	}

	void setCoordinates (TreeSet borderTree, int playerID)
	{
		int colorID = GameManager.instance.players [playerID].colorID;
		int[,] horizCoord = getCoordinatesOptimized2 (borderTree, true);
		int[,] vertCoord = getCoordinatesOptimized2 (borderTree, false);
		int[] leadingColor = new int[4];

		// current position in vertCoord
		int j = 0;
		TreeSet checkedCoord = null;

		for (int i = 0; i < horizCoord.GetLength (0); i++) {
			//if we exceeded upper limit of j stop
			if (j >= vertCoord.GetLength (0))
				break;

			// to eliminate repeted comparisons
			// if next coordinate is the same jump over
			if (i + 1 < horizCoord.GetLength(0) && coordEq(horizCoord, i, horizCoord, i + 1))
				continue;
			while (j + 1 < vertCoord.GetLength (0) && coordEq (vertCoord, j, vertCoord, j + 1))
				j++;

			// if coordinates are in both arrays
			if (coordEq (horizCoord, i, vertCoord, j)) {
				int[] posCoor = { horizCoord [i, 0], horizCoord [i, 1] };
				checkedCoord = new TreeSet (posCoor, checkedCoord, coordinates [horizCoord [i, 0], horizCoord [i, 1]]);

				// get the leading color
				int color = coordinates [horizCoord [i, 0], horizCoord [i, 1]];
				if (color > 1 && color != colorID)
					leadingColor [color - 2]++;
			} 
			// if not there are to options, coordinate is in vertical but not horizontal rendering mode or reversed
			else {
				// if both coordinates are in the same line
				if (horizCoord [i, 0] == vertCoord [j, 0]) {
					//  if horizontal coordinate is larger than vertical it means that vertical coordinate is wrong
					if (horizCoord [i, 1] > vertCoord [j, 1]) {
						// go to next j
						j++;
						// i must stay the same, so lower it by one
						i--;
						continue;
					} 
					// if horizontal coordinate is smaller than vertical it means that horizontal coordinate is wrong
					else {
						continue;
					}
				}
				// if coordinates are in different lines
				else {
					// if horizontal coordinate is larger than vertical it means that vertical coordinate is wrong
					if (horizCoord [i, 0] > vertCoord [j, 0]) {
						// go to next j
						j++;
						// i must stay the same, so lower it by one
						i--;
						continue;
					}
					// if horizontal coordinate is smaller than vertical it means that horizontal coordinate is wrong
					else {
						continue;
					}
				}
			}
		}

		// get the best color
		int biggestArea = 0;
		int finalColor = colorID;
		string printS = "";
		for (int i = 0; i < leadingColor.GetLength (0); i++) {
			if (leadingColor [i] > biggestArea) {
				biggestArea = leadingColor [i];
				finalColor = i + 2;
			}
		}
		if (biggestArea == 0 || GameManager.instance.players [playerID].eatArea)
			finalColor = colorID;
		int [,] changedCoord = checkedCoord.toOrderedArray ();
		for (int i = 0; i < changedCoord.GetLength (0); i++) {
			coordinates [changedCoord [i, 0], changedCoord [i, 1]] = finalColor;
		}
		removeForest(changedCoord);
		addForest (changedCoord, finalColor);
	}

	void removeForest (int [,] viableCoord) {
		// find trees that are in circle
		List <TreeInstance> repeatedTreeList = new List <TreeInstance> ();
		for (int i = 0; i < viableCoord.GetLength (0); i++) {
			int treeID = insideTree (viableCoord [i, 0], viableCoord [i, 1]);
			if (treeID != -1)
				repeatedTreeList.Add (treeList [treeID]);
		}

		// erase trees that arein circle
		List <TreeInstance> newTreeList = new List <TreeInstance> ();
		for (int i = 0; i < treeList.Count; i++) {
			bool isRepeated = false;
			for (int j = 0; j < repeatedTreeList.Count; j++) {
				if (treeList [i].Equals(repeatedTreeList [j]))
					isRepeated = true;
			}
			if (!isRepeated) {
				newTreeList.Add (treeList [i]);
			}
		}

		Terrain.activeTerrain.terrainData.treeInstances = newTreeList.ToArray();
		treeList = newTreeList;
	}

	int insideTree(int x, int z) {
		for (int j = 0; j < treeList.Count; j++) {
			if ((float)z == Mathf.Round((treeList [j].position.x * 500) - (float)fieldStartPosZ + wtfNumber) && (float)x == Mathf.Round((treeList [j].position.z * 500) - (float)fieldStartPosX + wtfNumber)) {
				return j;
			}
		}
		return -1;
	}

	void addForest (int [,] viableCoord, int color) {
		for (int i = 0; i < viableCoord.GetLength (0); i++) {
			if (viableCoord [i, 0] % 3 == 0 && viableCoord [i, 1] % 3 == 0 && Random.value < treeDensity)
				addTree (viableCoord [i, 0], viableCoord [i, 1], color);
		}
		Terrain.activeTerrain.terrainData.treeInstances = treeList.ToArray();
	}

	void addTree (int x, int z, int color){
		Terrain terrain = Terrain.activeTerrain;

		// 1 - Create Tree from Prototype Tree 0
		TreeInstance newtree = new TreeInstance();
		newtree.prototypeIndex = color - 2;
		newtree.color = new Color (1, 1, 1);
		newtree.lightmapColor = new Color (1, 1, 1); 
		newtree.heightScale = 0.30f;
		newtree.widthScale = 0.30f;


		// 2 - Displace tree position randomly and height adjustment
		// calculate  relative x and z
		newtree.position = new Vector3(((float)fieldStartPosZ + (float)z - wtfNumber)/500f,0,((float)fieldStartPosX + (float)x - wtfNumber)/500f);
		newtree.position.y = terrain.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z)/600f;
		treeList.Add(newtree);
	}

	bool coordEq (int [,] coord1, int pos1, int [,] coord2, int pos2) {
		return coord1 [pos1, 0] == coord2 [pos2, 0] && coord1 [pos1, 1] == coord2 [pos2, 1];
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
			GameManager.instance.players[0].areaColored = area1/(fieldSize*fieldSize);
			GameManager.instance.players[1].areaColored = area2/(fieldSize*fieldSize);
		}


		tData.SetAlphamaps (0, 0, alphaData);
	}

	int[,] getCoordinatesOptimized2 (TreeSet borderTree, bool horizRendering) {
		int [,] border = borderTree.toOrderedArray ();

		// area which will get colored
		TreeSet colorArea = null;

		// preparation for horizontal or vertical rendering
		int n1;
		int n2;
		if (horizRendering) {
			n1 = 0;
			n2 = 1;
		} else {
			n1 = 1;
			n2 = 0;
		}
		int borderLength = border.GetLength (0);
		for (int i = 0; i < borderLength; i++) {
			// variable which tells us if next element of border is in the same column
			bool endsInSameLine = i + 1 < borderLength && border [i, n1] == border [i + 1, n1];
			// variable that tells us the last column to color
			int endCol = border [i, n2];
			if (endsInSameLine){
				endCol = border [i + 1, n2];
			}
			for (int j = border [i, n2]; j <= endCol; j++) {
				//LOGIC ------------
				if (horizRendering) {
					int[] posCoor = { border [i, n1] + (fieldSize / 2), j + (fieldSize / 2) };
					colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, n1] + (fieldSize / 2), j + (fieldSize / 2)]);
				} else {
					int[] posCoor = { j + (fieldSize / 2), border [i, n1] + (fieldSize / 2) };
					colorArea = new TreeSet (posCoor, colorArea, coordinates [j + (fieldSize / 2), border [i, n1] + (fieldSize / 2)]);
				}
			}
		}
		return colorArea.toOrderedArray();
	}


	int[,] getCoordinatesOptimized3 (TreeSet borderTree, bool horizRendering) {
		int [,] border = borderTree.toOrderedArray ();
		int [,] extremes = borderTree.extremesTreeSet().toOrderedArray();
		//int [,] border = borderTree.toArray ();
		Debug.Log ("whole border");
		printArray(borderTree.toOrderedArray());
		Debug.Log ("extremes");
		printArray(borderTree.extremesTreeSet().toOrderedArray());
		// area which will get colored
		TreeSet colorArea = null;

		int iExtr = 0;	
			
		int borderLength = border.GetLength (0);
		int extremesLength = extremes.GetLength (0);
		bool fillGap = false;
		bool extreme = false;
		bool borderB = false;
		for (int i = 0; i < borderLength; i++) {
			// handles begining of extreme
			if (iExtr < extremesLength && border [i, 0] == extremes [iExtr, 0] && border [i, 1] == extremes [iExtr, 1]) {
				if (!fillGap) {
					Debug.Log ("NE " + border [i, 0] + " : " + border [i, 1]);
					
					int[] posCoor = { border [i, 0] + (fieldSize / 2), border [i, 1] + (fieldSize / 2) };
					colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, 0] + (fieldSize / 2), border [i, 1] + (fieldSize / 2)]);
				} else {
					Debug.Log ("FE " + border [i, 0] + " : " + border [i, 1]);
					for (int j = border [i - 1, 1] + 1; j <= border [i, 1]; j++) {
						//LOGIC ------------
						int[] posCoor = { border [i, 0] + (fieldSize / 2), j + (fieldSize / 2) };
						colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, 0] + (fieldSize / 2), j + (fieldSize / 2)]);
					}
				}
				if (i + 1 < borderLength && border [i, 0] == border [i + 1, 0] && border [i, 1] == border [i + 1, 1] - 1)
					extreme = true;
				iExtr++;


			// if previous element is touching this one and also has an element on its right or if it is the end of extreme
			} else if (i > 0 && border [i - 1, 0] == border [i, 0] && border [i - 1, 1] == border [i, 1] - 1 &&
					(i + 1 < borderLength && border [i, 0] == border [i + 1, 0] && border [i, 1] == border [i + 1, 1] - 1 || 
					(i + 1 < borderLength && ((border [i, 0] == border [i + 1, 0] && border [i, 1] != border [i + 1, 1] - 1) || border [i, 0] != border [i + 1, 0]) && extreme) ||
					(i + 1 < borderLength && ((border [i, 0] == border [i + 1, 0] && border [i, 1] != border [i + 1, 1] - 1) || border [i, 0] != border [i + 1, 0]) && borderB))){
				Debug.Log ("N " + border [i, 0] + " : " + border [i, 1]);
				int[] posCoor = { border [i, 0] + (fieldSize / 2), border [i, 1] + (fieldSize / 2) };
				colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, 0] + (fieldSize / 2), border [i, 1] + (fieldSize / 2)]);
				if (i > 0 && border [i - 1, 0] == border [i, 0] && border [i - 1, 1] == border [i, 1] - 1 && 
						i + 1 < borderLength && ((border [i, 0] == border [i + 1, 0] && border [i, 1] != border [i + 1, 1] - 1) || border [i, 0] != border [i + 1, 0]) && extreme)
					extreme = false;
				if (i > 0 && border [i - 1, 0] == border [i, 0] && border [i - 1, 1] == border [i, 1] - 1 && 
						i + 1 < borderLength && ((border [i, 0] == border [i + 1, 0] && border [i, 1] != border [i + 1, 1] - 1) || border [i, 0] != border [i + 1, 0]) && borderB)
					borderB = false;

			} else {
				// if it is the first element or if the line is switched or if not fill gap
				if (i == 0 || i > 0 && border [i - 1, 0] != border [i, 0] || !fillGap) {
					Debug.Log ("NB " + border [i, 0] + " : " + border [i, 1]);
					int[] posCoor = { border [i, 0] + (fieldSize / 2), border [i, 1] + (fieldSize / 2) };
					colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, 0] + (fieldSize / 2), border [i, 1] + (fieldSize / 2)]);
					fillGap = !fillGap;

				} else {
					Debug.Log ("FB " + border [i, 0] + " : " + border [i, 1]);
					for (int j = border [i - 1, 1] + 1; j <= border [i, 1]; j++) {
						//LOGIC ------------
						int[] posCoor = { border [i, 0] + (fieldSize / 2), j + (fieldSize / 2) };
						colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, 0] + (fieldSize / 2), j + (fieldSize / 2)]);
					}
					fillGap = !fillGap;
				}
				borderB = true;
//				// handles border elements that are drawn in a line before inner elements 
//				if (i + 1 < borderLength && border [i, 0] == border [i + 1, 0] && border [i, 1] == border [i + 1, 1] - 1) {
//					while (i + 1 < borderLength && border [i, 0] == border [i + 1, 0] && border [i, 1] == border [i + 1, 1] - 1) {
//						int[] posCoor2 = { border [i + 1, 0] + (fieldSize / 2), border [i + 1, 1] + (fieldSize / 2) };
//						colorArea = new TreeSet (posCoor2, colorArea, coordinates [border [i + 1, 0] + (fieldSize / 2), border [i + 1, 1] + (fieldSize / 2)]);
//						i++;
//					}
//					i--;
//				}
//
//				//handles inner elements
//				for (int j = border [i, 1]; j <= border [i + 1, 1]; j++) {
//					//LOGIC ------------
//					int[] posCoor = { border [i, 0] + (fieldSize / 2), j + (fieldSize / 2) };
//					colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, 0] + (fieldSize / 2), j + (fieldSize / 2)]);
//
//				}
			}

//			// handles border elements in a line after extreme or inner elements
//			while (i + 1 < borderLength && border [i, 0] == border [i + 1, 0] && border [i, 1] == border [i + 1, 1] - 1) {
//				int[] posCoor2 = { border [i + 1, 0] + (fieldSize / 2), border [i + 1, 1] + (fieldSize / 2) };
//				colorArea = new TreeSet (posCoor2, colorArea, coordinates [border [i + 1, 0] + (fieldSize / 2), border [i + 1, 1] + (fieldSize / 2)]);
//				i++;
//			}




//			// variable which tells us if next element of border is in the same column
//			bool endsInSameLine = i + 1 < borderLength && border [i, n1] == border [i + 1, n1];
//			// variable that tells us the last column to color
//			int endCol = border [i, n2];
//			if (endsInSameLine){
//				endCol = border [i + 1, n2];
//			}
//			for (int j = border [i, n2]; j <= endCol; j++) {
//				//LOGIC ------------
//				int[] posCoor = { border [i, n1] + (fieldSize / 2), j + (fieldSize / 2) };
//				colorArea = new TreeSet (posCoor, colorArea, coordinates [border [i, n1] + (fieldSize / 2), j + (fieldSize / 2)]);
//
//			}
		}
		return colorArea.toOrderedArray();
	}

	int[,] getCoordinatesOptimized4 (TreeSet borderTree, bool horizRendering) {
		int [,] border = borderTree.toOrderedArray ();
		int [,] extremes = borderTree.extremesTreeSet().toOrderedArray();

		Debug.Log ("FROM HERE!!!");

		LinkedTreeSet borderLinkedTreeSet = new LinkedTreeSet (null, borderTree, null);
		borderLinkedTreeSet.create ();


		//Debug.Log (borderTree.print(""));
		Debug.Log ("Whole border:");
		Debug.Log (borderLinkedTreeSet.print(""));
		//Debug.Log (borderLinkedTreeSet.find(extremes[0,0], extremes[0,1]).print(""));

		//Debug.Log (borderTree.extremesTreeSet ().toOrderedTreeSet ().print (""));
		Debug.Log ("All extremes:");
		printArray(extremes);

		TreeSet colorArea = null;

		// first extreme
		TreeSet extreme = borderTree.extremesTreeSet ().toOrderedTreeSet ();

		while (borderLinkedTreeSet != null) {
			
			// start position
			LinkedTreeSet l = borderLinkedTreeSet.find (extreme.coordinates [0], extreme.coordinates [1]);
			LinkedTreeSet r = borderLinkedTreeSet.find (extreme.coordinates [0], extreme.coordinates [1]);

			//remove first extreme
			extreme = extreme.son;

			int h = l.treeSet.coordinates [0];

			Debug.Log ("A");
			// find the leftish neighbour in the same line
			int numb = 0;
			while (h == l.left.treeSet.coordinates [0]) {
				l = l.left;
				Debug.Log (l.treeSet.coordinates[0] + ":" + l.treeSet.coordinates[1]);
				// ERASE THIS (CAUSE OF UNEXPECTED BEHAVIOUR)
				if (numb >= 10) {
					Debug.Log ("NOOOO!!!!");
					break;

				}
				numb += 1;
				//break;
			}

			Debug.Log ("AAA");
			numb = 0;
			// find the rightish neighbour in the same line
			while (h == r.right.treeSet.coordinates [0]) {
				r = r.right;
				Debug.Log (r.treeSet.coordinates[0] + ":" + r.treeSet.coordinates[1]);
				// ERASE THIS (CAUSE OF UNEXPECTED BEHAVIOUR)
				if (numb >= 10) {
					Debug.Log ("NOOOO!!!!");
					break;

				}
				numb += 1;
			}

			Debug.Log ("Right coordinates");
			Debug.Log (r.treeSet.coordinates);

			Debug.Log ("Left coordinates");
			Debug.Log (l.treeSet.coordinates);
			Debug.Log ("HERE!!!");
			break;
			//int[] posCoor = { border [i, n1] + (fieldSize / 2), j + (fieldSize / 2) };
			//colorArea = new TreeSet (posCoor, colorArea, 0);
		}




		return null;
	}

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
