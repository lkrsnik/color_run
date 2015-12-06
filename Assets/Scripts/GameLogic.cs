using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {


	public GameObject fieldStart ;
	public GameObject fieldEnd ;
	public GameObject [] players ;

	bool[,] coordinates;
	bool[,] activePath;
	
	static int fieldSize = 100;
	int fieldStartPosX;
	int fieldStartPosZ;
	//float fieldStartPosX = fieldStart.transform.position.x;
	//float fieldStartPosZ = fieldStart.transform.position.z;
	//26.1;
	int fieldEndPosX;// = 48.71;
	int fieldEndPosZ;// = 123.19;

	float unitSizeX;
	float unitSizeZ;


	private float[,,] alphaData;
	private TerrainData tData;
	private float percentage;


	// Use this for initialization
	void Start () {
		fieldStartPosX = (int) fieldStart.transform.position.x;
		fieldStartPosZ = (int) fieldStart.transform.position.z;

		fieldEndPosX = (int) fieldEnd.transform.position.x;
		fieldEndPosZ = (int) fieldEnd.transform.position.z;

		//unitSizeX = (fieldEnd.transform.position.x - fieldStartPosX) / fieldSize;
		//unitSizeZ = (fieldEnd.transform.position.z - fieldStartPosZ) / fieldSize;
		//print (unitSizeX + " || " + unitSizeZ);


		tData = Terrain.activeTerrain.terrainData;
		
		alphaData = tData.GetAlphamaps(0, 0, tData.alphamapWidth, tData.alphamapHeight);

		//for (int i = fieldStartPosX; i < alphaData.GetLength(0)-fieldEndPosX; i++)
		//	for (int j = fieldStartPosZ; j < alphaData.GetLength(1)-fieldEndPosZ; j++) 
		for (int i = 205; i < 305; i++)
			for (int j = 205; j < 305; j++) 
		{
			
			
			alphaData[i,j,0] = 0;
			alphaData[i,j,1] = 1;
			alphaData[i,j,2] = 0;
			
		}	
		
		tData.SetAlphamaps(0, 0, alphaData);

		print (tData.alphamapHeight);
		print (tData.alphamapWidth);
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
