using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    public BattleResourceSystem battleResourceSystem;
    public int camp;
    public int playerID;
    public string playerName;

    public void Start()
    {
        battleResourceSystem = new BattleResourceSystem(this, InitEmptyResource());
    }

    private Dictionary<BattleResourceTypes, BattleResource> InitEmptyResource()
    {
        Dictionary<BattleResourceTypes, BattleResource> battleResources = new Dictionary<BattleResourceTypes, BattleResource>();
        foreach(BattleResourceTypes type in System.Enum.GetValues(typeof(BattleResourceTypes)))
        {
            battleResources[type] = new BattleResource(type, 0, float.MaxValue);
        }
        return battleResources;
    }
}