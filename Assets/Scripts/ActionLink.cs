using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ActionLink : MonoBehaviour
{
    private Rider m_sourceObject    = null;
    private MeshRenderer m_renderer = null;
    private MeshFilter m_filter     = null;
    private Rider m_target          = null;
    private Rider m_previewTarget   = null;
    private int m_tempX             = -1;
    private int m_tempY             = -1;

    public Rider Target { get { return m_target; } }

    public void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();
        m_filter = GetComponent<MeshFilter>();

        BuildMesh();
    }

    public void SetSourceObject(GridObject sourceObject)
    {
        m_sourceObject = (Rider)sourceObject;
    }
	
    public void PreviewLink(GridObject previewTarget)
    {
        m_target = null;
        m_previewTarget = (Rider)previewTarget;
    }

    public void CommitPreviewLink()
    {
        m_target = m_previewTarget;
        m_previewTarget = null;
    }

    public void ShowTempLink(int x, int y)
    {
        m_tempX = x;
        m_tempY = y;

        m_previewTarget = null;
    }

    public void ClearTempLink()
    {
        m_tempX = -1;
        m_tempY = -1;
    }

    public void Highlight()
    {
        m_renderer.material.SetFloat("_selected", 1.0f);
    }

    public void Dehighlight()
    {
        m_renderer.material.SetFloat("_selected", 0.0f);
    }

    public void Update()
    {
        m_renderer.enabled = true;
        if (m_previewTarget != null)
        {
            Color mainColor = new Color(1.0f, 1.0f, 1.0f);

            if (m_previewTarget is Rider)
            {
                Rider otherRider = (Rider)m_previewTarget;

                if (otherRider.Gang == m_sourceObject.Gang)
                {
                    mainColor = new Color(0.0f, 1.0f, 0.0f);
                }
                else
                {
                    mainColor = new Color(1.0f, 0.0f, 0.0f);
                }
            }

            m_renderer.material.SetColor("_Color", mainColor);
            UpdateLinkQuad(m_previewTarget.transform.position);
        }

        else if (m_target != null)
        {
            Color mainColor = new Color(1.0f, 1.0f, 1.0f);

            if (m_target is Rider)
            {
                Rider otherRider = (Rider)m_target;

                if (otherRider.Gang == m_sourceObject.Gang)
                {
                    mainColor = new Color(0.0f, 1.0f, 0.0f);
                }
                else
                {
                    mainColor = new Color(1.0f, 0.0f, 0.0f);
                }
            }

            m_renderer.material.SetColor("_Color", mainColor);
            UpdateLinkQuad(m_target.transform.position);
        }
        else if(m_tempX != -1 && m_tempY != -1)
        {
            m_renderer.material.SetColor("_Color", Color.white);
            UpdateLinkQuad(new Vector3((float)m_tempX, 0.0f, (float)m_tempY));
        }
        else
        {
            m_renderer.enabled = false;
        }

    }
    
    private void UpdateLinkQuad(Vector3 targetPosition)
    {
        Vector3 diff = targetPosition - m_sourceObject.transform.position;

        Vector3 newPosition = (m_sourceObject.transform.position + diff / 2.0f);
        newPosition.y = 0.1f;
        transform.position = newPosition;
        Vector3 diffNorm = diff.normalized;

        m_renderer.material.SetFloat("_Scale", diff.magnitude / transform.lossyScale.z);

        float angle = Mathf.Atan2(diffNorm.z, diffNorm.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0.0f, -angle, 0.0f));

        transform.localScale = new Vector3(diff.magnitude * 2.0f, transform.localScale.y, transform.localScale.z);
    }
    
    private void BuildMesh()
    {
        Mesh newMesh = new Mesh();

        newMesh.name = "GeometryFactory:Plane";

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs0 = new Vector2[4];
        Vector2[] uvs1 = new Vector2[4];
        int[] triangles = new int[6];

        float vertDimensions = 0.5f;
        float UVXScale0 = 1.0f;
        float UVYScale0 = 1.0f;
        float UVXScale1 = 1.0f;
        float UVYScale1 = 1.0f;

        vertices[0] = new Vector3(-vertDimensions, 0.0f, -vertDimensions);
        vertices[1] = new Vector3(vertDimensions, 0.0f, -vertDimensions);
        vertices[2] = new Vector3(-vertDimensions, 0.0f, vertDimensions);
        vertices[3] = new Vector3(vertDimensions, 0.0f, vertDimensions);

        normals[0] = new Vector3(0.0f, 1.0f, 0.0f);
        normals[1] = new Vector3(0.0f, 1.0f, 0.0f);
        normals[2] = new Vector3(0.0f, 1.0f, 0.0f);
        normals[3] = new Vector3(0.0f, 1.0f, 0.0f);

        uvs0[0] = new Vector2(0.0f, 0.0f);
        uvs0[1] = new Vector2(UVXScale0, 0.0f);
        uvs0[2] = new Vector2(0.0f, UVYScale0);
        uvs0[3] = new Vector2(UVXScale0, UVYScale0);

        uvs1[0] = new Vector2(0.0f, 0.0f);
        uvs1[1] = new Vector2(UVXScale1, 0.0f);
        uvs1[2] = new Vector2(0.0f, UVYScale1);
        uvs1[3] = new Vector2(UVXScale1, UVYScale1);

        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        newMesh.vertices = vertices;
        newMesh.normals = normals;
        newMesh.uv = uvs0;
        newMesh.uv2 = uvs1;
        newMesh.triangles = triangles;

        newMesh.RecalculateBounds();

        m_filter.sharedMesh = newMesh;
    }
}
