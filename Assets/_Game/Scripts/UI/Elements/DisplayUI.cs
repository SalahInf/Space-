using UnityEngine;
using TMPro;

namespace UIParty
{
    public class DisplayUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TextMeshProUGUI _displayText;

        public RectTransform Rect => _rect;



        public void SetDisplay(string display)
        {
            Precondition.CheckNotNull(display);
            _displayText.text = display;
        }
    }
}