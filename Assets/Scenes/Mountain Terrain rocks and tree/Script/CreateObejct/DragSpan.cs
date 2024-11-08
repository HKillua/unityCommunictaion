using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UIElements;
using TMPro;

public class DragSpan : MonoBehaviour, IPointerDownHandler
{


    public GameObject prefab;   //Ҫ���ɵ�Ԥ�Ƽ�
    

    //��ק������
    private GameObject _objDragSpawning;

    //�Ƿ�������ק
    private bool _isDragSpawning = false;

    // Update is called once per frame
    void Update()
    {
        if (_isDragSpawning)
        {
            //ˢ��λ��


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _objDragSpawning.transform.position = ray.GetPoint(10);


            RaycastHit hit;    // 
            if (Physics.Raycast(ray,out hit) )
            {
                if (hit.collider != null && hit.collider.gameObject != _objDragSpawning)
                {
                    _objDragSpawning.transform.position = hit.point;
                }
            }  // 




            //������ק , ͬʱ����ˢ����ʾ����
            if (Input.GetMouseButtonUp(0))
            {
                _isDragSpawning = false;
                _objDragSpawning = null;
                CombatUnitPanel.UpdateUITrigger();
            }
        }
    }
    //��ק���ܵ�ʵ�֣��ú�����Ӧ�õ��κνű���
    IEnumerator OnMouseDown()
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

    //�������ʱ��ʼ����ʵ��Ԥ�Ƽ�
    public void OnPointerDown(PointerEventData eventData)
    {
        // GameObject prefab = Resources.Load<GameObject>("plane");

        if (prefab != null)
        {
            _objDragSpawning = Instantiate(prefab);
            _objDragSpawning.transform.SetParent(null);
            _isDragSpawning = true;

            Rigidbody rb = _objDragSpawning.GetComponent<Rigidbody>(); // 
            if (rb == null)
            {
                rb = _objDragSpawning.AddComponent<Rigidbody>();
            }
            rb.isKinematic = true;  // 


        }
    }

}


