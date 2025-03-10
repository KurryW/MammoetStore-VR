using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweening
{
	/**
	 * Easing class
	 * Tim Falken
	 * For use with the Tween class
	 * */

    public class Easing
    {
		/**
		 *Delegate function used for making eases.
		 *Created as, for example: 
		 *  (t) =>
		 *  { 
		 *   return t;
		 *  };
		 **/
		public delegate float Ease(float t);

		/**
		 * Experimental
		 * Chains eases to play one after another
		 **/
		public static Ease Combine(Ease[] eases)
		{
			return (t) =>
			{
				if (t == 0)
				{
					return 0;
				}

				int index = (int)Math.Round(t * (eases.Length - 1));

				float stepSize = 1.0f / (float)eases.Length;

				float t2 = t % stepSize;

				float step = eases[index](t2 / stepSize) / (float)eases.Length;

				return ((float)index / (float)eases.Length) + step;
			};
		}

		public static Ease Merge(Ease ease1, Ease ease2)
		{
			return (t) =>
			{
				if (t == 0)
				{
					return 0;
				}

				float r1 = ease1(t);
				float r2 = ease2(t);

				return r1 + ((r2 - r1) * t);
			};
		}

		public static Ease Linear
		{
			get
			{
				return (t) =>
				{
					return t;
				};
			}
		}

		public static Ease EaseInQuad
		{
			get
			{
				return (t) =>
				{
					return t * t;
				};
			}
		}

		public static Ease EaseOutQuad
		{
			get
			{
				return (t) =>
				{
					return t * (2 - t);
				};
			}
		}

		public static Ease EaseInOutQuad
		{
			get
			{
				return (t) =>
				{
					if ((t /= 0.5f) < 1)
					{
						return 0.5f * t * t;
					}

					return -0.5f * ((--t) * (t - 2) - 1);
				};
			}
		}

		public static Ease EaseInCubic
		{
			get
			{
				return (t) =>
				{
					return t * t * t;
				};
			}
		}

		public static Ease EaseOutCubic
		{
			get
			{
				return (t) =>
				{
					return (--t) * t * t + 1;
				};
			}
		}

		public static Ease EaseInOutCubic
		{
			get
			{
				return (t) =>
				{
					return t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
				};
			}
		}

		public static Ease EaseInElastic
		{
			get
			{
				return (t) =>
				{
					if (t == 0)
					{
						return 0;
					}

					return (t == 0 ? 0 : (0.04f - 0.04f / t)) * (float)Math.Sin(25 * t) + 1;
				};
			}
		}

		public static Ease EaseOutElastic
		{
			get
			{
				return (t) =>
				{
					if (t == 1)
					{
						return 1;
					}

					return 0.04f * t / (--t) * (float)Math.Sin(25 * t);
				};
			}
		}

		public static Ease EaseInOutElastic
		{
			get
			{
				return (t) =>
				{
					if (t == 0)
					{
						return 0;
					}

					return (t -= 0.5f) < 0 ? (0.02f + 0.01f / t) * (float)Math.Sin(50 * t) : (0.02f - 0.01f / t) * (float)Math.Sin(50 * t) + 1;
				};
			}
		}

		public static Ease EaseInBack
		{
			get
			{
				return (t) =>
				{
					float s = 1.70158f;
					return t * t * ((s + 1) * t - s);
				};
			}
		}

		public static Ease EaseOutBack
		{
			get
			{
				return (t) =>
				{
					float s = 1.70158f;
					return ((t -= 1) * t * ((s + 1) * t + s) + 1);
				};
			}
		}

		public static Ease EaseInOutBack
		{
			get
			{
				return (t) =>
				{
					float s = 1.70158f;

					if ((t /= 0.5f) < 1)
					{
						return 0.5f * (t * t * (((s *= 1.525f) + 1) * t - s));
					}

					return 0.5f * ((t -= 2.0f) * t * (((s *= 1.525f) + 1.0f) * t + s) + 2.0f);
				};
			}
		}

		public static Ease EaseInBounce
		{
			get
			{
				return (t) =>
				{
					return 1 - EaseOutBounce(1 - t);
				};
			}
		}

		public static Ease EaseOutBounce
		{
			get
			{
				return (t) =>
				{
					if (t < (1 / 2.75f))
					{
						return (7.5625f * t * t);
					}
					else if (t < (2 / 2.75f))
					{
						return (7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f);
					}
					else if (t < (2.5f / 2.75f))
					{
						return (7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f);
					}
					else
					{
						return (7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f);
					}
				};
			}
		}

		public static Ease EaseInOutBounce
		{
			get
			{
				return (t) =>
				{
					if (t < 0.5f)
					{
						return EaseInBounce(t * 2) * 0.5f;
					}

					return EaseOutBounce(t * 2 - 1) * 0.5f + 0.5f;
				};
			}
		}

	}
}
