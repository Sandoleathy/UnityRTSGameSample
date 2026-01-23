using System.Collections.Generic;
using UnityEngine;

public class RTSCommandController : MonoBehaviour
{
    public LayerMask groundLayerMask; // 地面图层
    private RTSUnitSelector selector;

    private void Start()
    {
        selector = GetComponent<RTSUnitSelector>();
    }

    private void Update()
    {
        // 左键下达移动命令
        if (Input.GetMouseButtonDown(0))
        {
            Vector3? targetPos = GetGroundPosition();
            if (targetPos.HasValue)
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
}
