using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    [HideInInspector]
    public bool m_IsGameOver = false;
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject mainCamera;

    [HideInInspector]
    private CinemachineConfiner confiner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (mainCamera != null)
        {
            confiner = mainCamera.GetComponent<CinemachineConfiner>();

        }
        else
        {
            Debug.LogError("Main Camera no está asignada.");
        }
    }

    public void ChangeBounds(PolygonCollider2D newBounds)
    {
        if (confiner != null && newBounds != null)
        {
            confiner.m_BoundingShape2D = newBounds;
            Debug.Log("Bounding Shape 2D actualizado correctamente.");
        }
        else
        {
            Debug.LogError("No se encontró el CinemachineConfiner o el nuevo Bounding Shape es nulo.");
        }
    }
}
