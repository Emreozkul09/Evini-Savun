using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKutusuOlustur : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Healthnoktalari=new List<GameObject>();
    public GameObject Healthninkendisi;
    public static bool Healthkutusuvarmi;
    public float cikmasuresi;
   
    int randomsayi;
    void Start()
    {
        Healthkutusuvarmi=false;
        StartCoroutine(HealthKutusuYap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator HealthKutusuYap()
    {
        while (true)
        {

                yield return new WaitForSeconds(5f);
                if(!Healthkutusuvarmi){
                randomsayi = Random.Range(0, 6);
                GameObject objem=Instantiate(Healthninkendisi, Healthnoktalari[randomsayi].transform.position, Healthnoktalari[randomsayi].transform.rotation);
                Healthkutusuvarmi=true; 

                }
                      
        }


      
    }
}
