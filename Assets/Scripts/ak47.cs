using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Video;


public class ak47 : MonoBehaviour
{
    Animator animatorum;
    [Header("Ayarlar")]
    public bool atesedebilirmi;
    float iceriatesetmesikligi;
    public float disariatesşiklik;
    public float menzil;

    [Header("Sesler")]
    public AudioSource Atesses;
    public AudioSource sarjorses;
    public AudioSource MermiBittiSes;
    public AudioSource mermialmases;

    [Header("Efektler")]
    public ParticleSystem Ates_efekt;
    public ParticleSystem kan_efekti;
    public ParticleSystem mermi_efekti;

    [Header("Diger")]
    public Camera benimcam;
     float camfieldpov;
    float yakkinpov=30;
    public GameObject cross;

    [Header("Silah Ayarlar")]
    int ToplamMermiSayisi;
    public int sarjorlimiti;
    public string silahinadi;
    public int kalanmermi;
    public TextMeshProUGUI ToplamMermi_text;
    public TextMeshProUGUI KalanMermi_text;
    int AtilanMermiSayisi;
    bool zoomvarmi;
    public float darbegucu;


    bool KovanCiksinMi;
    public GameObject KavanCikisNoktasi;
    public GameObject KovanObjesi;

    public MermiKutusuOlustur mermikutusuolusturyonetim;






