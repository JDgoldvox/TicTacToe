using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CheckSimulationWin
{
    public static RESULT CheckWin(TURN turn, Dictionary<TILE_POSITION, BoardState> board)
    {
        //{
        //    string s = "";
        //    int i = 0;
        //    string currentStr = "";
        //    foreach (var state in board.Values)
        //    {
        //        if (state.isActive)
        //        {
        //            if (state.tileType == TILETYPE.NAUGHT)
        //            {
        //                currentStr += "O";
        //            }
        //            else if(state.tileType == TILETYPE.CROSS)
        //            {
        //                currentStr += "X";
        //            }

        //            i++;
        //        }
        //        else
        //        {
        //            currentStr += "+";
        //            i++;
        //        }

        //        if (i == 3)
        //        {
        //            currentStr += "\n";
        //            s = currentStr + s;
        //            currentStr = "";
        //            i = 0;
        //        }
        //    }
        //    Debug.Log(s);
        //}

        if (CheckRows(turn, board))
        {
            //Debug.Log("3 in a row");
            return RESULT.WIN;
        }
        else if (CheckColumns(turn, board))
        {
            //Debug.Log("3 column");
            return RESULT.WIN;
        }
        else if (CheckDiagonals(turn, board))
        {
            //Debug.Log("3 diagonal"); 
            return RESULT.WIN;
        }

        //check draw
        if (!IsSpacesOnBoard(board))
        {
            //Debug.Log("DRAW");
            return RESULT.DRAW;
        }

        return RESULT.NONE;
    }

    private static bool CheckRows(TURN turn, Dictionary<TILE_POSITION, BoardState> boardInput)
    {
        //check whos turn it is, the person's whos turn it is, means they are able to win
        TILETYPE type = TILETYPE.NONE;
        type = (turn == TURN.NAUGHT) ? TILETYPE.NAUGHT : TILETYPE.CROSS;

        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_LEFT].tileType, boardInput[TILE_POSITION.BOTTOM_MIDDLE].tileType, boardInput[TILE_POSITION.BOTTOM_RIGHT].tileType))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.MIDDLE_LEFT].tileType, boardInput[TILE_POSITION.MIDDLE_MIDDLE].tileType, boardInput[TILE_POSITION.MIDDLE_RIGHT].tileType))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.TOP_LEFT].tileType, boardInput[TILE_POSITION.TOP_MIDDLE].tileType, boardInput[TILE_POSITION.TOP_RIGHT].tileType))
        {
            return true;
        }

        return false;
    }

    private static bool CheckColumns(TURN turn, Dictionary<TILE_POSITION, BoardState> boardInput)
    {
        //check whos turn it is, the person's whos turn it is, means they are able to win
        TILETYPE type = TILETYPE.NONE;
        type = (turn == TURN.NAUGHT) ? TILETYPE.NAUGHT : TILETYPE.CROSS;

        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_LEFT].tileType, boardInput[TILE_POSITION.MIDDLE_LEFT].tileType, boardInput[TILE_POSITION.TOP_LEFT].tileType))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_MIDDLE].tileType, boardInput[TILE_POSITION.MIDDLE_MIDDLE].tileType, boardInput[TILE_POSITION.TOP_MIDDLE].tileType))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_RIGHT].tileType, boardInput[TILE_POSITION.MIDDLE_RIGHT].tileType, boardInput[TILE_POSITION.TOP_RIGHT].tileType))
        {
            return true;
        }

        return false;
    }

    private static bool CheckDiagonals(TURN turn, Dictionary<TILE_POSITION, BoardState> boardInput)
    {
        //check whos turn it is, the person's whos turn it is, means they are able to win
        TILETYPE type = TILETYPE.NONE;
        type = (turn == TURN.NAUGHT) ? TILETYPE.NAUGHT : TILETYPE.CROSS;

        if (CheckType(type, boardInput[TILE_POSITION.BOTTOM_LEFT].tileType, boardInput[TILE_POSITION.MIDDLE_MIDDLE].tileType, boardInput[TILE_POSITION.TOP_RIGHT].tileType))
        {
            return true;
        }
        if (CheckType(type, boardInput[TILE_POSITION.TOP_LEFT].tileType, boardInput[TILE_POSITION.MIDDLE_MIDDLE].tileType, boardInput[TILE_POSITION.BOTTOM_RIGHT].tileType))
        {
            return true;
        }

        return false;
    }

    private static bool CheckType(TILETYPE type, TILETYPE item1, TILETYPE item2, TILETYPE item3)
    {
        //check if 3 match
        if (item1 != type) { return false; }

        if (item2 != type) { return false; }

        if (item3 != type) { return false; }

        return true;
    }

    private static bool IsSpacesOnBoard(Dictionary<TILE_POSITION, BoardState> tempBoard)
    {
        foreach (BoardState state in tempBoard.Values)
        {
            if (!state.isActive)
            {
                return true;
            }
        }

        return false;
    }
}
