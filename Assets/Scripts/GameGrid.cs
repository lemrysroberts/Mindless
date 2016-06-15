using UnityEngine;
using System.Collections.Generic;

public class GameGrid : MonoBehaviour
{
    public int CellsX = 12;
    public int CellsY = 12;

    public GameObject CellObject = null;

    private GridCell[,] m_cells;

    private List<GridObject> m_gridObjects = new List<GridObject>();

    private GridObject m_hoveredObject = null;
    private GridObject m_selectedObject = null;

    // Use this for initialization
    void Start ()
    {
        m_cells = new GridCell[CellsX, CellsY];

        for (int x = 0; x < CellsX; ++x)
        {
            for (int y = 0; y < CellsY; ++y)
            {
                GameObject newObject = Instantiate(CellObject);
                newObject.transform.position = new Vector3(x, 0.0f, y);
                newObject.transform.parent = transform;
                m_cells[x, y] = newObject.GetComponent<GridCell>();
                m_cells[x, y].Init(x, y);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<GridObject> GetGridObjects()
    {
        return m_gridObjects;
    }

    public GridObject GetCellContents(int x, int y)
    {
        if (x < 0 || y < 0 || x >= CellsX || y >= CellsY)
        {
            Debug.LogError("Requested invalid cell: " + x + ", " + y);
            return null;
        }

        return m_cells[x, y].Contents;
    }

    public void AddObjectToGrid(GridObject gridObject)
    {
        bool slotFound = false;

        while(!slotFound)
        {
            int newX = Random.Range(0, CellsX);
            int newY = Random.Range(0, CellsY);

            if(!m_cells[newX, newY].IsOccupied())
            {
                m_gridObjects.Add(gridObject);
                gridObject.GetGameObject().transform.position = new Vector3(newX, 0.3f, newY);
                m_cells[newX, newY].OccupyCell(gridObject);
                slotFound = true;
            }
        }
    }

    public void ObjectHovered(GridObject gridObject)
    {
        if(gridObject != m_hoveredObject)
        {
            if (m_hoveredObject != null) { m_hoveredObject.HandleMouseExit(); }
            if (gridObject != null) { gridObject.HandleMouseOver(); }
            m_hoveredObject = gridObject;
        }
    }

    public void ObjectClicked(GridObject gridObject)
    {
        if (gridObject != m_selectedObject)
        {
            if (m_selectedObject != null) { m_selectedObject.OnDeselected(); }
            if (m_selectedObject != null) { m_selectedObject.OnAltDeselect(); }
            if (gridObject != null && gridObject.IsInteractive()) { gridObject.OnSelected(); }
            m_selectedObject = gridObject;
        }
    }

    public void PrimaryClicked()
    {
        if (m_selectedObject != null)
        {
            m_selectedObject.OnPrimarySelect();
        }
    }

    public void PrimaryReleased()
    {
        if (m_selectedObject != null)
        {
            m_selectedObject.OnPrimaryDeselect();
        }
    }

    public void AltClicked()
    {
        if(m_selectedObject != null)
        {
            m_selectedObject.OnAltSelect();
        }
    }

    public void AltReleased()
    {
        if (m_selectedObject != null)
        {
            m_selectedObject.OnAltDeselect();
        }
    }

    public void CellHovered(GridCell cell)
    {
        if (m_selectedObject != null && cell != null)
        {
            m_selectedObject.OnCellHovered(cell.X, cell.Y);
        }
    }

}
