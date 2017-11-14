using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    public float spawnDelay = .5f;

    private bool movingRight = true;
    private float xMin, xMax;

	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMax = rightBoundary.x;
        xMin = leftBoundary.x;
        SpawnEnemies();
	}
    //Draw wirefram box around formation
    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    void Update () {
        if (movingRight) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else {
            transform.position += Vector3.left * speed * Time.deltaTime; ;
        }
        //Edges of formation box
        float rightEdge = transform.position.x + (0.5f * width);
        float leftEdge = transform.position.x - (0.5f * width);
        if (leftEdge < xMin) {
            movingRight = true;
        }
        else if (rightEdge > xMax) {
            movingRight = false;
        }

        if (AllMembersDead()) {
            Debug.Log("Empty Formation");
            SpawnUntilFull();
        }

	}

    void SpawnEnemies() {
        foreach (Transform child in transform) {
            //Spawn an enemy with the public GameObject enemyPrefab as GameObject
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            //Make the spawned enemy a child of the position in enemyFormation
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull() {
        Transform freePos = NextFreePosition();
        if (freePos) {
            GameObject enemy = Instantiate(enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePos;
        }
        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    Transform NextFreePosition() {
        foreach (Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount == 0) {
                return childPositionGameObject;
            }
        }
        return null;
    }

    bool AllMembersDead() {
        foreach(Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount > 0) {
                return false;
            }
        }
        return true;
    }
}
