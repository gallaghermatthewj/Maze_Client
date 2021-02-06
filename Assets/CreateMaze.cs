using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMaze : MonoBehaviour
{
    
    public GameObject FloorTile;
    public GameObject playerObject;
    public GameObject rewardObject;
    public GameObject pillObject;
    public GameObject buttonObject;

    private int width;
    private int depth;
    public int stopLimit;
    bool[,] hasNeighborBeenVisited;
    int stopper;
    bool[,,] wallStateArray;
    int[] correspondingWallIndex = new int[] { 2, 3, 0, 1 };
    private int ranOnce;
    private IEnumerator coroutine;

    public int Width { get => width; set => width = value; }
    public int Depth { get => depth; set => depth = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeRoom(int _width, int _depth)
    {
        Width = _width;
        Depth = _depth;
        //transform.GetComponent<AudioSource>()
        hasNeighborBeenVisited = new bool[Width, Depth];
        wallStateArray = new bool[Width, Depth, 4];


        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                GameObject mostRecent = Instantiate(FloorTile, new Vector3(i * 5, 0, j * 5), Quaternion.identity);
                mostRecent.name = "grid_" + i + "_" + j;
                wallStateArray[i, j, 0] = true;
                wallStateArray[i, j, 1] = true;
                wallStateArray[i, j, 2] = true;
                wallStateArray[i, j, 3] = true;
            }
        }
    }
    void checkValueOfMazePacketization()
    {
        byte[] packetizedArray = CreateWalls.ToBytes<bool>(wallStateArray);
        Debug.Log("Stop Here");
        byte[] temp;
        temp = null;
        bool[,,] tempWallStateArray = new bool[Width, Depth, 4];
        bool[,,] tempB = new bool[Width, Depth, 4];
        CreateWalls.FromBytes<bool>(tempB, packetizedArray);

        bool checkVal = true;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (tempB[i, j, k] != wallStateArray[i, j, k])
                    {
                        checkVal = false;
                    }
                }
            }
        }
        if (checkVal)
        {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  Debug.Log("It worked!");
        }
        else
        {
            Debug.Log("Dang it!");
        }
    }

    void recursiveStep(Vector2 currentNodeCoordinates)
    {
        //MarkCurrentNodeAsVisited
        int x = (int)currentNodeCoordinates.x;
        int y = (int)currentNodeCoordinates.y;
        hasNeighborBeenVisited[x, y] = true;
        Debug.Log("Current: " + x + " " + y);

        //Needs to be loopy.
        tryAgain:
        //GetNeighborsForCurrentNode
        int selection = Random.Range(0, 4);
        Vector2[] neighborNodes = getNeighborCoordinates(currentNodeCoordinates);
        bool allNeighborsVisited = true;
        for (int i = 0; i < neighborNodes.Length; i++)
        {
            if (neighborNodes[i].x != Vector2.negativeInfinity.x)
            {
                if (!hasNeighborBeenVisited[(int)neighborNodes[i].x, (int)neighborNodes[i].y])
                {
                    allNeighborsVisited = false;
                }
            }
        }


        //If All neighbors visited, end
        if (allNeighborsVisited)// || stopper >= stopLimit
        {
            //All Done for this Node!
        }
        else
        {
            //still need to visit someone
            //Else, Pick Random neighbor until it is among unvisited options. 
            
            Debug.Log("Current: " + x + " " + y + ", Selection: " + selection);
            if (neighborNodes[selection].x != Vector2.negativeInfinity.x)
            {
                //Node does exist.
                if (hasNeighborBeenVisited[(int)neighborNodes[selection].x, (int)neighborNodes[selection].y])
                {
                    //Neighbor exists, but it's been visited. Try again.
                    goto tryAgain;
                }
                else
                {
                    //Good! Now, visit that neighbor.
                    Debug.Log("Visiting " + neighborNodes[selection].x + " " + neighborNodes[selection].y + " from " + currentNodeCoordinates.x + " " + currentNodeCoordinates.y);

                    //Clear wall on current node between current node and that neighbor
                    wallStateArray[(int)currentNodeCoordinates.x, (int)currentNodeCoordinates.y, selection] = false;

                    //Clear that neighbor's corresponding wall
                    wallStateArray[(int)neighborNodes[selection].x, (int)neighborNodes[selection].y, correspondingWallIndex[selection]] = false;

                    //Ok, now that neighbor is the new current node. Recurse.
                    recursiveStep(neighborNodes[selection]);
                    //Make sure everybody's visited?
                    goto tryAgain;
                }
            }
            else
            {
                //Node does not exist. Try again.
                goto tryAgain;
            }
        }
    }
    void logState()
    {
        string logState = "";
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
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

    Vector2[] getNeighborCoordinates(Vector2 currentNodeCoordinates)
    {
        Vector2[] neighborNodes = new Vector2[4];
        //East Neighbor
        if (currentNodeCoordinates.x < Width - 1) { neighborNodes[0] = new Vector2(currentNodeCoordinates.x + 1,currentNodeCoordinates.y); } else { neighborNodes[0] = Vector2.negativeInfinity; }
        //North Neighbor
        if (currentNodeCoordinates.y < Depth - 1) { neighborNodes[1] = new Vector2(currentNodeCoordinates.x, currentNodeCoordinates.y + 1); } else { neighborNodes[1] = Vector2.negativeInfinity; }
        //West Neighbor
        if (currentNodeCoordinates.x > 0) { neighborNodes[2] = new Vector2(currentNodeCoordinates.x - 1, currentNodeCoordinates.y); } else { neighborNodes[2] = Vector2.negativeInfinity; }
        //South Neighbor
        if (currentNodeCoordinates.y > 0) { neighborNodes[3] = new Vector2(currentNodeCoordinates.x, currentNodeCoordinates.y - 1); } else { neighborNodes[3] = Vector2.negativeInfinity; }
        return neighborNodes;
    }

    int knockDownWall(int wallIndex, int wallState)
    {
        //wallState |= (1 << wallIndex);
        wallState &= ~(1 << wallIndex);
        return wallState;
    }
    byte getWallStateAsByte(bool eastWall, bool northWall, bool westWall, bool southWall)
    {
        byte result = 0;
        bool[] wallDef = new bool[]
        {
            eastWall,northWall,westWall,southWall
        };

        int index = 4 - wallDef.Length;

        // Loop through the array
        foreach (bool b in wallDef)
        {
            // if the element is 'true' set the bit at that position
            if (b)
                result |= (byte)(1 << (7 - index));

            index++;
        }
        return result;
    }
    bool[] getWallStateAsBoolArray(byte wallState)
    {
            // prepare the return result
            bool[] result = new bool[3];

            // check each bit in the byte. if 1 set to true, if 0 set to false
            for (int i = 0; i < 3; i++)
                result[i] = (wallState & (1 << i)) == 0 ? false : true;

            // reverse the array
            //System.Array.Reverse(result);

            return result;
    }
    void setWallsAtCoordinates(int x, int y)
    {
        GameObject currentNode = GameObject.Find("grid_" + x + "_" + y);

        for (int selection = 0; selection < 4; selection++)
        {
            currentNode.transform.Find("Cube_" + (selection + 1)).GetComponent<Renderer>().enabled = wallStateArray[x,y,selection];
            currentNode.transform.Find("Cube_" + (selection + 1)).GetComponent<BoxCollider>().enabled = wallStateArray[x, y, selection];
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(ranOnce<=0)
        {
            ranOnce=1;
            coroutine = generateMaze(true,true,true);
            StartCoroutine(coroutine);
        }
    }
    public void placeCheese()
    {
        int newX = Random.Range(0, Width);
        int newY = Random.Range(0, Depth);
        GameObject rewardInstance = Instantiate(rewardObject, new Vector3(newX * 5, 2, newY * 5), Quaternion.identity);
    }

    public void placePill()
    {
        int pillX = Random.Range(0, Width);
        int pillY = Random.Range(0, Depth);
        GameObject pillInstance = Instantiate(pillObject, new Vector3(pillX * 5, 2, pillY * 5), Quaternion.identity);
    }

    public void placeButton()
    {
        int buttonX = Random.Range(0, Width);
        int buttonY = Random.Range(0, Depth);
        GameObject pillInstance = Instantiate(buttonObject, new Vector3(buttonX * 5, 2, buttonY * 5), Quaternion.identity);
    }

    public void refreshWalls()
    {
        /*width++;
        depth++;
        //right Up
        for (int i = width-1; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                GameObject mostRecent = Instantiate(FloorTile, new Vector3(i * 5, 0, j * 5), Quaternion.identity);
                mostRecent.name = "grid_" + i + "_" + j;
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = depth - 2; j < depth; j++)
            {
                GameObject mostRecent = Instantiate(FloorTile, new Vector3(i * 5, 0, j * 5), Quaternion.identity);
                mostRecent.name = "grid_" + i + "_" + j;
            }
        }*/

        hasNeighborBeenVisited = new bool[Width, Depth];
        wallStateArray = new bool[Width, Depth, 4];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                wallStateArray[i, j, 0] = true;
                wallStateArray[i, j, 1] = true;
                wallStateArray[i, j, 2] = true;
                wallStateArray[i, j, 3] = true;
            }
        }
        coroutine = generateMaze(false, false, true);
        StartCoroutine(coroutine);
        
    }
    
    void knockDownPercentageOfInteriorWalls(float percentageToKnockDown)
    {

    }


    IEnumerator generateMaze(bool respawnPlayer, bool respawnTokens, bool respawnResetButton)
    {
        //Depth Based Search
        //Start at Random Point
        int currentX = Random.Range(0, Width);
        int currentY = Random.Range(0, Depth);
        if (respawnPlayer)
        {
            GameObject playerInstance = Instantiate(playerObject, new Vector3(currentX * 5, 2, currentY * 5), Quaternion.identity);
        }
        

        Vector2 startingNode = new Vector2(currentX, currentY);
        yield return new WaitForSeconds(0.001f);
        recursiveStep(startingNode);
        logState();

        if (respawnTokens)
        {
            placeCheese();
            placePill();
            placePill();
            placePill();
        }

        if (respawnResetButton)
        {
            coroutine = waitBeforeShowingButton(5.0f);
            StartCoroutine(coroutine);
        }

        //Finish up
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                setWallsAtCoordinates(i, j);
            }
        }

        checkValueOfMazePacketization();
    }

    IEnumerator waitBeforeShowingButton(float effectDuration)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / effectDuration;
            yield return null;
        }
        
        placeButton();
    }

}
