using System;
using System.Collections.Generic;

using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace Snake
{

	public class GameView : PieceView
	{

		private static Random Rand = new Random ();
		private GameMode mode = GameMode.Ready;
		private Direccion mDireccion = Direccion.Arriba;
		private Direccion mSig_Direccion = Direccion.Arriba;
		private long mPuntos = 0;
		private int mMoveDelay = 600;
		private long mLastMove;
		private TextView mStatusText;
		private List<Coordenadas> snake_campo = new List<Coordenadas> ();
		private List<Coordenadas> apples = new List<Coordenadas> ();
		private Refresh mReDrawHandler;

		public GameView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			InitGameView ();
			mReDrawHandler = new Refresh (this);
		}

		public GameView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			InitGameView ();
			mReDrawHandler = new Refresh (this);
		}

		private void InitGameView ()
		{
			Focusable = true;
			FocusableInTouchMode = true;


			ResetPieces (4);

			LoadPiece (PieceType.Rojo, Resources.GetDrawable (Resource.Drawable.cRojo));
			LoadPiece (PieceType.Amarillo, Resources.GetDrawable (Resource.Drawable.cAmarillo));
			LoadPiece (PieceType.Verde, Resources.GetDrawable (Resource.Drawable.cVerde));

			Click += new EventHandler (GameView_Click);
		}

		private void InitNewGame ()
		{
			snake_campo.Clear ();
			apples.Clear ();
	
			snake_campo.Add (new Coordenadas (7, 10));
			snake_campo.Add (new Coordenadas (6, 10));
			snake_campo.Add (new Coordenadas (5, 10));
			snake_campo.Add (new Coordenadas (4, 10));
			snake_campo.Add (new Coordenadas (3, 10));
			snake_campo.Add (new Coordenadas (2, 10));

			mSig_Direccion = Direccion.Arriba;


			AddRandomCheese ();
			AddRandomCheese ();

			mMoveDelay = 200;
			mPuntos = 0;
		}

		private void AddRandomCheese ()
		{
			Coordenadas newCoord = null;
			bool found = false;

			while (!found) {
	
				int nX = 1 + Rand.Next (x_piece_count - 2);
				int nY = 1 + Rand.Next (y_piece_count - 2);

				newCoord = new Coordenadas (nX, nY);

	
				bool colision = false;

				int snakelength = snake_campo.Count;
				for (int index = 0; index < snakelength; index++) {
					if (snake_campo[index] == newCoord) {
						colision = true;
					}
				}

				found = !colision;
			}


			apples.Add (newCoord);
		}


		public void SetTextView (TextView newView)
		{
			mStatusText = newView;
		}

		public bool keypress(Keycode keyCode)
		{
			if (keyCode == Keycode.DpadUp || keyCode == Keycode.VolumeUp) {
				if (mode == GameMode.Ready | mode == GameMode.Lost) {
					
					InitNewGame ();
					
					SetMode (GameMode.Running);
					Update ();
					
					return true;
				}
				
				if (mode == GameMode.Paused) {
					
					SetMode (GameMode.Running);
					Update ();
					
					return true;
				}
				
				if (keyCode == Keycode.VolumeUp) {
					mSig_Direccion = (Direccion) (((int)mDireccion + 1) % 4);
				} else {
					if (mDireccion != Direccion.Abajo)
						mSig_Direccion = Direccion.Arriba;
				}
				
				return true;
			}
			
			if (keyCode == Keycode.DpadDown) {
				if (mDireccion != Direccion.Arriba)
					mSig_Direccion = Direccion.Abajo;
				
				return true;
			}
			
			if (keyCode == Keycode.VolumeDown) {
				mSig_Direccion = (Direccion) (((int)mDireccion - 1) % 4);
				
				return true;
			}
			
			if (keyCode == Keycode.DpadLeft) {
				if (mDireccion != Direccion.Derecha)
					mSig_Direccion = Direccion.Izquierda;
				
				return true;
			}
			
			if (keyCode == Keycode.DpadRight) {
				if (mDireccion != Direccion.Izquierda)
					mSig_Direccion = Direccion.Derecha;
				
				return (true);
			}
			return (true);
		}


		private void GameView_Click (object sender, EventArgs e)
		{

			if (mode == GameMode.Ready | mode == GameMode.Lost) {

				InitNewGame ();

				SetMode (GameMode.Running);
				Update ();
			}
		}
		public void SetMode (GameMode newMode)
		{
			GameMode exMode = mode;
			mode = newMode;

			if (newMode == GameMode.Running & exMode != GameMode.Running) {
				mStatusText.Visibility = ViewStates.Invisible;
				Update ();

				return;
			}

			var str = "";

			if (newMode == GameMode.Paused)
				str = Resources.GetText (Resource.String.mode_pause);
			else if (newMode == GameMode.Ready)
				str = Resources.GetText (Resource.String.mode_ready);
			else if (newMode == GameMode.Lost) {
				var lose_prefix = Resources.GetString (Resource.String.mode_lose_prefix);
				var lose_suffix = Resources.GetString (Resource.String.mode_lose_suffix);
				str = string.Format ("{0}{1}{2}", lose_prefix, mPuntos, lose_suffix);
			}

			mStatusText.Text = str;
			mStatusText.Visibility = ViewStates.Visible;
		}

		public void Update ()
		{
			if (mode == GameMode.Running) {
				long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

				if (now - mLastMove > mMoveDelay) {
					ClearPieces ();
					UpdateWalls ();
					UpdateSnake ();
					UpdateCheeses ();
					mLastMove = now;
				}

				mReDrawHandler.Sleep (mMoveDelay);
			}
		}

		private void UpdateWalls ()
		{
			for (int x = 0; x < x_piece_count; x++) {
				SetPiece (PieceType.Verde, x, 0);
				SetPiece (PieceType.Verde, x, y_piece_count - 1);
			}

			for (int y = 1; y < y_piece_count - 1; y++) {
				SetPiece (PieceType.Verde, 0, y);
				SetPiece (PieceType.Verde, x_piece_count - 1, y);
			}
		}

		private void UpdateCheeses ()
		{
			foreach (Coordenadas c in apples)
				SetPiece (PieceType.Amarillo, c.X, c.Y);
		}

		private void UpdateSnake ()
		{
			bool growSnake = false;

			Coordenadas head = snake_campo[0];
			Coordenadas newHead = new Coordenadas (1, 1);

			mDireccion = mSig_Direccion;

			switch (mDireccion) {
				case Direccion.Derecha:
					newHead = new Coordenadas (head.X + 1, head.Y);
					break;
				case Direccion.Izquierda:
					newHead = new Coordenadas (head.X - 1, head.Y);
					break;
				case Direccion.Arriba:
					newHead = new Coordenadas (head.X, head.Y - 1);
					break;
				case Direccion.Abajo:
					newHead = new Coordenadas (head.X, head.Y + 1);
					break;
			}


			if ((newHead.X < 1) || (newHead.Y < 1) || (newHead.X > x_piece_count - 2)
				|| (newHead.Y > y_piece_count - 2)) {
				SetMode (GameMode.Lost);

				return;
			}


			foreach (Coordenadas snake in snake_campo) {
				if (snake.Equals (newHead)) {
					SetMode (GameMode.Lost);
					return;
				}
			}


			foreach (Coordenadas apple in apples) {
				if (apple.Equals (newHead)) {
					apples.Remove (apple);
					AddRandomCheese ();

					mPuntos++;

					mMoveDelay = (int)(mMoveDelay * 0.9);


					growSnake = true;

					break;
				}
			}


			snake_campo.Insert (0, newHead);


			if (!growSnake)
				snake_campo.RemoveAt (snake_campo.Count - 1);

			int index = 0;


			foreach (Coordenadas c in snake_campo) {
				if (index == 0)
					SetPiece (PieceType.Verde, c.X, c.Y);
				else
					SetPiece (PieceType.Rojo, c.X, c.Y);

				index++;
			}
		}

		public Bundle SaveState ()
		{
			Bundle map = new Bundle ();

			map.PutIntArray ("mCheeseList", Coordenadas.ListToArray (apples));
			map.PutInt ("mDireccion", (int)mDireccion);
			map.PutInt ("mSig_Direccion", (int)mSig_Direccion);
			map.PutInt ("mMoveDelay", mMoveDelay);
			map.PutLong ("mPuntos", mPuntos);
			map.PutIntArray ("mSnakeTrail", Coordenadas.ListToArray (snake_campo));

			return map;
		}


		public void RestoreState (Bundle icicle)
		{
			SetMode (GameMode.Paused);

			apples = Coordenadas.ArrayToList (icicle.GetIntArray ("mCheeseList"));
			mDireccion = (Direccion)icicle.GetInt ("mDireccion");
			mSig_Direccion = (Direccion)icicle.GetInt ("mSig_Direccion");
			mMoveDelay = icicle.GetInt ("mMoveDelay");
			mPuntos = icicle.GetLong ("mPuntos");
			snake_campo = Coordenadas.ArrayToList (icicle.GetIntArray ("mSnakeTrail"));
		}

	}

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

	public enum Direccion
	{
		Arriba,
		Derecha,
		Abajo,
		Izquierda
	}
	public enum GameMode
	{
		Paused,
		Ready,
		Running,
		Lost
	}

}
