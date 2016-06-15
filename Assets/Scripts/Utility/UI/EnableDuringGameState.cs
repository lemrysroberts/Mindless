using UnityEngine;
using System.Collections.Generic;

public class EnableDuringGameState : MonoBehaviour
{
    public string StateName = string.Empty;

    public List<MonoBehaviour> behavioursToToggle = new List<MonoBehaviour>();

    private GameFlow m_gameFlow = null;

	void Start ()
    {
        m_gameFlow = FindObjectOfType<GameFlow>();
	}
	
	void Update ()
    {
	    if(m_gameFlow != null && m_gameFlow.State != null && m_gameFlow.State.GetStateName() == StateName)
        {
            foreach (var behaviour in behavioursToToggle)
            {
                behaviour.enabled = true;
            }
        }
        else
        {
            foreach (var behaviour in behavioursToToggle)
            {
                behaviour.enabled = false;
            }
        }
	}
}
