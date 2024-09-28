using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public delegate void DataChanged();
    public event DataChanged OnDataChanged;

    [SerializeField] private float moveSpeed = 0; // 移动速度
    [SerializeField] private float turnSpeed = 200f; // 旋转速度
    [SerializeField] private float changeDirectionInterval = 2f; // 改变方向的间隔
    private Vector3 moveDirection;
    private bool isOutOfBoundary = false; // 新增标志位

    [SerializeField]
    public List<Transceiver> transceivers = new List<Transceiver>();
    [SerializeField]
    public List<Antenna> antennas = new List<Antenna>();

    void Start()
    {
        // 初始化随机前进方向
        moveDirection = transform.forward;
        StartCoroutine(ChangeDirectionCoroutine());
    }

    void Update()
    {
        if (!isOutOfBoundary)
        {
            // 移动作战单元
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            // 如果超出边界，调用返回逻辑
            Vector3 boundaryCenter = Vector3.zero; // 这里应设置为你的边界中心
            ReturnToBoundary(boundaryCenter);
        }

        PreventFlip();
    }

    private void ChangeDirection()
    {
        // 随机生成一个新的移动方向（左右转弯）
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
        // 检查并限制翻转
        if (transform.rotation.eulerAngles.x > 10f && transform.rotation.eulerAngles.x < 350f)
        {
            // 将车辆的X轴旋转重置为0，以防止翻转
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, currentRotation.y, currentRotation.z);
        }
    }

    public void ReturnToBoundary(Vector3 boundaryCenter)
    {
        Vector3 directionToBoundary = (boundaryCenter - transform.position).normalized; // 方向指向边界中心
        transform.position += directionToBoundary * moveSpeed * Time.deltaTime;

        // 如果已经接近边界中心，停止返回
        if (Vector3.Distance(transform.position, boundaryCenter) < 0.1f)
        {
            isOutOfBoundary = false; // 标志为不在边界外
        }
    }

    public void AddTransceiver(Transceiver newTransceiver)
    {
        transceivers.Add(newTransceiver);
        OnDataChanged?.Invoke();
    }

    public void SetOutOfBoundary(bool value) // 新增方法设置是否在边界外
    {
        isOutOfBoundary = value;
    }

    // 添加一个新的 Antenna
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





