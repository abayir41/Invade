using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Car : MonoBehaviour
{
    public BoxCollider _triggerCollider;
    public BoxCollider _collisionCollider;

    public MeshRenderer Renderer;
    
    public Material original;
    public Material exploded;
    public bool explodedNormalCar;

    public GameObject explosionParticle;

    public bool Inside;

    public bool ExplosiveCar;
    public float radius;
    public float power;
    public AudioClip Boom;
    public bool explodedBool;
    public GameObject explosionParticleBig;
    public GameObject smoke;
    private GameObject smokeCached;
    public GameObject explosiceIcon;


    public bool healingCar;
    public float radiusHealing;
    public AudioClip HealClip;
    public bool healed;
    public GameObject healParticleBig;
    public GameObject healPaticleNormel;
    public GameObject healIcon;

    public static Action SomethingChanged;

    private void Awake()
    {
        smokeCached = Instantiate(smoke);
        _collisionCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (ExplosiveCar && !explodedBool)
        {
            if (Inside)
            {
                var colliders = Physics.OverlapSphere(transform.position, radius);
                foreach (var hit in colliders)
                {
                    if(!hit.transform.GetComponent<Car>()) continue;
                    if(!hit.transform.GetComponent<Car>().Inside) continue;
                    
                    
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null && hit.transform.GetComponent<Car>() != this)
                        rb.AddExplosionForce(power, transform.position, radius, 3.0F);
                    
                    
                    if(hit.transform.GetComponent<Car>().explodedNormalCar) continue;
                    
                    
                    
                    
                    StartCoroutine(hit.transform.GetComponent<Car>().Explode());
                    
                    hit.transform.GetComponent<Car>().explodedNormalCar = true;

                } 
            }
            else
            {
                StartCoroutine(Explode());
            }

            StartCoroutine(CameraShake.Instance.Shake(0.4f, 0.8f));

            SomethingChanged?.Invoke();
            
            explodedBool = true;
            explodedNormalCar = true;
            Camera.main!.GetComponent<AudioSource>().PlayOneShot(Boom);
        }
        else if (healingCar && !healed)
        {
            if (Inside)
            {
                var colliders = Physics.OverlapSphere(transform.position, radiusHealing);
                

                foreach (var hit in colliders)
                {
                    if(!hit.transform.GetComponent<Car>()) continue;
                    
                    
                    if(!hit.transform.GetComponent<Car>().Inside) continue;
                    

                    if(!hit.transform.GetComponent<Car>().explodedNormalCar) continue;


                    StartCoroutine(hit.transform.GetComponent<Car>().Fix());
                           
                }
            }

            if (healPaticleNormel != null)
            {
                var particle = Instantiate(healPaticleNormel);
                particle.SetActive(true);
                particle.transform.position = transform.position;
                Destroy(particle, 5f);
            }
            Camera.main!.GetComponent<AudioSource>().PlayOneShot(HealClip);
            healed = true;
            
            SomethingChanged?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("boom"))
        {
            _collisionCollider.enabled = true;
            StartCoroutine(Explode());
        }
        else if (other.gameObject.CompareTag("Nuke") && !Inside)
        {
            GameManager.Instance.SpawnNuke();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Tarama") && !Inside)
        {
            StartCoroutine(Spawner.Instance.TaramaMode());
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Health") && !Inside)
        {
            var colliders = Physics.OverlapSphere(transform.position, 100000);
                

            foreach (var hit in colliders)
            {
                if(!hit.transform.GetComponent<Car>()) continue;
                    
                    
                if(!hit.transform.GetComponent<Car>().Inside) continue;
                    

                if(!hit.transform.GetComponent<Car>().explodedNormalCar) continue;


                StartCoroutine(hit.transform.GetComponent<Car>().Fix());
                           
            }
            
            if (healPaticleNormel != null)
            {
                var particle = Instantiate(healPaticleNormel);
                particle.SetActive(true);
                particle.transform.position = transform.position;
                Destroy(particle, 5f);
            }
            Camera.main!.GetComponent<AudioSource>().PlayOneShot(HealClip);
            Destroy(other.gameObject);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            _collisionCollider.enabled = true;
            Inside = true;
            SomethingChanged?.Invoke();
        }
    }

    private void Update()
    {
        if (explodedNormalCar)
        {
            smokeCached.SetActive(true);
            smokeCached.transform.position = transform.position;
        }
        else
        {
            smokeCached.SetActive(false);
        }
    }

    public void throwed()
    {
        if (healingCar)
        {
            Destroy(healIcon, 1);
        }
        else if (ExplosiveCar)
        {
            Destroy(explosiceIcon, 1);
        }
    }

    public IEnumerator Explode()
    {
        if (explodedNormalCar) yield return null;
        explodedNormalCar = true;
        Renderer.material = exploded;
        if (explosionParticleBig != null && !explodedBool)
        {
            var particle = Instantiate(explosionParticleBig);
            particle.SetActive(true);
            particle.transform.position = transform.position;
            Destroy(particle, 5f);
        }
        
        if (explosionParticle != null)
        {
            var particle = Instantiate(explosionParticle);
            particle.SetActive(true);
            particle.transform.position = transform.position;
            Destroy(particle, 5f);
        }
            
        yield return null;
    }

    public IEnumerator Fix()
    {
        
        explodedNormalCar = false;
        Renderer.material = original;
        if (healParticleBig != null && !healed)
        {
            var particle = Instantiate(healParticleBig);
            particle.SetActive(true);
            particle.transform.position = transform.position;
            Destroy(particle, 5f);
        }
        
        if (healPaticleNormel != null)
        {
            var particle = Instantiate(healPaticleNormel);
            particle.SetActive(true);
            particle.transform.position = transform.position;
            Destroy(particle, 5f);
        }
        
        yield return null;
    }
}
