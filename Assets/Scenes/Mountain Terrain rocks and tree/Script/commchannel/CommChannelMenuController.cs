using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


// 此脚本应该要绑定在点击按钮后出现的窗口界面上     
public class CommChannelMenuController : MonoBehaviour
{
    // 绑定ui界面上面的对应的输入框
    
    public TMP_Dropdown startTransceiverDropdown;
    public TMP_Dropdown startAntennaDropdown;
    public TMP_Dropdown endTransceiverDropdown;
    public TMP_Dropdown endAntennaDropdown; 
    
    public TMP_InputField pathLossInput;
    public TMP_InputField shadowingEffectInput;
    public TMP_InputField multipathEffectInput;
    public TMP_InputField noisePowerDensityInput;

    public Button applyButton;                 // 配置完参数后并且关闭panel的界面

    public GameObject channelPrefab;                 // 用来实例化的管道模型

    private CombatUnit combatStart;            // 用鼠标选取的两个单元 
    private CombatUnit combatEnd;
    private CommChannelProperties channelProperties;

    private bool isSelectingStart = true; 


    void Start()
    {
        applyButton.onClick.AddListener(OnApplyButtonClickd); 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 鼠标左键点击
        {
            SelectCombatUnit();
        }
    }

    void SelectCombatUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            CombatUnit selectedUnit = hit.collider.GetComponent<CombatUnit>();
            if (selectedUnit != null)
            {
                if (isSelectingStart)
                {
                    combatStart = selectedUnit;
                    isSelectingStart = false; // 下一次点击选endUnit
                    Debug.Log("Selected Start Unit: " + combatStart.name);
                    InitializeDropdown(combatStart, startTransceiverDropdown, startAntennaDropdown);
                }
                else
                {
                    combatEnd = selectedUnit;
                    Debug.Log("Selected End Unit: " + combatEnd.name);
                    isSelectingStart = true; // 再次点击时选择startUnit
                    InitializeDropdown(combatEnd, endTransceiverDropdown, endAntennaDropdown);
                }
            }
        }
    }

    void InitializeDropdown(CombatUnit unit , TMP_Dropdown transceiverDropdown , TMP_Dropdown attenaDropdown)
    {
        // 初始化下拉模块的显示
        transceiverDropdown.ClearOptions();
        List<string> transceiverOptions = new List<string>(); 
        for (int i = 0; i < unit.transceivers.Count; i++)
        {
            transceiverOptions.Add("Tranceivers" + i); 

        }
        transceiverDropdown.AddOptions(transceiverOptions);

        attenaDropdown.ClearOptions();
        List<string> antennaOptions = new List<string>();
        for (int i = 0; i < unit.antennas.Count; i++)
        {
            antennaOptions.Add("Antenna " + i);
        }
        attenaDropdown.AddOptions(antennaOptions);

    }




    void OnApplyButtonClickd()
    {
        if (combatStart !=null && combatEnd != null)
        {
            GameObject newChannel = Instantiate(channelPrefab); 

            channelProperties = newChannel.GetComponent<CommChannelProperties>();
            CommChannelController channelController = newChannel.GetComponent<CommChannelController>();

            channelProperties.startUnit = combatStart;
            channelProperties.endUnit = combatEnd;

            // 赋值给 CommChannelController 中的 startTransform 和 endTransform
            channelController.startTransform = combatStart.transform;
            channelController.endTransform = combatEnd.transform;

            // 根据场景中的Dropdown组件配置index的数值
            channelProperties.startTransceiverIndex = startTransceiverDropdown.value;
            channelProperties.startAntennaIndex = startAntennaDropdown.value;
            channelProperties.endTransceiverIndex = endTransceiverDropdown.value;
            channelProperties.endAntennaIndex = endAntennaDropdown.value;

            // 读取场景中的对应的Input组件的内容并且改变对应脚本文件里的参数
            channelProperties.pathLoss = float.Parse(pathLossInput.text);
            channelProperties.shadowingEffect = float.Parse(shadowingEffectInput.text);
            channelProperties.multipathEffect = float.Parse(multipathEffectInput.text);
            channelProperties.noisePowerDensity = float.Parse(noisePowerDensityInput.text);
            
            // 显示出场景中对应的管道模型
            channelProperties.UpdateChannelVisuals();

            // 关闭当前面板
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Start and End units must be selected."); 
        }

    }

    
}
