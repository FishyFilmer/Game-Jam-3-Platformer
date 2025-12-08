using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class ValveScript : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;

    public SpriteRenderer SteamSr;
    public AreaEffector2D SteamForce;

    public bool inside = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inside)
        {
            animator1.Play("ValveSpin");
            animator2.Play("FanSpin");
            SteamSr.enabled = true;
            SteamForce.enabled = true;
            animator3.Play("Steam");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inside = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inside = false;
        }
    }
}
