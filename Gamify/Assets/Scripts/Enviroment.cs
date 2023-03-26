using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class Enviroment : MonoBehaviour
{
    public static Enviroment Instance { get { return instance; } }
    private static Enviroment instance;
    [SerializeField]
    private Transform ring1, ring2, ring3;
    [SerializeField]
    private float ringSpeed;
    [SerializeField]
    private CinemachineFreeLook freeLookCam;
    public bool freeLookActive = true;
    public Transform[] portals;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ring1.Rotate(ringSpeed * Time.deltaTime, 0, ringSpeed * Time.deltaTime);
        ring2.Rotate(0, -ringSpeed * Time.deltaTime, -ringSpeed * Time.deltaTime);
        ring3.Rotate(-ringSpeed * Time.deltaTime, ringSpeed * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.LeftControl) && freeLookActive)
        {

            freeLookCam.m_XAxis.m_MaxSpeed = 0;
            freeLookCam.m_YAxis.m_MaxSpeed = 0;
            freeLookActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && !freeLookActive)
        {
            freeLookCam.m_XAxis.m_MaxSpeed = 600;
            freeLookCam.m_YAxis.m_MaxSpeed = 20;
            freeLookActive = true;
        }


    }
}
