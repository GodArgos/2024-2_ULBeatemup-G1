using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;  // Para manejar las cinem�ticas
using UnityEngine.SceneManagement;  // Para reiniciar el nivel

public class CutSceneBoss : MonoBehaviour
{
    [SerializeField] private PlayableDirector m_PlayableDirector;
    [SerializeField] private PlayableAsset m_Cutscene;
    [SerializeField] private float restartDelay = 5f;  // Tiempo de espera para reiniciar el juego

    private bool startCinematic = false;  // El booleano que controla cu�ndo iniciar la cinem�tica

    void Update()
    {
        // Verifica si el booleano se ha activado
        if (startCinematic)
        {
            // Solo iniciamos la cinem�tica si no se ha empezado
            if (m_PlayableDirector.state != PlayState.Playing)
            {
                PlayCutscene();
            }
        }
    }

    // M�todo para iniciar la cinem�tica
    public void PlayCutscene()
    {
        // Asignar la cinem�tica al PlayableDirector y reproducirla
        m_PlayableDirector.playableAsset = m_Cutscene;
        m_PlayableDirector.Play();

        // Suscribirse al evento 'stopped' del PlayableDirector para cuando termine la cinem�tica
        m_PlayableDirector.stopped += OnPlayableDirectorStopped;

        // Desactivar el booleano para evitar m�ltiples reproducciones
        startCinematic = false;
    }

    // Este m�todo ser� llamado cuando termine la cinem�tica
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == m_PlayableDirector)
        {
            // Desuscribir el evento para evitar m�ltiples llamadas
            m_PlayableDirector.stopped -= OnPlayableDirectorStopped;

            // Iniciar el reinicio del juego despu�s de la cinem�tica
            StartCoroutine(RestartGame());
        }
    }

    // M�todo para reiniciar el nivel con un retraso
    IEnumerator RestartGame()
    {
        // Espera un tiempo antes de reiniciar el nivel
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // M�todo que puedes llamar desde BossHealth para iniciar la cinem�tica
    public void StartCinematic()
    {
        startCinematic = true;  // Activa el booleano para que la cinem�tica comience en Update
    }
}
