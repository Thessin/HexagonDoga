using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon
{
    private Vector3 position = Vector3.zero;
    private List<int> surroundingHexagonGroupingNumbers = new List<int>();
    private GameObject obj;
    private bool destroyed = false;

    public Hexagon(Vector3 position)
    {
        this.position = position;
    }

    public void SetGameObject(GameObject obj)
    {
        this.obj = obj;
    }
    public void SetSurroundingGroupNumbers(List<int> groupNumbers)
    {
        surroundingHexagonGroupingNumbers = groupNumbers;
    }
    public Vector3 GetPosition()
    {
        return position;
    }
}
