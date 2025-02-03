using UnityEngine;
using TMPro;


namespace UIParty
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TextMeshProUGUI _text;

        public RectTransform Rect => _rect;

        private DataValue _currency;

        public void Init (DataValue currency)
        {
            Precondition.CheckNotNull(currency);
            
            _currency = currency;
            SetCurrencyText(_currency.Value);
            _currency.OnValueChanged += SetCurrencyText;
        }
        
        private void SetCurrencyText (int value)
        {
            _text.text = value.ToString();
        }
    }
}