using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MazeBuilder : MonoBehaviour
{
    public GameObject FloorTileObject;
    public static int width;
    public static int depth;
    private static bool[,,] wallStateArray;
    private bool isInitialized = false;
    private static bool[,,] initialWallStateArray;
    public static bool[,,] WallStateArray
    {
        get => wallStateArray;
        set
        {
            wallStateArray = value;
            //updateWallsUsingNewState();
        }
    }

    
    public void loadInitialGrid()
    {
        isInitialized = true;
        if (FloorTileObject == null)
        {
            FloorTileObject = Resources.Load("Prefabs/FloorTile") as GameObject;
        }
        initialWallStateArray = new bool[width,depth,4];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                GameObject mostRecent = Instantiate(FloorTileObject, new Vector3(i * 5, 0, j * 5), Quaternion.identity);
                mostRecent.name = "grid_" + i + "_" + j;
                initialWallStateArray[i, j, 0] = true;
                initialWallStateArray[i, j, 1] = true;
                initialWallStateArray[i, j, 2] = true;
                initialWallStateArray[i, j, 3] = true;
                setInitialWallsAtCoordinates(i, j);
            }
        }
    }
    public void updateWallsUsingNewState(bool[,,] _wallStateArray)
    {
        WallStateArray = _wallStateArray;
        logState();
        //If No Tiles Set yet, then start with loadInitialGrid()
        if (!isInitialized)
        {
            loadInitialGrid();
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                setWallsAtCoordinates(i, j);
            }
        }
        
    }
    void setInitialWallsAtCoordinates(int x, int y)
    {
        GameObject currentNode = GameObject.Find("grid_" + x + "_" + y);

        for (int selection = 0; selection < 4; selection++)
        {
            currentNode.transform.Find("Cube_" + (selection + 1)).GetComponent<Renderer>().enabled = initialWallStateArray[x, y, selection];
            currentNode.transform.Find("Cube_" + (selection + 1)).GetComponent<BoxCollider>().enabled = initialWallStateArray[x, y, selection];
        }
    }
    void setWallsAtCoordinates(int x, int y)
    {
        GameObject currentNode = GameObject.Find("grid_" + x + "_" + y);

        for (int selection = 0; selection < 4; selection++)
        {
            currentNode.transform.Find("Cube_" + (selection + 1)).GetComponent<Renderer>().enabled = wallStateArray[x, y, selection];
            currentNode.transform.Find("Cube_" + (selection + 1)).GetComponent<BoxCollider>().enabled = wallStateArray[x, y, selection];
        }
    }
    void logState()
    {
        string logState = "";
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    //logState += hasNeighborBeenVisited[i, j];
                    logState += wallStateArray[i, j, k];
                    logState += " ";
                }
            }
            logState += System.Environment.NewLine;
        }
        Debug.Log(logState);
    }
}

