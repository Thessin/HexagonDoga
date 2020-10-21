using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class GridSystem : Singleton<GridSystem>
{
    [SerializeField] private GameObject gridGO;
    [SerializeField] private GameObject selectionGO;
    [SerializeField] private GameObject bombGO;

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

    private HexagonGroup selectedGroup;
    private List<GameObject> selectionObjects = new List<GameObject>();

    private float sendHexagonAnimationDuration = 0.4f;

    public UnityEvent OnActionMade;     // Invoked when user tried to make a move.

    private void Start()
    {
        hexagonX = gridGO.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        hexagonY = gridGO.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        hexagons = new Hexagon[tileCountX, tileCountY];

        OnActionMade = new UnityEvent();

        CreateGrid();
    }

    /// <summary>
    /// Create the grid.
    /// </summary>
    private void CreateGrid()
    {
        // Creating grouping points
        CreateGroupingPoints();

        // Creating hexagons
        CreateHexagons();

        // Showing grid
        ShowGrid();
    }
    
    /// <summary>
    /// Create grouping points(HexagonGroups).
    /// </summary>
    private void CreateGroupingPoints()
    {
        for (int y = 0; y < tileCountY - 1; y++)
        {
            for (int x = 0; x < tileCountX - 1; x++)
            {
                // For grouping points

                float xPos = x * 1.5f * hexagonX + (spacing * x);

                hexagonGroups.Add(new HexagonGroup(new Vector3(xPos + (hexagonX * (1.0f - (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f), 0.0f), (x * y)));
                hexagonGroups.Add(new HexagonGroup(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f), (x * y) + 1));
            }
        }
    }

    /// <summary>
    /// Create the hexagons.
    /// </summary>
    private void CreateHexagons()
    {
        Color lastGivenColor = Color.white;
        
        for (int y = 0; y < tileCountY; y++)
        {
            for (int x = 0; x < tileCountX; x++)
            {
                float xPos = x * 1.5f * hexagonX + (spacing * x);
                float yPos = ((x % 2) * (hexagonY + (spacing / 2))) + (-2 * y * hexagonY) - (spacing * y);

                List<HexagonGroup> groupingNumbers = new List<HexagonGroup>();

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
                        groupingNumbers.Add(hexagonGroups[leftTop]);
                        //hexagonGroups[leftTop].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " left1-top is " + leftTop);
                    }

                    if (x != (tileCountX - 1))
                    {
                        groupingNumbers.Add(hexagonGroups[rightTop]);
                        //hexagonGroups[rightTop].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " right1-top is " + rightTop);
                    }
                }

                // Bot grouping rules
                if (y != (tileCountY - 1))
                {
                    if (x != 0)
                    {
                        groupingNumbers.Add(hexagonGroups[leftBot]);
                        //hexagonGroups[leftBot].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " left2-bottom is " + leftBot);
                    }

                    if (x != (tileCountX - 1))
                    {
                        groupingNumbers.Add(hexagonGroups[rightBot]);
                        //hexagonGroups[rightBot].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " right2-bottom is " + rightBot);
                    }
                }

                // Sides grouping rules
                if (x != 0)
                {
                    if ((y > 0 && y < (tileCountY - 1)) || (y == 0 && x % 2 == 0) || (y == (tileCountY - 1) && x % 2 == 1))
                    {
                        groupingNumbers.Add(hexagonGroups[left]);
                        //hexagonGroups[left].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " left3 is " + left);
                    }
                }
                if (x != (tileCountX - 1))
                {
                    if ((y > 0 && y < (tileCountY - 1)) || (y == 0 && x % 2 == 0) || (y == (tileCountY - 1) && x % 2 == 1))
                    {
                        groupingNumbers.Add(hexagonGroups[right]);
                        //hexagonGroups[right].AddHexagonNumber((y * tileCountX) + x);

                        Debug.Log("x is " + x + " y is " + y + " right3 is " + right);
                    }
                }

                Hexagon hex = new Hexagon(new Vector3(xPos, yPos, 0.0f), groupingNumbers, x, y);          // Create the hex object.
                hex.RegisterToHexagonGroup();                                                       // Registering the hexagon to it's related group.

                do
                {
                    hex.color = colors[Random.Range(0, colors.Count)];
                } while (colors.Count > 1 && hex.color.IsEqualColor(lastGivenColor));              // Adding color information to the created hexagon.
                lastGivenColor = hex.color;

                hexagons[x, y] = hex;                                                               // Add the hex object to array.
            }
        }
    }

    /// <summary>
    /// Show grid to the user.
    /// </summary>
    private void ShowGrid()
    {
        for (int y = 0; y < hexagons.GetLength(1); y++)
        {
            for (int x = 0; x < hexagons.GetLength(0); x++)
            {
                SendHexagon(hexagons[x, y]);
            }
        }
    }

    /// <summary>
    /// Send hexagon to its specified place.
    /// </summary>
    /// <param name="hex"></param>
    /// <param name="cb"></param>
    private Tween SendHexagon(Hexagon hex)
    {
        GameObject obj = Instantiate(gridGO);
        Vector3 pos = hex.GetPosition();

        obj.transform.position = CameraSystem.Instance.GetCamPosition();
        Tween t = obj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), sendHexagonAnimationDuration);

        obj.GetComponent<SpriteRenderer>().color = hex.color;

        hex.SetGameObject(obj);

        return t;
    }

    /// <summary>
    /// Select a group.
    /// </summary>
    /// <param name="clickPos"></param>
    public void SelectGroup(Vector3 clickPos)
    {
        if (selectedGroup != null)      // When a selection already exists, clear it.
        {
            RemoveSelection();

            return;
        }

        Vector3 closestPoint = Vector3.zero;
        float smallestDistance = float.MaxValue;
        int midPointNumber = 0;

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

        selectedGroup = hexagonGroups[midPointNumber];

        foreach (var trans in hexagonGroups[midPointNumber].GetHexagonsTransformList())
        {
            GameObject obj = Instantiate(selectionGO, trans);
            selectionObjects.Add(obj);
        }

        Debug.Log("closest groupPoint to " + clickPos + " is " + closestPoint + " with a distance of " + smallestDistance);
        Debug.Log("selected grouping is " + midPointNumber);
    }

    /// <summary>
    /// Remove the current group selection.
    /// </summary>
    public void RemoveSelection()
    {
        Debug.Log("attempting to delete selection objects");

        foreach (var obj in selectionObjects)
        {
            Debug.Log("in selectionObjects");
            Destroy(obj);
        }
        selectionObjects.Clear();
        selectedGroup = null;
    }

    /// <summary>
    /// Check to see if any move exists. 
    /// </summary>
    public void CheckIfMoveExists()
    {
        bool result = false;

        for (int y = 0; y < hexagons.GetLength(1); y++)
        {
            if (result)         // Don't need more cycles since at least a move is found.
                break;

            for (int x = 0; x < hexagons.GetLength(0); x++)
            {
                List<Color> surroundingColors = new List<Color>();

                int[,] leftTop = { { x - 1 }, { y - (x % 2) } };
                int[,] rightTop = { { x + 1 }, { y - (x % 2) } };
                int[,] leftBot = { { x - 1 }, { y + 1 - (x % 2) } };
                int[,] rightBot = { { x + 1 }, { y + 1 - (x % 2) } };
                int[,] top = { { x }, { y - 1 } };
                int[,] bot = { { x }, { y + 1 } };

                if (x > 0)
                {
                    if (y > 0)
                        surroundingColors.Add(hexagons[leftTop[0, 0], leftTop[1, 0]].color);
                    if (y < tileCountY - 1)
                        surroundingColors.Add(hexagons[leftBot[0, 0], leftBot[1, 0]].color);
                }
                if (x < tileCountX - 1)
                {
                    if (y > 0)
                        surroundingColors.Add(hexagons[rightTop[0, 0], rightTop[1, 0]].color);
                    if (y < tileCountY - 1)
                        surroundingColors.Add(hexagons[rightBot[0, 0], rightBot[1, 0]].color);
                }

                if (y > 0)
                    surroundingColors.Add(hexagons[top[0, 0], top[1, 0]].color);
                else if (y < tileCountY - 1)
                    surroundingColors.Add(hexagons[bot[0, 0], bot[1, 0]].color);

                if(surroundingColors.GroupBy(item => item).Where(item => item.Count() > 1).Sum(item => item.Count()) >= 3)
                {
                    Debug.Log("found a move on x:" + x + " y:" + y);
                    result = true;
                    break;
                }
            }
        }

        if (!result) GameManager.Instance.GameFinished();   // If no more move can be made, the game is finished.
    }

    /// <summary>
    /// When a group of hexagons are destroyed, this function is called to make GridCreator create another set of hexagon objects.
    /// </summary>
    /// <param name="hex"></param>
    public void HexagonsDestroyed(List<Hexagon> hexList)
    {
        Sequence seq = DOTween.Sequence();

        foreach (var hex in hexList)
        {
            hex.color = colors[Random.Range(0, colors.Count)];

            seq.Append(SendHexagon(hex));
        }

        seq.OnComplete(() =>
        {
            ExplodeAllDestructibleGroups();
        });

        RemoveSelection();
        CheckIfMoveExists();
    }

    /// <summary>
    /// Explodes all groups that can be destructed. Needs to be called after new set of hexagons were created and they move into their places.
    /// </summary>
    private void ExplodeAllDestructibleGroups()
    {
        Debug.LogWarning("EXPLODING ALL DESTRUCTIBLE GROUPS");

        bool foundDestructible = false;

        foreach (var group in hexagonGroups)
        {
            if (group.IsExplodable())
            {
                group.DestroyGroup();
                foundDestructible = true;
            }
        }
    }

    /// <summary>
    /// Turn the selected HexagonGroup.
    /// </summary>
    /// <param name="clockWise"></param>
    public void TurnSelected(bool clockWise)
    {
        if (selectedGroup != null)
        {
            selectedGroup.RotateGroup(clockWise);
            OnActionMade?.Invoke();
        }
    }

    /// <summary>
    /// Attaches a bomb to a random hexagon.
    /// </summary>
    public void AttachBombToRandom()
    {
        Instantiate(bombGO, hexagons[Random.Range(0, tileCountX), Random.Range(0, tileCountY)].GetTransform());
    }

    /// <summary>
    /// Recreates the grid.
    /// </summary>
    public void RecreateGrid()
    {
        foreach (var hex in hexagons)
        {
            Destroy(hex.obj);
        }

        hexagonGroups.Clear();

        CreateGrid();
    }
}
