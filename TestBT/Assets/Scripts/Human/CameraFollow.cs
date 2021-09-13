using System;
using DG.Tweening;
using FSM.Player;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float m_Duration = 0.2f;
    [SerializeField] private float m_Strength = 0.5f;
    [SerializeField] private int m_Vibrato = 100;
    [SerializeField] private Vector3 m_RigOffset;
    private Transform m_Cam;
    private Transform m_Player;
    private float m_MouseX;
    private float m_MouseY;

    private void Awake()
    {
        m_Player = FindObjectOfType<PlayerController>().transform;
        m_Cam = Camera.main.transform;

        var weapons = FindObjectsOfType<Weapon>();
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Shake += () => m_Cam.DOShakePosition(m_Duration, m_Strength, m_Vibrato);
        }
    }

    private void Update()
    {
        CamMove();
    }

    private void LateUpdate()
    {
        transform.position = m_Player.transform.position + m_RigOffset;
    }

    void CamMove()
    {
        if (Input.GetMouseButton(1))
        {
            m_MouseX += Input.GetAxis("Mouse X");
            m_MouseY += Input.GetAxis("Mouse Y");
            m_MouseY = Mathf.Clamp(m_MouseY, -50f, 0f);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x + m_MouseY,
                transform.rotation.y + m_MouseX, 0));
        }
    }
}