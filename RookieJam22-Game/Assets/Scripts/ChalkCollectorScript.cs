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

        if (isCompleted)
        {
            Completed();
            return;
        }

        chalksDeposited++;
        transform.DOPunchScale(Vector3.one * 0.5f, 0.1f);
        UpdateText();

        if (chalksDeposited == chalksRequired)
            isCompleted = true;
    }

    void UpdateText()
    {
        dropZoneText.text = chalksDeposited.ToString() + "/" + chalksRequired.ToString();
    }

    void Completed()
    {
        dropZoneText.gameObject.SetActive(false);
        dropeZoneCompleteImage.SetActive(true);
        dropeZoneCompleteImage.transform.DOPunchScale(Vector3.one * 0.75f, 0.1f);

        isCompleted = true;
    }



}
