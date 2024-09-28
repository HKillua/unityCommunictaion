using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CombatUnit"))
        {
            Debug.Log("Combat unit entered the boundary.");
            CombatUnit combatUnit = other.GetComponent<CombatUnit>();
            if (combatUnit != null)
            {
                combatUnit.SetOutOfBoundary(false); // 进入边界时设置标志位
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CombatUnit"))
        {
            Debug.Log("Combat unit exited the boundary.");
            CombatUnit combatUnit = other.GetComponent<CombatUnit>();
            if (combatUnit != null)
            {
                Vector3 boundaryCenter = transform.position;
                combatUnit.SetOutOfBoundary(true); // 离开边界时设置标志位
                combatUnit.ReturnToBoundary(boundaryCenter); // 立刻返回
            }
        }
    }
}



