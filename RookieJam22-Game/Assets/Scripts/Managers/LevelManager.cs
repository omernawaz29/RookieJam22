using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    CinemachineBrain _camBrain;
    [SerializeField] CinemachineVirtualCamera playerFollowCam;
    [SerializeField] CinemachineVirtualCamera endCam;

    public bool levelEnd = false;


    bool chalksFull = false;
    bool followersFull = false;

    private void Start()
    {
        levelEnd = false;
        instance = this;
        _camBrain = Camera.main.GetComponent<CinemachineBrain>();
        endCam.gameObject.SetActive(false);
        playerFollowCam.gameObject.SetActive(true);

        chalksFull = false;
        followersFull = false;

        if (!AudioManager.instance.isPlaying("BG"))
            AudioManager.instance.Play("BG");

    }

    public void ChalksFull()
    {
        chalksFull = true;
        CheckLevelEnd();

    }

    public void FollowersFull()
    {
        followersFull = true;
        CheckLevelEnd();
    }

    void CheckLevelEnd()
    {
        if(followersFull && chalksFull)
            LevelComplete();
    }

    void LevelComplete()
    {
        endCam.gameObject.SetActive(true);
        playerFollowCam.gameObject.SetActive(false);
        levelEnd = true;

        UIManager.instance.LevelComplete();
    }


    public void FirstTouch()
    {
        UIManager.instance.FirstTouch();
    }

    public void NextLevel()
    {
        int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }






}
