using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;


namespace UIParty
{
    public class ButtonUI : MonoBehaviour
    {
        public bool isDamege;
        public bool isSpeed;
        public bool isJet;
        public bool isSac;

        public Button _button;
        [SerializeField] private RectTransform _rect;
        [SerializeField] Color color;

        [ShowIf("isSpeed")]
        public TMP_Text axeSpeedLevelTxt;
        [ShowIf("isSpeed")]
        public TMP_Text axeSpeedGoldTxt;

        [ShowIf("isDamege")]
        public TMP_Text axeDamagGoldTxt;
        [ShowIf("isDamege")]
        public TMP_Text axeDamagLevelTxt;

        [ShowIf("isJet")]
        public TMP_Text jetLevelTxt;
        [ShowIf("isJet")]
        public TMP_Text jetGoldTxt;

        [ShowIf("isSac")]
        public TMP_Text sacLevelTxt;
        [ShowIf("isSac")]
        public TMP_Text sacGoldTxt;

        [SerializeField] GameObject m_grayBtn;

        public RectTransform Rect => _rect;

        public void Init(Action callback)
        {
            Precondition.CheckNotNull(callback);

            _button.onClick.AddListener(() => callback());
            _button.onClick.AddListener(() => { AudioManager.Play("Click"); });
        }
        public bool ChangeBtnColor(bool state)
        {
            m_grayBtn.SetActive(!state);
            return state;
        }

    }
}