using UnityEngine;
using System.Collections.Generic;

    [System.Serializable]
    public class InterferenceUnit : MonoBehaviour
    {
        [SerializeField]
        public float interferenceFrequency; // ����Ƶ��
        [SerializeField]
        public float interferencePower; // ���Ź���
        [SerializeField]
        public float interferenceBandwidth; // ���Ŵ���
        [SerializeField]
        public string interferenceMode; // ����ģʽ

        private List<CommChannelController> commChannels;

        void Start()
        {
            commChannels = new List<CommChannelController>(FindObjectsOfType<CommChannelController>());
        }

        void Update()
        {
            foreach (var channel in commChannels)
            {
                ApplyInterference(channel);
            }
        }

        public void ApplyInterference(CommChannelController channel)
        {
            // ��������źŶ�Ŀ���źŵ�Ӱ��
            float interferenceImpact = CalculateInterferenceImpact(channel);

            // ���ݸ���ģʽִ���ض������߼�
            switch (interferenceMode)
            {
                default:
                    channel.AddInterferenceImpact(interferenceImpact);
                    break;
            }
        }

        public float CalculateInterferenceImpact(CommChannelController channel)
        {
            // ������ŵ�Ԫ��ͨ���ŵ��ľ���
            float distance = Vector3.Distance(transform.position, channel.transform.position);

            // ���ݸ���Ƶ�ʡ����ʡ�����;�����������ȵ�Ӱ��
            float impact = interferencePower / (channel.noisePowerDensity * interferenceBandwidth * Mathf.Pow(distance, 2));
            return impact;
        }
    }
