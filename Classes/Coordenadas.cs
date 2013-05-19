
using System;
using System.Collections.Generic;

namespace Snake
{

	public class Coordenadas
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Coordenadas (int x, int y)
		{
			X = x;
			Y = y;
		}

		public override bool Equals (object obj)
		{
			Coordenadas other = (Coordenadas)obj;

			return X == other.X && Y == other.Y;
		}

		public override String ToString ()
		{
			return string.Format ("Coordenadas: [{0}, {1}]", X, Y);
		}


		public static int[] ListToArray (List<Coordenadas> list)
		{
			List<int> array = new List<int> ();

			foreach (Coordenadas c in list) {
				array.Add (c.X);
				array.Add (c.Y);
			}

			return array.ToArray ();
		}


		public static List<Coordenadas> ArrayToList (int[] array)
		{
			List<Coordenadas> list = new List<Coordenadas> ();

			for (int index = 0; index < array.Length; index += 2)
				list.Add (new Coordenadas (array[index], array[index + 1]));

			return list;
		}
	}
}
