using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public DataValue coinData;
    public DataValue scoreData;
    public DataLevelValue levelData;

    public void Init()
    {
        coinData = new DataValue("coin");
        scoreData = new DataValue("score");
        levelData = new DataLevelValue("level");
    }

    public DataValue GetData(Datatype datatype)
    {
        switch (datatype)
        {
            case Datatype.Coin:
                return coinData;
            case Datatype.Score:
                return scoreData;
            case Datatype.Levee:
                return levelData;
        }
        return null;
    }

    public enum Datatype { Coin, Score, Levee}
}
