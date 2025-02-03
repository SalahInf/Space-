using System;
using UnityEngine;
using TMPro;

namespace UIParty
{
    public class InputTextUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_InputField _InputText;

        public RectTransform Rect => _rect;


        public void Init(Action<string> callback)
        {
            Precondition.CheckNotNull(callback);

            _InputText.onEndEdit.AddListener((t) => callback(t));
        }

        public void SetText(string text)
        {
            _InputText.text = text;
        }
    }
}