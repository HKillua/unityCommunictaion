using UnityEngine;
using System.Collections.Generic;

    [System.Serializable]
    public class InterferenceUnit : MonoBehaviour
    {
        [SerializeField]
        public float interferenceFrequency; // 干扰频率
        [SerializeField]
        public float interferencePower; // 干扰功率
        [SerializeField]
        public float interferenceBandwidth; // 干扰带宽
        [SerializeField]
        public string interferenceMode; // 干扰模式

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
            // 计算干扰信号对目标信号的影响
            float interferenceImpact = CalculateInterferenceImpact(channel);

            // 根据干扰模式执行特定干扰逻辑
            switch (interferenceMode)
            {
                default:
                    channel.AddInterferenceImpact(interferenceImpact);
                    break;
            }
        }

        public float CalculateInterferenceImpact(CommChannelController channel)
        {
            // 计算干扰单元与通信信道的距离
            float distance = Vector3.Distance(transform.position, channel.transform.position);

            // 根据干扰频率、功率、带宽和距离计算对信噪比的影响
            float impact = interferencePower / (channel.noisePowerDensity * interferenceBandwidth * Mathf.Pow(distance, 2));
            return impact;
        }
    }
