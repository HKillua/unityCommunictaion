/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//三维物体坐标转屏幕坐标
        //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }
    }

}
*/

using System.Collections;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public float baseSmoothSpeed = 10f; // 基础平滑速度
    public float closeDistanceThreshold = 0.1f; // 判断是否接近目标的距离阈值
    public float fastFollowDistance = 1.0f; // 快速跟随的距离阈值

    private Vector3 targetPosition; // 鼠标指向的目标位置

    public IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position); // 将三维物体坐标转为屏幕坐标
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z)); // 计算物体与鼠标之间的偏移

        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 desiredPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset; // 计算新的目标位置

            // 如果没有发生碰撞，则更新目标位置
            if (!IsColliding(desiredPosition))
            {
                targetPosition = desiredPosition;
            }

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // 动态调整平滑速度：距离远时更快，距离近时更慢
            float smoothSpeed = distanceToTarget > fastFollowDistance ? baseSmoothSpeed * 2 : baseSmoothSpeed;

            // 使用插值平滑移动到目标位置
            if (distanceToTarget > closeDistanceThreshold)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition; // 如果非常接近目标位置，直接设置
            }

            yield return null;
        }
    }

    // 检测目标位置是否会与其他物体碰撞
    private bool IsColliding(Vector3 targetPosition)
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            RaycastHit hit;
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, distance))
            {
                if (hit.collider != collider) // 如果检测到的是其他物体，则表示发生碰撞
                {
                    return true;
                }
            }
        }
        return false;
    }
}

