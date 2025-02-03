using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class DataValue
{
    public event Action<int> OnValueChanged;

    [ShowInInspector]
    public string DataName { get; }
    [ShowInInspector]
    public int Value { get; private set; }

    private string DataKey => $"Data_{DataName}_value";

    public DataValue(string dataName)
    {
        Precondition.CheckNotNull(dataName);
        DataName = dataName;
    }

    public void Init()
    {
        Value = GetDataFromDataManager();
    }

    public void SetData(int value)
    {
        Value = value;
        OnValueChanged?.Invoke(Value);
        UnityEngine.PlayerPrefs.SetInt(DataKey, value);
    }

    public void AddValue(int gain)
    {
        SetData(Value + gain);
    }

    public void Reset()
    {
        SetData(0);
    }

    private int GetDataFromDataManager()
    {
        return UnityEngine.PlayerPrefs.GetInt(DataKey, 0);
    }
}
