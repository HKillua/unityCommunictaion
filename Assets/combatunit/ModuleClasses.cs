using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleClasses : MonoBehaviour
{



    void Start()
    {

    }

    void Update()
    {

    }

}


[System.Serializable]
public class Transceiver
{
    public TransceiverType type;
    public int frequency;
    public float power;
    public float sensitivity;
    public string modulation;
    public float bandwidth; // ������ͨ�Ŵ�������
}

[System.Serializable]
public class Antenna
{
    public string type;
    public float gain;
    public Vector3 direction;
    public float beamWidth;
    public string polarization; // �����ļ�����ʽ����
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