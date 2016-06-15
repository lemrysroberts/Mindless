using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct GameFlowValue
{
    public string Key;
    public float Value;
}

public class GameFlow : MonoBehaviour
{
    public UnityEngine.UI.Text SpeedText;    

    public List<GameFlowValue> GameFlowValues = new List<GameFlowValue>();

    public GameState State
    {
        get
        {
            return m_states[m_stateIndex];
        }
    }

    private List<GameState> m_states = new List<GameState>();

    private int m_stateIndex = 0;

	void Start ()
    {
        m_states.Add(new PlanningState());
        m_states.Add(new MovementState());
        m_states.Add(new ActionState());

        foreach (var state in m_states)
        {
            state.Init();
        }

        m_states[m_stateIndex].BeginState();
    }
	
	void Update ()
    {
        m_states[m_stateIndex].UpdateState();
	}

    public void AdvanceState()
    {
        m_states[m_stateIndex].EndState();
        m_stateIndex = (m_stateIndex + 1) % m_states.Count;
        m_states[m_stateIndex].BeginState();
    }
}
