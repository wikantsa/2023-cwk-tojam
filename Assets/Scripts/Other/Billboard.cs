using DG.Tweening;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool bobAnimation;

    private void Start()
    {
        if (bobAnimation)
            transform.DOLocalMoveY(transform.localPosition.y + 0.05f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    void Update()
    {
        transform.forward = -Camera.main.transform.forward;
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
