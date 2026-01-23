
using System;
using System.Collections.Generic;

public class RTSUnitModuleContainer
{
    private Dictionary<Type, IModule> modules;
    private List<IUpdatableModule> updatableModules;
    public RTSUnitModuleContainer()
    {
        modules = new Dictionary<Type, IModule>();
        updatableModules = new List<IUpdatableModule>();
    }


    public void Add(IModule module)
    {
        modules[module.GetType()] = module;   
        if (module is IUpdatableModule updatableModule)
        {
            updatableModules.Add(updatableModule);
        }
    }

    /// <summary>
    /// 获取指定类型的模块
    public T Get<T>() where T : class, IModule
    {
        modules.TryGetValue(typeof(T), out var module);
        return module as T;
    }

    public List<string> GetModuleNames()
    {
        List<string> names = new List<string>();
        foreach (var module in modules.Values)
        {
            names.Add(module.GetName());
        }
        return names;
    }

    public void Tick(float dt)
    {
        foreach (var module in updatableModules)
        {
            module.Tick(dt);
        }
    }
}