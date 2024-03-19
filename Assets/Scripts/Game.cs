using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    //Board board = Board.Instance;
    TURN playerSymbol = TURN.NAUGHT;

    // Start is called before the first frame update
    void Start()
    {
        //StartGame(); // AI should start competing against you
        
    }

    void Update()
    {
        //if player turn disabled, robot makes a turn
        RobotTurn();
    }

    private void RobotTurn()
    {
        //return if not robot turn
        if (!Board.Instance.turnDisabled) { return; }

        //make random turn -------------------------------------------------------
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

        //randomise a number to choose
        while (true)
        {
            int rng = Random.Range(0, tilesAvailible.Count);

            if (Board.Instance.RobotTurn(tilesAvailible[rng]))
            {
                break;
            }
        }
    }
}
