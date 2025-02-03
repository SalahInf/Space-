using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class DataLevelValue : DataValue
{
    public event Action<float> OnProgressionChanged;
    [ShowInInspector]
    public float Progression { get; private set; }


    public DataLevelValue(string dataName) : base(dataName) { }

    public void SetLevel(int level)
    {
        SetData(Value + 1);
    }

    public void LevelUp()
    {
        SetLevel(Value + 1);

    }

    public void SetProgression(float progression)
    {
        Precondition.CheckNotNull(progression);
        Progression += progression;
        OnProgressionChanged?.Invoke(Progression);
    }

    public void SetCurrentLevel(int level)
    {
        Progression = level;
    }
}
