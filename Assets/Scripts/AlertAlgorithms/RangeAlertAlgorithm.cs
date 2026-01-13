using UnityEngine;

public class RangeAlertAlgorithm : IAlertAlgorithm
{
    public RTSUnit DetectEnemy(RTSUnit self)
    {
        Collider[] hits = Physics.OverlapSphere(self.transform.position, self.config.alertRange);
        foreach (var hit in hits)
        {
            RTSUnit other = hit.GetComponent<RTSUnit>();
            if (other != null && other.camp != self.camp)
            {
                return other; // 找到第一个敌人就返回
            }
        }
        return null;
    }
}
