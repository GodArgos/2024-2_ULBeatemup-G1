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
            gameObject.SetActive(false);
        }
    }
}
