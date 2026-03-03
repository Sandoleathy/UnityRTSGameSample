using UnityEngine;

public class Player: MonoBehaviour
{
    public BattleResourceSystem battleResourceSystem;
    public int camp;
    public int playerID;
    public string playerName;

    public void Start()
    {
        battleResourceSystem = new BattleResourceSystem(this, 0, 0, 200, 0,0 ,0);
    }
}