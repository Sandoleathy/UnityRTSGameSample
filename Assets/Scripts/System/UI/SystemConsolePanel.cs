using UnityEngine;

public class SystemConsolePanel : MonoBehaviour
{
    private bool isVisible = false;

    private GameManager gameManager;
    private Player selectedPlayer;

    private string fundInput = "";
    private string oreInput = "";

    private Rect windowRect = new Rect(100, 100, 420, 450);

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F12))
        {
            isVisible = !isVisible;
        }
    }

    private void OnGUI()
    {
        if (!isVisible) return;

        windowRect = GUI.Window(
            1001,
            windowRect,
            DrawWindow,
            "系统控制台"
        );
    }

    private void DrawWindow(int id)
    {
        GUILayout.Space(10);

        GUILayout.Label("玩家列表:");

        foreach (var player in gameManager.playerManager.players)
        {
            if (GUILayout.Button($"玩家 {player.playerName}"))
            {
                if (selectedPlayer == player)
                {
                    selectedPlayer = null;
                }
                else
                {
                    selectedPlayer = player;
                    fundInput = player.battleResourceSystem.funds.ToString();
                    oreInput = player.battleResourceSystem.refinedOre.ToString();
                }
            }
        }

        if (selectedPlayer != null)
        {
            GUILayout.Space(20);
            GUILayout.Label($"玩家 {selectedPlayer.playerName} 资源信息:");
            ResourceDebugPanel(selectedPlayer);
        }

        // 只允许拖动标题栏
        GUI.DragWindow(new Rect(0, 0, 10000, 25));
    }

    private void ResourceDebugPanel(Player player)
    {
        var resource = player.battleResourceSystem;

        // -------- 资金 --------
        GUILayout.BeginHorizontal();

        GUILayout.Label($"资金：{resource.funds}");

        fundInput = GUILayout.TextField(fundInput, GUILayout.Width(100));

        bool fundChanged = fundInput != resource.funds.ToString();

        Color oldColor = GUI.backgroundColor;
        if (fundChanged)
            GUI.backgroundColor = Color.green;

        if (GUILayout.Button("设置", GUILayout.Width(60)))
        {
            if (float.TryParse(fundInput, out float newFunds))
            {
                resource.funds = newFunds;
            }
        }

        GUI.backgroundColor = oldColor;

        GUILayout.EndHorizontal();

        // -------- 精炼矿石 --------
        GUILayout.BeginHorizontal();

        GUILayout.Label($"精炼矿石：{resource.refinedOre}");

        oreInput = GUILayout.TextField(oreInput, GUILayout.Width(100));

        bool oreChanged = oreInput != resource.refinedOre.ToString();

        oldColor = GUI.backgroundColor;
        if (oreChanged)
            GUI.backgroundColor = Color.green;

        if (GUILayout.Button("设置", GUILayout.Width(60)))
        {
            if (float.TryParse(oreInput, out float newOre))
            {
                resource.refinedOre = newOre;
            }
        }

        GUI.backgroundColor = oldColor;

        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label($"电力负载: {resource.powerload}");
    }
}