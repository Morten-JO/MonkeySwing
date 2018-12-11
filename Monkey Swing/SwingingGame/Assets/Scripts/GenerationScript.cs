using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GenerationScript: MonoBehaviour {

	public GameObject canvas;
	public GameObject cameraLook;
	public Terrain terrain;
	public Text procentText;
	public Camera camera;
	public GameObject swingingLog;
	public GameObject player;
	public GameObject startCube;
	public GameObject finishGoal;
	public GameObject playerCanvas;
	public GameObject banana;


	//scoreboard
	public GameObject videoPlayer;
	public GameObject winScreenCanvas;
	public Camera scoreBoardCamera;


	private bool pathGenerated = false;
	private bool firstCall = false;
	IEnumerator pathGeneration(){
		int depth = 20;

		int width = 256;
		int height = 256;

		bool[,] grid = new bool[8, 8];
		PathGenerator instance = new PathGenerator();
		instance.generator = new System.Random();


		int startingPoint = instance.generator.Next(1, 7);
		grid[0, startingPoint] = true;
		bool[,] path = instance.GenerateTerrain(grid, 1, startingPoint, true, 1, 0, 0);


		bool[,] reformedArray = new bool[256, 256];
		int scale = reformedArray.GetLength (0) / grid.GetLength (0);
		for (int i = 0; i < path.GetLength (0); i++) {
			for (int j = 0; j < path.GetLength (1); j++) {
				if (path [i, j]) {
					for (int x = 0; x < scale; x++) {
						for (int z = 0; z < scale; z++) {
							reformedArray [i * scale + x, j * scale + z] = true;
						}
					}
				}
			}
		}
		reformedArray = instance.fillCircular (reformedArray, 7);

		float scaleFloater = 30f;
		TerrainData td = terrain.terrainData;
		td.heightmapResolution = width + 1;
		td.size = new Vector3 (width, depth, height);
		float[,] heights = new float[width, height];
		int numberParsed = 0;
		int totalNumberToParse = width * height;
		float[, ,] terrainMapData = new float[td.alphamapWidth, td.alphamapHeight, td.alphamapLayers];
		for (int x = 0; x < reformedArray.GetLength(0); x++) { 
			for (int p = 0; p < 2; p++) {
				for (int y = 0; y < reformedArray.GetLength(1); y++) {
					if (p == 0) {
						numberParsed++;
						if (!reformedArray [x,y]) {
							float x_num = (float)x / (float)width * scaleFloater;
							float y_num = (float)y / (float)height * scaleFloater;
							heights [x, y] = Mathf.PerlinNoise (x_num, y_num);
						}
						float procentNumber = (((float)numberParsed) / ((float)totalNumberToParse)) * 100f;
						procentText.text = procentNumber.ToString ("n2") + "%";
					}
					for (int l = 0; l < 2; l++) { // 2

						float[] weights = new float[td.alphamapLayers+1];

						if (reformedArray [x, y]) {
							weights [0] = 0f;
							weights [1] = 1f;
						} else {
							weights [0] = 1f;
							weights [1] = 0f;
						}
						float z = weights.Sum();
						for (int k = 0; k < td.alphamapLayers; k++) {
							weights [k] /= z;
							terrainMapData [(x*2+p), (y*2+l), k] = weights [k];
						}
					}

				}
			}
			td.SetAlphamaps (0, 0, terrainMapData);
			td.SetHeights (0, 0, heights);
			terrain.terrainData = td;
			yield return null;
		}
		generateSwingingLogs (terrain, path, startingPoint);
		generateBananas (terrain, path, startingPoint);
		generateMapObjects (path, startingPoint);
		pathGenerated = true;

	}

	private void generateMapObjects(bool[,] map, int startPoint){
		float endPointX = 0f;
		float endPointY = 0f;
		float scale = 256 / map.GetLength (0);
		for (int i = 0; i < map.GetLength (0); i++) {
			for (int j = 0; j < map.GetLength (1); j++) {
				if (map [i, j]) {
					if (j == map.GetLength (0) - 1 ||
						i == map.GetLength (0) - 1 ||
						j == 0 && i != startPoint) {
						endPointX = i;
						endPointY = j;
						UnityEngine.Debug.Log ("Found end: (" + endPointX + "," + endPointY + ")");
					}
				}
			}
		}
		GameObject genStartCube = Instantiate (startCube) as GameObject;
		float startCubeScaledSize = genStartCube.GetComponent<Renderer> ().bounds.size.x *3f;
		genStartCube.transform.position = new Vector3 (startPoint* scale  + startCubeScaledSize, 10f, 0f + startCubeScaledSize);

		GameObject generatedPlayer = Instantiate (player) as GameObject;
		generatedPlayer.transform.position = new Vector3 (genStartCube.transform.position.x, genStartCube.transform.position.y + 2f, genStartCube.transform.position.z);

		GameObject canvasObject = Instantiate (playerCanvas) as GameObject;
		generatedPlayer.GetComponent<RopeSwingScript> ().slider = canvasObject.GetComponentInChildren<Slider> ();
		PlayerScore score = generatedPlayer.GetComponent<PlayerScore> ();
		score.ropeText = canvasObject.transform.GetChild (1).GetComponent<Text>();
		score.resetText = canvasObject.transform.GetChild (2).GetComponent<Text>();
		score.bananaText = canvasObject.transform.GetChild (3).GetComponent<Text>();
		generatedPlayer.GetComponent<RopeSwingScript> ().image = canvasObject.transform.GetChild (0).gameObject;


		GameObject finishCube = Instantiate (finishGoal) as GameObject;
		float size = finishCube.GetComponent<Renderer> ().bounds.size.x;
		finishCube.transform.position = new Vector3 (endPointY * scale + size / 1.72f, 2f, endPointX * scale + size / 1.72f);
		finishCube.GetComponent<FinishGoal> ().player = generatedPlayer;
		finishCube.GetComponent<FinishGoal> ().videoPlayer = videoPlayer;
		finishCube.GetComponent<FinishGoal> ().scoreBoardCamera = scoreBoardCamera;
		finishCube.GetComponent<FinishGoal> ().scoreBoard = winScreenCanvas;
		finishCube.GetComponent<FinishGoal> ().canvas = canvasObject;
		finishCube.GetComponent<FinishGoal> ().isGeneration = true;

	}

	private void generateSwingingLogs(Terrain terrain, bool [,] map, int startPoint){
		GameObject sizer = Instantiate (swingingLog) as GameObject;
		float size = sizer.GetComponent<Renderer> ().bounds.size.x;
		Destroy (sizer);
		float terrainWidth = 256;
		float terrainHeight = 256;
		float scale = terrainWidth / map.GetLength (0);
		for (int i = 0; i < map.GetLength (0); i++) {
			for (int j = 0; j < map.GetLength (1); j++) {
				if (startPoint == j && i == 0) { 

				} else {
					if (map [i, j]) {
						int indexerValues = i + j;
						if (indexerValues % UIControlScripts.randomMapGenerationDifficulty == 0) {
							GameObject log = Instantiate (swingingLog) as GameObject;
							log.transform.position = new Vector3 (j * scale + size * 0.45f, 30f, i * scale + size * 0.45f);
							int randomVal = UnityEngine.Random.Range (0, 180);
							log.transform.Rotate (90f, 0f, randomVal); 
						}
					}
				}
			}
		}
	}

	private void generateBananas(Terrain terrain, bool[,] map, int startPoint){
		float terrainWidth = 256;
		float terrainHeight = 256;
		float scale = terrainWidth / map.GetLength (0);
		for (int i = 0; i < map.GetLength (0); i++) {
			for (int j = 0; j < map.GetLength (0); j++) {
				if (startPoint == j && i == 0) {

				} else {
					if (map [i, j]) {
						int indexerValues = i + j;
						if (indexerValues % UIControlScripts.randomMapGenerationDifficulty == 0) {
							GameObject bananaObj = Instantiate (banana) as GameObject;
							int randomVal = UnityEngine.Random.Range (0, 11);
							bananaObj.transform.position = new Vector3 (j * scale+16f, 15f, i * scale+randomVal);
						}
					}
				}
			}
		}
	}



	public void paintGeneration(){
		TerrainData td = terrain.terrainData;
		float[, ,] terrainMapData = new float[td.alphamapWidth, td.alphamapHeight, td.alphamapLayers];
		for (int i = 0; i < td.alphamapHeight; i++) {
			for (int j = 0; j < td.alphamapWidth; j++) {
				float normalized_y = (float)i / (float)td.alphamapHeight;
					float normalized_x = (float)j / (float)td.alphamapWidth;

					float td_height = td.GetHeight (Mathf.RoundToInt (normalized_y * td.heightmapHeight), Mathf.RoundToInt (normalized_x * td.heightmapWidth));

					Vector3 normal = td.GetInterpolatedNormal (normalized_y, normalized_x);
					float sn = td.GetSteepness (normalized_x, normalized_y);
					float[] weights = new float[td.alphamapLayers+1];
					weights [0] = 1f;
					weights [1] = 0f;//Mathf.Clamp01 ((td.heightmapHeight - td_height));
					//weights [2] = 1.0f - td_height;//1.0f - Mathf.Clamp01 (sn * sn / (td.heightmapHeight / 5.0f));
					//weights [3] = td_height;
					float z = weights.Sum();
					for (int k = 0; k < td.alphamapLayers; k++) {
						//weights [k] /= z;
						terrainMapData [i, j, k] = weights [k];
					}

			}
		}
		td.SetAlphamaps (0, 0, terrainMapData);
		terrain.terrainData = td;
	}


	// Use this for initialization
	void Start () {
		
		StartCoroutine("pathGeneration");
	}

	float time = 0f;

	// Update is called once per frame
	void Update () {
		if (!pathGenerated) {
			camera.transform.LookAt (cameraLook.transform);
			time += Time.deltaTime;
			float x = 130f + Mathf.Cos (time) * 50f;
			float y = 250f;
			float z = 130f + Mathf.Sin (time) * 50f;
			camera.transform.position = new Vector3 (x, y, z);
		} else {
			if (!firstCall) {
				canvas.SetActive (false);
				firstCall = true;
			}

		}
	}
}
