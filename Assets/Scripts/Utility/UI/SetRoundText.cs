using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SetRoundText : MonoBehaviour
{
    public List<Text> TextTargets = new List<Text>();
    public string Prefix = string.Empty;

    private GameFlow m_gameFlow = null;

	void Start ()
    {
        m_gameFlow = FindObjectOfType<GameFlow>();
	}
	
	void Update ()
    {
	    foreach (var text in TextTargets)
	    {
            text.text = Prefix + m_gameFlow.State.GetStateName();
	    }
	}
}
