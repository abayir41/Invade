using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallForce : MonoBehaviour
{
    public float force;
    public GameObject prefab;
    public float time;
    public Transform direction;
    void Start()
    {
        StartCoroutine(Spanwer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spanwer()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            var asd = Instantiate(prefab);
            asd.transform.position = transform.position;
            asd.transform.rotation = transform.rotation;
            var rigd = asd.GetComponent<Rigidbody>();
            rigd.AddForce(direction.forward * force, ForceMode.VelocityChange);
        }
    }
}
