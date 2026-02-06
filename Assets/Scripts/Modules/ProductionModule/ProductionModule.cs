using System.Collections.Generic;
using UnityEngine;

public class ProductionModule : MonoBehaviour, IModule, IUpdatableModule
{
    private RTSUnit owner;
    [SerializeField]
    private bool isEnable = true;
    private Queue<RTSUnitConfig> productQueue;

    private float tickCounter = 0f;
    private float currentUnitProductTime;
    public void Init(RTSUnit owner)
    {
        this.owner = owner;
        productQueue = new Queue<RTSUnitConfig>();
    }

    public void ProductEnqueue(RTSUnitConfig unit)
    {
        productQueue.Enqueue(unit);
        //第一个单位入队时设置生产时间
        if(productQueue.Count == 1)
        {
            currentUnitProductTime = unit.productTime;
        }
    }    
    // 队头的单位生产出来
    private void ProduceUnit()
    {
        RTSUnitConfig unit = productQueue.Dequeue();

        Debug.Log($"[Production] 生产 {unit.unitName}");

        Instantiate(unit.prefab, owner.transform.position + owner.transform.forward * 2f, Quaternion.identity);
        // 计时器归零
        tickCounter = 0;
        // 当队列不为空时，取下一个队头元素的生产时间，并计算实际的生产时间
        if(productQueue.Count > 0)
        {
            currentUnitProductTime = productQueue.Peek().productTime;
        }
    }

    public void Tick(float dt)
    {
        // 队列为空时不执行任何操作
        if(productQueue.Count == 0) return;

        // 生产队列不为空，计时器累计
        tickCounter += dt;
        // 计时器大于生产时间，单位生产
        if(tickCounter >= currentUnitProductTime)
        {
            ProduceUnit();
        }
    }    

    public void Disable()
    {
        this.isEnable = false;
    }

    public void Enable()
    {
        this.isEnable = true;
    }

    public string GetName()
    {
        return "ProductionModule";
    }

    public bool IsEnable()
    {
        return isEnable;
    }
}