using UnityEngine;
using System.Collections;

public class MovementState : GameState
{
    private GameGrid m_grid = null;
    private GameFlow m_gameFlow = null;

    private float m_transitionTime = 2.0f;
    private float m_progress = 0.0f;

    public void Init()
    {
        m_grid = GameObject.FindObjectOfType<GameGrid>();
        m_gameFlow = GameObject.FindObjectOfType<GameFlow>();

        m_transitionTime = m_gameFlow.GameFlowValues.Find(x => x.Key == "movement_duration").Value;
    }

    public void BeginState()
    {
        m_progress = 0.0f;
    }

    public void EndState()
    {
    }

    public void UpdateState()
    {
        m_progress += Time.deltaTime / m_transitionTime;

        m_progress = Mathf.Clamp01(m_progress);

        foreach(GridObject gridObject in m_grid.GetGridObjects())
        {
            gridObject.UpdateMovement(m_progress);
        }

        if(m_progress == 1.0f)
        {
            m_gameFlow.AdvanceState();
        }
    }

    public string GetStateName()
    {
        return "movement";
    }
}
