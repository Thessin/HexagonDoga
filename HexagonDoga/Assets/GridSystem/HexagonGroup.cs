using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGroup
{
    private Vector3 position = Vector3.zero;
    private List<Hexagon> hexagons = new List<Hexagon>();

    private int groupingNumber;

    public HexagonGroup(Vector3 position, int groupingNumber)
    {
        this.position = position;
        this.groupingNumber = groupingNumber;
    }

    public void AddHexagon(Hexagon hex)
    {
        hexagons.Add(hex);
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void RotateGroup(bool clockWise)
    {
        if (groupingNumber % 2 == 0)
        {
            if (clockWise)
                MoveClockwise();
            else
                MoveCounterClockwise();
        }
        else
        {
            if (clockWise)
                MoveCounterClockwise();
            else
                MoveClockwise();
        }
    }

    private void MoveClockwise()
    {
        GameObject obj = hexagons[hexagons.Count - 1].obj;
        Color col = hexagons[hexagons.Count - 1].color;
        Debug.Log(hexagons.Count - 1 + "'s color is" + col);

        for (int i = hexagons.Count - 1; i >= 0; i--)
        {
            Debug.Log("in clockWise " + i);

            if (i != 0)
                hexagons[i].ExchangeData(hexagons[i-1].obj, hexagons[i-1].color);
            else
                hexagons[i].ExchangeData(obj, col);
        }

        bool foundDestroyableGroup = false;

        foreach (var hex in hexagons)
        {
            if (hex.IsGroupDestroyable())
                foundDestroyableGroup = true;
        }

        if (!foundDestroyableGroup)
            MoveCounterClockwise();
    }

    private void MoveCounterClockwise()
    {
        GameObject obj = hexagons[0].obj;
        Color col = hexagons[0].color;
        Debug.Log("0's color is" + col);

        for (int i = 0; i < hexagons.Count; i++)
        {
            Debug.Log("in counterClockwise " + i);

            if (i != hexagons.Count - 1)
                hexagons[i].ExchangeData(hexagons[i+1].obj, hexagons[i+1].color);
            else
                hexagons[i].ExchangeData(obj, col);
        }

        bool foundDestroyableGroup = false;

        foreach (var hex in hexagons)
        {
            if (hex.IsGroupDestroyable())
                foundDestroyableGroup = true;
        }

        if (!foundDestroyableGroup)
            MoveClockwise();
    }

    public bool IsExplodable()
    {
        if (hexagons[0].IsDestroyed() || hexagons[1].IsDestroyed() || hexagons[2].IsDestroyed())                                // If any one of the hexagons is destroyed, the group can not be explodable.
            return false;

        return hexagons[0].color.IsEqualColor(hexagons[1].color) && hexagons[0].color.IsEqualColor(hexagons[2].color);            // Check if all three hexagons' colors are equal.
    }

    public void DestroyGroup()
    {
        foreach (var hex in hexagons)
        {
            hex.DestroyHexagon();
        }
    }

    public List<Transform> GetHexagonsTransformList()
    {
        List<Transform> posList = new List<Transform>();

        foreach (var hex in hexagons)
        {
            posList.Add(hex.GetTransform());
        }

        return posList;
    }
}
