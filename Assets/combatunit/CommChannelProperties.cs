using UnityEngine;
using System.Collections;


public class CommChannelProperties : MonoBehaviour
{
    public CombatUnit startUnit; // 起始作战单元
    public CombatUnit endUnit; // 末端作战单元

    [SerializeField]
    public int startTransceiverIndex = 0; // 起始作战单元的收发器索引
    [SerializeField]
    public int startAntennaIndex = 0; // 起始作战单元的天线索引
    [SerializeField]
    public int endAntennaIndex = 0; // 末端作战单元的天线索引
    [SerializeField]
    public int endTransceiverIndex = 0; // 末端作战单元的收发器索引

    [SerializeField]
    private float transmitPower; // 发射功率 P_t
    [SerializeField]
    private float transmitAntennaGain; // 发射天线增益 G_t
    [SerializeField]
    private float receiveAntennaGain; // 接收天线增益 G_r

    public float pathLoss; // 自由空间路径损耗 L_f
    public float shadowingEffect; // 阴影效应 L_s
    public float multipathEffect; // 多径效应 L_m
    public float noisePowerDensity; // 噪声功率密度 N_0
    public CommChannelController commChannelController; // 引用 CommChannelController 脚本

    [SerializeField]
    private float bandwidth; // 带宽 B
    [SerializeField]
    private double snr; // 信噪比
    [SerializeField]
    private double capacity; // 信道容量



    private MaterialSelector materialSelector;

    void Start()
    {
        materialSelector = GetComponent<MaterialSelector>();
        // 初始化参数
        SelectTransceiverAndAntenna();
        CalculateSNR();
        CalculateCapacity();
        UpdateChannelVisuals();
        UpdateMaterial();
    }

    void Update()
    {
        // 动态更新信道属性
        SelectTransceiverAndAntenna();
        CalculateSNR();
        CalculateCapacity();
        UpdateChannelVisuals();
        UpdateMaterial();
    }

    void OnValidate()
    {
        // 更新参数以便在 Inspector 中查看
        SelectTransceiverAndAntenna();
        CalculateSNR();
        CalculateCapacity();
        UpdateMaterial();
    }

    void SelectTransceiverAndAntenna()
    {
        if (startUnit != null && endUnit != null)
        {
            if (startUnit.transceivers.Count > startTransceiverIndex && endUnit.transceivers.Count > endTransceiverIndex)
            {
                Transceiver startTransceiver = startUnit.transceivers[startTransceiverIndex];
                Transceiver endTransceiver = endUnit.transceivers[endTransceiverIndex];

                // 比较收发器类型
                if (startTransceiver.type != endTransceiver.type)
                {
                    Debug.LogError("Transceiver types do not match! Channel cannot be constructed.");
                    return; // 终止信道构建
                }

                transmitPower = startTransceiver.power;
                bandwidth = startTransceiver.bandwidth;
            }
            else
            {
                //Debug.LogError("Transceiver index out of range!");
                //return; // 终止信道构建
            }

            if (startUnit.antennas.Count > startAntennaIndex)
            {
                Antenna antenna = startUnit.antennas[startAntennaIndex];
                transmitAntennaGain = antenna.gain;
            }

            if (endUnit.antennas.Count > endAntennaIndex)
            {
                Antenna antenna = endUnit.antennas[endAntennaIndex];
                receiveAntennaGain = antenna.gain;
            }
        }
    }

    void CalculateSNR()
    {
        // 计算信噪比 SNR
        snr = transmitPower + transmitAntennaGain + receiveAntennaGain + pathLoss + shadowingEffect + multipathEffect - noisePowerDensity;
    }

    void CalculateCapacity()
    {
        // 计算信道容量 C
        // 使用 double 类型的 Math.Log 函数
        capacity = bandwidth * System.Math.Log(1 + System.Math.Pow(10, snr / 10), 2);

        // 防止容量为无穷大
        if (double.IsInfinity(capacity))
        {
            capacity = double.MaxValue; // 设置为 double 类型的最大值
        }
    }

    public void UpdateChannelVisuals()
    {
        commChannelController.UpdatchateVisuals(capacity, snr);
    }

    void UpdateMaterial()
    {
        if (materialSelector != null)
        {
            materialSelector.SetMaterial((int)startUnit.transceivers[startTransceiverIndex].type);
        }
    }
}

