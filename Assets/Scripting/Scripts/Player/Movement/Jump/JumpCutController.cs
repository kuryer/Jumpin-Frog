using UnityEngine;
using UnityEngine.InputSystem;

public class JumpCutController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoolVariable isGrounded;
    [SerializeField] PlayerMovementVariables playerVariables;

    private void Awake()
    {
        this.enabled = false;
    }

    public void JumpCut(InputAction.CallbackContext context)
    {
        if (!context.canceled || !enabled)
            return;
        if(rb.velocity.y > 0.1f)
        {
            Debug.Log("Jump Cut Performed");
            rb.AddForce(Vector2.down * rb.velocity.y * playerVariables.JumpCutMultiplier, ForceMode2D.Impulse);
            this.enabled = false;
        }
    }
    /*1. Zastanawia mnie czy jezeli to by³by input component (ten zewnetrzny z input systemu) to czy ten
     jump cut nie bedzie mozliwy to wywo³ania pomimo wy³¹czonego skryptu
      2. Czy nie potrzebuje jakiegoœ mechanizmu, który samodzielnie zadba o wy³¹czenie jump cuta po chwili
     ale chyba lepszym pomys³em bêdzie dodanie do zmian state'u modularnych metod, które bêd¹ siê 
     wywo³ywaæ w onEnter i onExit (to brzmi jak spoko pomys³ ale trzeba to przeanalizowaæ)*/
    /*1.1 no zadzia³a xd, tak¿e ten no !enabled ig
      2.1 potrzebuje i tak juz go napisa³em B), trzeba tylko dopisaæ do InAir.OnExit() -> JumpCutController.enabled = false*/
}