using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Camera camera;
    public float health = 250f;
    public float speed = 15.0f;
    public float padding = 1f;
    public GameObject projectile;
    public float projectileSpeed;
    public float fireRate = 0.2f;
    public AudioClip laserSound;
    public AudioClip deathSound;
    public bool AsteroidsMode = false;

    bool coroutineCalled = false;
    bool colorChange = false;
    bool damaged = false;
    
    float xMin;
    float xMax;
    float altFire = 1;
    Transform beamSpawnPos;
    private Vector3 mousePosition;

    // Use this for initialization
    void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftmost.x + padding;
        xMax = rightmost.x - padding;
    }
	
	// Update is called once per frame
	void Update () {
        if (!AsteroidsMode) { playerInput(); }
        else if (AsteroidsMode) { playerInputAsteroidsMode(); }        
	}

    void playerInput() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("Fire", 0.000001f, fireRate);
        }
        if (Input.GetKeyUp(KeyCode.Space)){
            CancelInvoke("Fire");
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
    void playerInputAsteroidsMode() {
        //Grab the current mouse position on the screen
        mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - camera.transform.position.z));

        //Rotates toward the mouse
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg - 90);

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            InvokeRepeating("Fire", 0.000001f, fireRate);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            CancelInvoke("Fire");
        }
    }

    void Fire() {
        altFire *= -1;
        if (altFire < 0) { beamSpawnPos = this.gameObject.transform.GetChild(2); }
        else beamSpawnPos = this.gameObject.transform.GetChild(3);
        Vector3 beamPos = new Vector3(beamSpawnPos.position.x, beamSpawnPos.position.y, 0);
        print(beamPos);
        AudioSource.PlayClipAtPoint(laserSound, transform.position);
        GameObject beam = Instantiate(projectile, beamPos, transform.rotation) as GameObject;
        if (!AsteroidsMode) { beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0); }
        else if (AsteroidsMode) {
            Vector3 direction = mousePosition - transform.position;
            beam.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();
        if (missile) {
            health -= missile.GetDamage();
            missile.Hit();
            if (health < 150f) {
                damaged = true;
                colorChange = true;
            }
            if (colorChange && damaged) {
                if (!coroutineCalled) {
                    StartCoroutine("color");
                }
            }
            else {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (health <= 0) {
                Invoke("End", 1f);
                Die();
                
            }
        }
    }

    void Die() {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        CancelInvoke("Fire");
        gameObject.SetActive(false);
        //Destroy(gameObject);
        
    }

    void End() {
        Debug.Log("End method");
        LevelManager lvlMan = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        lvlMan.LoadLevel("End");
    }

    IEnumerator color() {
        while (colorChange && damaged) {
            coroutineCalled = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, .4f, .4f);
            yield return new WaitForSeconds(0.3f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        coroutineCalled = false;
    }

}
