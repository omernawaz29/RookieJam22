using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] Image detectionMeter;
    [SerializeField] float detectionMeterOffset = 2.5f;

    [SerializeField] TextMeshProUGUI chalkCount;
    [SerializeField] TextMeshProUGUI followerCount;
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject EndScreen;


    Transform playerTransform;

    private void Start()
    {
        StartScreen.SetActive(true);
        EndScreen.SetActive(false);

        instance = this;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetDetectionMeter(0);
    }

    private void Update()
    {
        PlaceDetectionMeter();
    }

    private void FixedUpdate()
    {
        
    }

    void PlaceDetectionMeter()
    {
        var pos = Camera.main.WorldToScreenPoint(playerTransform.position + Vector3.up * detectionMeterOffset);
        detectionMeter.transform.position = pos;
    }

    public void SetDetectionMeter(float value)
    {
        detectionMeter.color = new Color(1, 1, 1, value);
    }

    public void UpdateChalkCount(int value)
    {
        chalkCount.text = value.ToString();
    }

    public void UpdateFollowerCount(int value)
    {
        followerCount.text = value.ToString();

    }


    public void FirstTouch()
    {
        StartScreen.SetActive(false);
    }

    public void LevelComplete()
    {
        EndScreen.SetActive(true);
    }
}
