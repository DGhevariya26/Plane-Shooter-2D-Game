using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyScript : MonoBehaviour
{

    public Transform []gunPoint;    

    public GameObject enemyBullet;
    public GameObject enemyFlash;
    public GameObject enemyExplosionPrefab;
    public GameObject damageEffect;
    public GameObject coinPrefab;
    public Healthbar healthbar;

    public AudioSource audioSource;
    public AudioClip bulletSound;
    public AudioClip damageSound;
    public AudioClip explosionSound;


    public float enemyBulletSpawnTime = 0.6f;
    public float enemySpeed = 1f;
    public float health = 10f;

    float barSize = 1f;
    float damage = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        enemyFlash.SetActive(false);
        StartCoroutine(EnemyShooting());
        damage = barSize / health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * enemySpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            audioSource.PlayOneShot(damageSound, 0.6f);
            DamageHealthbar();
            Destroy(collision.gameObject);
            GameObject damageVfx = Instantiate(damageEffect, collision.transform.position, Quaternion.identity);
            Destroy(damageVfx, 0.06f);

            if(health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.2f);
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.4f);
            }
                       
        }
    }

    void DamageHealthbar()
    {
        if (health > 0)
        {
            health -= 1;
            barSize = barSize - damage;
            healthbar.SetSize(barSize);
        }
    }

    void EnemyFire()
    {
        for(int i = 0; i < gunPoint.Length; i++)
        {
            Instantiate(enemyBullet, gunPoint[i].position, Quaternion.identity);
        }
      
    }

    IEnumerator EnemyShooting()
    {
        while(true)
        {
            yield return new WaitForSeconds(enemyBulletSpawnTime);
            EnemyFire();
            audioSource.PlayOneShot(bulletSound, 0.2f);
            enemyFlash.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            enemyFlash.SetActive(false);
        }
        
    }
}
