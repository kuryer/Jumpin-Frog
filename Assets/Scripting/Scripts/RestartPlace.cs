using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RestartPlace : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    Animator textAnim;

    private void Start()
    {
        textAnim = text.gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textAnim.SetTrigger("Change");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(0);  
    }
}
