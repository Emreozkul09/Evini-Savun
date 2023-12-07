using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermiKutusuOlustur : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> merminoktalari=new List<GameObject>();
    public GameObject mermininkendisi;
    public static bool mermikutusuvarmi;
    public List<int> noktalar =new List<int>();
    int randomsayi;
    void Start()
    {
        mermikutusuvarmi=false;
        StartCoroutine(MermiKutusuYap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator MermiKutusuYap()
    {
        while (true)
        {



            
            
                yield return new WaitForSeconds(5f);
                randomsayi = Random.Range(0, 5);
                if(!noktalar.Contains(randomsayi))
                {
                    noktalar.Add(randomsayi);

                }
                else{
                    randomsayi = Random.Range(0, 5);
                    continue;

                }
                GameObject objem=Instantiate(mermininkendisi, merminoktalari[randomsayi].transform.position, merminoktalari[randomsayi].transform.rotation);
                objem.transform.gameObject.GetComponentInChildren<MermiKutusu>().noktasi=randomsayi;
            
            
           
        }


        /*
        while(true){

        
        yield return new WaitForSeconds(5f);
        int randomsayi=Random.Range(0,4);
        Instantiate(mermininkendisi,merminoktalari[randomsayi].transform.position,merminoktalari[randomsayi].transform.rotation);
        mermikutusuvarmi=true;
        }*/
    }
    public void noktalarikaldir(int deger){
        noktalar.Remove(deger);
    }
}
