using UnityEngine;

public class WrackBallTrigger : MonoBehaviour
{
	private WrackBall wb;

	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			if ((bool)wb && !wb.collide)
			{
				wb.OnCollide(CharHelper.GetPlayer());
			}
			collide = true;
		}
	}

	public void SetParent(WrackBall wb)
	{
		this.wb = wb;
	}
}
