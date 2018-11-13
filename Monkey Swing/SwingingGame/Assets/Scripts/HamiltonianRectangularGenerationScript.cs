
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HamiltonianRectangularGenerationScript: MonoBehaviour {

	public Terrain terrain;

	public bool[,] generateTerrain(bool[,] currentState, int offsetX, int offsetY, bool initializingCall) {
		int xEdgeLeft = 0;
		int xEdgeRight = currentState.Length;
		int yEdgeBottom = currentState.Length;
		int yEdgeTop = 0;

		//Nice code
		bool[,] newState = currentState;
		int newOffSetX = offsetX;
		int newOffSetY = offsetY;
		newState[newOffSetX, newOffSetY] = true;


		if((offsetX == xEdgeLeft || offsetX == 0 || offsetY == yEdgeBottom || offsetY == 0) && !initializingCall) {
			for(int i = 0; i<xEdgeRight; i++){
				for(int j = 0; j < yEdgeBottom; j++){
					//
				}
			}
			return currentState;
		}
			newOffSetX += 1;


			return generateTerrain(newState, newOffSetX, newOffSetY, false);
		}


	public void pathGenerationV2(){
		int depth = 20;

		int width = 256;
		int height = 256;

		int requiredLengthBois = 250;

		bool[,]path = new bool[width, height];

		int startSide = Random.Range (0, 4);
		int xStart, yStart;
		int[] directions = new int[]{ 1, 1 };
		int rand = Random.Range (30, 220);
		if (startSide == 0) {
			yStart = 0;
			xStart = rand;
			for (int i = yStart; i < height; i++) {
				for (int x = -20; x < 21; x++) {
					path [xStart+x,i] = true;
				}
			}
		} else if (startSide == 1) {
			yStart = 255;
			xStart = rand;
			for (int i = yStart; i > -1; i--) {
				for (int x = -20; x < 21; x++) {
					path [xStart+x,i] = true;
				}
			}
		} else if (startSide == 2) {
			yStart = rand;
			xStart = 0;
			for (int i = 0; i < width; i++) {
				for (int x = -20; x < 21; x++) {
					path [i,yStart+x] = true;
				}
			}
		} else {
			yStart = rand;
			xStart = 255;
			for (int i = xStart; i > -1; i--) {
				for (int x = -20; x < 21; x++) {
					path [i,yStart+x] = true;
				}
			}
		}



		float scale = 40f;
		TerrainData td = terrain.terrainData;
		td.heightmapResolution = width + 1;
		td.size = new Vector3 (width, depth, height);
		float[,] heights = new float[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (path [x,y]) {
					print ("BONE");
				} else {
					float x_num = (float)x / (float)width * scale;
					float y_num = (float)y / (float)width * scale;
					heights [x, y] = Mathf.PerlinNoise (x_num, y_num);
				}

			}
		}
		td.SetHeights (0, 0, heights);
		terrain.terrainData = td;
	}


	public void pathGeneration(){
		TerrainData td = terrain.terrainData;
		float[, ,] terrainMapData = new float[td.alphamapWidth, td.alphamapHeight, td.alphamapLayers];
		for (int i = 0; i < td.alphamapHeight; i++) {
			for (int j = 0; j < td.alphamapWidth; j++) {
				float normalized_y = (float)i / (float)td.alphamapHeight;
				float normalized_x = (float)j / (float)td.alphamapWidth;

				float height = td.GetHeight (Mathf.RoundToInt (normalized_y * td.heightmapHeight), Mathf.RoundToInt (normalized_x * td.heightmapWidth));

				Vector3 normal = td.GetInterpolatedNormal (normalized_y, normalized_x);
				float sn = td.GetSteepness (normalized_x, normalized_y);
				float[] weights = new float[td.alphamapLayers+1];
				weights [0] = 0.5f;
				weights [1] = Mathf.Clamp01 ((td.heightmapHeight - height));
				weights [2] = 1.0f - Mathf.Clamp01 (sn * sn / (td.heightmapHeight / 5.0f));
				weights [3] = height * Mathf.Clamp01 (normal.z);
				float z = weights.Sum();
				for (int k = 0; k < td.alphamapLayers; k++) {
					weights [k] /= z;
					terrainMapData [i, j, k] = weights [k];
				}

			}
		}
		td.SetAlphamaps (0, 0, terrainMapData);
		terrain.terrainData = td;
	}


	// Use this for initialization
	void Start () {
		//pathGenerationV2 ();
	}

	// Update is called once per frame
	void Update () {

	}
}
