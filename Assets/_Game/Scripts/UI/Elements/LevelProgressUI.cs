using UnityEngine;


namespace UIParty
{
    public class LevelProgressUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private DisplayUI _currentLevelText;
        [SerializeField] private DisplayUI _nextLevelText;
        [SerializeField] private ProgressBarUI _progressBar;

        public RectTransform Rect => _rect;
        
        private DataLevelValue _level;

        public void Init (DataLevelValue level)
        {            
            _level = level;
            SetTexts();
            _level.OnProgressionChanged += UpdateProgression;
            _level.OnValueChanged += LevelChanged;
        }

        private void SetTexts ()
        {
            _currentLevelText.SetDisplay($"LEVEL {_level.Value + 1}");
            _nextLevelText?.SetDisplay($"LEVEL {_level.Value + 2}");
            _progressBar.SetProgessionRaw(_level.Progression);
        }

        private void LevelChanged (int level)
        {
            SetTexts();
        }

        private void UpdateProgression (float progression)
        {
            _progressBar.SetProgression(progression);
        }
    }
}