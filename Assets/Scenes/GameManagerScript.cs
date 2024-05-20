using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    int[,] map;
    GameObject[,] field;

    public GameObject clearText;
    public GameObject ParticlePrefab;

    /*void PrintArray()
    {
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ", ";
        }
        Debug.Log(debugText);
    }*/

    Vector3 IndexToPosition(Vector2Int index)
    {
        return new Vector3(index.x - field.GetLength(1) / 2, -index.y + field.GetLength(0) / 2, 0);
    }

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
    /// num配列の数字を移動させる
    /// </summary>
    /// <param name="number">動かす数字</param>
    /// <param name="moveFrom">動かす元の場所</param>
    /// <param name="moveTo">動かす先の場所</param>
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

        //field[moveFrom.y, moveFrom.x].transform.position = IndexToPosition(moveTo);

        Vector3 moveToposition = IndexToPosition(moveTo);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToposition);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    

    //クリア判定
    bool IsClead()
    {
        //Vector2int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {

                //格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    //格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }

            }
        }

        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f ==  null || f.tag != "Box")
            {
                return false;
            }
        }

        return true;

    }

    // Start is called before the first frame update
    void Start()
    {

        Screen.SetResolution(1280, 720, false);
      

        map = new int[,]
        {
            {0, 0, 3, 0, 0 },
            {1, 2, 2, 0, 0 },
            {0, 0, 0, 0, 3 },
            {0, 0, 0, 0, 0 },
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
                        IndexToPosition(new Vector2Int(x,y)),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 2)
                {
                    //GameObject instance = Instantiate(
                    field[y, x] = Instantiate(
                        boxPrefab,
                        IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 3)
                {
                    Instantiate(
                    goalPrefab,
                    new Vector3(x - field.GetLength(1) / 2, -y + field.GetLength(0) / 2, 0.01f),
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

            for (int i = 0; i < 5; i++)
            {
                Instantiate(
                    ParticlePrefab,
                    IndexToPosition(new Vector2Int(playerIndex.x, playerIndex.y)),
                    Quaternion.identity
                );
            }

            if (IsClead())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();


            MoveNumber(playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));

            //PrintArray();

            for (int i = 0; i < 5; i++)
            {
                Instantiate(
                    ParticlePrefab,
                    IndexToPosition(new Vector2Int(playerIndex.x, playerIndex.y)),
                    Quaternion.identity
                );
            }

            if (IsClead())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }

        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();


            MoveNumber(playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
            //PrintArray();

            for (int i = 0; i < 5; i++)
            {
                Instantiate(
                    ParticlePrefab,
                    IndexToPosition(new Vector2Int(playerIndex.x, playerIndex.y)),
                    Quaternion.identity
                );
            }

            if (IsClead())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();


            MoveNumber(playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
            //PrintArray();

            for (int i = 0; i < 5; i++)
            {
                Instantiate(
                    ParticlePrefab,
                    IndexToPosition(new Vector2Int(playerIndex.x, playerIndex.y)),
                    Quaternion.identity
                );
            }

            if (IsClead())
            {
                clearText.SetActive(true);
                Debug.Log("Clear");
            }

        }

        

    }
}
