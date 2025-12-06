using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem movementParticle2;

    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range (0, 1f)]
    [SerializeField] float tonkFormationPeriod;

    [SerializeField] Rigidbody2D rb;
         
    float counter;
    int anim = 1;

    private void Update()
    {
        counter += Time.deltaTime;

        if (Mathf.Abs(rb.linearVelocityX) > occurAfterVelocity)
        {
            if (counter > tonkFormationPeriod)
            {
                PlayParticleEffect();
                counter = 0;
            }
        }
    }

    private void PlayParticleEffect()
    {
        if (anim == 1)
        {
            movementParticle.Play();
            anim = 0;
        }
        else
        {
            movementParticle2.Play();
            anim = 1;
        }
    }
}
