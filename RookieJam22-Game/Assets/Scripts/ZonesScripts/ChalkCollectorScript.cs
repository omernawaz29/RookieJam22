using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ChalkCollectorScript : MonoBehaviour
{
    [SerializeField] TextMeshPro dropZoneText;
    [SerializeField] GameObject dropeZoneCompleteImage;
    [SerializeField] int chalksRequired;

    int chalksDeposited;
    bool isCompleted;

    private void Start()
    {
        chalksDeposited = 0;
        isCompleted = false;

        UpdateText();
    }

    public void AddChalk()
    {
        transform.DOComplete();
        transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
        chalksDeposited++;

        if (chalksDeposited >= chalksRequired)
            Completed();
        else 
            UpdateText();
    }

    void UpdateText()
    {
        dropZoneText.text = chalksDeposited.ToString() + "/" + chalksRequired.ToString();
    }

    void Completed()
    {
        if (isCompleted)
            return;
        transform.localScale = new Vector3(1, 1, 1);
        dropZoneText.gameObject.SetActive(false);
        dropeZoneCompleteImage.SetActive(true);
        isCompleted = true;
        LevelManager.instance.ChalksFull();
    }



}
