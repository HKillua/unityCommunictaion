using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CommChannelProperties))]
public class CommChannelPropertiesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CommChannelProperties properties = (CommChannelProperties)target;

        DrawDefaultInspector();

        if (properties.startUnit != null)
        {
            string[] transceiverOptions = new string[properties.startUnit.transceivers.Count];
            for (int i = 0; i < properties.startUnit.transceivers.Count; i++)
            {
                transceiverOptions[i] = $"Transceiver {i + 1}";
            }

            properties.startTransceiverIndex = EditorGUILayout.Popup("Start Transceiver", properties.startTransceiverIndex, transceiverOptions);

            string[] antennaOptions = new string[properties.startUnit.antennas.Count];
            for (int i = 0; i < properties.startUnit.antennas.Count; i++)
            {
                antennaOptions[i] = $"Antenna {i + 1}";
            }

            properties.startAntennaIndex = EditorGUILayout.Popup("Start Antenna", properties.startAntennaIndex, antennaOptions);
        }

        if (properties.endUnit != null)
        {
            string[] transceiverOptions = new string[properties.endUnit.transceivers.Count];
            for (int i = 0; i < properties.endUnit.transceivers.Count; i++)
            {
                transceiverOptions[i] = $"Transceiver {i + 1}";
            }

            properties.endTransceiverIndex = EditorGUILayout.Popup("End Transceiver", properties.endTransceiverIndex, transceiverOptions);

            string[] antennaOptions = new string[properties.endUnit.antennas.Count];
            for (int i = 0; i < properties.endUnit.antennas.Count; i++)
            {
                antennaOptions[i] = $"Antenna {i + 1}";
            }

            properties.endAntennaIndex = EditorGUILayout.Popup("End Antenna", properties.endAntennaIndex, antennaOptions);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(properties);
        }
    }
}
