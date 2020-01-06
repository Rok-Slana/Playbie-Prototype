using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTHolder : MonoBehaviour
{
    float time = 3.0f;

    public void OnCollisionEnter(Collision collision)
    {
        this.GetComponent<Collider>().enabled = false;
        Invoke("Kill", time);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
