using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(OutlineObject))]
public class RTSUnit : MonoBehaviour
{
    [Header("单位配置")]
    public RTSUnitConfig config;
    public string unitName;
    public bool canAttackWhileMove;
    [Header("模块")]
    public RTSUnitModuleContainer moduleContainer;
    public List<string> moduleNames;
    [Header("指令系统")]
    private List<ICommand> commandQueue;
    
    [Header("阵营")]
    public int camp;

    [Header("当前属性")]
    public bool isAlive = true;
    public IMoveAlgorithm moveAlgorithm;
    public IAlertAlgorithm alertAlgorithm;

    private OutlineObject outline;

//--------------------------------------

    void Start()
    {   
        // 初始化模块容器
        moduleContainer = new RTSUnitModuleContainer(this);

        // 初始化指令队列
        commandQueue = new List<ICommand>();

        // 临时固定算法
        moveAlgorithm = new NavMeshMoveAlgorithm(GetComponent<NavMeshAgent>());
        alertAlgorithm = new RangeAlertAlgorithm();


        unitName = config.unitName;
        canAttackWhileMove = config.canAttackWhileMove;

        // 临时手动增加模块，之后从配置文件中加载
        // AddModule(new HealthModule());
        // AddModule(new NavigationModule());   
        // AddModule(new MilitaryModule());

        //所有模块都继承了MonoBehavior，可以直接挂载到游戏物体上，由RTSUnit统一扫描并送入容器进行管理
        ScanModules();

        moduleNames = moduleContainer.GetModuleNames();
    }
    //扫描并将模块挂载到容器中统一管理
    private void ScanModules()
    {
        IModule[] modules = GetComponents<IModule>();
        Debug.Log(modules.Length);
        //先都进入容器，再初始化，防止出现NullReferenceException
        foreach(var module in modules)
        {
            moduleContainer.Add(module);
        }
        foreach(var module in modules)
        {
            module.Init(this);   
        }
    }
    public void EnqueueCommand(ICommand command)
    {
        commandQueue.Add(command);
    }

    private void ExecuteCommand()
    {
        if (commandQueue.Count > 0)
        {
            ICommand currentCommand = commandQueue[0];

            Debug.Log($"{unitName} 执行指令: {currentCommand.GetType().Name}");

            currentCommand.Execute(this);
            commandQueue.RemoveAt(0);
        }
    }

    void Update()
    {
        // 暂时每帧执行一个指令
        ExecuteCommand();

        moduleContainer.Tick(Time.deltaTime);
    }

    private void AddModule(IModule module)
    {
        moduleContainer.Add(module);
        module.Init(this);
    }

    /// <summary>
    /// 单位被鼠标点击选中
    /// </summary>
    public void OnSelected()
    {
        Debug.Log($"{name} 被选中！");

        if (outline == null)
        {
            outline = gameObject.AddComponent<OutlineObject>();
        }

        outline.enabled = true;
    }

    public void DeSelected()
    {
        Debug.Log($"{name} 取消选中");
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
