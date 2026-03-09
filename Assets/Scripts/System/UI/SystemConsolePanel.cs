using System.Collections.Generic;
using UnityEngine;

public class SystemConsolePanel : MonoBehaviour
{
    private bool isVisible = false;

    private GameManager gameManager;
    private Player selectedPlayer;
    private Dictionary<BattleResourceTypes, string> resourceInputMap = new Dictionary<BattleResourceTypes, string>();

    private Rect windowRect = new Rect(100, 100, 420, 450);

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        foreach (BattleResourceTypes type in System.Enum.GetValues(typeof(BattleResourceTypes)))
        {
            resourceInputMap[type] = "";
        }
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
                    GetPlayerResourceInfo();
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
        var resourceSys = player.battleResourceSystem;

        foreach (var r in resourceSys.GetBattleResources())
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{r.Key}");
            resourceInputMap[r.Key] = GUILayout.TextField(resourceInputMap[r.Key], GUILayout.Width(100));
            bool changed = resourceInputMap[r.Key] != r.Value.amount.ToString();
            Color oldColor = GUI.backgroundColor;
            if (changed) GUI.backgroundColor = Color.green;
            if (GUILayout.Button("设置", GUILayout.Width(60)))
            {
                if (float.TryParse(resourceInputMap[r.Key], out float newAmount))
                {
                    resourceSys.SetBattleResource(r.Key, newAmount);
                }
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
    }
    private void GetPlayerResourceInfo()
    {
        if(selectedPlayer == null) return;
        BattleResourceSystem resourceSys = selectedPlayer.battleResourceSystem;
        foreach(var r in resourceSys.GetBattleResources())
        {
            resourceInputMap[r.Key] = r.Value.amount.ToString();
        }
    }
}