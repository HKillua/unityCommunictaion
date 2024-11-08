using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleClasses : MonoBehaviour
{
    
}


[System.Serializable]
public class Transceiver
{
    public TransceiverType type;
    public int frequency;
    public float power;
    public float sensitivity;
    public string modulation = "null";
    public float bandwidth; // 新增的通信带宽属性
}

[System.Serializable]
public class Antenna
{
    public string type = "null";
    public float gain;
    public Vector3 direction;
    public float beamWidth;
    public string polarization = "null"; // 新增的极化方式属性
}

// 方便表示CombatUnit单元上面的模型类型
public enum CombatUnitType
{
    car, 
    airplane, 
}
public enum TransceiverType
{
    Type0,
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}