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
    public TILETYPE tileType = TILETYPE.NONE;

    /// <summary>
    /// returns false if was not successful
    /// </summary>
    /// <param name="currentPrefab"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool TriggerTile(Sprite currentPrefab, TILETYPE type)
    {
        if (triggered)
        {
            return false;
        }
        
        GetComponentInChildren<SpriteRenderer>().sprite = currentPrefab;
        triggered = true;
        tileType = type;

        return true;
    }
}
