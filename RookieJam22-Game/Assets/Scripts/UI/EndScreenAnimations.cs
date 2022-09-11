using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenAnimations : MonoBehaviour
{
    [SerializeField] Transform LevelComplete;
    [SerializeField] Transform WinText;
    [SerializeField] Transform NextButton;
    [SerializeField] Transform RetryButton;


    private void Start()
    {
        Button b =  NextButton.GetComponent<Button>();
        b.onClick.AddListener(() => LevelManager.instance.NextLevel());
        b = RetryButton.GetComponent<Button>();
        b.onClick.AddListener(() => LevelManager.instance.RetryLevel());
    }

    private void OnEnable()
    {
        LevelComplete.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        LevelComplete.DOScale(new Vector3(2, 2, 2), 1f);
        WinText.DOShakeScale(2f);
    }
}