    void Start()
    {
        ToplamMermiSayisi=PlayerPrefs.GetInt(silahinadi+"_mermi");
        KovanCiksinMi=true;
        baslangicmermidoldur();
        sarjordoldurteknikfonk("NormalYaz");
        animatorum = GetComponent<Animator>();

        ToplamMermi_text.text = ToplamMermiSayisi.ToString();
        KalanMermi_text.text = kalanmermi.ToString();
        camfieldpov=benimcam.fieldOfView;
        
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)&& !Input.GetKey(KeyCode.Mouse1))
        {
            if (atesedebilirmi && Time.time > iceriatesetmesikligi && kalanmermi > 0)
            {
                ateset(false);
                iceriatesetmesikligi = Time.time + disariatesşiklik;

            }
            if (kalanmermi == 0)
            {
                MermiBittiSes.Play();
            }

        }
        if (Input.GetKey(KeyCode.R))
        {


            animatorum.Play("sarjor_degis");


        }
        if (Input.GetKey(KeyCode.E))
        {

            MermiAl();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            zoomvarmi=true;
            animatorum.SetBool("zoomyap",true);
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            zoomvarmi=false;
            animatorum.SetBool("zoomyap",false);
            benimcam.fieldOfView=camfieldpov;
            cross.SetActive(true);
        }
        if (zoomvarmi)
        {
            
            if (Input.GetKey(KeyCode.Mouse0) )
            {Debug.Log("tiklandi");
            
                if (atesedebilirmi && Time.time > iceriatesetmesikligi && kalanmermi > 0)
                {
                    ateset(true);
                    iceriatesetmesikligi = Time.time + disariatesşiklik;

                }
                if (kalanmermi == 0)
                {
                    MermiBittiSes.Play();
                

            }
            }

        }


    }
    public void kamerazoomyap()//scope için gerekli
    {
        
        benimcam.fieldOfView=yakkinpov;
        cross.SetActive(false);
        
    }
    void OnTriggerEnter(Collider other)//yerden iki farkli sekilde mermi almak için kullanıyorum çarpişma ile bu
    {
        if(other.gameObject.CompareTag("Mermi"))
        {
           
            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().olusansilahinturu,other.transform.gameObject.GetComponent<MermiKutusu>().olusanmermisayisi);
            mermikutusuolusturyonetim.noktalarikaldir(other.transform.gameObject.GetComponent<MermiKutusu>().noktasi);
            Destroy(other.transform.parent.gameObject);
            
        }
        if(other.gameObject.CompareTag("healthbox"))
        {
            mermikutusuolusturyonetim.GetComponent<GameKontroller>().saglikdoldur();
            HealthKutusuOlustur.Healthkutusuvarmi=false;
            Destroy(other.transform.gameObject);
            
        }
    }

    IEnumerator cameratitre(float titremesuresi,float magnitude)//kamera titremesi için kulkanılan fonksiyon
    {
        UnityEngine.Vector3 orijinalpozisyon=benimcam.transform.localPosition;
        float gecensure=0.0f;
        while(gecensure<titremesuresi){
            float x=Random.Range(-1f,1)*magnitude;
            benimcam.transform.localPosition=new UnityEngine.Vector3(x,orijinalpozisyon.y,orijinalpozisyon.x);
            gecensure+=Time.deltaTime;
            yield return null;
        }
        benimcam.transform.localPosition=orijinalpozisyon;

    }
    
    public void sarjordegistirteknik(){
         sarjorses.Play();
         if (kalanmermi < sarjorlimiti && ToplamMermiSayisi != 0)
        {

            if (kalanmermi != 0)
            {

                sarjordoldurteknikfonk("MermiVar");
            }
            else
            {
                sarjordoldurteknikfonk("MermiYok");
            }

        }
    }
    void ateset(bool yakinlasmavarmi)
    {
        atesetmeteknikislemleri(yakinlasmavarmi);

        RaycastHit hit;
        if (Physics.Raycast(benimcam.transform.position, benimcam.transform.forward, out hit, menzil))
        {

            if (hit.transform.CompareTag("Dusman"))
            {
                Instantiate(kan_efekti, hit.point, UnityEngine.Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Dusmansc>().darbeal(darbegucu);
            }
            else if (hit.transform.CompareTag("DevrilecekObje"))
            {
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                rg.AddForce(-hit.normal * 50f);

                Instantiate(mermi_efekti, hit.point, UnityEngine.Quaternion.LookRotation(hit.normal));

            }
            else
            {
                Instantiate(mermi_efekti, hit.point, UnityEngine.Quaternion.LookRotation(hit.normal));
            }
        }
        // Instantiate(mermi_efekti, hit.point,Quaternion.LookRotation(hit.normal));

    }
    void MermiAl()// yerden mermi almak için 2. yöntem 'E' tuşunu kullanarak alma yöntemi
    {
         RaycastHit hit;
        if (Physics.Raycast(benimcam.transform.position, benimcam.transform.forward, out hit, 4))
        {
            if(hit.transform.gameObject.CompareTag("Mermi")){
                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().olusansilahinturu,hit.transform.gameObject.GetComponent<MermiKutusu>().olusanmermisayisi);
                mermikutusuolusturyonetim.noktalarikaldir(hit.transform.gameObject.GetComponent<MermiKutusu>().noktasi);
                Destroy(hit.transform.parent.gameObject);
            }

        }

         
    }
    void baslangicmermidoldur()
    {
        if (ToplamMermiSayisi <= sarjorlimiti)
        {

            kalanmermi = ToplamMermiSayisi;
            ToplamMermiSayisi = 0;
            PlayerPrefs.SetInt(silahinadi + "_mermi", 0);
        }
        else{
            kalanmermi=sarjorlimiti;
            ToplamMermiSayisi-=sarjorlimiti;
            PlayerPrefs.SetInt(silahinadi + "_mermi", ToplamMermiSayisi);

        }

    }
    void sarjordoldurteknikfonk(string tur)
    {
        switch (tur)
        {
            case "MermiVar":
                if (ToplamMermiSayisi <= sarjorlimiti)
                {
                    int olusantoplamdeger = ToplamMermiSayisi + kalanmermi;
                    if (olusantoplamdeger > sarjorlimiti)
                    {
                        kalanmermi = sarjorlimiti;
                        ToplamMermiSayisi = olusantoplamdeger - sarjorlimiti;
                        PlayerPrefs.SetInt(silahinadi+"_mermi",ToplamMermiSayisi);
                    }
                    else
                    {
                        kalanmermi += ToplamMermiSayisi;
                        ToplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(silahinadi+"_mermi",0);
                    }

                }
                else
                {
                    ToplamMermiSayisi -= sarjorlimiti - kalanmermi;
                    kalanmermi = sarjorlimiti;
                    PlayerPrefs.SetInt(silahinadi+"_mermi",ToplamMermiSayisi);

                }

                ToplamMermi_text.text = ToplamMermiSayisi.ToString();
                KalanMermi_text.text = kalanmermi.ToString();

                break;
            case "MermiYok":
                if (ToplamMermiSayisi <= sarjorlimiti)
                {
                    kalanmermi = ToplamMermiSayisi;
                    ToplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(silahinadi+"_mermi",0);

                }
                else
                {
                    ToplamMermiSayisi -= sarjorlimiti;
                    PlayerPrefs.SetInt(silahinadi+"_mermi",ToplamMermiSayisi);
                    kalanmermi = sarjorlimiti;
                }

                ToplamMermi_text.text = ToplamMermiSayisi.ToString();
                KalanMermi_text.text = kalanmermi.ToString();
                break;

            case "NormalYaz":
                
                ToplamMermi_text.text = ToplamMermiSayisi.ToString();
                KalanMermi_text.text = kalanmermi.ToString();
                break;


        }

    }
    void atesetmeteknikislemleri(bool yakinlasmavarmi){
        if(KovanCiksinMi){
            GameObject obje= Instantiate(KovanObjesi,KavanCikisNoktasi.transform.position,KavanCikisNoktasi.transform.rotation);
            Rigidbody rgb=obje.GetComponent<Rigidbody>();
            rgb.AddRelativeForce(new UnityEngine.Vector3(-10,1,0)*60);

        }
       StartCoroutine(cameratitre(.90f,.10f));
        Atesses.Play();
        Ates_efekt.Play();
        if(!yakinlasmavarmi){
            animatorum.Play("atesett");
        }
        

        kalanmermi--;
        KalanMermi_text.text = kalanmermi.ToString();


    }
    void MermiKaydet(string SilahTuru, int mermisayisi)//yerden alınan mermilarin sayısını ve türüne göre envantere ekleme fonk
    {
        mermialmases.Play();
        switch (SilahTuru)
        {
            
            case "taramali":
            ToplamMermiSayisi+=mermisayisi;
            PlayerPrefs.SetInt(silahinadi+"_mermi",ToplamMermiSayisi);
            sarjordoldurteknikfonk("NormalYaz");

                break;
            case "pompali":
            PlayerPrefs.SetInt("pompali_mermi",PlayerPrefs.GetInt("pompali_mermi")+mermisayisi);

                break;
            case "sniper":
            PlayerPrefs.SetInt("sniper_mermi",PlayerPrefs.GetInt("sniper_mermi")+mermisayisi);
                break;
            case "magnum":
            PlayerPrefs.SetInt("magnum_mermi",PlayerPrefs.GetInt("magnum_mermi")+mermisayisi);
                break;


        }

    }
}
