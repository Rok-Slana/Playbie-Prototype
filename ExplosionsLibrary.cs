using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionsLibrary : MonoBehaviour
{
    private Component[] explosionLib;

    void Start()
    {
        explosionLib = GetComponentsInChildren<ParticleSystem>();        
    }
    
    public void PlayRandomExplosion()
    {
        int libIndex = Random.Range(0, explosionLib.Length-1);
        explosionLib[libIndex].GetComponent<ParticleSystem>().transform.position = this.transform.position;
        explosionLib[libIndex].GetComponent<ParticleSystem>().Play();
    }
}
