using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public TILE_POSITION tileMove = TILE_POSITION.BOTTOM_MIDDLE;
    public int visits = 0;
    public int wins = 0;
    public float uct = float.MaxValue;
    public Node parent = null;
    public List<Node> children = new List<Node>();
    public List<TILE_POSITION> possibleAnswers = new List<TILE_POSITION>();
    public bool childrenFull = false;
    public int maxChildren = 0;
    public TURN turn = TURN.NONE;

    public Node(List<TILE_POSITION> answers) 
    {
        Debug.Log("Creating new node with " + answers.Count + " children");

        //set possible answers
        possibleAnswers = new List<TILE_POSITION>(answers);
        maxChildren = possibleAnswers.Count;
    }

    /// <summary>
    /// Adds a child to a parent
    /// </summary>
    /// <param name="pos"></param>
    public Node Add(TILE_POSITION pos)
    {
        //create a new list without this position
        List<TILE_POSITION> newListOfPossibles = new List<TILE_POSITION>(possibleAnswers);
        newListOfPossibles.Remove(pos);

        //add new node with availible choices for next
        Node newNode = new Node(newListOfPossibles);

        //attach parent in new node
        newNode.parent = this;

        //make sure we are adding the child we created to the our parent
        children.Add(newNode);

        //return the new node just created
        return newNode;
    }
}