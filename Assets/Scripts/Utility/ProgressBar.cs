using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour
{

    private float m_initialSize = 1.0f;

    public void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        transform.localScale = new Vector3(m_initialSize * progress, transform.localScale.y, transform.localScale.z);
    }

	void Start ()
    {
        m_initialSize = transform.localScale.x;
	}
	
	void Update ()
    {
	}
}
