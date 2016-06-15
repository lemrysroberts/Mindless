using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void AdvanceState()
    {
        FindObjectOfType<GameFlow>().AdvanceState();
    }
}
