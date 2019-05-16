using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public int pathLength = 20;

    //public int columns = 2 * pathLength + 1;
    //public int rows = pathLength + 1;

    public int myX = 25;
    public int myY = 0;
    
    public GameObject chest;
    public GameObject goal;
    public GameObject start;
    public GameObject[] wallTiles;

    public int[,] board;

    private Transform boardHolder;

    // **BOARD MEANINGS**
    // 1 = left
    // 2 = up
    // 3 = right
    // 4 = start (up)
    // 5 = wall
    // 6 = goal 

    // **WALL OPENINGS**
    // 0 = UD
    // 1 = LU
    // 2 = DL
    // 3 = DR
    // 4 = RU
    // 5 = LR

    public void boardSetup()
    {
        boardHolder = new GameObject("NewBoard").transform;

        myX = pathLength / 2;
        myY = 0;

        board = new int[2 * pathLength + 1, pathLength];

        //outer walls
        //for(int x = 0; x <= columns - 1; x++)
        //{
        //    for(int y = 0; y <= rows - 1; y++)
        //    {
        //        if (x == 0 || x == columns - 1 || y == 0 || y == columns - 1)
        //        {
        //            board[x, y] = 5;
        //        }
        //    }
        //}

        //randomized path from starting point

        int prevX = myX;
        int prevY = myY;
        int curX = myX;
        int curY = myY + 1;
        
        board[prevX, prevY] = 2;

        for(int i = 0; i < pathLength - 1; i++)
        {
            int prevDir = board[prevX, prevY];
            int newDir = 0;

            if (prevDir == 2)
            {
                newDir = Random.Range(1, 4);
            }
            else if (prevDir == 1)
            {
                newDir = Random.Range(1, 3);
            }
            else if (prevDir == 3)
            {
                newDir = Random.Range(2, 4);
            }
            
            board[curX, curY] = newDir;

            int tileIndex = 0;

            prevX = curX;
            prevY = curY;

            if (prevDir == 2 && newDir == 2)
            {
                tileIndex = 0;
                curY++;
            }
            else if (prevDir == 3 && newDir == 2)
            {
                tileIndex = 1;
                //curX++;
                curY++;
            }
            else if (prevDir == 2 && newDir == 1)
            {
                tileIndex = 2;
                //curY++;
                curX--;
            }
            else if (prevDir == 2 && newDir == 3)
            {
                tileIndex = 3;
                //curY++;
                curX++;
            }
            else if (prevDir == 1 && newDir == 2)
            {
                tileIndex = 4;
                //curX--;
                curY++;
            }
            else if (prevDir == 1 && newDir == 1)
            {
                tileIndex = 5;
                curX--;
            }
            else if (prevDir == 3 && newDir == 3)
            {
                tileIndex = 5;
                curX++;
            }

            GameObject toInstantiate = wallTiles[tileIndex];
            GameObject instance = Instantiate(toInstantiate, new Vector3(2 * (prevX - myX), 2 * (prevY - myY), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder);
        }
        GameObject toInstantiateChest = chest;
        GameObject instanceChest = Instantiate(toInstantiateChest, new Vector3(2 * (curX - myX), 2 * (curY - myY), 0f), Quaternion.identity) as GameObject;
        instanceChest.transform.SetParent(boardHolder);
        board[curX, curY] = 6;

        GameObject toInstantiateGoal = goal;
        GameObject instanceGoal = Instantiate(toInstantiateGoal, new Vector3(2 * (curX - myX), 2 * (curY - myY) + 2, 0f), Quaternion.identity) as GameObject;
        instanceGoal.transform.SetParent(boardHolder);
    }
}
