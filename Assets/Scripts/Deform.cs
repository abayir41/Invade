using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deform : MonoBehaviour
{
    [Range(0, 0.3f)]
    public float deformRadius = 0.2f;
    [Range(0, 0.3f)]
    public float maxDeform = 0.1f;
    [Range(0, 0.1f)]
    public float damageMultiplier = 1;
    [Range(0, 100000)]
    public float minDamage = 1;

    public int maxColAmount;

    private int _colCounter;
 
    public AudioClip[] collisionSounds;
 
    private MeshFilter filter;
    private MeshCollider coll;
    private Vector3[] meshVerticies;
 
    void Start()
    {
        filter = GetComponent<MeshFilter>();
 
        if (GetComponent<MeshCollider>())
            coll = GetComponent<MeshCollider>();
 
        meshVerticies = filter.mesh.vertices;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_colCounter >= maxColAmount) return;

        _colCounter++;

        float collisionPower = collision.impulse.magnitude;

        
        if (!(collisionPower > minDamage)) return;
        
        if (collisionSounds.Length > 0)
            AudioSource.PlayClipAtPoint(collisionSounds[Random.Range(0, collisionSounds.Length)], transform.position, 0.5f);
        
        foreach (var point in collision.contacts)
        {
            for (var i = 0; i < meshVerticies.Length; i++)
            {
                var vertexPosition = meshVerticies[i];
                var pointPosition = transform.InverseTransformPoint(point.point);
                var distanceFromCollision = Vector3.Distance(vertexPosition, pointPosition);

                if (!(distanceFromCollision < deformRadius)) continue; // If within collision radius and within max deform
                
                var xDeform = point.normal.x * damageMultiplier / distanceFromCollision;
                var yDeform = point.normal.y * damageMultiplier / distanceFromCollision;
                var zDeform = point.normal.z * damageMultiplier / distanceFromCollision;

                xDeform = xDeform switch
                {
                    > 0 => xDeform > maxDeform ? maxDeform : xDeform,
                    < 0 => xDeform < -maxDeform ? -maxDeform : xDeform,
                    _ => xDeform
                };
                
                yDeform = yDeform switch
                {
                    > 0 => yDeform > maxDeform ? maxDeform : yDeform,
                    < 0 => yDeform < -maxDeform ? -maxDeform : yDeform,
                    _ => yDeform
                };
                
                zDeform = zDeform switch
                {
                    > 0 => zDeform > maxDeform ? maxDeform : zDeform,
                    < 0 => zDeform < -maxDeform ? -maxDeform : zDeform,
                    _ => zDeform
                };

                var deform = new Vector3(xDeform, yDeform, zDeform);
                Debug.Log(deform);
                
                var fixedDeform = transform.InverseTransformDirection(deform);

                meshVerticies[i] += fixedDeform;
            }
        }
 
        UpdateMeshVerticies();
    }
 
    void UpdateMeshVerticies()
    {
        var mesh = filter.mesh;
        mesh.vertices = meshVerticies;
        coll.sharedMesh = mesh;
    }
    
    
}
