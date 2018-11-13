using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

class PathGenerator
{

	public System.Random generator { get; set; }

	public PathGenerator()
        {

        }
        public bool[,] GenerateTerrain(bool[,] currentState, int offsetX, int offsetY, bool initializingCall, int currentLength, int illegalDirectionY, int illegalDirectionX)
        {
           
            int xEdgeTop = 0;
   
            int xEdgeBottom = currentState.GetLength(0);
            int yEdgeLeft = 0;
            int yEdgeRight = currentState.GetLength(1);

            //Nice code 
            bool[,] newState = currentState;
            int newOffSetX = offsetX;
            int newOffSetY = offsetY;



            newState[newOffSetX, newOffSetY] = true;

            if ((offsetX == xEdgeBottom - 1 || offsetX == xEdgeTop || offsetY == yEdgeLeft || offsetY == yEdgeRight - 1) && !initializingCall)
            {
                return currentState;
            }

            else
            {
                int right = newOffSetY + 1;
                int down = newOffSetX + 1;
                int up = newOffSetX - 1;
                int left = newOffSetY - 1;

                bool legitDirection = false;
                int desiredLength = 15;
                int[] directionOptions = new int[] { 0, 1, 2, 3 };
                int count = 0;



                while (!legitDirection)
                {

                    int direction = this.generator.Next(0, directionOptions.Length);


                    switch (direction)
                    {

                        case 0:
                            directionOptions = directionOptions.Where((val, idx) => idx != 0).ToArray();
                            if(illegalDirectionY == right)
                            {
                                break;
                            }
                            if (currentState[offsetX, right] == true)
                            {
                                break;
                            }
                            if (right == yEdgeRight - 1 && currentLength + 1 < desiredLength)
                            {
                                break;
                            }
                            try
                            {
                                if (currentState[offsetX - 1, newOffSetY + 1] || currentState[offsetX + 1, newOffSetY + 1] || currentState[offsetX, newOffSetY + 2])
                                {
                                    break;
                                }
                            }
                            catch(IndexOutOfRangeException e)
                            {
                                if (currentState[offsetX - 1, newOffSetY + 1] || currentState[offsetX + 1, newOffSetY + 1])
                                {
                                    break;
                                }
                            }

                            newOffSetY = right;
                            legitDirection = true;
                            break;

                        case 1:
                            directionOptions = directionOptions.Where((val, idx) => idx != 1).ToArray();
                            if (illegalDirectionX == down)
                            {
                                break;
                            }
                            if (currentState[down, offsetY] == true)
                            {
                                break;
                            }
                            if (down >= xEdgeBottom - 1 && currentLength + 1 < desiredLength)
                            {
                                break;
                            }
                          

                            try
                            {
                                if (currentState[offsetX + 1, newOffSetY + 1] || currentState[offsetX + 1, newOffSetY - 1] || currentState[offsetX + 2, newOffSetY])
                                {
                                    break;
                                }


                            }
                            catch (IndexOutOfRangeException e)
                            {
                                if (currentState[offsetX + 1, newOffSetY + 1] || currentState[offsetX + 1, newOffSetY - 1])
                                {
                                    break;
                                }

                            }

                            newOffSetX = down;
                            legitDirection = true;
                            break;

                        case 2:

                            directionOptions = directionOptions.Where((val, idx) => idx != 2).ToArray();
                            if (illegalDirectionY == left)
                            {
                                break;
                            }
                            if (currentState[offsetX, left] == true)
                            {
                                break;
                            }
                            if (left == yEdgeLeft && currentLength + 1 < desiredLength)
                            {
                                break;
                            }
                          

                            try
                            {
                                if (currentState[offsetX - 1, newOffSetY - 1] || currentState[offsetX + 1, newOffSetY - 1] || currentState[offsetX, newOffSetY - 2])
                                {
                                    break;
                                }

                            }
                            catch (IndexOutOfRangeException e)
                            {
                                if (currentState[offsetX - 1, newOffSetY - 1] || currentState[offsetX + 1, newOffSetY - 1])
                                {
                                    break;
                                }

                            }

                         
                            newOffSetY = left;
                            legitDirection = true;
                            break;

                        case 3:

                            directionOptions = directionOptions.Where((val, idx) => idx != 3).ToArray();
                            if (illegalDirectionX == up)
                            {
                                break;
                            }
                            if (currentState[up, newOffSetY] == true)
                            {
                                break;
                            }
                            if (up == xEdgeTop && currentLength + 1 < desiredLength)
                            {
                                break;
                            }                     
                            try
                            {
                                if (currentState[offsetX - 1, newOffSetY - 1] || currentState[offsetX - 1, newOffSetY + 1] || currentState[offsetX - 2, newOffSetY])
                                {
                                    break;
                                }

                            } 
                            catch (IndexOutOfRangeException e)
                            {
                                if (currentState[offsetX - 1, newOffSetY - 1] || currentState[offsetX - 1, newOffSetY + 1])
                                {
                                    break;
                                }

                            }

                            newOffSetX = up;
                            legitDirection = true;
                            break;

                        default:
                            break;

                    }
                    count++;

                    if (count > 4)
                    {

                        //Need a "DONT GO THIS WAY" param
                        newState[offsetX, offsetY] = false;
                        if (currentState[newOffSetX - 1, newOffSetY] == true)
                        {
                            return GenerateTerrain(newState, newOffSetX - 1, newOffSetY, false, currentLength - 1, offsetX, 0);
                        }
                        else if (currentState[newOffSetX, right] == true)
                        {
                            return GenerateTerrain(newState, newOffSetX, newOffSetY + 1, false, currentLength - 1, 0, offsetY);

                        }
                        else if (currentState[newOffSetX, left] == true)
                        {
                            return GenerateTerrain(newState, newOffSetX, newOffSetY - 1, false, currentLength - 1, 0, offsetY);

                        }
                        else if (currentState[down, newOffSetY] == true)
                        {
                            return GenerateTerrain(newState, down, newOffSetY, false, currentLength - 1, offsetX, 0);
                        }
						
                    }
                }

                return GenerateTerrain(newState, newOffSetX, newOffSetY, false, currentLength + 1, 0, 0 );
            }

        }
}

