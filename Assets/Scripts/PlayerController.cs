using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public GameObject explosion;
    public GameObject damageEffect;

    public PlayerHealthbarScript playerHealthBar;
    public CoinCount coinCountScript;
    public GameController gameController;

    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip explosionSound;
    public AudioClip coinSound;

    public float speed = 10f;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float health = 20f;
    float barFillAmount = 1f;
    float damage = 0;

    private float padding = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        FindBoundries();
        damage = barFillAmount / health;
    }

    void FindBoundries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        
          float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
          float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed; 

          float newXpos = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
          float newYpos = Mathf.Clamp(transform.position.y + deltaY, minY, maxY);

          transform.position = new Vector2(newXpos, newYpos);      
        

        if (Input.GetMouseButton(0))
        {
            Vector2 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = Vector2.Lerp(transform.position, newPos, 20 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            audioSource.PlayOneShot(damageSound, 0.3f);
            DamagePlayerHealthbar();
            Destroy(collision.gameObject);
            GameObject playerDamageVfx = Instantiate(damageEffect, collision.transform.position, Quaternion.identity);
            Destroy(playerDamageVfx, 0.06f);

            if(health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.3f);
                gameController.GameOver();
                Destroy(gameObject);
                GameObject blast = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(blast, 2f);
            }               
        }

        if(collision.gameObject.tag == "Coin")
        {
            audioSource.PlayOneShot(coinSound, 0.6f);
            Destroy(collision.gameObject);
            coinCountScript.AddCount();
        }
    }

    void DamagePlayerHealthbar()
    {
        if(health > 0)
        {
            health -= 1;
            barFillAmount = barFillAmount - damage;
            playerHealthBar.SetAmount(barFillAmount);
        }
    }
}
