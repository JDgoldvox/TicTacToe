using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TURN
{
    NAUGHT, CROSS
}

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject[] boardClickables = new GameObject[9];

    //tile position enum to tile
    private protected Dictionary<TILE_POSITION, GameObject> board = new Dictionary<TILE_POSITION, GameObject>();

    [SerializeField] private Sprite naughtPrefab, crossPrefab;
    private Sprite currentPrefab;

    private TURN turn = TURN.NAUGHT;

    private void Awake()
    {
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


        if(board.ContainsKey(tileClicked))
        {
            Debug.Log("CONTAINS THIS KEY!");
            board[tileClicked].GetComponent<Tile>().triggered = true;
            board[tileClicked].GetComponentInChildren<SpriteRenderer>().sprite = currentPrefab;

            ChangeTurn();
        }

    }

    private void ChangePrefabToPlace()
    {
        if(turn == TURN.NAUGHT)
        {
            if(currentPrefab != naughtPrefab) { currentPrefab = naughtPrefab; }
        }
        else
        {
            if (currentPrefab != crossPrefab) { currentPrefab = crossPrefab; }
        }
    }

    private void ChangeTurn()
    {
        if(turn == TURN.NAUGHT) { turn = TURN.CROSS; }
        else { turn = TURN.NAUGHT; }
    }
            
}
