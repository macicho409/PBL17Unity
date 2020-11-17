using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidModel : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject AnotherAgent;
    public GameObject ThisAgent;
   

    private System.Random rand;
    private float weightr;
    private float weighthealth;
    private float weightmask;
    private float rsphare;
    private Helath health;


    private float _probability;
    public float Probability { 
        get { return _probability; }
        set
        {
            if (value >= 1f)
                _probability = 1f;
            else if (value <= 0f)
                _probability = 0f;
            else
                _probability = value;
        }
    }

    public CovidModel(GameObject AnotherAgent, GameObject ThisAgent, float weightr, float weighthealth, float weightmask, float rsphare)
    {
        this.AnotherAgent = AnotherAgent;
        this.ThisAgent = ThisAgent;
        this.weightr = weightr;
        this.weighthealth = weighthealth;
        this.weightmask = weightmask;
        this.rsphare = rsphare;
        rand = new System.Random();
        health = ThisAgent.GetComponent<Helath>();
    }

    public bool CalculatingProbabilities()
    {
        new WaitForSeconds(1);
        return (int)(Probability * 100) <= rand.Next(0, 100);
    }

    public void ChanceINfacted()
    {
        float BoolMask;
        if (AnotherAgent.GetComponent<Mask>().MaskOn)
            BoolMask = 1.0f;
        else
            BoolMask = 0.0f;

        float r = rsphare - Mathf.Sqrt(Mathf.Pow(ThisAgent.transform.position.x - AnotherAgent.transform.position.x, 2) + Mathf.Pow(ThisAgent.transform.position.y - AnotherAgent.transform.position.y, 2) + Mathf.Pow(ThisAgent.transform.position.z - AnotherAgent.transform.position.z, 2));
        Probability += (r * weightr + (1 - health.Value) * weighthealth - 0.5f * weightmask * BoolMask) * Time.deltaTime;
    }

}
