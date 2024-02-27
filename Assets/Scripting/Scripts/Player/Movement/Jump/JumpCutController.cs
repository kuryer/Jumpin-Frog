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
    /*1. Zastanawia mnie czy jezeli to by�by input component (ten zewnetrzny z input systemu) to czy ten
     jump cut nie bedzie mozliwy to wywo�ania pomimo wy��czonego skryptu
      2. Czy nie potrzebuje jakiego� mechanizmu, kt�ry samodzielnie zadba o wy��czenie jump cuta po chwili
     ale chyba lepszym pomys�em b�dzie dodanie do zmian state'u modularnych metod, kt�re b�d� si� 
     wywo�ywa� w onEnter i onExit (to brzmi jak spoko pomys� ale trzeba to przeanalizowa�)*/
    /*1.1 no zadzia�a xd, tak�e ten no !enabled ig
      2.1 potrzebuje i tak juz go napisa�em B), trzeba tylko dopisa� do InAir.OnExit() -> JumpCutController.enabled = false*/
}