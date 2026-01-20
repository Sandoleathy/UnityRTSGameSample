
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    private RTSUnit owner;
    public bool isAlive;
    public void Init(RTSUnit owner)
    {
        currentHealth = maxHealth;
        this.owner = owner;
        isAlive = true;
    }

    public void OnDestroy()
    {
        isAlive = false;
        return;
    }

    public void OnTakeDamage(float amount)
    {
        // 当amount为负数时，表示治疗
        currentHealth = Mathf.Min(currentHealth - amount, maxHealth);
        if (currentHealth <= 0)
        {
            OnDead();
        }
        // 播放临时受伤效果
        owner.TakeDamage();
    }

    /// <summary>
    /// 死亡时触发
    /// </summary>
    public void OnDead()
    {
        isAlive = false;
        gameObject.layer = LayerMask.NameToLayer("DeadLayer");
        //TODO: 死亡效果
        Debug.Log($"{name} HP耗尽, 死亡了!");
        Destroy(gameObject, 2f);
        //临时留下调用死亡特效
        owner.OnDead();
    }

    void Start()
    {
        
    }

    void Awake()
    {
        
    }

    void Update()
    {
        
    }
}