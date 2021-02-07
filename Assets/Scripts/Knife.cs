using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed;

    public Rigidbody2D rigidbody2D;
    public ParticleSystem knifeParticle;
    public bool IsRealsed { get; set; }
    public bool Hit { get; set; }

    private void Start()
    {
        Vibration.Init();
    }
    public void FireKnife()
    {
        if (!IsRealsed&&!GameManager.Instance.IsGameOVer)
        {
            IsRealsed = true;
            rigidbody2D.AddForce(new Vector2(x: 0f, y: speed), ForceMode2D.Impulse);
            
            LevelManager.Instance.TotalSpawnKnife++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wheel1")&&!Hit&&!GameManager.Instance.IsGameOVer)
        {
            knifeParticle.Play();
            collision.gameObject.GetComponent<Wheel>().KnifeHit(this);
            
            GameManager.Instance.Score++;
            Vibration.Vibrate(100);

        }
        else if (collision.gameObject.CompareTag("Knife1")&&!Hit&&!GameManager.Instance.IsGameOVer&&IsRealsed)
        {
              Hit = true;
           
          

            rigidbody2D.gravityScale = 5;
            rigidbody2D.AddTorque(50f);
            GameManager.Instance.IsGameOVer = true;
            Invoke(nameof(GameOver), time: 0.5f);
            Vibration.Vibrate(100);
        }
    }

    private void GameOver()
    {
        UI.Instance.GameOver();
    }
}
