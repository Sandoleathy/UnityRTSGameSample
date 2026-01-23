
using System;
using System.Collections.Generic;

public class RTSUnitModuleContainer
{
    private Dictionary<Type, IModule> modules = new();

    public void Add(IModule module)
    {
        modules[module.GetType()] = module;   
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
}