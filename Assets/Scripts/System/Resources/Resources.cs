
public enum BattleResourceTypes
{
    Funds,
    Power,
    RefinedOre,
    Population,
}

[System.Serializable]
public struct BattleResourceStruct
{
    public BattleResourceTypes resourceType;
    public float amount;
}