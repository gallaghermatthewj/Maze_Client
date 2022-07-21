using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseHandler : MonoBehaviour
{
    public GameObject heightenedSenses;
    // Start is called before the first frame update
    void Start()
    {
        heightenedSenses.GetComponent<ParticleSystem>().Play();
        heightenedSenses.GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
