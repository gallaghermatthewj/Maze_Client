using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    public GameObject particle1;
    public GameObject particle2;
    public Material material1;
    public Material material2;
    public AudioSource audioSource;
    private GameObject particleInstance1;
    private GameObject particleInstance2;
    

    // Start is called before the first frame update
    void Start()
    {
        particle1.GetComponent<ParticleSystem>().Play();
        particle2.GetComponent<ParticleSystem>().Play();

        particle1.GetComponent<ParticleSystem>().enableEmission = false;
        particle2.GetComponent<ParticleSystem>().enableEmission = false;
        //particleInstance1 = Instantiate(particle1, transform.position, transform.rotation);
        //particleInstance2 = Instantiate(particle2, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            particle1.GetComponent<ParticleSystem>().enableEmission = true;
            particle2.GetComponent<ParticleSystem>().enableEmission = true;
            if (!audioSource.isPlaying) { audioSource.Play(); }
            
            //particleInstance.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            particle1.GetComponent<ParticleSystem>().enableEmission = false;
            particle2.GetComponent<ParticleSystem>().enableEmission = false;
            if (audioSource.isPlaying) { audioSource.Pause(); }
            
            //particleInstance.GetComponent<ParticleSystem>().Stop();
        }

    }
    
    IEnumerator bePatient(int delay)
    {
        yield return new WaitForSeconds(delay);
        
        
    }

}
