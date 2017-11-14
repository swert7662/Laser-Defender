using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public AudioClip startClip, gameClip, endClip;
    static MusicPlayer instance = null;
    private AudioSource music;

    void Awake () {
        //Debug.Log("Music player Awake " + GetInstanceID());
        if (instance != null && instance != this) {
            Destroy(gameObject);
            //print("Dupe music player self-destruct!");
        }
        else {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
            music = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }   
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode loadscenemode) {
        if (scene.buildIndex == 0) {
            music.clip = startClip;
            music.volume = .2f;
        }
        else if(scene.buildIndex == 1) {
            music.clip = gameClip;
            music.volume = .2f;
        }
        else if(scene.buildIndex == 2) {
            music.clip = endClip;
            music.volume = .1f;
        }
        music.loop = true;
        music.Play();
    }
}
