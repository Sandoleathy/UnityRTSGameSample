using System.Collections.Generic;
using UnityEngine;

public class RTSCommandController : MonoBehaviour
{
    public static RTSCommandController Instance;
    public LayerMask groundLayerMask; // 地面图层
    private SelectionManager selector;
    public LayerMask selectorLayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        selector = SelectionManager.Instance;
    }

    private void Update()
    {
        // 左键下达移动命令
        if (Input.GetMouseButtonDown(0))
        {
            Vector3? targetPos = GetGroundPosition();
            // 如果点击的位置有单位，那么选中单位，不进行移动
            if (targetPos.HasValue && !IsClickingOnUnit(Input.mousePosition))
            {
                IssueMoveCommand(targetPos.Value);
            }
        }
        // S键停止开火，G键开始开火
        if(Input.GetKeyDown(KeyCode.S)){
            List<RTSUnit> selectedUnits = selector.GetSelectedUnits();
            foreach(RTSUnit unit in selectedUnits){
                ChangeFireStateCommand cmd = new(true);
                unit.EnqueueCommand(cmd);
                // if(unit.militaryModule != null)unit.militaryModule.isCeaseFire = true;
                Debug.Log($"{unit.name} 停止开火");
            }
        }else if(Input.GetKeyDown(KeyCode.G)){
            List<RTSUnit> selectedUnits = selector.GetSelectedUnits();
            foreach(RTSUnit unit in selectedUnits){
                ChangeFireStateCommand cmd = new(false);
                unit.EnqueueCommand(cmd);
                // if(unit.militaryModule != null)unit.militaryModule.isCeaseFire = false;
                Debug.Log($"{unit.name} 开始开火");
            }
        }
    }

    // 获取鼠标点击的地面位置
    private Vector3? GetGroundPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            return hit.point; // 命中地面
        }
        return null;
    }

    // 给所有选中单位下达移动命令
    private void IssueMoveCommand(Vector3 target)
    {
        List<RTSUnit> selectedUnits = selector.GetSelectedUnits();
        MoveCommand moveCommand = new MoveCommand(target);

        foreach (RTSUnit unit in selectedUnits)
        {
            unit.EnqueueCommand(moveCommand);
            // if(unit.navigationModule != null) unit.navigationModule.MoveTo(target);
        }
    }

    public bool IsClickingOnUnit(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectorLayer))
        {
            RTSUnit unit = hit.collider.GetComponent<RTSUnit>();
            if (unit != null)
            {
                // 选中单位
                return true;   
            }
        }
        return false;
    }
}
