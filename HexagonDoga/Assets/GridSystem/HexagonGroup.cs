using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGroup
{
    public Vector3 position = Vector3.zero;
    public List<int> hexagonNumbers = new List<int>();

    public HexagonGroup(Vector3 position)
    {
        this.position = position;
    }

    public void AddHexagonNumber(int number)
    {
        hexagonNumbers.Add(number);
    }
}
