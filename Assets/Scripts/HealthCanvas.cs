using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HealthCanvas;

    public void HealthCanvasActivation()
    {
        if (m_HealthCanvas.activeInHierarchy)
        {
            m_HealthCanvas.SetActive(false);
        }
        else
        {
            m_HealthCanvas.SetActive(true);
        }
    }
}
