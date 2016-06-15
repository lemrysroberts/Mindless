using UnityEngine;
using System.Collections;

public class ActionState : GameState
{
    private GameGrid m_grid = null;
    private GameFlow m_gameFlow = null;

    private float m_transitionTime = 1.0f;
    private float m_progress = 0.0f;
    private bool m_complete = false;

    public void Init()
    {
        m_grid = GameObject.FindObjectOfType<GameGrid>();
        m_gameFlow = GameObject.FindObjectOfType<GameFlow>();
    }

    public void BeginState()
    {
        m_progress = 0.0f;
        m_complete = false;
    }

    public void EndState()
    {
        foreach (GridObject gridObject in m_grid.GetGridObjects())
        {
            gridObject.OnEndRound();
        }
    }

    public void UpdateState()
    {
        m_progress += Time.deltaTime / m_transitionTime;

        m_progress = Mathf.Clamp01(m_progress);

        if(!m_complete)
        {

            foreach (GridObject gridObject in m_grid.GetGridObjects())
            {
                gridObject.UpdateAction(m_progress);
            }

            m_complete = true;
        }

        if (m_progress == 1.0f)
        {
            m_gameFlow.AdvanceState();
        }
    }

    public string GetStateName()
    {
        return "action";
    }
}
