using UnityEngine;

public class JumpCutController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoolVariable isGrounded;
    [SerializeField] PlayerVarsSO playerVariables;


    private void Awake()
    {
        this.enabled = false;
    }

    //wywo³ane je¿eli gracz podniesie jump button
    public void JumpCut()
    {
        if(rb.velocity.y > 0.1f)
        {
            Debug.Log("Jump Cut Performed");
            rb.AddForce(Vector2.down * rb.velocity.y * playerVariables.jumpCutMultiplier, ForceMode2D.Impulse);
            this.enabled = false;
        }
    }
    /*1. Zastanawia mnie czy jezeli to by³by input component (ten zewnetrzny z input systemu) to czy ten
     jump cut nie bedzie mozliwy to wywo³ania pomimo wy³¹czonego skryptu
      2. Czy nie potrzebuje jakiegoœ mechanizmu, który samodzielnie zadba o wy³¹czenie jump cuta po chwili
     ale chyba lepszym pomys³em bêdzie dodanie do zmian state'u modularnych metod, które bêd¹ siê 
     wywo³ywaæ w onEnter i onExit (to brzmi jak spoko pomys³ ale trzeba to przeanalizowaæ)*/

}