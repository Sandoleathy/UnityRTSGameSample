using System.Collections.Generic;

public class BattleResourceSystem
{
    public Player owner;
    public float funds;
    public int population;
    public int populationLimit;
    public float powerGeneration;
    public float powerload;
    public float refinedOre;
    public Dictionary<BattleResourceTypes, BattleResource> battleResources;

    public BattleResourceSystem(Player owner, Dictionary<BattleResourceTypes, BattleResource> battleResources)
    {
        this.owner = owner;
        this.battleResources = battleResources;
    }
    public void ConsumeResource(BattleResourceTypes resourceType, float value)
    {
        battleResources[resourceType].amount -= value;
    }
    public void GainResource(BattleResourceTypes resourceType, float value)
    {
        battleResources[resourceType].amount += value;
    }
    public Dictionary<BattleResourceTypes, BattleResource> GetBattleResources()
    {
        return battleResources;
    }
    public void SetBattleResource(BattleResourceTypes resourceType, float value)
    {
        battleResources[resourceType].amount = value;
    }
}

public class BattleResource
{
    public BattleResourceTypes resourceType;
    public float amount;
    public float limit;

    public BattleResource(BattleResourceTypes resourceType, float amount, float limit)
    {
        this.resourceType = resourceType;
        this.amount = amount;
        this.limit = limit;
    }
}