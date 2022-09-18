using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenJoyStickAnimation : MonoBehaviour
{
    [SerializeField] Transform Handle;
    [SerializeField] Transform Text;
    [SerializeField] Vector3[] Points;


    private void Start()
    {
        PathType pathtype = PathType.CatmullRom;
        Text.DOScale(Vector3.one * 2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        Handle.DOLocalPath(Points, 2.5f, pathtype).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);
    }



}
