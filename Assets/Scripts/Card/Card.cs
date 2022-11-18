using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( fileName = "CardData", menuName = "Card/CreateCard", order = 0)]
public class Card : ScriptableObject
{
    [SerializeField] private List<RouteData> corridorDirections = new List<RouteData>();
    [SerializeField] private GroundType type;
    [SerializeField] private Sprite image;
    [SerializeField] private Tile cardObjectPrefab;
    [SerializeField] private int numberInTheDeck;

    public int NumberInTheDeck { get => numberInTheDeck; set => numberInTheDeck = value; }
    public Sprite Image { get => image; }
    public List<RouteData> CorridorDirections { get => corridorDirections; }
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

[System.Serializable]
public struct RouteData
{
    [SerializeField] private Directions direction;
    [SerializeField]private Vector3 playerPosition;

    public Directions Direction { get => direction; set => direction = value; }
    public Vector3 PlayerPosition { get => playerPosition; set => playerPosition = value; }
}

