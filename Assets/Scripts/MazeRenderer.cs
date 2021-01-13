using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MazeRenderer : MonoBehaviour
{
    
    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab = null;

    [SerializeField]
    private GameObject wall;

    [SerializeField]
    [Range(1, 3)]
    private int MazeKind = 1;

    [SerializeField]
    private MazeCell[,] mazeCells;


    //When the generate button is pressed this will destroy an existing maze and then generate the new maze
    public void Generate()
    {
        switch (MazeKind)
        {
            case 2:
                foreach (Transform child in transform)
                    GameObject.Destroy(child.gameObject);
                DrawHK();
                MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
                ma.CreateMaze();
                break;
            case 1:
                foreach (Transform child in transform)
                    GameObject.Destroy(child.gameObject);
                var maze = MazeGenerator.Generate(width, height);
                DrawRB(maze);
                break;
            default:
                print("Broken");
                break;
        }

    }

    //draws the Recursive Backtracking Algorithm
    private void DrawRB(WallState[,] maze)
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }

        }

    }

    //draws the Hunt and Kill Algorithm
    private void DrawHK()
    {

        mazeCells = new MazeCell[height, width];

        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                mazeCells[r, c] = new MazeCell();

                if (c == 0)
                {
                    mazeCells[r, c].westWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) - (size / 2f)), Quaternion.identity) as GameObject;
                    mazeCells[r, c].westWall.transform.parent = GameObject.Find("MazeRenderer").transform;
                }

                mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity) as GameObject;
                mazeCells[r, c].eastWall.transform.parent = GameObject.Find("MazeRenderer").transform;

                if (r == 0)
                {
                    mazeCells[r, c].northWall = Instantiate(wall, new Vector3((r * size) - (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
                    mazeCells[r, c].northWall.transform.parent = GameObject.Find("MazeRenderer").transform;
                }

                mazeCells[r, c].southWall = Instantiate(wall, new Vector3((r * size) + (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
                mazeCells[r, c].southWall.transform.parent = GameObject.Find("MazeRenderer").transform;
            }
        }
    }    
    //this allows the slider to adjust height
    public void adjustHeight(float newHeight)
    {
        height = (int)newHeight;
    }
    //this allows the slider to adjust width
    public void adjustWidth(float newWidth)
    {
        width = (int)newWidth;
    }
    //toggles between Hunt and kill and Recursive Backtracking
    public void toggle()
    {
        switch (MazeKind)
        {
            case 2:
                MazeKind = 1;
                break;
            case 1:
                MazeKind = 2;
                break;
            default:
                print("Broken");
                break;
        }

    }
}