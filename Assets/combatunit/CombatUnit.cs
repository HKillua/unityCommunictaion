using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public delegate void DataChanged();
    public event DataChanged OnDataChanged;

    [SerializeField] private float moveSpeed = 0; // �ƶ��ٶ�
    [SerializeField] private float turnSpeed = 200f; // ��ת�ٶ�
    [SerializeField] private float changeDirectionInterval = 2f; // �ı䷽��ļ��
    private Vector3 moveDirection;
    private bool isOutOfBoundary = false; // ������־λ

    [SerializeField]
    public List<Transceiver> transceivers = new List<Transceiver>();
    [SerializeField]
    public List<Antenna> antennas = new List<Antenna>();

    void Start()
    {
        // ��ʼ�����ǰ������
        moveDirection = transform.forward;
        StartCoroutine(ChangeDirectionCoroutine());
    }

    void Update()
    {
        if (!isOutOfBoundary)
        {
            // �ƶ���ս��Ԫ
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            // ��������߽磬���÷����߼�
            Vector3 boundaryCenter = Vector3.zero; // ����Ӧ����Ϊ��ı߽�����
            ReturnToBoundary(boundaryCenter);
        }

        PreventFlip();
    }

    private void ChangeDirection()
    {
        // �������һ���µ��ƶ���������ת�䣩
        float turnAmount = Random.Range(-45f, 45f);
        Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
        moveDirection = turnRotation * transform.forward;
    }

    private IEnumerator ChangeDirectionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionInterval);
            ChangeDirection();
        }
    }

    private void PreventFlip()
    {
        // ��鲢���Ʒ�ת
        if (transform.rotation.eulerAngles.x > 10f && transform.rotation.eulerAngles.x < 350f)
        {
            // ��������X����ת����Ϊ0���Է�ֹ��ת
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, currentRotation.y, currentRotation.z);
        }
    }

    public void ReturnToBoundary(Vector3 boundaryCenter)
    {
        Vector3 directionToBoundary = (boundaryCenter - transform.position).normalized; // ����ָ��߽�����
        transform.position += directionToBoundary * moveSpeed * Time.deltaTime;

        // ����Ѿ��ӽ��߽����ģ�ֹͣ����
        if (Vector3.Distance(transform.position, boundaryCenter) < 0.1f)
        {
            isOutOfBoundary = false; // ��־Ϊ���ڱ߽���
        }
    }

    public void AddTransceiver(Transceiver newTransceiver)
    {
        transceivers.Add(newTransceiver);
        OnDataChanged?.Invoke();
    }

    public void SetOutOfBoundary(bool value) // �������������Ƿ��ڱ߽���
    {
        isOutOfBoundary = value;
    }

    // ���һ���µ� Antenna
    public void AddAntenna(Antenna newAntenna)
    {
        antennas.Add(newAntenna);
        OnDataChanged?.Invoke();
    }

    public void RemoveTransceiver(int index)
    {
        if (index >= 0 && index < transceivers.Count)
        {
            transceivers.RemoveAt(index);
            OnDataChanged?.Invoke();
        }
    }

    public void RemoveAntenna(int index)
    {
        if (index >= 0 && index < antennas.Count)
        {
            antennas.RemoveAt(index);
            OnDataChanged?.Invoke();
        }
    }
}





