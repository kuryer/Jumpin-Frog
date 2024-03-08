using UnityEngine;

public class SwingTemporaryCheck : MonoBehaviour
{
    [SerializeField] SwingVariable ActualSwing;

    private void Update()
    {
        if (ActualSwing.CanSwing)
            this.enabled = false;
        else
        {
            if (transform.position.y > ActualSwing.Value.transform.position.y)
            {
                ActualSwing.CanSwing = true;
                this.enabled = false;
            }
        }
    }
}
