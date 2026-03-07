using UnityEngine;

public class ResourceProductionModule : MonoBehaviour, IModule, IUpdatableModule
{
    public RTSUnit owner;
    public void Disable()
    {
        throw new System.NotImplementedException();
    }

    public void Enable()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        return "资源生产模块";
    }

    public void Init(RTSUnit owner)
    {
        this.owner = owner;
    }

    public bool IsEnable()
    {
        throw new System.NotImplementedException();
    }
    public void Tick(float dt)
    {
        throw new System.NotImplementedException();
    }
    public void OnSelect()
    {
        
    }
}