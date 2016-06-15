using UnityEngine;
using System.Collections;

public class PlanningState :  GameState
{
    private GameGrid m_grid = null;

    public void Init ()
    {
        m_grid = GameObject.FindObjectOfType<GameGrid>();
    }

    public void BeginState()
    {

    }

    public void EndState()
    {

    }

    public void UpdateState ()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (hits.Length > 0)
        {
            foreach (var hitInfo in hits)
            {
                // Check hovers
                var gridObject = hitInfo.collider.gameObject.GetComponent(typeof(GridObject));
                m_grid.ObjectHovered((GridObject)gridObject);

                if (hitInfo.collider.gameObject.GetComponent<Rider>() != null)
                {
                    // Check general clicks
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                    {
                        m_grid.ObjectClicked((GridObject)gridObject);
                    }
                }

                if(hitInfo.collider.gameObject.GetComponent<GridCell>() != null)
                {
                    var gridCell = hitInfo.collider.gameObject.GetComponent(typeof(GridCell));
                    m_grid.CellHovered((GridCell)gridCell);
                }
            }

            // Check left click
            if (Input.GetMouseButtonDown(0))
            {
                m_grid.PrimaryClicked();
            }

            // Check right click
            if (Input.GetMouseButtonDown(1))
            {
                m_grid.AltClicked();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_grid.PrimaryReleased();
        }

        if (Input.GetMouseButtonUp(1))
        {
            m_grid.AltReleased();
        }
    }

    public string GetStateName()
    {
        return "planning";
    }
}
