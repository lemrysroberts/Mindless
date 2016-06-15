using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRiderStats : MonoBehaviour
{
    public Text SpeedText = null;

    private Rider m_rider = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetRider(Rider rider)
    {
        m_rider = rider;

        if(rider != null)
        {
            SpeedText.text = "speed: " + rider.Stats.Find(x => x.StatName == "speed").StatValue.ToString("0.");
        }
        else
        {
            SpeedText.text = "";
        }
    }
}
