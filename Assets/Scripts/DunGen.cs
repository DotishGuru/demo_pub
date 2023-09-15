using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class DunGen : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [Header("Maze setup")]
    public Vector2 size;
    public int startPos = 0;
    public GameObject room;
    public Vector2 offset;

    [Header("Player spawn setup")]
    public GameObject player;
    public CinemachineVirtualCamera virtCamera;

    [Header("Enemies spawn setup")]
    public GameObject enemy;

    [Header("Maze end")]
    public GameObject levelEnd;
    

    List<Cell> board;
    List<Vector2> roomsPositions;

    void Start()
    {
        InitMazeBoard();
        FillMazeBoard();
        GenerateMaze();
        PlacePlayer();
        PlaceLevelEnd();
        PlaceEnemies();
    }

    void PlacePlayer()
    {
        GameObject pl = Instantiate(player, roomsPositions[0], Quaternion.identity);
        virtCamera.Follow = pl.transform;
        virtCamera.LookAt = pl.transform;
    }

    void PlaceEnemies()
    {
        for (int i = 1; i < roomsPositions.Count - 1; i += 2)
        {
            Instantiate(enemy, roomsPositions[i], Quaternion.identity);
        }
    }

    void PlaceLevelEnd()
    {
        Instantiate(levelEnd, roomsPositions[roomsPositions.Count - 1], Quaternion.identity);
    }

    void GenerateMaze()
    {
        Vector2 roomPos;

        roomsPositions = new List<Vector2>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                roomPos = new Vector2(i * offset.x, -j * offset.y);

                roomsPositions.Add(roomPos);

                RoomBehaviour newRoom = Instantiate(room, roomPos, Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                newRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].status);

                newRoom.name += " " + i + "-" + j;
            }
        }
    }

    void InitMazeBoard()
    {
        board = new List<Cell>();
        
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }
    }

    void FillMazeBoard()
    {
        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;

            board[currentCell].visited = true;

            //check neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if(neighbors.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];

                if(newCell > currentCell)
                {
                    if(newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    if(newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up
        if(cell - size.x >= 0
            && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }

        //check down
        if(cell + size.x < board.Count
            && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }

        //check right
        if((cell + 1) % size.x != 0
            && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        //check left
        if(cell % size.x != 0
            && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }
}
