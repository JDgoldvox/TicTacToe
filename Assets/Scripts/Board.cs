using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TURN
{
    NAUGHT, CROSS
}

public enum TILETYPE
{
    NAUGHT, CROSS, NONE
}

public class Board : MonoBehaviour
{
    public static Board Instance;

    [SerializeField] private GameObject[] boardClickables = new GameObject[9];

    //tile position enum to tile
    public Dictionary<TILE_POSITION, GameObject> board { get; private set; } = new Dictionary<TILE_POSITION, GameObject>();

    [SerializeField] private Sprite naughtPrefab, crossPrefab;
    private Sprite currentPrefab;

    [SerializeField] public TURN turn = TURN.NAUGHT;//{ get; private set; }
    [SerializeField] public bool turnDisabled = false; //{ get; private set; } 

    private void Awake()
    {
        Instance = this;
        currentPrefab = naughtPrefab;
    }

    void Start()
    {
        InitiateBoardMap();
    }

    private void InitiateBoardMap()
    {
        board[TILE_POSITION.BOTTOM_LEFT] = boardClickables[0];
        board[TILE_POSITION.BOTTOM_MIDDLE] = boardClickables[1];
        board[TILE_POSITION.BOTTOM_RIGHT] = boardClickables[2];

        board[TILE_POSITION.MIDDLE_LEFT] = boardClickables[3];
        board[TILE_POSITION.MIDDLE_MIDDLE] = boardClickables[4];
        board[TILE_POSITION.MIDDLE_RIGHT] = boardClickables[5];

        board[TILE_POSITION.TOP_LEFT] = boardClickables[6];
        board[TILE_POSITION.TOP_MIDDLE] = boardClickables[7];
        board[TILE_POSITION.TOP_RIGHT] = boardClickables[8];
    }

    // Update is called once per frame
    void Update()
    {
        ChangePrefabToPlace();
    }

    public void Interact(TILE_POSITION tileClicked)
    {
        if (turnDisabled)
        {
            return;
        }

        //check we clicked on the board
        if(!board.ContainsKey(tileClicked))
        {
            return;
        }

        //check if tile can be changed
        if (!CanTileChange(tileClicked))
        {
            return;
        }

        //if check if we won
        CheckWin();

        //change prefab to place 
        ChangePrefabToPlace();

        //change turn
        ChangeTurn();

        DisablePlayerTurn();

        Debug.Log("Finished robot turn, changing to: " + turn.ToString());
    }

    /// <summary>
    /// run a robot turn, if fail, returns false
    /// </summary>
    /// <param name="tileClicked"></param>
    /// <returns></returns>
    public bool RobotTurn(TILE_POSITION tileClicked)
    {
        //check we clicked on the board
        if (!board.ContainsKey(tileClicked))
        {
            return false;
        }

        //check if tile can be changed
        if (!CanTileChange(tileClicked))
        {
            return false;
        }

        //if check if we won
        CheckWin();

        //change prefab to place 
        ChangePrefabToPlace();

        Debug.Log("robot before change" + turn.ToString());
        //change turn
        ChangeTurn();
        Debug.Log("robot after change" + turn.ToString());

        EnablePlayerTurn();

        //Debug.Log("Finished robot turn, changing to: " + turn.ToString());
        return true;
    }

    bool CanTileChange(TILE_POSITION clickedTile)
    {
        //decide which type of prefab it is
        TILETYPE type = TILETYPE.NONE;
        type = (turn == TURN.NAUGHT) ? type = TILETYPE.NAUGHT : type = TILETYPE.CROSS;

        bool canTileChange = board[clickedTile].GetComponent<Tile>().TriggerTile(currentPrefab, type);

        //if tile has already triggered, ignore this turn
        if (!canTileChange) { return false; }
        return true;
    }

    private void ChangePrefabToPlace()
    {
        if (currentPrefab == crossPrefab) { 
            currentPrefab = naughtPrefab;
        }
        else
        {
            currentPrefab = crossPrefab;
        }
        
    }

    public void ChangeTurn()
    {
        if (turn == TURN.NAUGHT) { turn = TURN.CROSS; }
        else { turn = TURN.NAUGHT; }
    }
    
    private void CheckWin()
    {
        if (CheckRows(board))
        {
            Debug.Log("WON");
        }

        if (CheckColumns(board))
        {
            Debug.Log("WON");
        }

        if (CheckDiagonals(board))
        {
            Debug.Log("WON");
        }
    }

    private bool CheckRows(Dictionary<TILE_POSITION, GameObject> boardInput)
    {
        //check whos turn it is, the person's whos turn it is, means they are able to win
        TILETYPE type = TILETYPE.NONE;
        type = (turn == TURN.NAUGHT) ? TILETYPE.NAUGHT : TILETYPE.CROSS;

        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_LEFT], boardInput[TILE_POSITION.BOTTOM_MIDDLE], boardInput[TILE_POSITION.BOTTOM_RIGHT]))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.MIDDLE_LEFT], boardInput[TILE_POSITION.MIDDLE_MIDDLE], boardInput[TILE_POSITION.MIDDLE_RIGHT]))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.TOP_LEFT], boardInput[TILE_POSITION.TOP_MIDDLE], boardInput[TILE_POSITION.TOP_RIGHT]))
        {
            return true;
        }

        return false;
    }

    private bool CheckColumns(Dictionary<TILE_POSITION, GameObject> boardInput)
    {
        //check whos turn it is, the person's whos turn it is, means they are able to win
        TILETYPE type = TILETYPE.NONE;
        type = (turn == TURN.NAUGHT) ? TILETYPE.NAUGHT : TILETYPE.CROSS;

        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_LEFT], boardInput[TILE_POSITION.MIDDLE_LEFT], boardInput[TILE_POSITION.TOP_LEFT]))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_MIDDLE], boardInput[TILE_POSITION.MIDDLE_MIDDLE], boardInput[TILE_POSITION.TOP_MIDDLE]))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_RIGHT], boardInput[TILE_POSITION.MIDDLE_RIGHT], boardInput[TILE_POSITION.TOP_RIGHT]))
        {
            return true;
        }

        return false;
    }

    private bool CheckDiagonals(Dictionary<TILE_POSITION, GameObject> boardInput)
    {
        //check whos turn it is, the person's whos turn it is, means they are able to win
        TILETYPE type = TILETYPE.NONE;
        type = (turn == TURN.NAUGHT) ? TILETYPE.NAUGHT : TILETYPE.CROSS;

        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_LEFT], boardInput[TILE_POSITION.MIDDLE_MIDDLE], boardInput[TILE_POSITION.TOP_RIGHT]))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.TOP_LEFT], boardInput[TILE_POSITION.MIDDLE_MIDDLE], boardInput[TILE_POSITION.BOTTOM_RIGHT]))
        {
            return true;
        }

        return false;
    }

    private bool CheckType(TILETYPE type, GameObject item1, GameObject item2, GameObject item3)
    {
        //check if 3 match
        Tile tileScript = item1.GetComponent<Tile>();
        if (tileScript.tileType != type) { return false; }

        tileScript = item2.GetComponent<Tile>();
        if (tileScript.tileType != type) { return false; }

        tileScript = item3.GetComponent<Tile>();
        if (tileScript.tileType != type) { return false; }

        return true;
    }

    public void DisablePlayerTurn()
    {
        turnDisabled = true;
    }

    public void EnablePlayerTurn()
    {
        turnDisabled = false;
    }
}