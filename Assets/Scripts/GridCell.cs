using UnityEngine;
using System.Collections;

public class GridCell : MonoBehaviour
{
    public int X { get { return m_x; } }
    public int Y { get { return m_y; } }

    public GridObject Contents { get { return m_occupyingObject; } }

    private GridObject m_occupyingObject = null;
    private int m_x;
    private int m_y;

    public void Start()
    {
    }

    public void Init(int x, int y)
    {
        m_x = x;
        m_y = y;
    }

    public void OccupyCell(GridObject gridObject)
    {
        if(m_occupyingObject != null)
        {
            Debug.LogError("Adding multiple objects to cell");
        }

        m_occupyingObject = gridObject;
    }

    public void VacateCell()
    {
        m_occupyingObject = null;
    }

    public bool IsOccupied()
    {
        return m_occupyingObject != null;
    }

    
}
