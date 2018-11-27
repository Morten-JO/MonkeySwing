using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;
using UnityEngine;

class PathGenerator
{

	public System.Random generator { get; set; }

	public PathGenerator()
	{

	}
	public bool[,] GenerateTerrain(bool[,] currentState, int offsetX, int offsetY, bool initializingCall, int currentLength, int illegalDirectionY, int illegalDirectionX)
	{
		int xEdgeTop = 0;
		int iterationCount = 0;
		int xEdgeBottom = currentState.GetLength(0);
		int yEdgeLeft = 0;
		int yEdgeRight = currentState.GetLength(1);

		//Nice code 
		bool[,] newState = currentState;
		int newOffSetX = offsetX;
		int newOffSetY = offsetY;
		int currentLengthLocal = currentLength;
		int illegalDirectionYLocal = illegalDirectionY;
		int illegalDirectionXLocal = illegalDirectionX;
		bool initializingCallLocal = initializingCall;
		bool pathNotFound = true;
		while (pathNotFound) {
			//Console.Write(iterationCount);
			//Console.Write("-");
			newState[newOffSetX, newOffSetY] = true;

			if ((newOffSetX == xEdgeBottom - 1 || newOffSetX == xEdgeTop || newOffSetY == yEdgeLeft || newOffSetY == yEdgeRight - 1) && !initializingCallLocal)
			{
				pathNotFound = false;
				return currentState;
			}

			else
			{
				initializingCallLocal = false;
				int right = newOffSetY + 1;
				int down = newOffSetX + 1;
				int up = newOffSetX - 1;
				int left = newOffSetY - 1;

				bool legitDirection = false;
				int desiredLength = 13;
				int[] directionOptions = new int[] { 0, 1, 2, 3 };
				int count = 0;


				bool shouldExecuteFinalCommand = true;

				while (!legitDirection)
				{

					int direction = this.generator.Next(0, directionOptions.Length);


					switch (direction)
					{

					case 0:
						directionOptions = directionOptions.Where((val, idx) => idx != 0).ToArray();
						if (illegalDirectionYLocal == right)
						{
							break;
						}
						if (newState[newOffSetX, right] == true)
						{
							break;
						}
						if (right == yEdgeRight - 1 && currentLengthLocal + 1 < desiredLength)
						{
							break;
						}
						try
						{
							if (newState[newOffSetX - 1, newOffSetY + 1] || newState[newOffSetX + 1, newOffSetY + 1] || newState[newOffSetX, newOffSetY + 2])
							{
								break;
							}
						}
						catch (IndexOutOfRangeException e)
						{
							if (newState[newOffSetX - 1, newOffSetY + 1] || newState[newOffSetX + 1, newOffSetY + 1])
							{
								break;
							}
						}


						newOffSetY = right;
						legitDirection = true;
						break;

					case 1:
						directionOptions = directionOptions.Where((val, idx) => idx != 1).ToArray();
						if (illegalDirectionXLocal == down)
						{
							break;
						}
						if (newState[down, newOffSetY] == true)
						{
							break;
						}
						if (down >= xEdgeBottom - 1 && currentLengthLocal + 1 < desiredLength)
						{
							break;
						}


						try
						{
							if (newState[newOffSetX + 1, newOffSetY + 1] || newState[newOffSetX + 1, newOffSetY - 1] || newState[newOffSetX + 2, newOffSetY])
							{
								break;
							}
						}
						catch (IndexOutOfRangeException e)
						{
							if (newState[newOffSetX + 1, newOffSetY + 1] || newState[newOffSetX + 1, newOffSetY - 1])
							{
								break;
							}


						}



						newOffSetX = down;
						legitDirection = true;
						break;

					case 2:

						directionOptions = directionOptions.Where((val, idx) => idx != 2).ToArray();
						if (illegalDirectionYLocal == left)
						{
							break;
						}
						if (newState[newOffSetX, left] == true)
						{
							break;
						}
						if (left == yEdgeLeft && currentLengthLocal + 1 < desiredLength)
						{
							break;
						}


						try
						{
							if (newState[newOffSetX - 1, newOffSetY - 1] || newState[newOffSetX + 1, newOffSetY - 1] || newState[newOffSetX, newOffSetY - 2])
							{
								break;
							}

						}
						catch (IndexOutOfRangeException e)
						{
							if (newState[newOffSetX - 1, newOffSetY - 1] || newState[newOffSetX + 1, newOffSetY - 1])
							{
								break;
							}

						}


						newOffSetY = left;
						legitDirection = true;
						break;

					case 3:

						directionOptions = directionOptions.Where((val, idx) => idx != 3).ToArray();
						if (illegalDirectionXLocal == up)
						{
							break;
						}
						if (newState[up, newOffSetY] == true)
						{
							break;
						}
						if (up == xEdgeTop && currentLengthLocal + 1 < desiredLength)
						{
							break;
						}
						try
						{
							if (newState[newOffSetX - 1, newOffSetY - 1] || newState[newOffSetX - 1, newOffSetY + 1] || newState[newOffSetX - 2, newOffSetY])
							{
								break;
							}

						}
						catch (IndexOutOfRangeException e)
						{
							if (newState[newOffSetX - 1, newOffSetY - 1] || newState[newOffSetX - 1, newOffSetY + 1])
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


					//Didnt find legit direction, must go back
					if (count > 4)
					{
						shouldExecuteFinalCommand = false;

						//Need a "DONT GO THIS WAY" param
						newState[newOffSetX, newOffSetY] = false;
						if (newState[newOffSetX - 1, newOffSetY] == true)
						{
							// return GenerateTerrain(newState, newOffSetX - 1, newOffSetY, false, currentLength - 1, offsetX, 0);
							illegalDirectionXLocal = newOffSetX;
							illegalDirectionYLocal = 0;
							currentLengthLocal--;
							newOffSetX = up;
							break;
						}
						else if (newState[newOffSetX, right] == true)
						{
							//return GenerateTerrain(newState, newOffSetX, newOffSetY + 1, false, currentLength - 1, 0, offsetY);
							illegalDirectionXLocal = 0;
							illegalDirectionYLocal = newOffSetY;
							currentLengthLocal--;
							newOffSetY = right;
							break;
						}
						else if (newState[newOffSetX, left] == true)
						{
							// return GenerateTerrain(newState, newOffSetX, newOffSetY - 1, false, currentLength - 1, 0, offsetY);
							illegalDirectionXLocal = 0;
							illegalDirectionYLocal = newOffSetY;
							currentLengthLocal--;
							newOffSetY = left;
							break;

						}
						else if (newState[down, newOffSetY] == true)
						{
							//return GenerateTerrain(newState, down, newOffSetY, false, currentLength - 1, offsetX, 0);
							illegalDirectionXLocal = newOffSetX;
							illegalDirectionYLocal = 0;
							currentLengthLocal--;
							newOffSetX = down;
							break;
						}

						legitDirection = true;
					}


				}
				iterationCount++;
				//   return GenerateTerrain(newState, newOffSetX, newOffSetY, false, currentLength + 1, 0, 0);

				if (shouldExecuteFinalCommand)
				{
					illegalDirectionXLocal = 0;
					illegalDirectionYLocal = 0;
					currentLengthLocal++;
				}

			}

		}
		return currentState;
	}

	public bool[,] fillCircular(bool[,] map, int r){
		
		int length = map.GetLength (0);
		bool[,] newMap = new bool[length,length];
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < length; j++){
				if (map [i, j]) {
					int newRadius = UnityEngine.Random.Range (r - 2, r + 2);
					int x = newRadius;
					int y = -newRadius+1;
					for (int k = y; k < x; k++) {
						if (i + k < length && i + k >= 0 && j + k < length && j+k >= 0) {
							newMap [i + k, j + k] = true;
						}
					}
				}


				/*
				int radiusError = 1 - x;
				try{
					while (x >= y)
					{
						int startX = -x + i;
						int endX = x + i;
						for(int q=startX;q<endX+1;q++){
							if ((y + j) < length && q < length) {
								map[y+j,q]=true;
							}
						}
						if (y != 0)
							for(int q=startX;q<endX+1;q++){
								if ((-y + j) < length && q < length) {
									map [-y + j, q] = true;
								}
							}

						y++;
						//if(false)
						if (radiusError<0)
						{
							radiusError += 2 * y + 1;
						} 
						else 
						{
							if (x >= y)
							{
								startX = -y + 1 + i;
								endX = y - 1 + j;
								for(int q=startX;q<endX+1;q++){
									if ((x + j) < length && q < length) {
										if(!map[x+j,q])map[x+j,q]=true;
									}

								}
								for(int q=startX;q<endX+1;q++){
									if ((-x + j) < length && q < length) {
										if(!map[-x+j,q])map[-x+j,q]=true;
									}

								}
							}
							x--;
							radiusError += 2 * (y - x + 1);
						}

					}
				} catch(Exception e){

				}*/
			}
		}
	    

		return newMap;

	}
}