/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//��ά��������ת��Ļ����
        //�������Ļ����תΪ��ά���꣬�ټ�������λ�������֮��ľ���
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
    public float baseSmoothSpeed = 10f; // ����ƽ���ٶ�
    public float closeDistanceThreshold = 0.1f; // �ж��Ƿ�ӽ�Ŀ��ľ�����ֵ
    public float fastFollowDistance = 1.0f; // ���ٸ���ľ�����ֵ

    private Vector3 targetPosition; // ���ָ���Ŀ��λ��

    public IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position); // ����ά��������תΪ��Ļ����
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z)); // �������������֮���ƫ��

        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 desiredPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset; // �����µ�Ŀ��λ��

            // ���û�з�����ײ�������Ŀ��λ��
            if (!IsColliding(desiredPosition))
            {
                targetPosition = desiredPosition;
            }

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // ��̬����ƽ���ٶȣ�����Զʱ���죬�����ʱ����
            float smoothSpeed = distanceToTarget > fastFollowDistance ? baseSmoothSpeed * 2 : baseSmoothSpeed;

            // ʹ�ò�ֵƽ���ƶ���Ŀ��λ��
            if (distanceToTarget > closeDistanceThreshold)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition; // ����ǳ��ӽ�Ŀ��λ�ã�ֱ������
            }

            yield return null;
        }
    }

    // ���Ŀ��λ���Ƿ��������������ײ
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
                if (hit.collider != collider) // �����⵽�����������壬���ʾ������ײ
                {
                    return true;
                }
            }
        }
        return false;
    }
}

