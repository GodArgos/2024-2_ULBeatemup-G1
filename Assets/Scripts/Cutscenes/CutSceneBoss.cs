using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;  // Para manejar las cinemáticas
using UnityEngine.SceneManagement;  // Para reiniciar el nivel

public class CutSceneBoss : MonoBehaviour
{
    [SerializeField] private PlayableDirector m_PlayableDirector;
    [SerializeField] private PlayableAsset m_Cutscene;
    [SerializeField] private float restartDelay = 5f;  // Tiempo de espera para reiniciar el juego

    private bool startCinematic = false;  // El booleano que controla cuándo iniciar la cinemática

    void Update()
    {
        // Verifica si el booleano se ha activado
        if (startCinematic)
        {
            // Solo iniciamos la cinemática si no se ha empezado
            if (m_PlayableDirector.state != PlayState.Playing)
            {
                PlayCutscene();
            }
        }
    }

    // Método para iniciar la cinemática
    public void PlayCutscene()
    {
        // Asignar la cinemática al PlayableDirector y reproducirla
        m_PlayableDirector.playableAsset = m_Cutscene;
        m_PlayableDirector.Play();

        // Suscribirse al evento 'stopped' del PlayableDirector para cuando termine la cinemática
        m_PlayableDirector.stopped += OnPlayableDirectorStopped;

        // Desactivar el booleano para evitar múltiples reproducciones
        startCinematic = false;
    }

    // Este método será llamado cuando termine la cinemática
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == m_PlayableDirector)
        {
            // Desuscribir el evento para evitar múltiples llamadas
            m_PlayableDirector.stopped -= OnPlayableDirectorStopped;

            // Iniciar el reinicio del juego después de la cinemática
            StartCoroutine(RestartGame());
        }
    }

    // Método para reiniciar el nivel con un retraso
    IEnumerator RestartGame()
    {
        // Espera un tiempo antes de reiniciar el nivel
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Método que puedes llamar desde BossHealth para iniciar la cinemática
    public void StartCinematic()
    {
        startCinematic = true;  // Activa el booleano para que la cinemática comience en Update
    }
}
