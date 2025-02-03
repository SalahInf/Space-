using UnityEngine;
using TMPro;
using DG.Tweening;

namespace UIParty
{
    public class UIFeedback : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI[] feedbackTMP;
        [SerializeField] Feedback[] feedbacks;

        int index = 0;

        public void Init()
        {
            foreach (var item in feedbackTMP)
            {
                item.transform.localScale = Vector3.zero;
                item.gameObject.SetActive(true);
            }
        }

        public void ShowFeedback()
        {
            ShowFeedback(Random.Range(0, feedbacks.Length));
        }

        public void ShowFeedback(int indexfeedBack)
        {
            index = (index++) % feedbackTMP.Length;

            feedbackTMP[index].text = feedbacks[indexfeedBack].name;
            feedbackTMP[index].color = feedbacks[indexfeedBack].color;

            feedbackTMP[index].gameObject.SetActive(true);

            DOTween.Kill(feedbackTMP[index].gameObject, true);

            feedbackTMP[index].transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetId(feedbackTMP[index].gameObject).OnComplete(() =>
            {
                feedbackTMP[index].transform.DOScale(0, 0.5f).SetEase(Ease.OutSine).SetId(feedbackTMP[index].gameObject).OnComplete(() =>
                {
                    feedbackTMP[index].gameObject.SetActive(false);
                });
            });
        }
    }
}
[System.Serializable]
public class Feedback
{
    public string name;
    public Color color;
}