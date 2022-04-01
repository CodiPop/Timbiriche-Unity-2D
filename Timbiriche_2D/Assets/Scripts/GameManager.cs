using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameState _gameState;
    public GameState GetGameState => _gameState;
    public int gridSizeX;
    public int gridSizeY;
    [SerializeField]private GameObject gridParent;
    [SerializeField]private GameObject linesParent;
    [SerializeField]private Material lineMaterial;
    [SerializeField]private Gradient unclaimedColor;
    [SerializeField]private Gradient playerColor;
    [SerializeField]private Gradient player2Color;
    [SerializeField]private GameObject nodePrefab;
    [SerializeField]private Sprite squareSprite;


    public Canvas canvas;
    public Button button;
    public Dropdown dropdown;

    private Node[,] grid;
    [SerializeField]private List<Node> squareAnchors = new List<Node>();



    public enum GameState
    {
        start,
        player1,
        player2,
        end

    }
    public void UpdateGameState(GameState gameState)
    {
        _gameState = gameState;
    }

    public void SwitchPlayer()
    {
        if (_gameState == GameState.player1)
        {
            _gameState = GameState.player2;
            Debug.Log(_gameState);
        }
        else
        {
            _gameState = GameState.player1;
            Debug.Log(_gameState);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        button.onClick.AddListener(() =>
        {
            canvas.gameObject.SetActive(false);
            int index = dropdown.value;
            int n = int.Parse(dropdown.options[index].text);
            Debug.Log(n);
            gridSizeY = n;
            gridSizeX = n;
            GridSetup();
            LineSetup();
            _gameState = GameState.start;

        }




            );

        


    }

    void Update()
    {
        switch (_gameState)
        {
            case GameState.start:
                break;
            case GameState.player1:
                break;
            case GameState.player2:
                break;
            case GameState.end:
                break;
            default:
                break;
        }
    }
    private void GridSetup()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Transform nodeAnchor = gridParent.transform.GetChild(0);
        GameObject currentNode;
        

        for (int i = 0; i < gridSizeY; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                currentNode = Instantiate(nodePrefab, gridParent.transform);
                currentNode.transform.position = new Vector3(nodeAnchor.position.x + (j * 0.6f), nodeAnchor.position.y - (i * 0.6f), 0);
                currentNode.GetComponent<Node>().positionX = j;
                currentNode.GetComponent<Node>().positionY = i;
                grid[j, i] = currentNode.GetComponent<Node>();
            }
        }
        
        NodeSetup();
    }

    private void NodeSetup()
    {
        grid[0, 0].GetComponent<Node>().SouthNode = grid[0, 1];
        grid[0, 0].GetComponent<Node>().EastNode = grid[1, 0];

        grid[gridSizeX - 1, 0].GetComponent<Node>().SouthNode = grid[gridSizeX - 1, 1];
        grid[gridSizeX - 1, 0].GetComponent<Node>().WestNode = grid[gridSizeX - 2, 0];

        grid[0, gridSizeY - 1].GetComponent<Node>().NorthNode = grid[0, gridSizeY - 2];
        grid[0, gridSizeY - 1].GetComponent<Node>().EastNode = grid[1, gridSizeY - 1];

        grid[gridSizeX - 1, gridSizeY - 1].GetComponent<Node>().NorthNode = grid[gridSizeX - 1, gridSizeY - 2];
        grid[gridSizeX - 1, gridSizeY - 1].GetComponent<Node>().WestNode = grid[gridSizeX - 2, gridSizeY - 1];

        for (int i = 1; i < gridSizeX - 1; i++)
        {
            grid[i, 0].GetComponent<Node>().WestNode = grid[i - 1, 0];
            grid[i, 0].GetComponent<Node>().SouthNode = grid[i, 1];
            grid[i, 0].GetComponent<Node>().EastNode = grid[i + 1, 0];

            grid[i, gridSizeY - 1].GetComponent<Node>().WestNode = grid[i - 1, gridSizeY - 1];
            grid[i, gridSizeY - 1].GetComponent<Node>().NorthNode = grid[i, gridSizeY - 2];
            grid[i, gridSizeY - 1].GetComponent<Node>().EastNode = grid[i + 1, gridSizeY - 1];
        }

        for (int i = 1; i < gridSizeY - 1; i++)
        {
            grid[0, i].GetComponent<Node>().NorthNode = grid[0, i - 1];
            grid[0, i].GetComponent<Node>().SouthNode = grid[0, i + 1];
            grid[0, i].GetComponent<Node>().EastNode = grid[1, i];

            grid[gridSizeX - 1, i].GetComponent<Node>().NorthNode = grid[gridSizeX - 1, i - 1];
            grid[gridSizeX - 1, i].GetComponent<Node>().SouthNode = grid[gridSizeX - 1, i + 1];
            grid[gridSizeX - 1, i].GetComponent<Node>().WestNode = grid[gridSizeX - 2, i];
        }

        for (int i = 1; i < gridSizeY - 1; i++)
        {
            for (int j = 1; j < gridSizeX - 1; j++)
            {
                grid[j, i].GetComponent<Node>().NorthNode = grid[j, i - 1];
                grid[j, i].GetComponent<Node>().SouthNode = grid[j, i + 1];
                grid[j, i].GetComponent<Node>().EastNode = grid[j + 1, i];
                grid[j, i].GetComponent<Node>().WestNode = grid[j - 1, i];
            }
        }

        for (int i = 0; i < gridSizeY; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                grid[j, i].GetComponent<Node>().SetNodeLinks();
            }
        }
    }

    private void LineSetup()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (var j = 0; j < gridSizeY; j++)
            {
                if (grid[j, i].SouthNode != null)
                {
                    GameObject newLine = new GameObject("Line");
                    newLine.transform.parent = linesParent.transform;
                    newLine.AddComponent<LineRenderer>();
                    newLine.GetComponent<LineRenderer>().SetPosition(0, grid[j, i].transform.position);
                    newLine.GetComponent<LineRenderer>().SetPosition(1, grid[j, i].SouthNode.transform.position);
                    newLine.GetComponent<LineRenderer>().startWidth = .1f;
                    newLine.GetComponent<LineRenderer>().endWidth = .1f;
                    newLine.GetComponent<LineRenderer>().material = lineMaterial;
                    newLine.GetComponent<LineRenderer>().colorGradient = unclaimedColor;
                    newLine.GetComponent<LineRenderer>().sortingOrder = -1;
                    grid[j, i].southLine = newLine;
                    grid[j, i].SouthNode.northLine = newLine;
                }

                if (grid[j, i].EastNode != null)
                {
                    GameObject newLine = new GameObject("Line");
                    newLine.transform.parent = linesParent.transform;
                    newLine.AddComponent<LineRenderer>();
                    newLine.GetComponent<LineRenderer>().SetPosition(0, grid[j, i].transform.position);
                    newLine.GetComponent<LineRenderer>().SetPosition(1, grid[j, i].EastNode.transform.position);
                    newLine.GetComponent<LineRenderer>().startWidth = .1f;
                    newLine.GetComponent<LineRenderer>().endWidth = .1f;
                    newLine.GetComponent<LineRenderer>().material = lineMaterial;
                    newLine.GetComponent<LineRenderer>().colorGradient = unclaimedColor;
                    newLine.GetComponent<LineRenderer>().sortingOrder = -1;
                    grid[j, i].eastLine = newLine;
                    grid[j, i].EastNode.westLine = newLine;
                }
            }
        }
    }

    public Node GetNode(int positionX, int positionY)
    {
        return grid[positionX, positionY];
    }

    public void CheckLink(Node firstNode, Node secondNode)
    {
        if (firstNode != secondNode)
        {
            if (firstNode.NorthNode == secondNode)
            {
                if (firstNode.nodeLinks[secondNode] == 0)
                {
                    firstNode.LockNode(secondNode);
                    secondNode.LockNode(firstNode);
                    
                    if (_gameState == GameState.player1)
                    {
                        firstNode.northLine.GetComponent<LineRenderer>().colorGradient = playerColor;
                    }
                    else
                    {
                        firstNode.northLine.GetComponent<LineRenderer>().colorGradient = player2Color;
                    }
                    squareAnchors.Add(firstNode);
                    squareAnchors.Add(secondNode);
                    Debug.Log("anchors num: " + squareAnchors.Count);
                    CheckForSquare(secondNode, firstNode, firstNode, 1);
                    squareAnchors.Remove(firstNode);
                    squareAnchors.Remove(secondNode);
                }
            }
            else if (firstNode.SouthNode == secondNode)
            {
                if (firstNode.nodeLinks[secondNode] == 0)
                {
                    firstNode.LockNode(secondNode);
                    secondNode.LockNode(firstNode);
                    
                    if (_gameState == GameState.player1)
                    {
                        firstNode.southLine.GetComponent<LineRenderer>().colorGradient = playerColor;
                    }
                    else
                    {
                        firstNode.southLine.GetComponent<LineRenderer>().colorGradient = player2Color;
                    }
                    squareAnchors.Add(firstNode);
                    squareAnchors.Add(secondNode);
                    CheckForSquare(secondNode, firstNode, firstNode, 1);
                    squareAnchors.Remove(firstNode);
                    squareAnchors.Remove(secondNode);
                }
            }
            else if (firstNode.EastNode == secondNode)
            {
                if (firstNode.nodeLinks[secondNode] == 0)
                {
                    firstNode.LockNode(secondNode);
                    secondNode.LockNode(firstNode);
                    
                    if (_gameState == GameState.player1)
                    {
                        firstNode.eastLine.GetComponent<LineRenderer>().colorGradient = playerColor;
                    }
                    else
                    {
                        firstNode.eastLine.GetComponent<LineRenderer>().colorGradient = player2Color;
                    }
                    squareAnchors.Add(firstNode);
                    squareAnchors.Add(secondNode);
                    CheckForSquare(secondNode, firstNode, firstNode, 1);
                    squareAnchors.Remove(firstNode);
                    squareAnchors.Remove(secondNode);
                }
            }
            else if (firstNode.WestNode == secondNode)
            {
                if (firstNode.nodeLinks[secondNode] == 0)
                {
                    firstNode.LockNode(secondNode);
                    secondNode.LockNode(firstNode);
                    if (_gameState == GameState.player1)
                    {
                        firstNode.westLine.GetComponent<LineRenderer>().colorGradient = playerColor;
                    }
                    else
                    {
                        firstNode.westLine.GetComponent<LineRenderer>().colorGradient = player2Color;
                    }
                    
                    squareAnchors.Add(firstNode);
                    squareAnchors.Add(secondNode);
                    CheckForSquare(secondNode, firstNode, firstNode, 1);
                    squareAnchors.Remove(firstNode);
                    squareAnchors.Remove(secondNode);
                }
            }
        }
    }

    private void CheckForSquare(Node nextNode, Node initialNode, Node previousNode, int steps)
    {
        if (steps > 3)
        {

            int repeatCounter = 0;
            foreach (Node n in squareAnchors)
            {
                if (n == initialNode)
                {
                    repeatCounter++;
                }
            }

            if (repeatCounter > 1)
            {
                DrawSquare();
            }
        }
        else
        {
            if (nextNode.NorthNode != null)
            {
                if (nextNode.nodeLinks[nextNode.NorthNode] != 0)
                {
                    if (previousNode == null)
                    {
                        steps++;
                        squareAnchors.Add(nextNode.NorthNode);
                        CheckForSquare(nextNode.NorthNode, initialNode, nextNode, steps);
                        squareAnchors.Remove(nextNode.NorthNode);
                        steps--;

                    }
                    else
                    {
                        if (previousNode != nextNode.NorthNode)
                        {
                            steps++;
                            squareAnchors.Add(nextNode.NorthNode);
                            CheckForSquare(nextNode.NorthNode, initialNode, nextNode, steps);
                            squareAnchors.Remove(nextNode.NorthNode);
                            steps--;

                        }
                    }
                }
            }

            if (nextNode.SouthNode != null)
            {
                if (nextNode.nodeLinks[nextNode.SouthNode] != 0)
                {
                    if (previousNode == null)
                    {
                        steps++;
                        squareAnchors.Add(nextNode.SouthNode);
                        CheckForSquare(nextNode.SouthNode, initialNode, nextNode, steps);
                        squareAnchors.Remove(nextNode.SouthNode);
                        steps--;
                    }
                    else
                    {
                        if (previousNode != nextNode.SouthNode)
                        {
                            steps++;
                            squareAnchors.Add(nextNode.SouthNode);
                            CheckForSquare(nextNode.SouthNode, initialNode, nextNode, steps);
                            squareAnchors.Remove(nextNode.SouthNode);
                            steps--;
                        }
                    }
                }
            }

            if (nextNode.EastNode != null)
            {
                if (nextNode.nodeLinks[nextNode.EastNode] != 0)
                {
                    if (previousNode == null)
                    {
                        steps++;
                        squareAnchors.Add(nextNode.EastNode);
                        CheckForSquare(nextNode.EastNode, initialNode, nextNode, steps);
                        squareAnchors.Remove(nextNode.EastNode);
                        steps--;
                    }
                    else
                    {
                        if (previousNode != nextNode.EastNode)
                        {
                            steps++;
                            squareAnchors.Add(nextNode.EastNode);
                            CheckForSquare(nextNode.EastNode, initialNode, nextNode, steps);
                            squareAnchors.Remove(nextNode.EastNode);
                            steps--;
                        }
                    }
                }
            }

            if (nextNode.WestNode != null)
            {
                if (nextNode.nodeLinks[nextNode.WestNode] != 0)
                {
                    if (previousNode == null)
                    {
                        steps++;
                        squareAnchors.Add(nextNode.WestNode);
                        CheckForSquare(nextNode.WestNode, initialNode, nextNode, steps);
                        squareAnchors.Remove(nextNode.WestNode);
                        steps--;
                    }
                    else
                    {
                        if (previousNode != nextNode.WestNode)
                        {
                            steps++;
                            squareAnchors.Add(nextNode.WestNode);
                            CheckForSquare(nextNode.WestNode, initialNode, nextNode, steps);
                            squareAnchors.Remove(nextNode.WestNode);
                            steps--;
                        }
                    }
                }
            }
        }
    }

    private void DrawSquare()
    {
        GameObject newSquare = new GameObject("Square");
        newSquare.transform.position = (squareAnchors[0].transform.position + squareAnchors[1].transform.position + squareAnchors[2].transform.position + squareAnchors[3].transform.position) / 4;
        newSquare.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        newSquare.AddComponent<SpriteRenderer>();
        newSquare.GetComponent<SpriteRenderer>().sprite = squareSprite;
        if (_gameState == GameState.player1)
        {
            newSquare.GetComponent<SpriteRenderer>().color = playerColor.colorKeys[0].color;
        }
        else
        {
            newSquare.GetComponent<SpriteRenderer>().color = player2Color.colorKeys[0].color;
        }
        if (_gameState == GameState.player1)
        {
            _gameState = GameState.player2;
            Debug.Log(_gameState);
        }
        else
        {
            _gameState = GameState.player1;
            Debug.Log(_gameState);
        }
        newSquare.GetComponent<SpriteRenderer>().sortingOrder = -2;
    }
}
