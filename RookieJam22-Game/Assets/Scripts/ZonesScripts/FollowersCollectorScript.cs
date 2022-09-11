using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowersCollectorScript : MonoBehaviour
{
    [SerializeField] TextMeshPro dropZoneText;
    [SerializeField] GameObject dropeZoneCompleteImage;
    [SerializeField] Transform[] idlePositions;
    [SerializeField] int followersRequired;

    int followersDeposited;
    bool isCompleted;


    private void Start()
    {
        followersDeposited = 0;
        isCompleted = false;

        UpdateText();
    }

    public void AddFollower()
    {
        transform.DOPunchScale(Vector3.one * 0.5f, 0.25f);
        followersDeposited++;

        if (followersDeposited >= followersRequired)
            Completed();
        else
        {
            UpdateText();
        }
    }

    void UpdateText()
    {
        dropZoneText.text = followersDeposited.ToString() + "/" + followersRequired.ToString();
    }

    void Completed()
    {
        if (isCompleted)
            return;

        dropZoneText.gameObject.SetActive(false);
        dropeZoneCompleteImage.SetActive(true);
        isCompleted = true;
        LevelManager.instance.FollowersFull();
    }


    public Transform GetFollowerIdlePosition()
    {
        return idlePositions[followersDeposited];
    }
}
