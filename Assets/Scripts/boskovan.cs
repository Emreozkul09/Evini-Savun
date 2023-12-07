using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boskovan : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource yeredusmesesi;
    void Start()
    {
        yeredusmesesi=GetComponent<AudioSource>();
        Destroy(gameObject,2f);
    }

private  void OnCollisionEnter(Collision other)
{
    if(other.gameObject.CompareTag("yol"))
    {
        yeredusmesesi.Play();
        if(!yeredusmesesi.isPlaying){
            Destroy(gameObject,1f);

        }

    }
}
 
    // Update is called once per frame
    void Update()
    {
        
    }
}



