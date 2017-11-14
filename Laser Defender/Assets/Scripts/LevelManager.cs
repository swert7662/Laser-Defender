using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private int _activeSceneIndex = 0;

    void Start() {
        Scene activeScene = SceneManager.GetActiveScene();
        _activeSceneIndex = activeScene.buildIndex;
    }

    public void LoadLevel(string name) {
        Debug.Log("Level load requested for: " + name);
       // UnityEngine.Cursor.visible = mouseShow(name);
        SceneManager.LoadSceneAsync(name);
    }
    public void QuitRequest() {
        //Debug.Log("I wanna quit the game!");
        Application.Quit();
    }

    public void LoadNextLevel() {
        SceneManager.LoadSceneAsync(++_activeSceneIndex);
    }
    /*
    public bool mouseShow(string name) {
        if(name == "Win" || name == "Start" || name == "Lose") {
            return true;
        }
        else {
            return false;
        }
    }*/
}