public class GenerationScript: MonoBehaviour {

	public GameObject canvas;
	public GameObject cameraLook;
	public Terrain terrain;
	public Text procentText;
	public Camera camera;
	private bool pathGenerated = false;
	private bool firstCall = false;
	IEnumerator pathGeneration(){
		int depth = 20;

		int width = 256;
		int height = 256;

		bool[,] grid = new bool[10, 10];
		PathGenerator instance = new PathGenerator();
		instance.generator = new System.Random();


		int startingPoint = instance.generator.Next(1, 9);
		grid[0, startingPoint] = true;
		bool[,] path = instance.GenerateTerrain(grid, 1, startingPoint, true, 1, 0, 0);


		bool[,] reformedArray = new bool[256, 256];
		int scale = reformedArray.GetLength (0) / grid.GetLength (0);
		for (int i = 0; i < path.GetLength (0); i++) {
			for (int j = 0; j < path.GetLength (1); j++) {
				if (path [i, j] == true) {
					for (int x = 0; x < scale; x++) {
						for (int z = 0; z < scale; z++) {
							reformedArray [i * scale + x, j * scale + z] = true;
						}
					}
				}
			}
		}

		float scaleFloater = 30f;
		TerrainData td = terrain.terrainData;
		td.heightmapResolution = width + 1;
		td.size = new Vector3 (width, depth, height);
		float[,] heights = new float[width, height];
		int numberParsed = 0;
		int totalNumberToParse = width * height;
		float[, ,] terrainMapData = new float[td.alphamapWidth, td.alphamapHeight, td.alphamapLayers];
		UnityEngine.Debug.Log ("Width: " + td.alphamapWidth);
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
		pathGenerated = true;

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
