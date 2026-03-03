public class BattleResourceSystem
{
    public Player owner;
    public float funds;
    public int population;
    public int populationLimit;
    public float powerGeneration;
    public float powerload;
    public float refinedOre;

    public BattleResourceSystem(Player owner)
    {
        this.owner = owner;
    }
    public BattleResourceSystem(Player owner, float funds, int population, int populationLimit, float powerGeneration, float powerload, float refinedOre)
    {
        this.owner = owner;
        this.funds = funds;
        this.population = population;
        this.populationLimit = populationLimit;
        this.powerGeneration = powerGeneration;
        this.powerload = powerload;
        this.refinedOre = refinedOre;
    }
}