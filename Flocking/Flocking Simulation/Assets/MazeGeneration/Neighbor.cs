using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor 
{
    public int row, col;
    public char direction; //U = up, R = right, D = down, L = left

    public Neighbor(int r, int c, char dir)
    {
        row = r;
        col = c;
        direction = dir;
    }
}
