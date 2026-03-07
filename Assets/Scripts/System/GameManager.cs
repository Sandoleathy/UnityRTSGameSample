using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerManager playerManager;
    public UIManager uiManager;

    private void Awake()
    {
        // 确保GameManager是一个单例
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}