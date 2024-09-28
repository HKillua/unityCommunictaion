using UnityEngine;


public class RangeSphereVisualizer : MonoBehaviour
{
    public GameObject rangeSpherePrefab; // 引用 RangeSpherePrefab 预制件
    private GameObject rangeSphereInstance; // 实例化的球体对象

    public float interferenceLevel = 0; // 可在Inspector中设置的干扰因素值

    public float d = 0  ; 
    private CombatUnit combatUnit;

    void Start()
    {
        // 初始化并实例化球体
        if (rangeSpherePrefab != null)
        {
            rangeSphereInstance = Instantiate(rangeSpherePrefab, transform.position, Quaternion.identity);
            rangeSphereInstance.transform.parent = null;
        }

        combatUnit = GetComponent<CombatUnit>();

        UpdateRangeSpheres();
    }

    void Update()
    {
        // 确保球体位置跟随作战单元
        if (rangeSphereInstance != null)
        {
            rangeSphereInstance.transform.position = transform.position;
        }

        UpdateRangeSpheres();
    }

    void UpdateRangeSpheres()
    {
        if (combatUnit != null && rangeSphereInstance != null)
        {
            float maxL = float.NegativeInfinity; // 初始值设为负无穷大
            float selectedFrequency = 1; // 默认值，防止除零错误
            float maxGain = float.NegativeInfinity; // 初始值设为负无穷大

            // 找到 (发射功率 - 接收灵敏度) 最大的收发机和对应的频率
            foreach (Transceiver transceiver in combatUnit.transceivers)
            {
                float L = transceiver.power - transceiver.sensitivity;
                if (L > maxL)
                {
                    maxL = L;
                    selectedFrequency = transceiver.frequency;
                    //selectedFrequency = 2400000000;
                }
            }

            // 找到增益最大的天线
            foreach (Antenna antenna in combatUnit.antennas)
            {
                if (antenna.gain > maxGain)
                {
                    maxGain = antenna.gain;
                }
            }

            // 计算通信覆盖范围
            float L_max = maxL + 2 * maxGain - interferenceLevel; // 发射天线增益和接收天线增益相同
            //float L_max = 60f;
            float c = 3e8f; // 光速
            d = (c / (4 * Mathf.PI * selectedFrequency)) * Mathf.Pow(10, L_max / 20);

            d = Mathf.Clamp(d, 1, 20000);   // 限制d的取值在1-20000 不能无限制的扩张d的大小

            // 调整球体的大小
            rangeSphereInstance.transform.localScale = new Vector3(d, d, d);
        }
    }
}

