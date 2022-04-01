using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node NorthNode;
    public Node SouthNode;
    public Node WestNode;
    public Node EastNode;
    public GameObject northLine;
    public GameObject southLine;
    public GameObject eastLine;
    public GameObject westLine;
    public int positionX;
    public int positionY;
    public Dictionary<Node, int> nodeLinks = new Dictionary<Node, int>();

    public void SetNodeLinks()
    {
        if (NorthNode != null)
        {
            nodeLinks.Add(NorthNode, 0);
        }
        if (SouthNode != null)
        {
            nodeLinks.Add(SouthNode, 0);
        }
        if (EastNode != null)
        {
            nodeLinks.Add(EastNode, 0);
        }
        if (WestNode != null)
        {
            nodeLinks.Add(WestNode, 0);
        }
    }

    public void LockNode(Node nodeToLock)
    {
        nodeLinks[nodeToLock] = 1;
    }
}
