using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Player player;

    private Rigidbody rb;
    private Transform mainCameraTransform;
    private AudioManager aM;
    private ExplosionsLibrary explosionLib;
    private float timer;
    private bool timerFlag = false;

    private void Update()
    {
        if (timerFlag)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f) ProjectileExplode();
        }
    }

    public void FireProjectile(float thrust, Vector3 direction)
    {
        transform.position = player.transform.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddRelativeForce(direction*thrust, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        aM.Play("explode");
        if (collision.gameObject.tag == "Bouncer") ;
        else
        {
            ProjectileExplode();
        }       
    }

    //This used to be done in Start() but has been moved here to be used via prefab
    public void SetUpProjectile()
    {
        player = FindObjectOfType<Player>();
        rb = gameObject.GetComponent<Rigidbody>();
        mainCameraTransform = Camera.main.transform;
        explosionLib = FindObjectOfType<ExplosionsLibrary>();
        aM = FindObjectOfType<AudioManager>();
    }

    public void SetSelfDestructTime(float time)
    {
        timer = time;
        timerFlag = true;
    }

    private void ProjectileExplode()
    {
        explosionLib.transform.position = this.transform.position;
        explosionLib.PlayRandomExplosion();
        Destroy(gameObject);
    }


}
