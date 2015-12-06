using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {


	public GameObject fieldStart ;
	public GameObject fieldEnd ;
	public GameObject [] players ;

	bool[,] coordinates;
	bool[,] activePath;
	
	static int fieldSize = 100;
	float fieldStartPosX;
	float fieldStartPosZ;
	//float fieldStartPosX = fieldStart.transform.position.x;
	//float fieldStartPosZ = fieldStart.transform.position.z;
	//26.1;
	//float fieldEndPosX;// = 48.71;
	//float fieldEndPosZ;// = 123.19;

	float unitSizeX;
	float unitSizeZ;


	private float[,,] alphaData;
	private TerrainData tData;
	private float percentage;


	// Use this for initialization
	void Start () {
		fieldStartPosX = fieldStart.transform.position.x;
		fieldStartPosZ = fieldStart.transform.position.z;

		//fieldEndPosX = fieldEnd.transform.position.x;
		//fieldEndPosZ = fieldEnd.transform.position.z;

		unitSizeX = (fieldEnd.transform.position.x - fieldStartPosX) / fieldSize;
		unitSizeZ = (fieldEnd.transform.position.z - fieldStartPosZ) / fieldSize;
		print (unitSizeX + " || " + unitSizeZ);


		tData = Terrain.activeTerrain.terrainData;
		
		alphaData = tData.GetAlphamaps(0, 0, tData.alphamapWidth, tData.alphamapHeight);
		
		//SetPercentage(0);

		//print (fieldStartPosX + " || " + fieldStart.transform.position.z);
		//print (fieldEnd.transform.position.x + " || " + fieldEnd.transform.position.z);
	}

	public void SetPercentage(double perc){
		percentage = (float) perc /100f;
		
		for(int y=0; y<tData.alphamapHeight; y++){
			for(int x = 0; x < tData.alphamapWidth; x++){
				alphaData[x, y, 0] = 1 - percentage;
				alphaData[x, y, 2] = percentage;
			}
		}
		
		tData.SetAlphamaps(0, 0, alphaData);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
