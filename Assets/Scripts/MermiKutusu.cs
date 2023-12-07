using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;



public class MermiKutusu : MonoBehaviour
{
    string [] silahlar=
    {

    "magnum",
    "pompali",
    "sniper",
    "taramali"


    };
    int[] mermisayisi={
        10,
        20,
        5,
        30
    };
    public List<Sprite> silahresimleri=new List<Sprite>();
    public UnityEngine.UI.Image silahinresmi;
    public string olusansilahinturu;
    public int olusanmermisayisi;
    public int noktasi;
    
    void Start()
    {
        int gelenanahtar=Random.Range(0,silahlar.Length);
        olusansilahinturu=silahlar[gelenanahtar];
        olusanmermisayisi=mermisayisi[Random.Range(0,mermisayisi.Length)];
        silahinresmi.sprite=silahresimleri[gelenanahtar];



            //olusansilahinturu="taramali";
            //olusanmermisayisi=30;
       
        
    }

    
    void Update()
    {
        
    }
}
