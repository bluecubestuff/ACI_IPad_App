using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticles : MonoBehaviour {

    private ParticleSystem ps;

    //Auto destroys particle (Dust/Bubble)
    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
