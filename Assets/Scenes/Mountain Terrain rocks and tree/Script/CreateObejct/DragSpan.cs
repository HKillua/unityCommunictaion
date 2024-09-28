using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UIElements;

public class DragSpan : MonoBehaviour, IPointerDownHandler
{


    public GameObject prefab;   //要生成的预制件
    

    //拖拽的物体
    private GameObject _objDragSpawning;

    //是否正在拖拽
    private bool _isDragSpawning = false;

    // Update is called once per frame
    void Update()
    {
        if (_isDragSpawning)
        {
            //刷新位置


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _objDragSpawning.transform.position = ray.GetPoint(10);


            //结束拖拽
            if (Input.GetMouseButtonUp(0))
            {
                _isDragSpawning = false;
                _objDragSpawning = null;
            }
        }
    }
    //拖拽功能的实现，该函数可应用到任何脚本中
    IEnumerator OnMouseDown()
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

    //按下鼠标时开始生成实体预制件
    public void OnPointerDown(PointerEventData eventData)
    {
        // GameObject prefab = Resources.Load<GameObject>("plane");

        if (prefab != null)
        {
            _objDragSpawning = Instantiate(prefab);
            _objDragSpawning.transform.SetParent(null);
            _isDragSpawning = true;
        }
    }

}


