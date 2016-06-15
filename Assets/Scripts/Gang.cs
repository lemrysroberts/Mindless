using UnityEngine;
using System.Collections.Generic;

public class Gang : MonoBehaviour
{
    public int NumGangMembers = 5;

    public GameObject RiderObject = null;

    private List<Rider> m_riders = new List<Rider>();
    private GameGrid m_gameGrid = null;

	// Use this for initialization
	void Start ()
    {
        m_gameGrid = FindObjectOfType<GameGrid>();

	    for(int i = 0; i < NumGangMembers; i++)
        {
            m_riders.Add(Instantiate(RiderObject).GetComponent<Rider>());
        }

        foreach (Rider rider in m_riders)
        {
            rider.SetGang(this);
            m_gameGrid.AddObjectToGrid(rider);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
