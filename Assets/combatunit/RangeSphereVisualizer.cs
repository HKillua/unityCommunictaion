using UnityEngine;


public class RangeSphereVisualizer : MonoBehaviour
{
    public GameObject rangeSpherePrefab; // ���� RangeSpherePrefab Ԥ�Ƽ�
    private GameObject rangeSphereInstance; // ʵ�������������

    public float interferenceLevel = 0; // ����Inspector�����õĸ�������ֵ

    public float d = 0  ; 
    private CombatUnit combatUnit;

    void Start()
    {
        // ��ʼ����ʵ��������
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
        // ȷ������λ�ø�����ս��Ԫ
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
            float maxL = float.NegativeInfinity; // ��ʼֵ��Ϊ�������
            float selectedFrequency = 1; // Ĭ��ֵ����ֹ�������
            float maxGain = float.NegativeInfinity; // ��ʼֵ��Ϊ�������

            // �ҵ� (���书�� - ����������) �����շ����Ͷ�Ӧ��Ƶ��
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

            // �ҵ�������������
            foreach (Antenna antenna in combatUnit.antennas)
            {
                if (antenna.gain > maxGain)
                {
                    maxGain = antenna.gain;
                }
            }

            // ����ͨ�Ÿ��Ƿ�Χ
            float L_max = maxL + 2 * maxGain - interferenceLevel; // ������������ͽ�������������ͬ
            //float L_max = 60f;
            float c = 3e8f; // ����
            d = (c / (4 * Mathf.PI * selectedFrequency)) * Mathf.Pow(10, L_max / 20);

            d = Mathf.Clamp(d, 1, 20000);   // ����d��ȡֵ��1-20000 ���������Ƶ�����d�Ĵ�С

            // ��������Ĵ�С
            rangeSphereInstance.transform.localScale = new Vector3(d, d, d);
        }
    }
}

