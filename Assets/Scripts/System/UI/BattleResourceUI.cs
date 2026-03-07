using TMPro;
using UnityEngine;

public class BattleResourceUI : MonoBehaviour
{
    public TMP_Text fundNum;
    public TMP_Text oreNum;
    public TMP_Text powerNum;
    public Player player;

    void Update()
    {
        fundNum.SetText(player.battleResourceSystem.funds.ToString());
        oreNum.SetText(player.battleResourceSystem.refinedOre.ToString());
        powerNum.SetText($"{player.battleResourceSystem.powerGeneration}/{player.battleResourceSystem.powerload}");
    }
}