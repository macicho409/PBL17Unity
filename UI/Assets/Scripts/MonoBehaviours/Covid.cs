using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Covid : MonoBehaviour
{
    private CovidModel covid;
    public bool Infacted;

    private int LenghtPreviosly;
    void Start()
    {
        LenghtPreviosly = 0;
    }

    
    void Update()
    {
        
    }

    private void Colision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 4.0f,256);
        foreach (var hitCollider in hitColliders)
        {
            if((hitCollider.name != this.name) && hitCollider.transform.GetComponent<Covid>().Infacted && LenghtPreviosly != hitColliders.Length)
            {
                covid = new CovidModel(hitCollider.gameObject, this.gameObject, 0.01f, 0.01f, 0.01f, 4.0f);
                LenghtPreviosly = hitColliders.Length;
            }
        }
    }
}
