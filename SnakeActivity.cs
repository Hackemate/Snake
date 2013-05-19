using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace Snake
{

	[Activity (Label = "Snake", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class SnakeActivity : Activity
	{
		private GameView snake_view;

		private static String ICICLE_KEY = "game-view";


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.snake_layout);

			Button b1 = (Button)FindViewById (Resource.Id.button1);
			b1.SetX (200);
			b1.SetY (530);
			b1.SetHeight (60);
			b1.SetWidth (60);
			b1.Text = "Arriba";
			b1.Click += delegate {
				snake_view.keypress(Keycode.DpadUp);
			};

			Button b2 = (Button)FindViewById(Resource.Id.button2);
			b2.SetX(200);
			b2.SetY(610);
			b2.SetHeight (60);
			b2.SetWidth (60);
			b2.Text = "Abajo";
			b2.Click += delegate {
				snake_view.keypress(Keycode.DpadDown);
			};
			Button b3 = (Button)FindViewById(Resource.Id.button3);
			b3.SetX(100);
			b3.SetY(610);
			b3.Text = "Izq.";
			b3.SetHeight (60);
			b3.SetWidth (60);
			b3.Click += delegate {
				snake_view.keypress(Keycode.DpadLeft);
			};
			Button b4 = (Button)FindViewById(Resource.Id.button4);
			b4.SetX(300);
			b4.SetY(610);
			b4.Text = "Der.";
			b4.SetHeight (60);
			b4.SetWidth (60);
			b4.Click += delegate {
				snake_view.keypress(Keycode.DpadRight);
			};


			snake_view = (GameView)FindViewById(Resource.Id.snake);
			snake_view.SetTextView((TextView)FindViewById(Resource.Id.text));

			if (savedInstanceState == null) {

				snake_view.SetMode (GameMode.Ready);
			} else {

				Bundle map = savedInstanceState.GetBundle (ICICLE_KEY);

				if (map != null)
					snake_view.RestoreState (map);
				else
					snake_view.SetMode (GameMode.Paused);
			}
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			snake_view.SetMode (GameMode.Paused);
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{

			outState.PutBundle (ICICLE_KEY, snake_view.SaveState ());
		}
	}
}
