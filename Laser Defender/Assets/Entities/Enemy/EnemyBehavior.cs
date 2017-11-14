using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public float health = 150;
    public int pointsValue = 150;
    public GameObject projectile;
    public float projectileSpeed = 15f;
    public float shotsPerSecond = .5f;
    public AudioClip laserSound;
    public AudioClip deathSound;

    float altFire = 1;
    private ScoreKeeper scoreKeeper;

    private void Start() {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    private void Update() {
        float probability = shotsPerSecond * Time.deltaTime;
        if (Random.value < probability) {
            Fire();
        }
    }

    void Fire() {
        altFire *= -1;
        Vector3 beamPos = new Vector3(transform.position.x + (altFire * .22f), (transform.position.y - .7f), 0);
        AudioSource.PlayClipAtPoint(laserSound, transform.position);
        GameObject beam = Instantiate(projectile, beamPos, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log(collision);
        Projectile missile = collision.gameObject.GetComponent<Projectile>();
        if (missile) {
            health -= missile.GetDamage();
            missile.Hit();
            if(health <= 0) {
                Die();
            }
        }
    }

    void Die() {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        scoreKeeper.Score(pointsValue);
        Destroy(gameObject);  
    }
}
