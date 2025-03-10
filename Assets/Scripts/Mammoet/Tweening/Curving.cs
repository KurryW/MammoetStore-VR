using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweening
{
	/**
	 * Curving class
	 * Tim Falken
	 * 
	 * Can lerp points through a Bezier-like curve
	 */

	public class Curving
	{
		public static double Lerp(double[] points, double t)
		{
			if (points.Length == 0)
			{
				return 0;
			}
			else if (points.Length == 1)
			{
				return points[0];
			}
			else if (points.Length == 2)
			{
				return Lerping.Lerp(points[0], points[1], t);
			}
			else
			{
				double[] newPoints = new double[points.Length - 1];

				for (int i = 0; i < newPoints.Length; i++)
				{
					newPoints[i] = Lerping.Lerp(points[i], points[i + 1], t);
				}

				return Lerp(newPoints, t);
			}
		}

		public static float Lerp(float[] points, float t)
		{
			if (points.Length == 0)
			{
				return 0;
			}
			else if (points.Length == 1)
			{
				return points[0];
			}
			else if (points.Length == 2)
			{
				return Lerping.Lerp(points[0], points[1], t);
			}
			else
			{
				float[] newPoints = new float[points.Length - 1];

				for (int i = 0; i < newPoints.Length; i++)
				{
					newPoints[i] = Lerping.Lerp(points[i], points[i + 1], t);
				}

				return Lerp(newPoints, t);
			}
		}

		//Hetzelfde als Lerp() voor floats, maar met elke class dir ICurvable implementeerd.
		public static ICurvable Lerp(ICurvable[] points, double t)
		{
			if (points.Length == 0)
			{
				throw new ArgumentNullException("Given points array is empty!");
			}
			else if (points.Length == 1)
			{
				return points[0];
			}
			else if (points.Length == 2)
			{
				return points[0].Add(points[1].Subtract(points[0]).Multiply(t));
			}
			else
			{
				ICurvable[] newPoints = new ICurvable[points.Length - 1];

				for (int i = 0; i < newPoints.Length; i++)
				{
					newPoints[i] = points[i].Add(points[i + 1].Subtract(points[i]).Multiply(t));
				}

				return Lerp(newPoints, t);
			}
		}

		//Hetzelfde als Lerp() voor floats, maar met elke class dir ICurvable implementeerd.
		public static ICurvable Lerp(ICurvable[] points, float t)
		{
			if (points.Length == 0)
			{
				throw new ArgumentNullException("Given points array is empty!");
			}
			else if (points.Length == 1)
			{
				return points[0];
			}
			else if (points.Length == 2)
			{
				return points[0].Add(points[1].Subtract(points[0]).Multiply(t));
			}
			else
			{
				ICurvable[] newPoints = new ICurvable[points.Length - 1];

				for (int i = 0; i < newPoints.Length; i++)
				{
					newPoints[i] = points[i].Add(points[i + 1].Subtract(points[i]).Multiply(t));
				}

				return Lerp(newPoints, t);
			}
		}

		//Implementeer deze interface om een custom datatype te laten curven.
		public interface ICurvable
		{
			ICurvable Add(ICurvable other);
			ICurvable Subtract(ICurvable other);
			ICurvable Multiply(float factor);
			ICurvable Multiply(double factor);
		}
	}

}
