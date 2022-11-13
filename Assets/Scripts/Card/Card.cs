using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( fileName = "CardData", menuName = "Card/CreateCard", order = 0)]
public class Card : ScriptableObject
{
    [SerializeField] private List<Directions> corridorDirections = new List<Directions>();
    [SerializeField] private GroundType type;
    [SerializeField] private Sprite image;
    [SerializeField] private Tile cardObjectPrefab;
    [SerializeField] private int numberInTheDeck;

    public int NumberInTheDeck { get => numberInTheDeck; set => numberInTheDeck = value; }
    public Sprite Image { get => image; }
    public List<Directions> CorridorDirections { get => corridorDirections; }
    public Tile CardObjectPrefab { get => cardObjectPrefab; }
}

[Flags]
public enum Directions
{
    none = 0,
    north = 1,
    south = 2,
    west = 4,
    east = 8
}

public enum GroundType
{
    land,
    water,
    wall
}


