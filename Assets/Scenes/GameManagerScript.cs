using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    int[,] map;
    GameObject[,] field;

    /*void PrintArray()
    {
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ", ";
        }
        Debug.Log(debugText);
    }*/

    Vector2Int GetPlayerIndex()
    {
        
        for(int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {

                if (field[y, x] == null)
                {
                    continue;
                }
                else if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }

            }
        }

        return new Vector2Int(-1, -1);
    }
    /// <summary>
    /// num”z—ñ‚Ì”š‚ğˆÚ“®‚³‚¹‚é
    /// </summary>
    /// <param name="number">“®‚©‚·”š</param>
    /// <param name="moveFrom">“®‚©‚·Œ³‚ÌêŠ</param>
    /// <param name="moveTo">“®‚©‚·æ‚ÌêŠ</param>
    /// <returns></returns>
    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.x < 0 || moveTo.y < 0 || moveTo.y >= field.GetLength(0) || moveTo.x >= field.GetLength(1))
        {
            return false;
        }
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(-field.GetLength(1) / 2 + moveTo.x, field.GetLength(0) / 2 - moveTo.y, 0);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

      

        map = new int[,]
        {
            {0, 0, 0, 0, 0 },
            {1, 2, 2, 0, 0 },
            {0, 0, 0, 0, 0 },
        };

        field = new GameObject
        [
            map.GetLength(0),
            map.GetLength(1)
        ];

        string debugText = "";

        for (int y = 0; y < map.GetLength(0);  y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    //GameObject instance = Instantiate(
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 2)
                {
                    //GameObject instance = Instantiate(
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0),
                        Quaternion.identity
                    );
                }

                debugText += map[y, x].ToString() + ",";
            }
            debugText += "\n";
        }
        Debug.Log(debugText);

        //PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            
            MoveNumber(playerIndex, new Vector2Int(playerIndex.x + 1, playerIndex.y));
            //PrintArray();

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();


            MoveNumber(playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));

            //PrintArray();

        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();


            MoveNumber(playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
            //PrintArray();

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();


            MoveNumber(playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
            //PrintArray();

        }
    }
}
