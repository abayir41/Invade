using System;
using UnityEngine;
using System.Collections;

namespace PolygonArsenal
{
    public class PolygonProjectileScript : MonoBehaviour
    {
        public GameObject impactParticle;
        public GameObject projectileParticle;
        public GameObject muzzleParticle;
        public GameObject[] trailParticles;
        public float radius;
        public float power;
        public AudioClip Boom;
        [Header("Adjust if not using Sphere Collider")]
        public float colliderRadius = 1f;
        [Range(0f, 1f)]
        public float collideOffset = 0.15f;

        void Start()
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
                muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
                Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
            }
        }

        /*
        void FixedUpdate()
        {
            RaycastHit hit;

            float rad;
            if (transform.GetComponent<SphereCollider>())
                rad = transform.GetComponent<SphereCollider>().radius;
            else
                rad = colliderRadius;

            Vector3 dir = transform.GetComponent<Rigidbody>().velocity;
            if (transform.GetComponent<Rigidbody>().useGravity)
                dir += Physics.gravity * Time.deltaTime;
            dir = dir.normalized;

            float dist = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;

            if (Physics.SphereCast(transform.position, rad, dir, out hit, dist))
            {
                transform.position = hit.point + (hit.normal * collideOffset);

                GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;

                if (hit.transform.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
                {
                    Destroy(hit.transform.gameObject);
                }

                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }
                Destroy(projectileParticle, 3f);
                Destroy(impactP, 5.0f);
                Destroy(gameObject);

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
        }
*/
        private bool hasCollided = false;

        void OnCollisionEnter(Collision hit)
        {
            if (!hasCollided)
            {
                hasCollided = true;
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.contacts[0].normal)) as GameObject;

                if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
                {
                    Destroy(hit.gameObject);
                }

                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }
                Destroy(projectileParticle, 3f);
                Destroy(impactParticle, 5f);
                Destroy(gameObject);

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                    trail.transform.SetParent(null);
                    Destroy(trail.gameObject, 2f);
                    }
                }
                
                var colliders = Physics.OverlapSphere(transform.position, radius);
                foreach (var hit2 in colliders)
                {
                    if(!hit2.transform.GetComponent<Car>()) continue;
                    if(!hit2.transform.GetComponent<Car>().Inside) continue;
                    
                    
                    Rigidbody rb = hit2.GetComponent<Rigidbody>();

                    if (rb != null && hit2.transform.GetComponent<Car>() != this)
                        rb.AddExplosionForce(power, transform.position, radius, 3.0F);
                    
                    
                    if(hit2.transform.GetComponent<Car>().explodedNormalCar) continue;
                    
                    
                    
                    
                    StartCoroutine(hit2.transform.GetComponent<Car>().Explode());
                    
                    hit2.transform.GetComponent<Car>().explodedNormalCar = true;

                } 
                Camera.main!.GetComponent<AudioSource>().PlayOneShot(Boom);
                CameraShake.Instance.Shakeeee(0.6f, 1.0f);
                Car.SomethingChanged?.Invoke();
            }
        }
    }
}