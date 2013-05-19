using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;


namespace Snake
{

	public class PieceView : View
	{
		protected static int piece_size = 22;

		protected static int x_piece_count;
		protected static int y_piece_count;

		private static int x_offset; 
		private static int y_offset;

		private Bitmap[] piece_bitmaps;
		private PieceType[,] pieces;

		private Paint paint = new Paint ();

		public PieceView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle){}
		
		public PieceView (Context context, IAttributeSet attrs) :
		base (context, attrs){}


		public void ResetPieces (int pieceCount)
		{
			piece_bitmaps = new Bitmap[pieceCount];
		}


		public void LoadPiece (PieceType type, Drawable piece)
		{
			Bitmap bitmap = Bitmap.CreateBitmap (piece_size, piece_size, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas (bitmap);

			piece.SetBounds (0, 0, piece_size, piece_size);
			piece.Draw (canvas);

			piece_bitmaps[(int)type] = bitmap;
		}

	
		public void ClearPieces ()
		{
			for (int x = 0; x < x_piece_count; x++)
				for (int y = 0; y < y_piece_count; y++)
					SetPiece (0, x, y);
		}

		public void SetPiece (PieceType piece, int x, int y)
		{
			pieces[x, y] = piece;
		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			x_piece_count = (int)System.Math.Floor ((double)w / piece_size);
			y_piece_count = (int)System.Math.Floor ((double)h / piece_size)-10;

			x_offset = 0;
			y_offset = 0;
			pieces = new PieceType[x_piece_count, y_piece_count];

			ClearPieces ();
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);

			for (int x = 0; x < x_piece_count; x += 1)
				for (int y = 0; y < y_piece_count; y += 1)
					if (pieces[x, y] > 0)
						canvas.DrawBitmap (piece_bitmaps[(int)pieces[x, y]],
							    x_offset + x * piece_size,
							    y_offset + y * piece_size,
							    paint);
		}

	}
	public enum PieceType
	{
		Rojo = 1,
		Amarillo = 2,
		Verde = 3
	}
}