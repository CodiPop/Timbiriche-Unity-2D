using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Node firstNode;
    [SerializeField]
    private Node secondNode;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Node nodeHit = hit.collider.GetComponent<Node>();
                firstNode = GameManager.instance.GetNode(nodeHit.positionX, nodeHit.positionY);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (firstNode != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    Node nodeHit = hit.collider.GetComponent<Node>();
                    secondNode = GameManager.instance.GetNode(nodeHit.positionX, nodeHit.positionY);
                    GameManager.instance.CheckLink(firstNode, secondNode);
                    GameManager.instance.SwitchPlayer();
                }
            }

            ResetNodes();
        }
    }

    private void ResetNodes()
    {
        firstNode = null;
        secondNode = null;
    }
}
