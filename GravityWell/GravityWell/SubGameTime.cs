using System;

namespace GravityWell
{
	public class SubGameTime
	{

		public float Seconds;
		public float Milliseconds;

		public SubGameTime (int fps, int seconds)
		{

			Seconds = 1 / (float)fps;
			Milliseconds = Seconds / 1000;

		}
	}
}

