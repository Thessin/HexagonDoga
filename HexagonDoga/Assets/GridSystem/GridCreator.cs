using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : Singleton<GridCreator>
{
    [SerializeField]
    private GameObject gridGO;

    private float hexagonX = 0.0f;
    private float hexagonY = 0.0f;

    [SerializeField] private int tileCountX = 8;
    [SerializeField] private int tileCountY = 9;
    [SerializeField] private float spacing = 0.0f;

    [SerializeField]
    private List<Color> colors;

    private Hexagon[,] hexagons;

    private List<HexagonGroup> hexagonGroups = new List<HexagonGroup>();

    private List<Vector3> midPoints = new List<Vector3>();

    private void Start()
    {
        hexagonX = gridGO.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        hexagonY = gridGO.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        hexagons = new Hexagon[tileCountX, tileCountY];

        CreateGrid();
    }

    private void CreateGrid()
    {
        // Creating grouping points
        for (int y = 0; y < tileCountY - 1; y++)
        {
            for (int x = 0; x < tileCountX - 1; x++)
            {
                // For grouping points

                float xPos = x * 1.5f * hexagonX + (spacing * x);

                hexagonGroups.Add(new HexagonGroup(new Vector3(xPos + (hexagonX * (1.0f - (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f), 0.0f)));
                hexagonGroups.Add(new HexagonGroup(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f)));

                //midPoints.Add(new Vector3(xPos + (hexagonX * (1.0f - (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f), 0.0f));
                //midPoints.Add(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f));
                
            }
        }

        // Creating hexagons
        for (int y = 0; y < tileCountY; y++)
        {
            for (int x = 0; x < tileCountX; x++)
            {
                float xPos = x * 1.5f * hexagonX + (spacing * x);
                float yPos = ((x % 2) * (hexagonY + (spacing / 2))) + (-2 * y * hexagonY) - (spacing * y);

                List<int> groupingNumbers = new List<int>();

                int rightTop = 0;
                int right = 0;
                int rightBot = 0;
                int leftTop = 0;
                int left = 0;
                int leftBot = 0;

                rightTop = (tileCountX - 1) * 2 * (y - 1) + (2 * x) + (1 - (x % 2));                // PERFECT
                leftTop = rightTop - 2;                                                             // PERFECT
                right = rightTop + (((tileCountX - 1) * 2) - 1) * (1 - (x % 2)) + (1 * (x % 2));    // PERFECT
                left = right - 2;                                                                   // PERFECT
                rightBot = rightTop + ((tileCountX - 1) * 2);                                       // PERFECT
                leftBot = leftTop + ((tileCountX - 1) * 2);                                         // PERFECT

                // Top grouping rules
                if (y != 0)
                {
                    if (x != 0)
                    {
                        groupingNumbers.Add(leftTop);
                        hexagonGroups[leftTop].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " left1-top is " + leftTop);
                    }

                    if (x != (tileCountX - 1))
                    {
                        groupingNumbers.Add(rightTop);
                        hexagonGroups[rightTop].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " right1-top is " + rightTop);
                    }
                }

                // Bot grouping rules
                if (y != (tileCountY - 1))
                {
                    if (x != 0)
                    {
                        groupingNumbers.Add(leftBot);
                        hexagonGroups[leftBot].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " left2-bottom is " + leftBot);
                    }

                    if (x != (tileCountX - 1))
                    {
                        groupingNumbers.Add(rightBot);
                        hexagonGroups[rightBot].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " right2-bottom is " + rightBot);
                    }
                }

                // Sides grouping rules
                if (x != 0)
                {
                    if ((y > 0 && y < (tileCountY - 1)) || (y == 0 && x % 2 == 0) || (y == (tileCountY - 1) && x % 2 == 1))
                    {
                        groupingNumbers.Add(left);
                        hexagonGroups[left].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " left3 is " + left);
                    }
                }
                if (x != (tileCountX - 1))
                {
                    if ((y > 0 && y < (tileCountY - 1)) || (y == 0 && x % 2 == 0) || (y == (tileCountY - 1) && x % 2 == 1))
                    {
                        groupingNumbers.Add(right);
                        hexagonGroups[right].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " right3 is " + right);
                    }
                }

                Hexagon hex = new Hexagon(new Vector3(xPos, yPos, 0.0f), groupingNumbers);          // Create the hex object.
                hexagons[x, y] = hex;                                                               // Add the hex object to array.
            }
        }

        ShowGrid();
    }
    
    private void ShowGrid()
    {
        for (int y = 0; y < hexagons.GetLength(1); y++)
        {
            for (int x = 0; x < hexagons.GetLength(0); x++)
            {
                Debug.Log("x is " + x + " y is " + y);
                SendHexagon(hexagons[x, y].GetPosition(), 1.0f);
            }
        }

        //foreach (var pos in hexagonPositions)
        //{
        //    SendHexagon(pos,1.0f);
        //}

        // For debugging purposes, delete on prod
        foreach (var group in hexagonGroups)
        {
            SendHexagon(group.GetPosition(), 0.15f);
        }


        FindGroup(new Vector3(1, 0, 0));
    }

    private void SendHexagon(Vector3 pos,float scale)
    {
        GameObject obj = Instantiate(gridGO);

        obj.transform.position = new Vector3(pos.x, pos.y, pos.z);
        obj.transform.localScale = new Vector3(scale, scale, scale);
        if (colors.Count != 0)
            obj.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Count)];
    }

    private void FindGroup(Vector3 clickPos)
    {
        Vector3 closestPoint = Vector3.zero;
        float smallestDistance = float.MaxValue;
        int midPointNumber = 0;

        List<int> selectedHexagonNumbers = new List<int>();

        for (int i = 0; i < hexagonGroups.Count; i++)
        {
            float clickDist = Vector3.Distance(clickPos, hexagonGroups[i].GetPosition());
            if (smallestDistance > clickDist)
            {
                smallestDistance = clickDist;
                closestPoint = hexagonGroups[i].GetPosition();
                midPointNumber = i;
            }
        }

        foreach (var num in hexagonGroups[midPointNumber].GetHexagonNumbersDeepCopy())
        {
            Debug.Log("selectedHexNumber is " + num);
        }

        Debug.Log("closest groupPoint to " + clickPos + " is " + closestPoint + " with a distance of " + smallestDistance);
    }
}
