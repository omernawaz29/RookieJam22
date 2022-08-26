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
    
    Transform playerTransform;

    private void Start()
    {
        instance = this;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        PlaceDetectionMeter();
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

}
