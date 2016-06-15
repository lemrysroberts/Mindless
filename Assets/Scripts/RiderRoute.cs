using UnityEngine;
using System.Linq;

using System.Collections.Generic;
using System;

public struct RiderRouteNode
{
    public int x;
    public int y;

    public static implicit operator Vector2(RiderRouteNode node)
    {
        return new Vector2(node.x, node.y);
    }
}

public class RiderRoute : MonoBehaviour
{
    public bool EnablePreview
    {
        get;
        set;
    }

    public GameObject RangePreviewObject = null;
    private List<RiderRouteNode> m_nodes = new List<RiderRouteNode>();
    private GameGrid m_grid = null;
    private int m_nodeLimit = 5;

    private static List<GameObject> m_rangePreviewTiles = new List<GameObject>();

    public void Start()
    {
        m_grid = FindObjectOfType<GameGrid>();

        ClearNodes();
        EnablePreview = false;
    }

    public void SetNodeLimit(int limit)
    {
        m_nodeLimit = limit;
    }

    public void AddNode(int x, int y)
    {
        // Handle backing out a node
        if (m_nodes.Count > 2)
        {
            if (m_nodes[m_nodes.Count - 2].x == x && m_nodes[m_nodes.Count - 2].y == y)
            {
                m_nodes.RemoveAt(m_nodes.Count - 1);
            }
        }

        // Don't allow more nodes than the limit
        if (m_nodes.Count >= m_nodeLimit)
        {
            return;
        }

        // Dont allow entering a cell that another team-mate is also entering

        // Don't allow jumping more than one node in any direction at a time
        if(m_nodes.Count > 1)
        {
            int lastX = m_nodes[m_nodes.Count - 1].x;
            int lastY = m_nodes[m_nodes.Count - 1].y;

            if(Mathf.Abs(x - lastX) > 1 || Mathf.Abs(y - lastY) > 1)
            {
                return;
            }
        }

       

        if(m_nodes.Any(node => node.x == x && node.y == y)) { return; }

        RiderRouteNode newNode = new RiderRouteNode();
        newNode.x = x;
        newNode.y = y;

        m_nodes.Add(newNode);
    }

    public void ClearNodes()
    {
        m_nodes.Clear();

        RiderRouteNode newNode = new RiderRouteNode();
        newNode.x = (int)transform.position.x;
        newNode.y = (int)transform.position.z;
        m_nodes.Add(newNode);
    }

    public void Update()
    {
        int lastX = (int)m_nodes[0].x;
        int lastY = (int)m_nodes[0].y;

        foreach (var node in m_nodes)
        {
            Debug.DrawLine(new Vector3(lastX, 0.1f, lastY), new Vector3(node.x, 0.1f, node.y));

            lastX = node.x;
            lastY = node.y;
        }

        if(EnablePreview)
        {
            ShowPreview(lastX, lastY, m_nodeLimit - m_nodes.Count);
        }
    }

    public Vector2 GetPositionAlongRoute(float progress)
    {
        int nodeIndex = (int)((float)(m_nodes.Count - 1) * progress);

        return m_nodes[nodeIndex];
    }

    private void ShowPreview(int cellX, int cellY, int range)
    {
        HidePreview();

        int minX = Math.Max(cellX - range, 0);
        int maxX = Math.Min(cellX + range + 1, m_grid.CellsX);
        int minY = Math.Max(cellY - range, 0);
        int maxY = Math.Min(cellY + range + 1 , m_grid.CellsY);

        for (int x = minX; x < maxX; ++x)
        {
            for (int y = minY; y < maxY; ++y)
            {
                int previewIndex = ((maxY - minY) * (x - minX)) + (y - minY);

                int distanceToTile = Math.Abs(cellX - x) + Math.Abs(cellY - y);
                if (distanceToTile >= range) continue;

                while(previewIndex >= m_rangePreviewTiles.Count) { m_rangePreviewTiles.Add(Instantiate(RangePreviewObject)); }

                GameObject preview = m_rangePreviewTiles[previewIndex];
                preview.transform.position = new UnityEngine.Vector3(x, 0.1f, y);
                preview.SetActive(true);
            }
        }
    }

    public void HidePreview()
    {
        foreach (var previewTile in m_rangePreviewTiles)
        {
            previewTile.SetActive(false);
        }
    }
}
