using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Utilities/Variables/Swing")]
public class SwingVariable : BaseVariable<Swing>
{
    public bool CanSwing;

    public void SetCanSwing(bool canSwing)
    {
        CanSwing = canSwing;
    }
}
