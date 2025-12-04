using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;

    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range (0, 1f)]
    [SerializeField] float tonkFormationPeriod;

    [SerializeField] Rigidbody2D rb;
         
    float counter;

    private void Update()
    {
        counter += Time.deltaTime;

        if (Mathf.Abs(rb.linearVelocityX) > occurAfterVelocity)
        {
            if (counter > tonkFormationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }
}
