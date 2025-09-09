using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameBoard : MonoBehaviour
{
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap newState;
    [SerializeField] private Tile deadTile;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private float updateInterval = 0.05f;


}
