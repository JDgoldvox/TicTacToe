using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_POSITION
{
    BOTTOM_LEFT , BOTTOM_MIDDLE, BOTTOM_RIGHT, MIDDLE_LEFT, MIDDLE_MIDDLE, MIDDLE_RIGHT, TOP_LEFT, TOP_MIDDLE, TOP_RIGHT
};

public class Tile : MonoBehaviour
{
    public TILE_POSITION tilePosition;
    public bool triggered = false;

}
