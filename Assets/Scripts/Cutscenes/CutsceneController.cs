using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector m_PlayableDirector;
    [SerializeField]
    private PlayableAsset m_Cutscene;
    [SerializeField]
    private PolygonCollider2D newBound;
    [SerializeField]
    private GameObject m_Colliders;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if(newBound != null)
            {
                GameManager.Instance.ChangeBounds(newBound);
            }

            if(m_Colliders != null)
            {
                m_Colliders.SetActive(true);
            }

            m_PlayableDirector.playableAsset = m_Cutscene;
            m_PlayableDirector.Play();

            // if(enemySpawner != null)
            // {
            //     enemySpawner.ActivateSpawner(); // Activa el spawner después de la cutscene
            // }

            // Suscribirse al evento 'stopped' del PlayableDirector para cuando termine la cutscene
            m_PlayableDirector.stopped += OnPlayableDirectorStopped;

            gameObject.SetActive(false);
        }
    }

     // Método llamado cuando la cinemática termina
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if(director == m_PlayableDirector)
        {
            if(enemySpawner != null)
            {
                enemySpawner.ActivateSpawner(); // Activar el spawner después de la cinemática
            }

            // Desuscribirse del evento para evitar llamadas innecesarias
            m_PlayableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
