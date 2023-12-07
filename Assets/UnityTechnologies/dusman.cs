using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dusman : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent ajan;
    public GameObject hedef;
    void Start()
    {
        ajan=GetComponent<NavMeshAgent>();
    }
    public void hedefbelirle(GameObject objem){
        hedef=objem;

    }

    // Update is called once per frame
    void Update()
    {
        ajan.SetDestination(hedef.transform.position);
    }
}
