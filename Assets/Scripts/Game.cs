using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    TURN playerSymbol = TURN.CROSS;
    TILETYPE botSymbol = TILETYPE.NAUGHT;
    void Update()
    {
        //if player turn disabled, robot makes a turn
        BeginRobotTurn();
    }

    private void BeginRobotTurn()
    {
        //return if not robot turn
        if (!Board.Instance.turnDisabled) { return; }

        List<TILE_POSITION> tilesAvailible = new List<TILE_POSITION>();

        //gather the availible tiles to select
        foreach (GameObject tile in Board.Instance.board.Values)
        {
            //if tile not yet triggered, add it to availible
            if(!tile.GetComponent<Tile>().triggered)
            {
                tilesAvailible.Add(tile.GetComponent<Tile>().tilePosition);
            }
        }

        //create board state for simulation
        Dictionary<TILE_POSITION, BoardState> currentBoard = new Dictionary<TILE_POSITION, BoardState>();
        currentBoard[TILE_POSITION.BOTTOM_LEFT] = new BoardState();
        currentBoard[TILE_POSITION.BOTTOM_MIDDLE] = new BoardState();
        currentBoard[TILE_POSITION.BOTTOM_RIGHT] = new BoardState();

        currentBoard[TILE_POSITION.MIDDLE_LEFT] = new BoardState();
        currentBoard[TILE_POSITION.MIDDLE_MIDDLE] = new BoardState();
        currentBoard[TILE_POSITION.MIDDLE_RIGHT] = new BoardState();

        currentBoard[TILE_POSITION.TOP_LEFT] = new BoardState();
        currentBoard[TILE_POSITION.TOP_MIDDLE] = new BoardState();
        currentBoard[TILE_POSITION.TOP_RIGHT] = new BoardState();

        //set all symbols for each position
        foreach (GameObject tile in Board.Instance.board.Values)
        {
            //if tile not yet triggered, add it to availible
            TILE_POSITION newPos = tile.GetComponent<Tile>().tilePosition;
            currentBoard[newPos].tileType = tile.GetComponent<Tile>().tileType;
            currentBoard[newPos].isActive = tile.GetComponent<Tile>().triggered;
        }

        /////////////////////////////////////////////////////////////
        TILE_POSITION bestMove = MonteCarlo.Instance.Run(tilesAvailible, currentBoard, botSymbol);

        //execute best move for robot
        Board.Instance.RobotTurn(bestMove);
    }

    //private void RandomSelect(List<TILE_POSITION> tilesAvailible)
    //{
    //    //randomise a number to choose
    //    while (true)
    //    {
    //        int rng = Random.Range(0, tilesAvailible.Count);

    //        if (Board.Instance.RobotTurn(tilesAvailible[rng]))
    //        {
    //            break;
    //        }
    //    }
    //}
}
