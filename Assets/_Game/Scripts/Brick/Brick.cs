using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;

    private void Start()
    {
        LevelManager.Ins.ActiveColor(objectRenderer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PLayer))
        {
            gameObject.SetActive(false);
        }
    }
}
