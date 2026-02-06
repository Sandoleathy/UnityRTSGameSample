
public interface IModule
{
    void Init(RTSUnit owner);
    string GetName();
    void Disable();
    void Enable();
    bool IsEnable();
}

public interface IUpdatableModule
{
    void Tick(float dt);
}

public enum ModuleTypes
{
    HealthModule,
    MilitaryModule,
    NavigationModule,
    ProductionModule

}