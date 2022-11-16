using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    private int row;
    private int column;
    private bool isTrap;
    private TileType type;
    private Card cardTile;
    
    public TileData(int rowNew, int columnNew, TileType newType, bool trap)
    {
        row = rowNew;
        column = columnNew;
        type = newType;
        isTrap = trap;
    }

    public int Row { get => row; set => row = value; }
    public int Column { get => column; set => column = value; }
    public TileType Type { get => type; set => type = value; }
    public Card CardTile { get => cardTile; set => cardTile = value; }
    public bool IsTrap { get => isTrap; set => isTrap = value; }
}
public enum TileType
{
    empty,
    starting,
    ending,
    corridor
}
