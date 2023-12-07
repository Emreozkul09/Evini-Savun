using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class Dusmansc : MonoBehaviour
{
    // Start is called before the first frame update
     NavMeshAgent ajan;
    GameObject hedef;
     
     public float health;
     public float dusmandarbegucu=5;
      GameObject anakontrolcum;
      Animator animatorum;
    
    void Start()
    {
        animatorum=gameObject.GetComponent<Animator>();
        anakontrolcum=GameObject.FindWithTag("Anakontrolcum");
        ajan=GetComponent<NavMeshAgent>();
    }
     public void Hedefbelirle(GameObject objem){
        hedef=objem;

     }
     public void OnTriggerEnter(Collider other)
     {
        if(other.gameObject.CompareTag("korumaliyim")){
           
            anakontrolcum.GetComponent<GameKontroller>().DarbeAl(dusmandarbegucu);
            oldun();
        }
        
     }
    // Update is called once per frame
    void Update()
    {  if(hedef != null)
    {
        Debug.Log("Hedef pozisyon: " + hedef.transform.position);
        ajan.SetDestination(hedef.transform.position);
    
    }
         
    }
    public void darbeal(float darbe){
        health-=darbe;
        
        if(health<=0){
            oldun();
        }
    }
    void oldun(){
        anakontrolcum.GetComponent<GameKontroller>().dusmansayisiguncelle();
        animatorum.SetTrigger("olme");
        Destroy(gameObject,5f);
    }
}
