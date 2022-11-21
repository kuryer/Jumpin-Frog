using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPoint : MonoBehaviour
{
    SwingPoint thisScript;
    [SerializeField] Animator swingPointAnim;
    [SerializeField] float cooldown;
    bool isSwinging;
    bool canSwing;
    private void Start()
    {
        thisScript = gameObject.GetComponent<SwingPoint>();    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Helpers.PlayerMovement.SetCanSwingTrue(Position(), GetComponent<Rigidbody2D>(), thisScript);
            //PlayInRangeAnimation();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.transform.position.y < transform.position.y && !canSwing)
            {
                Helpers.PlayerMovement.SetCanSwingTrue(Position(), GetComponent<Rigidbody2D>(), thisScript);
                PlayInRangeAnimation();
                canSwing = true;
            }
            else if(collision.transform.position.y > transform.position.y && canSwing)
            {
                Helpers.PlayerMovement.SetCanSwingFalse();
                PlayOutOfRangeAnimation();
                canSwing = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Helpers.PlayerMovement.SetCanSwingFalse();
            PlayOutOfRangeAnimation();
            canSwing = false;
        }
    }
    #region Animations
    void PlayInRangeAnimation()
    {
        if (!isSwinging)
            swingPointAnim.SetBool("InRange", true);
    }
    void PlayOutOfRangeAnimation()
    {
        if (!isSwinging)
            swingPointAnim.SetBool("InRange", false);
    }
    public void PlaySwingAnimation()
    {
        isSwinging = true;
        //swingPointAnim.StopPlayback();
    }
    void PlayCooldownAnimation()
    {
        swingPointAnim.Play("Cooldown");
    }
    #endregion


    #region Gizmos
    private void OnDrawGizmos()
    {
        
    }

    #endregion


    #region Variables
    Vector2 Position()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    #endregion


    #region Timer
    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        thisScript.enabled = false;
        isSwinging = false;
        PlayOutOfRangeAnimation();
        //Debug.Log("unabled");
        //PlayCooldownAnimation();
        yield return new WaitForSeconds(cooldown);
        //Debug.Log("enabled");
        thisScript.enabled = true;
    }
    #endregion
}
