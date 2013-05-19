using System;
using Android.OS;

namespace Snake
{
	public class Refresh : Handler
	{
		private GameView view;

		public Refresh (GameView view)
		{
			this.view = view;
		}

		public override void HandleMessage (Message msg)
		{
			view.Update ();
			view.Invalidate ();
		}

		public void Sleep (long delayMillis)
		{
			this.RemoveMessages (0);
			SendMessageDelayed (ObtainMessage (0), delayMillis);
		}
	}
}