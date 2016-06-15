using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof (MeshFilter))]
public class GeometryFactory : MonoBehaviour 
{
	public enum GeometryType
	{
		Plane,	
		ScreenPlane,
        Line,
        Circle
	}
	
	public GeometryType geometryType      = GeometryType.Plane;
	public bool ScaleUVs                  = false;
    public float VertScale                = 1.0f;
	public float UVScale0                 = 1.0f;
	public float UVScale1                 = 1.0f;
	public Camera ScreenPlaneTargetCamera = null;
	public bool ForceRebuildOnStart       = false;
    public Vector3 LineStart              = Vector3.zero;
    public Vector3 LineEnd                = Vector3.zero;
    public float LineWidth                = 1.0f;

    public float CircleRadius = 1.0f;
    public int CircleSegments = 20;

	[SerializeField]
	private bool m_meshBuilt = false;

	[SerializeField]
	private Mesh m_mesh = null;

	private float m_height = 0.0f;

	// Use this for initialization
	void Start () 
	{

		if (!m_meshBuilt || ForceRebuildOnStart)
		{
        #if UNITY_EDITOR
            // In editor, the camera takes a frame or so to snap to play dimensions, so defer building the geometry for a moment
            if(geometryType == GeometryType.ScreenPlane)
            {

                StartCoroutine(DeferScreenPlane());
            }
            else
        #endif
            {
                RebuildMesh();	
            }
		}

        if(geometryType == GeometryType.Line && LineStart == Vector3.zero && LineEnd == Vector3.zero)
        {
            LineStart = transform.position - new Vector3(10.0f, 0.0f, 0.0f);
            LineEnd = transform.position + new Vector3(10.0f, 0.0f, 0.0f);
        }
	}

	public void RebuildMesh()
	{
		MeshFilter mesh = GetComponent<MeshFilter>();

		if (mesh != null)
		{
            if(geometryType == GeometryType.Line)
            {
                m_mesh = CreateLine(LineStart, LineEnd, LineWidth, transform.localScale.x * UVScale0, transform.localScale.z * UVScale0, transform.localScale.x * UVScale1, transform.localScale.z * UVScale1, m_height);
                mesh.sharedMesh = m_mesh; 
            }

            if (ScaleUVs)
			{
				switch (geometryType)
				{
					case GeometryType.Plane:
						{
							m_mesh = CreatePlane(transform.localScale.x * UVScale0, transform.localScale.z * UVScale0, transform.localScale.x * UVScale1, transform.localScale.z * UVScale1, m_height, VertScale); 
							mesh.sharedMesh = m_mesh; 
							break;
						}

					case GeometryType.ScreenPlane:
						{
                            m_mesh = CreateScreenPlane(ScreenPlaneTargetCamera, transform.localScale.x * UVScale0, transform.localScale.z * UVScale0, transform.localScale.x * UVScale1, transform.localScale.z * UVScale1, m_height, VertScale); 
							mesh.sharedMesh = m_mesh;
							break;
						}
                    case GeometryType.Circle:
                        {
                            m_mesh = CreateCircle(CircleRadius, CircleSegments, m_height, transform.localScale.x * UVScale0, transform.localScale.z * UVScale0, transform.localScale.x * UVScale1, transform.localScale.z * UVScale1);
                            mesh.sharedMesh = m_mesh;
                            break;
                        }
				}
			}
			else
			{
				switch (geometryType)
				{
					case GeometryType.Plane:
						{
                            m_mesh = CreatePlane(1.0f, 1.0f, 1.0f, 1.0f, m_height, VertScale);
							mesh.sharedMesh = m_mesh;
							break;
						}

					case GeometryType.ScreenPlane:
						{
                            m_mesh = CreateScreenPlane(ScreenPlaneTargetCamera, 1.0f, 1.0f, 1.0f, 1.0f, m_height, VertScale);
							mesh.sharedMesh = m_mesh;
							break;
						}
                    case GeometryType.Circle:
                        {
                            m_mesh = CreateCircle(CircleRadius, CircleSegments, m_height, 1.0f, 1.0f, 1.0f, 1.0f);
                            mesh.sharedMesh = m_mesh;
                            break;
                        }
                }
			}
			m_meshBuilt = true;
		}
		else
		{

		}
	}
	
	public static Mesh CreatePlane(float UVXScale0, float UVYScale0, float UVXScale1, float UVYScale1, float height, float vertScale)
	{
		Mesh newMesh = new Mesh();
		
		newMesh.name = "GeometryFactory:Plane";
		
		Vector3[] 	vertices 	= new Vector3[4];
		Vector3[] 	normals 	= new Vector3[4];
		Vector2[] 	uvs0 		= new Vector2[4];
		Vector2[] 	uvs1 		= new Vector2[4];
		int[] 		triangles 	= new int[6];

        float vertDimensions = 0.5f * vertScale;

        vertices[0] = new Vector3(-vertDimensions, height, -vertDimensions);
		vertices[1] = new Vector3(vertDimensions, height, -vertDimensions);
		vertices[2] = new Vector3(-vertDimensions, height, vertDimensions);
		vertices[3] = new Vector3(vertDimensions, height, vertDimensions);
		
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
		
		return newMesh;
	}
	
