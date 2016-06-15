using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RiderStat
{
    public string StatName;
    public float StatValue;
}

[RequireComponent(typeof(MeshRenderer))]
public class Rider : MonoBehaviour, GridObject
{
    public Gang Gang { get { return m_gang; } }
    public List<RiderStat> Stats {  get { return m_stats; } }

    public GameObject RouteObject = null;
    public GameObject LinkObject = null;
    public GameObject DamageObject = null;
    public ProgressBar HealthProgress = null;
    public bool Interactive = true;

    private MeshRenderer m_renderer      = null;
    private MeshFilter m_filter        = null;
    private GameObject m_highlightObject = null;
    private RiderRoute m_route           = null;
    private ActionLink m_link            = null;
    private GameGrid m_grid              = null;
    private UIRiderStats m_uiStats       = null;
    private Gang m_gang                  = null;

    private bool m_planningRoute = false;
    private bool m_linking = false;

    private int m_cellX = -1;
    private int m_cellY = -1;

    private float m_health = 100.0f;

    private List<RiderStat> m_stats = new List<RiderStat>();

    // Use this for initialization
    void Start()
    {
        m_grid = FindObjectOfType<GameGrid>();
        m_uiStats = FindObjectOfType<UIRiderStats>();
        m_renderer = GetComponent<MeshRenderer>();
        m_filter = GetComponent<MeshFilter>();

        // Add route
        GameObject routeObject = Instantiate(RouteObject);
        routeObject.transform.parent = transform;
        routeObject.transform.localPosition = Vector3.zero;
        m_route = routeObject.GetComponent<RiderRoute>();

        // Add link
        GameObject linkObject = Instantiate(LinkObject);
        linkObject.transform.parent = transform;
        linkObject.transform.localPosition = Vector3.zero;
        m_link = linkObject.GetComponent<ActionLink>();
        m_link.SetSourceObject(this);

        // Find highlight
        if (transform.childCount > 0)
        {
            m_highlightObject = transform.GetChild(0).gameObject;
        }

        if (m_highlightObject != null) { m_highlightObject.GetComponent<MeshRenderer>().enabled = false; }

        m_stats.Add(new RiderStat { StatName = "speed", StatValue = Random.Range(5.0f, 9.0f) });


        m_route.SetNodeLimit( (int)m_stats.Find(x => x.StatName == "speed").StatValue);
    }
	
	void Update ()
    {
    }

    public void SetGang(Gang gang)
    {
        m_gang = gang;
    }

    public bool IsInteractive()
    {
        return Interactive;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void HandleMouseOver()
    {
        m_renderer.material.SetFloat("_selected", 1.0f);
        m_link.Highlight();

        m_uiStats.SetRider(this);
    }

    public void HandleMouseExit()
    {
        m_renderer.material.SetFloat("_selected", 0.0f);

        if(!IsHighlightEnabled())
        {
            m_link.Dehighlight();
        }

        m_uiStats.SetRider(null);
    }

    public void OnSelected()
    {
        SetHighlightEnabled(true);
        m_link.Highlight();
    }

    public void OnDeselected()
    {
        SetHighlightEnabled(false);
        m_link.Dehighlight();
    }

    public void OnPrimarySelect()
    {
        if (!Interactive) return;
        m_linking = true;
    }

    public void OnPrimaryDeselect()
    {
        if (!Interactive) return;

        m_link.CommitPreviewLink();

        m_linking = false;
    }

    public void OnAltSelect()
    {
        m_route.EnablePreview = true;

        if (!Interactive) return;

        m_route.ClearNodes();
        m_planningRoute = true;
    }

    public void OnAltDeselect()
    {
        m_route.EnablePreview = false;
        m_route.HidePreview();

        if (!Interactive) return;
        m_planningRoute = false;
    }

    public void OnCellHovered(int cellX, int cellY)
    {
        if (!Interactive) return;

        m_link.ClearTempLink();

        if (m_planningRoute)
        {
            m_route.AddNode(cellX, cellY);
        }

        if(m_linking)
        {
            GridObject cellContents = m_grid.GetCellContents(cellX, cellY);
            if (cellContents != null && (Rider)cellContents != this)
            {
                m_link.PreviewLink(cellContents);
            }
            else
            {
                m_link.ShowTempLink(cellX, cellY);
            }
        }
    }

    public void UpdateMovement(float progress)
    {
        Vector2 newPos = m_route.GetPositionAlongRoute(progress);

        transform.position = new Vector3(newPos.x, transform.position.y, newPos.y);
        m_cellX = (int)newPos.x;
        m_cellY = (int)newPos.y;
    }

    public void UpdateAction(float progress)
    {
        if(m_link.Target != null)
        {
            if(m_link.Target.Gang != m_gang)
            {
                int targetX = -1;
                int targetY = -1;

                m_link.Target.GetGridPosition(ref targetX, ref targetY);

                if (Mathf.Abs(targetX - m_cellX) > 1) return;
                if (Mathf.Abs(targetY - m_cellY) > 1) return;

                m_link.Target.ApplyDamage(Random.Range(0.0f, 100.0f));
            }
        }
    }

    public void OnEndRound()
    {
        m_route.ClearNodes();
    }

    public void ApplyDamage(float damage)
    {
        GameObject damageObject = Instantiate(DamageObject);
        damageObject.transform.position = transform.position;
        TextMesh text = damageObject.GetComponent<TextMesh>();

        m_health -= damage;

        if (text != null) { text.text = damage.ToString("0.0"); }
        if(HealthProgress != null) { HealthProgress.SetProgress(m_health / 100.0f); }
    }

    private void SetHighlightEnabled(bool enabled)
    {
        if (m_highlightObject != null) { m_highlightObject.GetComponent<MeshRenderer>().enabled = enabled; }
    }

    private bool IsHighlightEnabled()
    {
        if (m_highlightObject != null) { return m_highlightObject.GetComponent<MeshRenderer>().enabled; }

        return false;
    }

    public void SetGridPosition(int cellX, int cellY)
    {
        m_cellX = cellX;
        m_cellY = cellY;

        transform.position = new Vector3(m_cellX, 0.0f, m_cellY);
    }

    public void GetGridPosition(ref int cellX, ref int cellY)
    {
        cellX = m_cellX;
        cellY = m_cellY;
    }
}
