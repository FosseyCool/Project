using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Apple : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem appleParticle;
    private SpriteRenderer sp;
   
    private BoxCollider2D appleCollison;
    private  void Start()
    {
        appleCollison = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knife1"))
        {
            appleCollison.enabled = false;
            sp.enabled = false;
            transform.parent = null;
            
            appleParticle.Play();
            Destroy(gameObject, t:2f);

            UI.Instance.AmountApple++;
            UI.Instance.Apples.text = UI.Instance.AmountApple.ToString();
        }
    }

}
