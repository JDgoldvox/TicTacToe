using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public TILETYPE tileType = TILETYPE.NONE;
    public bool isActive = true;
};

public class MonteCarlo : MonoBehaviour {

    private TILETYPE botSymbol = TILETYPE.NONE;
    private TILETYPE playerSymbol = TILETYPE.NONE;
    private TURN playerTurn = TURN.NONE;
    private TURN botTurn = TURN.NONE;
    private TURN currentTurn = TURN.NONE;

    Dictionary<TILE_POSITION, BoardState> tempBoard = new Dictionary<TILE_POSITION, BoardState>();

    public static MonteCarlo Instance;
    private void Awake()
    {
        Instance = this;
    }

    Node root = null;
    public int simulationsUntilTermination = 100;

    //Input - list of all possible answers
    //output - best answer 
    public TILE_POSITION Run(List<TILE_POSITION> possibleAnswers, Dictionary<TILE_POSITION, BoardState> currentBoardInput, TILETYPE botSymbolInput)
    {
        //set bot and player symbols
        botSymbol = botSymbolInput;
        playerSymbol = (botSymbol == TILETYPE.CROSS) ? TILETYPE.NAUGHT : TILETYPE.CROSS;
        botTurn = (botSymbol == TILETYPE.CROSS) ? TURN.CROSS : TURN.NAUGHT;
        playerTurn = (botSymbol == TILETYPE.CROSS) ? TURN.NAUGHT : TURN.CROSS;

        //create new root
        root = new Node(possibleAnswers);

        //set termination condition
        for(int i = 0; i < simulationsUntilTermination; i++)
        {
            Debug.Log("Total visits: " + root.visits);

            //pre actions
            tempBoard.Clear();

            //remove shadow copy...and set to original
            tempBoard = new Dictionary<TILE_POSITION, BoardState>();
            foreach(var keyValue in currentBoardInput)
            {
                //createa a new BoardState
                tempBoard[keyValue.Key] = new BoardState()
                {
                    tileType = keyValue.Value.tileType,
                    isActive = keyValue.Value.isActive
                };
            }

            Node node = Selection();

            //expansion returns the node added
            Node nodeCreated = Expansion(node);

            RESULT result = Simulation(nodeCreated);

            BackTrack(nodeCreated, result);
        }

        return ReturnBestResult();
    }


    /// <summary>
    /// Recurse down the tree to find a leaf node with children it can explore
    /// </summary>
    /// <returns></returns>
    private Node Selection()
    {
        Debug.Log("SELECTION STAGE ------------------------------------------");

        //find a leaf node by comparing uct
        Node current = root;

        //if current node has 0 children
        if(root.children.Count == 0)
        {
            Debug.Log("No children, returning this");
            return current;
        }

        while (true)
        {
            //if this nodes children are full, we cannot add any more children. So, choose highest uct
            if (current.childrenFull)
            {
                //otherwise, set current to root's children to kick this off
                current = root.children[0];

                Debug.Log("children full... attempting to return best child");
                Debug.Log("amount of children: " +  current.children.Count);

                //if children node is full on current node, keep searching
                Node bestUCTChild = current.children[0];

                string outPut = "UCTS: ";
                //go through list of children to find highest uct
                foreach (Node node in current.children)
                {
                    outPut += " " + node.uct;
                    //if uct higher, set currentHighestUCTChild to the node found
                    if (node.uct > bestUCTChild.uct)
                    {
                        bestUCTChild = node;
                    }
                }

                //update current
                current = bestUCTChild;
                Debug.Log("move: " + current.tileMove);
                return current;
            }
            else //children is not full
            {
                Debug.Log("move: " + current.tileMove);

                if(current == root)
                {
                    Debug.Log("Returning parent... cuz I need to.");
                }
                //return this node
                return current;
            }
        }
    }

    private Node Expansion(Node parentNodeToExpand)
    {
        Debug.Log("EXPANSION STAGE ------------------------------------------");
        Debug.Log(parentNodeToExpand.possibleAnswers.Count + " possible choices to expand");

        TILE_POSITION expandedPosition;
        if (parentNodeToExpand.possibleAnswers.Count == 1)
        {
            expandedPosition = parentNodeToExpand.possibleAnswers[0];
        }
        else //take a random possible answer
        {
            int randomIndex = UnityEngine.Random.Range(0, parentNodeToExpand.possibleAnswers.Count);
            expandedPosition = parentNodeToExpand.possibleAnswers[randomIndex];
        }

        //add it to the children
        Node newChildNode = parentNodeToExpand.Add(expandedPosition);
        newChildNode.tileMove = expandedPosition;

        Debug.Log("Chosen expansion: " + expandedPosition);
        Debug.Log("node max children: " + newChildNode.maxChildren);

        //add expansion to temp board
        tempBoard[expandedPosition].isActive = true;

        //set turn, so we can set symbol on expanded node
        TURN thisTurn = TURN.NONE;
        if(parentNodeToExpand.parent == null) { //if parent node...its was always player's turn before this one
            thisTurn = botTurn;
        }
        else //if not root node, it's opposite to parent
        {
            if (parentNodeToExpand.parent.turn == TURN.NAUGHT)
            {
                thisTurn = TURN.CROSS;

                //set next turn
                currentTurn = TURN.NAUGHT;
            }
            else
            {
                thisTurn = TURN.NAUGHT;

                //set next turn
                currentTurn = TURN.CROSS;
            }
        }

        //make into tile type
        TILETYPE expandedNodeTileType = TILETYPE.NONE;
        if (thisTurn == TURN.NAUGHT)
        {
            expandedNodeTileType = TILETYPE.NAUGHT;

        }
        else
        {
            expandedNodeTileType = TILETYPE.CROSS;
        }


        //set tile type
        tempBoard[expandedPosition].tileType = expandedNodeTileType;
        

        return newChildNode;
    }

