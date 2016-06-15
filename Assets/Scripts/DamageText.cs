using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public float HeightTransition = 4.0f;
    public float TimeToLive = 1.0f;

    private float m_progress = 0.0f;
    private float m_heightProgress = 0.0f;
    private Vector3 m_startPosition;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
               Camera.main.transform.rotation * Vector3.up);

        m_progress += Time.deltaTime / TimeToLive;

        float heightDiff = Time.deltaTime * HeightTransition;

        transform.position = transform.position + new Vector3(0.0f, heightDiff, 0.0f);

        if (m_progress >= 1.0f) Destroy(gameObject);
    }
}
