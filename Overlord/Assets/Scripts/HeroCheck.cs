using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCheck : MonoBehaviour
{
    private bool useTouch = false;
    private float speed = 125f;

    private bool startPosFlag = true;

    private Vector2 ft_pos = Vector2.zero;
    private Vector2 t_pos = Vector2.zero;

    private Vector3 fm_pos = Vector3.zero;
    private Vector3 m_pos = Vector3.zero;

    void Update()
    {
        if (useTouch == true)
        {
            TouchRotate();
        }
        else ClickRotate();
    }

    public void TouchRotate()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && startPosFlag == true)
            {
                //Debug.Log("======开始触摸======");
                ft_pos = Input.GetTouch(0).position;
                startPosFlag = false;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                //Debug.Log("======释放触摸======");
                startPosFlag = true;
            }
            t_pos = Input.GetTouch(0).position;
            if (t_pos.x < ft_pos.x)
            {
                //Debug.Log("=======正方向转动======");
                transform.Rotate(new Vector3(0, (t_pos.x / t_pos.x), 0) * Time.deltaTime * speed);
            }
            else
            {
                //Debug.Log("=======负方向转动======");
                transform.Rotate(new Vector3(0, -(t_pos.x / t_pos.x), 0) * Time.deltaTime * speed);
            }
        }
    }

    public void ClickRotate()
    {
        if (Input.GetMouseButton(0))
        {
            m_pos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0) && m_pos.x < Screen.width / 2)
            {
                fm_pos = Input.mousePosition;
            }
            if (m_pos.x < fm_pos.x && fm_pos != Vector3.zero)
                {
                    transform.Rotate(new Vector3(0, (m_pos.x / m_pos.x), 0) * Time.deltaTime * speed);
                }
            else if (m_pos.x > fm_pos.x && fm_pos != Vector3.zero && m_pos.x < Screen.width / 2)
                {
                    transform.Rotate(new Vector3(0, -(m_pos.x / m_pos.x), 0) * Time.deltaTime * speed);
                }  
        }
    }
}