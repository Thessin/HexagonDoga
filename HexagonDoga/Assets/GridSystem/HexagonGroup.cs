using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGroup
{
    private Vector3 position = Vector3.zero;
    private List<Hexagon> hexagons = new List<Hexagon>();

    public HexagonGroup(Vector3 position)
    {
        this.position = position;
    }

    public void AddHexagon(Hexagon hex)
    {
        hexagons.Add(hex);
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void MoveClockwise()
    {

    }

    public void MoveCounterClockwise()
    {

    }

    public bool IsExplodable()
    {
        return hexagons[0].color.IsEqualColor(hexagons[1].color) && hexagons[0].color.IsEqualColor(hexagons[2].color);            // Check if all three hexagons' colors are equal.
    }

    //public List<int> GetHexagonNumbersDeepCopy()
    //{
    //    List<int> nums = new List<int>();
    //    nums.AddRange(hexagonNumbers);

    //    return nums;
    //}
}
