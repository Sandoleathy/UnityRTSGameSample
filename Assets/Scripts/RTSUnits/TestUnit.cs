// using UnityEngine;
// using UnityEngine.AI;

// [RequireComponent(typeof(OutlineObject))]
// public class TestUnit : RTSUnit
// {
//     private OutlineObject outline;
//     void Start()
//     {
//         SetMoveAlgorithm(new NavMeshMoveAlgorithm(GetComponent<NavMeshAgent>()));
//         SetAlertAlgorithm(new RangeAlertAlgorithm());
//     }

//     public override void OnSelected()
//     {
//         base.OnSelected();

//         if (outline == null)
//         {
//             outline = gameObject.AddComponent<OutlineObject>();
//         }

//         outline.enabled = true;
//     }

//     public override void DeSelected()
//     {
//         base.DeSelected();
//         if (outline != null)
//         {
//             outline.enabled = false;
//         }
//     }
// }