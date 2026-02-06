
using System.Collections;
using UnityEngine;

public class HealthModule : MonoBehaviour, IModule
{
    public float currentHealth;
    public float maxHealth;
    private RTSUnit owner;
    public bool isAlive;
    [SerializeField]
    private bool isEnable = true;

    private Renderer unitRenderer;
    private Color originalColor;


    private IEnumerator HitEffect(float duration = 0.2f)
    {
        if (unitRenderer != null)
        {
            unitRenderer.material.color = Color.red; // 变红
            yield return new WaitForSeconds(duration);  // 持续0.2秒
            unitRenderer.material.color = originalColor; // 恢复原色
        }
    }


    public void Init(RTSUnit owner)
    {
        isEnable = true;
        currentHealth = maxHealth;
        this.owner = owner;
        isAlive = true;
        unitRenderer = owner.GetComponentInChildren<Renderer>();
        if (unitRenderer != null) originalColor = unitRenderer.material.color;    
    }

    public void OnTakeDamage(float amount)
    {
        // 当amount为负数时，表示治疗
        currentHealth = Mathf.Min(currentHealth - amount, maxHealth);
        if (currentHealth <= 0)
        {
            OnDead();
        }
        else owner.StartCoroutine(HitEffect());    // 临时受击特效
    }

    /// <summary>
    /// 死亡时触发
    /// </summary>
    public void OnDead()
    {
        isAlive = false;
        owner.gameObject.layer = LayerMask.NameToLayer("DeadLayer");
        //TODO: 死亡效果
        Debug.Log($"{owner.unitName} HP耗尽, 死亡了!");
        owner.StartCoroutine(HitEffect(2f));    // 临时受击特效
        Object.Destroy(owner.gameObject, 2f);
    }
    
    public string GetName()
    {
        return "HealthModule";
    }
    public void Disable()
    {
        isEnable = false;
    }
    public void Enable()
    {
        isEnable = true;
    }
    public bool IsEnable(){return isEnable;}
}