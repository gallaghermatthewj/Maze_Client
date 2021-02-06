
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PowerupManager : MonoBehaviour
{
    public GameObject rewardObject;
    public GameObject pillObject;
    public GameObject buttonObject;
    public MazeBuilder maze;


    /*public void placeCheese()
    {
        int newX = Random.Range(0, maze.width);
        int newY = Random.Range(0, maze.depth);
        GameObject rewardInstance = Instantiate(rewardObject, new Vector3(newX * 5, 2, newY * 5), Quaternion.identity);
    }

    public void placePill()
    {
        int pillX = Random.Range(0, maze.width);
        int pillY = Random.Range(0, maze.depth);
        GameObject pillInstance = Instantiate(pillObject, new Vector3(pillX * 5, 2, pillY * 5), Quaternion.identity);
    }

    public void placeButton()
    {
        int buttonX = Random.Range(0, maze.width);
        int buttonY = Random.Range(0, maze.depth);
        GameObject pillInstance = Instantiate(buttonObject, new Vector3(buttonX * 5, 2, buttonY * 5), Quaternion.identity);
    }*/
}