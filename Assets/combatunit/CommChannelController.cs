using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommChannelController : MonoBehaviour
{
    public ParticleSystem particleSystemInstance;
    public ParticleSystem staticParticleSystemInstance; // �����ľ�̬����ϵͳ
    public Transform startTransform;
    public Transform endTransform;
    public CommChannelProperties channelProperties; // ���� CommChannelProperties �ű�

    public double dataVolume; // ����������
    public int dataPacketSize; // ���ݰ���������λ���ֽ�

    private float cylinderRadius = 0.4f; // Ĭ��Բ����뾶
    private float particleSize = 0.3f; // ���ӵĴ�С
    private float totalInterferenceImpact; // �ܸ���Ӱ��

    public float noisePowerDensity; // ���������ܶ� N_0, �̳��� CommChannelProperties
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
            // �����ڲ㼶��ͼ���Ѿ�����˾�̬����ϵͳ
            staticParticleSystemInstance = transform.Find("StaticParticleSystem").GetComponent<ParticleSystem>();
            ConfigureStaticParticleSystem();
        }

        // ��ȡ���������еĸ��ŵ�Ԫ
        interferenceUnits = new List<InterferenceUnit>(FindObjectsOfType<InterferenceUnit>());

        // ��ʼ�� channelProperties ����
        channelProperties = GetComponent<CommChannelProperties>();
    }

    void Update()
    {
        if (startTransform != null && endTransform != null)
        {
            UpdateChannel();
            UpdateStaticParticles();
            UpdateInterferenceImpact();
            UpdateNoisePowerDensity(); // ʵʱ���� noisePowerDensity
        }
    }

    void UpdateChannel()
    {
        // ����Բ�����λ�úͳ���
        Vector3 direction = endTransform.position - startTransform.position;
        distance = direction.magnitude;

        transform.position = (startTransform.position + endTransform.position) / 2;
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
        transform.localScale = new Vector3(cylinderRadius, distance / 2, cylinderRadius);

        // ��������ϵͳ��λ�úͷ���
        particleSystemInstance.transform.position = startTransform.position;

        // ��Ӷ������ת����
        particleSystemInstance.transform.rotation = Quaternion.LookRotation(direction);

        // ��������ϵͳ����״�ʹ�С
        ParticleSystem.ShapeModule shape = particleSystemInstance.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 0;
        shape.radius = 0; // ʹ��Բ����İ뾶
        shape.length = distance; // ������ϵͳ�ĳ�������ΪԲ����ĳ���
        shape.position = new Vector3(0, 0, distance / 2);

        // �������ӵĴ�С
        ParticleSystem.MainModule main = particleSystemInstance.main;
        main.startSize = particleSize; // �������Ӵ�С
        main.startSpeed = distance / main.startLifetime.constant; // ���������ٶ�
        main.startColor = Color.red;
    }

    void ConfigureStaticParticleSystem()
    {
        // ���þ�̬����ϵͳ
        ParticleSystem.MainModule main = staticParticleSystemInstance.main;
        main.startSpeed = 0f; // ��ֹ����
        main.startSize = cylinderRadius; // �������Ӵ�С
        main.startLifetime = Mathf.Infinity; // �������ô���
        main.startColor = Color.red;

        ParticleSystem.EmissionModule emission = staticParticleSystemInstance.emission;
        emission.rateOverTime = 0f; // �����Զ�����

        ParticleSystem.ShapeModule shape = staticParticleSystemInstance.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 0;
        shape.radius = 0; // ʹ��Բ����İ뾶

        // �����ƶ�����ײ��ģ��
        ParticleSystem.VelocityOverLifetimeModule velocity = staticParticleSystemInstance.velocityOverLifetime;
        velocity.enabled = false;

        ParticleSystem.CollisionModule collision = staticParticleSystemInstance.collision;
        collision.enabled = false;

        // ������ȾģʽΪ Billboard
        ParticleSystemRenderer renderer = staticParticleSystemInstance.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        // �������ģ����ʵ���Ŷ�
        ParticleSystem.NoiseModule noise = staticParticleSystemInstance.noise;
        noise.enabled = true;
        noise.strengthX = 6.0f; // X�����Ŷ�ǿ��
        noise.strengthY = 6.0f; // Y�����Ŷ�ǿ��
        noise.frequency = 3.0f; // ����Ƶ��
        noise.scrollSpeed = 15.0f; // ���ƶ����ٶ�
    }

    void UpdateStaticParticles()
    {
        Vector3 direction = endTransform.position - startTransform.position;
        float distance = direction.magnitude;

        // ������̬����ϵͳ��λ��
        staticParticleSystemInstance.transform.position = startTransform.position;
        staticParticleSystemInstance.transform.rotation = Quaternion.LookRotation(direction);

        ParticleSystem.ShapeModule shape = staticParticleSystemInstance.shape;
        shape.length = distance; // ������ϵͳ�ĳ�������ΪԲ����ĳ���
        shape.position = new Vector3(0, 0, distance / 2);

        ParticleSystem.MainModule main = staticParticleSystemInstance.main;
        main.startSize = cylinderRadius; // ͬ���������Ӵ�С

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.applyShapeToPosition = true;

        staticParticleSystemInstance.Clear();
        int particleCount = Mathf.CeilToInt(distance / 0.2f); // �����������������Ϊ0.2��λ
        for (int i = 0; i < particleCount; i++)
        {
            float positionAlongPipe = i * (distance / (particleCount - 1)); // ���������ڹܵ��ϵ�λ��
            emitParams.position = new Vector3(0, 0, positionAlongPipe - (distance / 2));
            staticParticleSystemInstance.Emit(emitParams, 1); // ���侲̬����
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

        // ��������ǿ�Ȼ������߼�
        ParticleSystem.NoiseModule noise = staticParticleSystemInstance.noise;
        noise.strengthX = totalInterferenceImpact * 6.0f; // �����ܸ���Ӱ����� X�����Ŷ�ǿ��
        noise.strengthY = totalInterferenceImpact * 6.0f; // �����ܸ���Ӱ����� Y�����Ŷ�ǿ��
    }

    public void AddInterferenceImpact(float impact)
    {
        totalInterferenceImpact += impact;
    }

    void UpdateNoisePowerDensity()
    {
        if (channelProperties != null)
        {
            noisePowerDensity = channelProperties.noisePowerDensity; // �� CommChannelProperties ʵʱ��ȡ���������ܶ�
        }
    }


    // ���ӻ����ֵ���Ⱦ����
    public void UpdatchateVisuals(double capacity, double snr)
    {
        // �������� bps ת��Ϊ Mbps
        float capacityMbps = (float)(capacity / 1_000_000.0);

        // ��������ϵͳ�ķ�������
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

        // ����Բ�������״
        UpdateChannel();
    }
}











