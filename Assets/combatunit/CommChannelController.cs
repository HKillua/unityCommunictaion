using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommChannelController : MonoBehaviour
{
    public ParticleSystem particleSystemInstance;
    public ParticleSystem staticParticleSystemInstance; // 新增的静态粒子系统
    public Transform startTransform;
    public Transform endTransform;
    public CommChannelProperties channelProperties; // 引用 CommChannelProperties 脚本

    public double dataVolume; // 传输数据量
    public int dataPacketSize; // 数据包容量，单位：字节

    private float cylinderRadius = 0.4f; // 默认圆柱体半径
    private float particleSize = 0.3f; // 粒子的大小
    private float totalInterferenceImpact; // 总干扰影响

    public float noisePowerDensity; // 噪声功率密度 N_0, 继承自 CommChannelProperties
    public float distance; 

    private List<InterferenceUnit> interferenceUnits;

    void Start()
    {
        if (particleSystemInstance == null)
        {
            particleSystemInstance = GetComponentInChildren<ParticleSystem>();
        }

        if (staticParticleSystemInstance == null)
        {
            // 假设在层级视图中已经添加了静态粒子系统
            staticParticleSystemInstance = transform.Find("StaticParticleSystem").GetComponent<ParticleSystem>();
            ConfigureStaticParticleSystem();
        }

        // 获取场景中所有的干扰单元
        interferenceUnits = new List<InterferenceUnit>(FindObjectsOfType<InterferenceUnit>());

        // 初始化 channelProperties 引用
        channelProperties = GetComponent<CommChannelProperties>();
    }

    void Update()
    {
        if (startTransform != null && endTransform != null)
        {
            UpdateChannel();
            UpdateStaticParticles();
            UpdateInterferenceImpact();
            UpdateNoisePowerDensity(); // 实时更新 noisePowerDensity
        }
    }

    void UpdateChannel()
    {
        // 更新圆柱体的位置和长度
        Vector3 direction = endTransform.position - startTransform.position;
        distance = direction.magnitude;

        transform.position = (startTransform.position + endTransform.position) / 2;
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
        transform.localScale = new Vector3(cylinderRadius, distance / 2, cylinderRadius);

        // 更新粒子系统的位置和方向
        particleSystemInstance.transform.position = startTransform.position;

        // 添加额外的旋转调整
        particleSystemInstance.transform.rotation = Quaternion.LookRotation(direction);

        // 更新粒子系统的形状和大小
        ParticleSystem.ShapeModule shape = particleSystemInstance.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 0;
        shape.radius = 0; // 使用圆柱体的半径
        shape.length = distance; // 将粒子系统的长度设置为圆柱体的长度
        shape.position = new Vector3(0, 0, distance / 2);

        // 更新粒子的大小
        ParticleSystem.MainModule main = particleSystemInstance.main;
        main.startSize = particleSize; // 设置粒子大小
        main.startSpeed = distance / main.startLifetime.constant; // 更新粒子速度
        main.startColor = Color.red;
    }

    void ConfigureStaticParticleSystem()
    {
        // 配置静态粒子系统
        ParticleSystem.MainModule main = staticParticleSystemInstance.main;
        main.startSpeed = 0f; // 静止粒子
        main.startSize = cylinderRadius; // 设置粒子大小
        main.startLifetime = Mathf.Infinity; // 粒子永久存在
        main.startColor = Color.red;

        ParticleSystem.EmissionModule emission = staticParticleSystemInstance.emission;
        emission.rateOverTime = 0f; // 禁用自动发射

        ParticleSystem.ShapeModule shape = staticParticleSystemInstance.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 0;
        shape.radius = 0; // 使用圆柱体的半径

        // 禁用移动和碰撞等模块
        ParticleSystem.VelocityOverLifetimeModule velocity = staticParticleSystemInstance.velocityOverLifetime;
        velocity.enabled = false;

        ParticleSystem.CollisionModule collision = staticParticleSystemInstance.collision;
        collision.enabled = false;

        // 设置渲染模式为 Billboard
        ParticleSystemRenderer renderer = staticParticleSystemInstance.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        // 添加噪声模块以实现扰动
        ParticleSystem.NoiseModule noise = staticParticleSystemInstance.noise;
        noise.enabled = true;
        noise.strengthX = 6.0f; // X方向扰动强度
        noise.strengthY = 6.0f; // Y方向扰动强度
        noise.frequency = 3.0f; // 噪声频率
        noise.scrollSpeed = 15.0f; // 控制动画速度
    }

    void UpdateStaticParticles()
    {
        Vector3 direction = endTransform.position - startTransform.position;
        float distance = direction.magnitude;

        // 调整静态粒子系统的位置
        staticParticleSystemInstance.transform.position = startTransform.position;
        staticParticleSystemInstance.transform.rotation = Quaternion.LookRotation(direction);

        ParticleSystem.ShapeModule shape = staticParticleSystemInstance.shape;
        shape.length = distance; // 将粒子系统的长度设置为圆柱体的长度
        shape.position = new Vector3(0, 0, distance / 2);

        ParticleSystem.MainModule main = staticParticleSystemInstance.main;
        main.startSize = cylinderRadius; // 同步更新粒子大小

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.applyShapeToPosition = true;

        staticParticleSystemInstance.Clear();
        int particleCount = Mathf.CeilToInt(distance / 0.2f); // 计算粒子数量，间隔为0.2单位
        for (int i = 0; i < particleCount; i++)
        {
            float positionAlongPipe = i * (distance / (particleCount - 1)); // 计算粒子在管道上的位置
            emitParams.position = new Vector3(0, 0, positionAlongPipe - (distance / 2));
            staticParticleSystemInstance.Emit(emitParams, 1); // 发射静态粒子
        }
    }

    void UpdateInterferenceImpact()
    {
        totalInterferenceImpact = 0f;
        foreach (var unit in interferenceUnits)
        {
            float impact = unit.CalculateInterferenceImpact(this);
            totalInterferenceImpact += impact;
        }

        // 更新噪声强度或其他逻辑
        ParticleSystem.NoiseModule noise = staticParticleSystemInstance.noise;
        noise.strengthX = totalInterferenceImpact * 6.0f; // 根据总干扰影响调整 X方向扰动强度
        noise.strengthY = totalInterferenceImpact * 6.0f; // 根据总干扰影响调整 Y方向扰动强度
    }

    public void AddInterferenceImpact(float impact)
    {
        totalInterferenceImpact += impact;
    }

    void UpdateNoisePowerDensity()
    {
        if (channelProperties != null)
        {
            noisePowerDensity = channelProperties.noisePowerDensity; // 从 CommChannelProperties 实时获取噪声功率密度
        }
    }


    // 可视化部分的渲染代码
    public void UpdatchateVisuals(double capacity, double snr)
    {
        // 将容量从 bps 转换为 Mbps
        float capacityMbps = (float)(capacity / 1_000_000.0);

        // 更新粒子系统的发射速率
        var emission = particleSystemInstance.emission;
       
        if (capacityMbps < 20)
        {
            cylinderRadius = Mathf.Lerp(0.1f, 0.2f, Mathf.InverseLerp(0, 20, capacityMbps));
        }
        else if (capacityMbps >= 20 && capacityMbps <= 200)
        {
            cylinderRadius = Mathf.Lerp(0.2f, 0.45f, Mathf.InverseLerp(20, 200, capacityMbps));
        }
        else
        {
            cylinderRadius = Mathf.Lerp(0.45f, 0.55f, Mathf.InverseLerp(200, 1000, capacityMbps));
        }
        particleSize = 0.15f + cylinderRadius;
        emission.rateOverTime = 1; // 

        // 更新圆柱体的形状
        UpdateChannel();
    }
}











