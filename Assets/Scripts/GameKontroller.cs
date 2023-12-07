using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GameKontroller : MonoBehaviour
{


    float health = 100;
    [Header("Saglik Ayarlari")]
    public Image healthbar;
    public GameObject[] silahlarim;
    public AudioSource degistirmeses;
    [Header("Dusman Ayarlari")]
    public GameObject[] dusmanlar;

    public GameObject[] cikisnoktalari;

    public GameObject[] hedefnoktalar;
    public float cikmasuresi;
    public TextMeshProUGUI kalandusmansayisitext;
    public int baslangicdusmansayisi;
    public static int kalandusmansayisi;
    [Header("Diger Ayarlar")]
    public GameObject GameOverCanvas;
    public GameObject kazandinCanvas;




    void Start()
    {
        kalandusmansayisitext.text = baslangicdusmansayisi.ToString();
        kalandusmansayisi = baslangicdusmansayisi;
        

        if (!PlayerPrefs.HasKey("oyunbasladmi"))
        {
            PlayerPrefs.SetInt("taramali_mermi", 80);
            PlayerPrefs.SetInt("pompali_mermi", 50);
            PlayerPrefs.SetInt("magnum_mermi", 30);
            PlayerPrefs.SetInt("sniper_mermi", 20);

            PlayerPrefs.SetInt("oyunbasladimi", 1);
        }
        StartCoroutine(dusmancikar());
    }
    public void DarbeAl(float darbegucu)
    {
        health -= darbegucu;
        healthbar.fillAmount = health / 100;
        if (health <= 0)
        {
            gameover();
        }

    }
    public void saglikdoldur()
    {
        health = 100;
        healthbar.fillAmount = health / 100;

    }
    void gameover()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    IEnumerator dusmancikar()
    {

        while (true)
        {
            yield return new WaitForSeconds(cikmasuresi);
            if (baslangicdusmansayisi != 0)
            {
                int dusmanim = Random.Range(0, 5);
                int cikisnotasi = Random.Range(0, 2);
                int hedefnokta = Random.Range(0, 2);
                GameObject obje = Instantiate(dusmanlar[dusmanim], cikisnoktalari[cikisnotasi].transform.position, Quaternion.identity);
                obje.GetComponent<Dusmansc>().Hedefbelirle(hedefnoktalar[hedefnokta]);
                baslangicdusmansayisi--;
            }

        }


    }
    public void dusmansayisiguncelle()
    {
        kalandusmansayisi--;
        if (kalandusmansayisi <= 0)
        {
            kazandinCanvas.SetActive(true);
            kalandusmansayisitext.text = "0";
            Time.timeScale = 0;

        }
        else
        {
            kalandusmansayisitext.text = kalandusmansayisi.ToString();


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
        {
            silahlarim[0].SetActive(false);
            silahlarim[1].SetActive(true);
            degistirmeses.Play();

        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
        {
            silahlarim[1].SetActive(false);
            silahlarim[0].SetActive(true);
            degistirmeses.Play();
        }
    }
    public void BastanBasla()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
