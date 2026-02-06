using System.Collections.Generic;
using UnityEngine;

public class RTSUnitSelector : MonoBehaviour
{
    [Header("框选颜色")]
    public Color selectionBoxColor = new Color(0, 1, 0, 0.25f);
    public LayerMask selectorLayer;

    private Vector3 dragStartPos;
    private bool isDragging = false;

    private List<RTSUnit> selectedUnits = new List<RTSUnit>();

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        // 鼠标按下左键
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            isDragging = true;
        }

        // 鼠标释放左键
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                Vector3 dragEndPos = Input.mousePosition;
                if (Vector3.Distance(dragStartPos, dragEndPos) < 5f)
                {
                    // 点击操作
                    HandleClick(dragEndPos);
                }
                else
                {
                    // 框选操作
                    HandleDragSelection(dragStartPos, dragEndPos);
                }
            }
            isDragging = false;
        }
        // 右键取消选中
        if (Input.GetMouseButtonDown(1))
        {
            ClearSelection();
        }
    }

    private void HandleClick(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectorLayer))
        {
            RTSUnit unit = hit.collider.GetComponent<RTSUnit>();
            if (unit != null)
            {
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                {
                    // 清空之前选择
                    ClearSelection();
                }
                SelectUnit(unit);
            }
            //没有选中任何单位，此时点击左键若有已经选中的单位则执行移动操作
        }
        // else
        // {
        //     if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        //         ClearSelection();
        // }
    }

    private void HandleDragSelection(Vector3 start, Vector3 end)
    {
        // 左下和右上
        Vector3 min = Vector3.Min(start, end);
        Vector3 max = Vector3.Max(start, end);

        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            ClearSelection();

        // 检测场景中所有单位
        RTSUnit[] allUnits = GameObject.FindObjectsOfType<RTSUnit>();
        foreach (var unit in allUnits)
        {
            if (((1 << unit.gameObject.layer) & selectorLayer) == 0) continue; // 不在指定图层则跳过

            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (screenPos.z < 0) continue; // 摄像机背面
            if (screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y)
            {
                SelectUnit(unit);
            }
        }
    }

    private void SelectUnit(RTSUnit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.OnSelected();
        }
    }

    private void ClearSelection()
    {
        foreach (var unit in selectedUnits)
        {
            unit.DeSelected();
        }
        selectedUnits.Clear();
    }

    public List<RTSUnit> GetSelectedUnits()
    {
        return selectedUnits;
    }

    // 在Scene视图显示框选矩形
    private void OnGUI()
    {
        if (isDragging)
        {
            Rect rect = Utils.GetScreenRect(dragStartPos, Input.mousePosition);
            Utils.DrawScreenRect(rect, selectionBoxColor);
            Utils.DrawScreenRectBorder(rect, 2, Color.green);
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

// 辅助工具类
public static class Utils
{
    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }
}
