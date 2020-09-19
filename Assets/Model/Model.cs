using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip
{
    public Color Color { get; private set; }
    public Cell Cell { get; private set; }

}

public class Cell
{
    public int X { get; private set; }
    public int Y { get; private set; }

}

public enum Color
{
    White,
    Black
}

public enum GameMode
{
    HumanToRobot,
    HumanToHuman
}

public class Model : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
