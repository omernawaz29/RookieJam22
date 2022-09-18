using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] Image detectionMeter;
    [SerializeField] float detectionMeterOffset = 2.5f;

    [SerializeField] TextMeshProUGUI chalkCount;
    [SerializeField] TextMeshProUGUI followerCount;
    [SerializeField] TextMeshProUGUI randomHitText;
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject EndScreen;
    [SerializeField] string[] HitTexts;


    Transform playerTransform;

    private void Start()
    {
        StartScreen.SetActive(true);
        EndScreen.SetActive(false);

        instance = this;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetDetectionMeter(0);
        randomHitText.text = " ";
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

    public void ShowHitText()
    {
        if (randomHitText.text != " ")
            return;
        var pos = Camera.main.WorldToScreenPoint(playerTransform.position + Vector3.up * detectionMeterOffset);


        randomHitText.transform.position = pos;
        randomHitText.transform.localScale = Vector3.one;

        int textindex = Random.Range(0, HitTexts.Length);
        randomHitText.text = HitTexts[textindex];
        
        AudioManager.instance.Play("Hit " + textindex.ToString());


        randomHitText.transform.DOMove(pos + Vector3.up * 1f, 2f).OnComplete(() => randomHitText.text = " ");
        randomHitText.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 1f);
        
        
    }
    void PlaceHitText()
    {
        var pos = Camera.main.WorldToScreenPoint(playerTransform.position + Vector3.up * detectionMeterOffset);
        randomHitText.transform.position = pos + Vector3.up * 0.25f;
    }



}
