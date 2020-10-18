using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGroup
{
    private Vector3 position = Vector3.zero;
    private List<int> hexagonNumbers = new List<int>();

    public HexagonGroup(Vector3 position)
    {
        this.position = position;
    }

    public void AddHexagonNumber(int number)
    {
        hexagonNumbers.Add(number);
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public List<int> GetHexagonNumbersDeepCopy()
    {
        List<int> nums = new List<int>();
        nums.AddRange(hexagonNumbers);

        return nums;
    }
}
