using System;

public class Purchase : ButtonBehaviour
{
    public event Action<Purchasable> Purchased;
    public Purchasable purchasable;

    protected override void HandleClick()
    {
        if (Purchased != null) Purchased(purchasable);
    }
}