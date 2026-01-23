
public interface IModule
{
    void Init(RTSUnit owner);
    string GetName();
}

public interface IUpdatableModule
{
    void Tick(float dt);
}