    /// <summary>
    /// Simulate 1 game randomly - play out a game to fill all squares or terminate when there is a win or lose
    /// </summary>
    /// <param name="simulatedNode"></param>
    /// <returns></returns>
    private RESULT Simulation(Node simulatedNode)
    {
        Debug.Log("SIMULATION STAGE ------------------------------------------");

        List<TILE_POSITION> possiblePositions = new List<TILE_POSITION>(simulatedNode.possibleAnswers);
        Debug.Log(simulatedNode.possibleAnswers.Count + " possible choices to expand");

        {
            string s = "";
            int i = 0;
            string currentStr = "";
            foreach (var state in tempBoard.Values)
            {
                if (state.isActive)
                {
                    if(state.tileType == TILETYPE.NAUGHT)
                    {
                        currentStr += "O";
                    }
                    else
                    {
                        currentStr += "X";
                    }
                    
                    i++;
                }
                else
                {
                    currentStr += "+";
                    i++;
                }

                if (i == 3)
                {
                    currentStr += "\n";
                    s = currentStr + s;
                    currentStr = "";
                    i = 0;
                }
            }
            Debug.Log(s);
        }

        Debug.Log("-------------------- ^^ prep ^^");

        //create board bools form possiblePositions
        foreach (TILE_POSITION position in possiblePositions)
        {
            switch (position)
            {
                case TILE_POSITION.BOTTOM_LEFT:
                    tempBoard[TILE_POSITION.BOTTOM_LEFT].isActive = false;
                    break;
                case TILE_POSITION.BOTTOM_MIDDLE:
                    tempBoard[TILE_POSITION.BOTTOM_MIDDLE].isActive = false;
                    break;
                case TILE_POSITION.BOTTOM_RIGHT:
                    tempBoard[TILE_POSITION.BOTTOM_RIGHT].isActive = false;
                    break;
                case TILE_POSITION.MIDDLE_LEFT:
                    tempBoard[TILE_POSITION.MIDDLE_LEFT].isActive = false;
                    break;
                case TILE_POSITION.MIDDLE_MIDDLE:
                    tempBoard[TILE_POSITION.MIDDLE_MIDDLE].isActive = false;
                    break;
                case TILE_POSITION.MIDDLE_RIGHT:
                    tempBoard[TILE_POSITION.MIDDLE_RIGHT].isActive = false;
                    break;
                case TILE_POSITION.TOP_LEFT:
                    tempBoard[TILE_POSITION.TOP_LEFT].isActive = false;
                    break;
                case TILE_POSITION.TOP_MIDDLE:
                    tempBoard[TILE_POSITION.TOP_MIDDLE].isActive = false;
                    break;
                case TILE_POSITION.TOP_RIGHT:
                    tempBoard[TILE_POSITION.TOP_RIGHT].isActive = false;
                    break;
                default:
                    Debug.Log("Should never get here, should be a");
                    break;
            }
        }

        //use random position
        while (true)
        {
            //get a random positions from tile positions availible
            int rng = UnityEngine.Random.Range(0, possiblePositions.Count);

            //remove it from availible for next iteration
            TILE_POSITION posToMove = possiblePositions[rng];
            possiblePositions.Remove(posToMove);

            //add it to our board
            tempBoard[posToMove].isActive = true;

            //set symbol depending on whos turn it is
            if(currentTurn == TURN.NAUGHT)
            {
                tempBoard[posToMove].tileType = TILETYPE.NAUGHT;
            }
            else
            {
                tempBoard[posToMove].tileType = TILETYPE.CROSS;
            }

            //check if somebody has won
            RESULT result = CheckSimulationWin.CheckWin(currentTurn, tempBoard);

            //if we win
            if (result == RESULT.WIN && currentTurn == botTurn)
            {
                Debug.Log("bot won!");
                return RESULT.WIN;
            }
            else if(result == RESULT.WIN) //if human wins
            {
                Debug.Log("bot lost!");
                return RESULT.LOSS;
            }

            if (result == RESULT.DRAW)
            {
                Debug.Log("draw!");
                return RESULT.DRAW;
            }

            //swap turns
            currentTurn = (currentTurn == TURN.NAUGHT) ? TURN.CROSS : TURN.NAUGHT;
        }
    }

    private void BackTrack(Node deepestNode, RESULT result)
    {
        Debug.Log("BACKTRACK STAGE ------------------------------------------");
        int valueAddition = 0;

        if(result == RESULT.WIN)
        {
            valueAddition = 1;
        }
        else if (result == RESULT.LOSS)
        {
            valueAddition = -1;
        }

        Node current = deepestNode;

        //update if children nodes are full or not
        if (current.children.Count == current.maxChildren && !current.childrenFull)
        {
            Debug.Log("setting children to full");
            current.childrenFull = true;
        }

        /////////////////////////////////////////////
        
        while (current.parent != null)
        {
            //update all scores
            current.wins += valueAddition;

            //update all visits
            current.visits++;

            //update all ucts
            float winRatio = current.wins / current.visits;
            current.uct = UCT.Calculate(winRatio, current.parent.visits, current.visits, 1.41f);

            //progress to next parent
            current = current.parent;
        }

        //when its the root node
        current.visits += 1;
    }

    private TILE_POSITION ReturnBestResult()
    {
        Node bestUCTNode = root.children[0];

        foreach (Node node in root.children)
        {
            if(node.uct > bestUCTNode.uct)
            {
                bestUCTNode = node;
            }
        }

        return bestUCTNode.tileMove;
    }
}
