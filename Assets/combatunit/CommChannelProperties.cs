using UnityEngine;
using System.Collections;


public class CommChannelProperties : MonoBehaviour
{
    public CombatUnit startUnit; // ��ʼ��ս��Ԫ
    public CombatUnit endUnit; // ĩ����ս��Ԫ

    [SerializeField]
    public int startTransceiverIndex = 0; // ��ʼ��ս��Ԫ���շ�������
    [SerializeField]
    public int startAntennaIndex = 0; // ��ʼ��ս��Ԫ����������
    [SerializeField]
    public int endAntennaIndex = 0; // ĩ����ս��Ԫ����������
    [SerializeField]
    public int endTransceiverIndex = 0; // ĩ����ս��Ԫ���շ�������

    [SerializeField]
    private float transmitPower; // ���书�� P_t
    [SerializeField]
    private float transmitAntennaGain; // ������������ G_t
    [SerializeField]
    private float receiveAntennaGain; // ������������ G_r

    public float pathLoss; // ���ɿռ�·����� L_f
    public float shadowingEffect; // ��ӰЧӦ L_s
    public float multipathEffect; // �ྶЧӦ L_m
    public float noisePowerDensity; // ���������ܶ� N_0
    public CommChannelController commChannelController; // ���� CommChannelController �ű�

    [SerializeField]
    private float bandwidth; // ���� B
    [SerializeField]
    private double snr; // �����
    [SerializeField]
    private double capacity; // �ŵ�����



    private MaterialSelector materialSelector;

    void Start()
    {
        materialSelector = GetComponent<MaterialSelector>();
        // ��ʼ������
        SelectTransceiverAndAntenna();
        CalculateSNR();
        CalculateCapacity();
        UpdateChannelVisuals();
        UpdateMaterial();
    }

    void Update()
    {
        // ��̬�����ŵ�����
        SelectTransceiverAndAntenna();
        CalculateSNR();
        CalculateCapacity();
        UpdateChannelVisuals();
        UpdateMaterial();
    }

    void OnValidate()
    {
        // ���²����Ա��� Inspector �в鿴
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

                // �Ƚ��շ�������
                if (startTransceiver.type != endTransceiver.type)
                {
                    Debug.LogError("Transceiver types do not match! Channel cannot be constructed.");
                    return; // ��ֹ�ŵ�����
                }

                transmitPower = startTransceiver.power;
                bandwidth = startTransceiver.bandwidth;
            }
            else
            {
                //Debug.LogError("Transceiver index out of range!");
                //return; // ��ֹ�ŵ�����
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
        // ��������� SNR
        snr = transmitPower + transmitAntennaGain + receiveAntennaGain + pathLoss + shadowingEffect + multipathEffect - noisePowerDensity;
    }

    void CalculateCapacity()
    {
        // �����ŵ����� C
        // ʹ�� double ���͵� Math.Log ����
        capacity = bandwidth * System.Math.Log(1 + System.Math.Pow(10, snr / 10), 2);

        // ��ֹ����Ϊ�����
        if (double.IsInfinity(capacity))
        {
            capacity = double.MaxValue; // ����Ϊ double ���͵����ֵ
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

