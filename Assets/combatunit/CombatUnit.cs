using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public delegate void DataChanged();
    public event DataChanged OnDataChanged;


    public CombatUnitType UnitType;
    public Sprite CombatImage;
    public List<Transceiver> transceivers = new List<Transceiver>();
    public List<Antenna> antennas = new List<Antenna>();

    
    public void AddTransceiver(Transceiver newTransceiver)
    {
        transceivers.Add(newTransceiver);
        OnDataChanged?.Invoke();
    }


    // 添加一个新的 Antenna
    public void AddAntenna(Antenna newAntenna)
    {
        antennas.Add(newAntenna);
        OnDataChanged?.Invoke();
    }

    public void RemoveTransceiver(int index)
    {
        if (index >= 0 && index < transceivers.Count)
        {
            transceivers.RemoveAt(index);
            OnDataChanged?.Invoke();
        }
    }

    public void RemoveAntenna(int index)
    {
        if (index >= 0 && index < antennas.Count)
        {
            antennas.RemoveAt(index);
            OnDataChanged?.Invoke();
        }
    }
}