	public static Mesh CreateScreenPlane(Camera targetCamera, float UVXScale0, float UVYScale0, float UVXScale1, float UVYScale1, float vertHeight, float vertScale)
	{
		if(targetCamera == null)
		{
			Debug.LogWarning("Target Camera not set when creating screen-plane. Defaulting to plane.");
			return CreatePlane(UVXScale0, UVYScale0, UVXScale1, UVYScale1, vertHeight, vertScale);
		}
		
        
        
		Mesh newMesh = new Mesh();
		
		newMesh.name = "GeometryFactory:ScreenPlane";

        Vector3 worldPosStart = targetCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -10.0f));
        Vector3 worldPosEnd = targetCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, -10.0f));
        Debug.DrawLine(worldPosStart, worldPosEnd, Color.red, 100.0f);
		
		float width = (worldPosEnd - worldPosStart).x / 2.0f;
        float height = (worldPosEnd - worldPosStart).z / 2.0f;
		
		Vector3[] 	vertices 	= new Vector3[4];
		Vector3[] 	normals 	= new Vector3[4];
		Vector2[] 	uvs0 		= new Vector2[4];
		Vector2[] 	uvs1 		= new Vector2[4];
		int[] 		triangles 	= new int[6];
		
		vertices[0] = new Vector3(-width, vertHeight, -height);
		vertices[1] = new Vector3(width, vertHeight, -height);
		vertices[2] = new Vector3(-width, vertHeight, height);
		vertices[3] = new Vector3(width, vertHeight, height);
		
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
		
		return newMesh;
	}

    public static Mesh CreateLine(Vector3 start, Vector3 end, float width, float UVXScale0, float UVYScale0, float UVXScale1, float UVYScale1, float height)
    {
        Mesh newMesh = new Mesh();

        newMesh.name = "GeometryFactory:Plane";

        Vector3 tangent = Vector3.Cross((start - end).normalized, Vector3.up);
        float length = (start - end).magnitude;
        float angle = Mathf.Acos(Vector3.Dot(Vector3.right, (end - start).normalized));

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs0 = new Vector2[4];
        Vector2[] uvs1 = new Vector2[4];
        int[] triangles = new int[6];

        vertices[2] = new Vector3(start.x - (tangent.x * width), height, start.z -(tangent.z * width));
        vertices[3] = new Vector3(end.x - (tangent.x * width), height, end.z - (tangent.z * width));
        vertices[0] = new Vector3(start.x + (tangent.x * width), height, start.z + (tangent.z * width));
        vertices[1] = new Vector3(end.x + (tangent.x * width), height, end.z + (tangent.z * width));

        normals[0] = new Vector3(0.0f, 1.0f, 0.0f);
        normals[1] = new Vector3(0.0f, 1.0f, 0.0f);
        normals[2] = new Vector3(0.0f, 1.0f, 0.0f);
        normals[3] = new Vector3(0.0f, 1.0f, 0.0f);

        uvs0[0] = new Vector2(0.0f, 0.0f);
        uvs0[1] = new Vector2(UVXScale0 * length, 0.0f);
        uvs0[2] = new Vector2(0.0f, UVYScale0);
        uvs0[3] = new Vector2(UVXScale0 * length, UVYScale0);

        uvs1[0] = new Vector2(0.0f, 0.0f);
        uvs1[1] = new Vector2(UVXScale1 * length, 0.0f);
        uvs1[2] = new Vector2(0.0f, UVYScale1);
        uvs1[3] = new Vector2(UVXScale1 * length, UVYScale1);

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

        return newMesh;
    }

    public static Mesh CreateCircle(float radius, int numSegments, float height, float UVXScale0, float UVYScale0, float UVXScale1, float UVYScale1)
    {
        Mesh newMesh = new Mesh();

        newMesh.name = "GeometryFactory:Circle";

        float segmentRadius = 360.0f / (float)numSegments;
        int numVerts = numSegments + 2;

        Vector3[] vertices = new Vector3[numVerts];
        Vector2[] uvs0 = new Vector2[numVerts];
        Vector2[] uvs1 = new Vector2[numVerts];
        int[] triangles = new int[(numSegments + 1) * 3];

        vertices[0] = new Vector3(0.0f, height, 0.0f);

        uvs0[0] = new Vector2(0.5f * UVXScale0, 0.5f * UVYScale0);
        uvs1[0] = new Vector2(0.5f * UVXScale1, 0.5f * UVYScale1);

        float currentAngle = 0.0f;
        for(int i = 1; i < numSegments + 2; ++i)
        {
            float x = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            float y = Mathf.Cos(currentAngle * Mathf.Deg2Rad);

            vertices[i] = new Vector3(x * radius, height, y * radius);
            currentAngle += segmentRadius;

            x = (x + 1.0f) / 2.0f;
            y = (y + 1.0f) / 2.0f;

            uvs0[i] = new Vector2(x * UVXScale0, y * UVYScale0);
            uvs1[i] = new Vector2(x * UVXScale1, y * UVYScale1);
        }

        for(int segmentIndex = 0; segmentIndex < numSegments +1; ++segmentIndex)
        {
            int startIndex = segmentIndex * 3;
            triangles[startIndex] = 0;
            triangles[startIndex + 1] = segmentIndex;
            triangles[startIndex + 2] = segmentIndex + 1;
        }

        newMesh.vertices = vertices;
        newMesh.uv = uvs0;
        newMesh.uv2 = uvs1;
        
        newMesh.triangles = triangles;

        return newMesh;
    }

    private System.Collections.IEnumerator DeferScreenPlane()
	{
        yield return new WaitForSeconds(0.5f);

        RebuildMesh();

    }

}
