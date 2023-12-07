using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Video;


public class sniper : MonoBehaviour
{
    Animator animatorum;
    [Header("Ayarlar")]
    public bool atesedebilirmi;
    float iceriatesetmesikligi;
    public float disariatesşiklik;
    public float menzil;
    public GameObject cross;
    public GameObject scope;

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
    float yakkinpov=20;

    [Header("Silah Ayarlar")]
    int ToplamMermiSayisi;
    public int sarjorlimiti;
    public string silahinadi;
    public int kalanmermi;
    public TextMeshProUGUI ToplamMermi_text;
    public TextMeshProUGUI KalanMermi_text;
    int AtilanMermiSayisi;
    public float darbegucu;


    bool KovanCiksinMi;
    public GameObject KavanCikisNoktasi;
    public GameObject KovanObjesi;

    public MermiKutusuOlustur mermikutusuolusturyonetim;






    void Start()
    {
        ToplamMermiSayisi = PlayerPrefs.GetInt(silahinadi + "_mermi");
        KovanCiksinMi = true;
        baslangicmermidoldur();
        sarjordoldurteknikfonk("NormalYaz");
        animatorum = GetComponent<Animator>();

        ToplamMermi_text.text = ToplamMermiSayisi.ToString();
        KalanMermi_text.text = kalanmermi.ToString();
        camfieldpov=benimcam.fieldOfView;

    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (atesedebilirmi && Time.time > iceriatesetmesikligi && kalanmermi > 0)
            {
                ateset();
                iceriatesetmesikligi = Time.time + disariatesşiklik;

            }
            if (kalanmermi == 0)
            {
                MermiBittiSes.Play();
            }





        }
        if (Input.GetKey(KeyCode.R))
        {

            if (kalanmermi < sarjorlimiti && ToplamMermiSayisi != 0)
            {
                animatorum.Play("sarjor_degis");
            }


        }
        if (Input.GetKey(KeyCode.E))
        {

            MermiAl();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            kamerazoomyap(true);
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            kamerazoomyap(false);
        }


    }
     public void kamerazoomyap(bool durum)//scope için gerekli
    {
        if(durum)
        {
        cross.SetActive(false);
        benimcam.cullingMask= ~ (1<<8);
        animatorum.SetBool("zoomyap",durum);
        benimcam.fieldOfView=yakkinpov;
        scope.SetActive(true);
        }
        else{
            scope.SetActive(false);
            benimcam.cullingMask=-1;
            animatorum.SetBool("zoomyap",durum);
            benimcam.fieldOfView=camfieldpov;
            cross.SetActive(true);

        }
        
    }
    void OnTriggerEnter(Collider other)//yerden iki farkli sekilde mermi almak için kullanıyorum çarpişma ile bu

    {
        if (other.gameObject.CompareTag("Mermi"))
        {

            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().olusansilahinturu, other.transform.gameObject.GetComponent<MermiKutusu>().olusanmermisayisi);
            mermikutusuolusturyonetim.noktalarikaldir(other.transform.gameObject.GetComponent<MermiKutusu>().noktasi);
            Destroy(other.transform.parent.gameObject);

        }
    }
    
    public void sarjordegistirteknik()
    {
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
    void ateset()
    {
        atesetmeteknikislemleri();

        RaycastHit hit;
        if (Physics.Raycast(benimcam.transform.position, benimcam.transform.forward, out hit, menzil))
        {

            if (hit.transform.CompareTag("Dusman"))
            {
                Instantiate(kan_efekti, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Dusmansc>().darbeal(darbegucu);

            }
            else if (hit.transform.CompareTag("DevrilecekObje"))
            {
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                rg.AddForce(-hit.normal * 50f);

                Instantiate(mermi_efekti, hit.point, Quaternion.LookRotation(hit.normal));

            }
            else
            {
                Instantiate(mermi_efekti, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        // Instantiate(mermi_efekti, hit.point,Quaternion.LookRotation(hit.normal));

    }
    void MermiAl()// yerden mermi almak için 2. yöntem 'E' tuşunu kullanarak alma yöntemi
  
    {
        RaycastHit hit;
        if (Physics.Raycast(benimcam.transform.position, benimcam.transform.forward, out hit, 4))
        {
            if (hit.transform.gameObject.CompareTag("Mermi"))
            {
                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().olusansilahinturu, hit.transform.gameObject.GetComponent<MermiKutusu>().olusanmermisayisi);
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
        else
        {
            kalanmermi = sarjorlimiti;
            ToplamMermiSayisi -= sarjorlimiti;
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
                        PlayerPrefs.SetInt(silahinadi + "_mermi", ToplamMermiSayisi);
                    }
                    else
                    {
                        kalanmermi += ToplamMermiSayisi;
                        ToplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(silahinadi + "_mermi", 0);
                    }

                }
                else
                {
                    ToplamMermiSayisi -= sarjorlimiti - kalanmermi;
                    kalanmermi = sarjorlimiti;
                    PlayerPrefs.SetInt(silahinadi + "_mermi", ToplamMermiSayisi);

                }

                ToplamMermi_text.text = ToplamMermiSayisi.ToString();
                KalanMermi_text.text = kalanmermi.ToString();

                break;
            case "MermiYok":
                if (ToplamMermiSayisi <= sarjorlimiti)
                {
                    kalanmermi = ToplamMermiSayisi;
                    ToplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(silahinadi + "_mermi", 0);

                }
                else
                {
                    ToplamMermiSayisi -= sarjorlimiti;
                    PlayerPrefs.SetInt(silahinadi + "_mermi", ToplamMermiSayisi);
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
    void atesetmeteknikislemleri()
    {
        if (KovanCiksinMi)
        {
            GameObject obje = Instantiate(KovanObjesi, KavanCikisNoktasi.transform.position, KavanCikisNoktasi.transform.rotation);
            Rigidbody rgb = obje.GetComponent<Rigidbody>();
            rgb.AddRelativeForce(new Vector3(10, 1, 0) * 60);

        }

        Atesses.Play();
        Ates_efekt.Play();
        animatorum.Play("atesett");

        kalanmermi--;
        KalanMermi_text.text = kalanmermi.ToString();


    }
    void MermiKaydet(string SilahTuru, int mermisayisi)//yerden alınan mermilarin sayısını ve türüne göre envantere ekleme fonk
 
    {
        mermialmases.Play();
        switch (SilahTuru)
        {

            case "taramali":
                PlayerPrefs.SetInt("sniper_mermi", PlayerPrefs.GetInt("sniper_mermi") + mermisayisi);


                break;
            case "pompali":
                PlayerPrefs.SetInt("pompali_mermi", PlayerPrefs.GetInt("pompali_mermi") + mermisayisi);

                break;
            case "sniper":
                ToplamMermiSayisi += mermisayisi;
                PlayerPrefs.SetInt(silahinadi + "_mermi", ToplamMermiSayisi);
                sarjordoldurteknikfonk("NormalYaz");

                break;
            case "magnum":
                PlayerPrefs.SetInt("magnum_mermi", PlayerPrefs.GetInt("magnum_mermi") + mermisayisi);
                break;


        }

    }
   
}
