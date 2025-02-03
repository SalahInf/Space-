using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HandTuto : MonoBehaviour
{
    [SerializeField] float timeFade = 0.25f;

    [SerializeField] RectTransform _rectHand;
    [SerializeField] Image _imageHandMan;
    [SerializeField] Image _imageHandWoman;

    Image imageHand => (handType == HandType.woman) ? _imageHandWoman : _imageHandMan;

    public HandType handType;

    bool hide = true;

    void Start()
    {
        switch (handType)
        {
            case HandType.none:
                gameObject.SetActive(false);
                break;
            case HandType.man:
                _imageHandWoman.gameObject.SetActive(false);
                break;
            case HandType.woman:
                _imageHandMan.gameObject.SetActive(false);
                break;
        }
        imageHand.DOFade(0, 0).SetId(this);
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (hide)
            {
                DOTween.Kill(this);
                imageHand.DOFade(1, timeFade).SetId(this);
                hide = false;
            }
        }
        else
        {
            if (!hide)
            {
                DOTween.Kill(this);
                imageHand.DOFade(0, timeFade).SetId(this);
                hide = true;
            }
        }

        _rectHand.position = Input.mousePosition;
    }

    public enum HandType { none, man, woman }
}
