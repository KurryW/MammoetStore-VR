using System;
using System.Collections.Generic;

namespace Tweening
{
    /**
	 * Tweening
	 * Created by Tim Falken
	 * 
	 * Usage:
	 * 1: Create a tween using: Tween newTween = new Tween(ease);
	 *    You can supply an ease function as parameter. 
	 *    Either use a built-in from the Easing class, 
	 *    or make one yourself using for example:
	 *       ...new Tween((t)=> { return t * t });
	 * 
	 * 2: define what you want to do with the tween results:
	 *    newTween.onChange = (t)=>
	 *    {
	 *       Debug.Log(t);
	 *    }
	 * 
	 * 3: Start the tween:
	 *    newTween.Start(from, to, duration);
	 * 
	 * You can optionaly also define an action to 
	 * be performed upon completion;
	 * 
	 *    newTween.onComplete = ()=>
	 *    {
	 *       Debug.Log("Done!");
	 *    };
	 **/

    public class Tween
	{
		public Easing.Ease EaseFunction;
		public TweenOnCompleteEvent onComplete;
		public TweenOnChangeEvent onChange;

		/**
		 * Use to see if the tween is currently active. When the tween completes, this will return to being false.
		 **/
		public bool Active { get; private set; }

		private static List<ActiveTween> _tweens = new List<ActiveTween>();

		/**
		 * Create a new tween with a linear ease function.
		 **/
		public Tween()
		{
			EaseFunction = Easing.Linear;
			Active = false;
		}

		/**
		 * Create a new tween with a given ease function.
		 **/
		public Tween(Easing.Ease ease)
		{
			EaseFunction = ease;
		}

		/**
		 *Start the tween. `from` and `to` will be used as the initial and end value of the tween, respectively.
		 *
		 *The last onChance call is guaranteed to have `t` equal to the given `to` value.
		 *
		 *`time` is measured in seconds.
		 **/
		public void Start(float from, float to, float time)
		{
			ActiveTween newTween = new ActiveTween();

			newTween.tween = this;
			newTween.progress = 0;
			newTween.from = from;
			newTween.to = to;
			newTween.time = time;

			_tweens.Add(newTween);

			Active = true;
		}

		private float Lerp(float t, float from, float to)
		{
			return from + (to - from) * t;
		}

		private void UpdateTween(ActiveTween settings)
		{
			float result = settings.time > 0 ? Lerp(EaseFunction(settings.progress / settings.time), settings.from, settings.to) : settings.to;

			if (settings.progress >= settings.time)
			{
				result = settings.to;
			}

			if (!float.IsNaN(result))
			{
				if (onChange != null)
				{
					onChange(result);
				}
			}
			else
			{
				throw new DivideByZeroException("NaN received from easing function. (Divide by zero?)");
			}

			if (settings.progress >= settings.time)
			{
				Active = false;

				if (onComplete != null)
				{
					onComplete();
				}
			}
		}

		/**
		 * Immediately abort the tween (will NOT execute OnComplete)
		 **/
		public void Abort()
		{
			_tweens.RemoveAll(o => o.tween == this);
		}

		/**
		 * Immediately complete the tween (WILL execute OnComplete)
		 **/
		public void InstantComplete()
		{
			ActiveTween t = _tweens.Find(o => o.tween == this);

			t.CompleteTween();

			_tweens.Remove(t);
		}

		/**
		 * Update all active tweens with the given `deltaTime` value.
		 **/
		public static void UpdateTweens(float deltaTime)
		{
			if (_tweens.Count > 0)
			{
				foreach (ActiveTween tween in _tweens.FindAll(o => o.progress >= o.time))
				{
					try
					{
						tween.CompleteTween();
					}
					catch (Exception)
					{
						tween.tween.Abort();
					}
				}

				_tweens.RemoveAll(o => o.progress >= o.time);

				foreach (ActiveTween tween in _tweens)
				{
					try
					{
						tween.UpdateTween(deltaTime);
					}
					catch (Exception)
					{
						tween.tween.Abort();
					}
				}
			}
		}

		private class ActiveTween
		{
			public Tween tween;
			public float progress;
			public float from, to, time;

			public void CompleteTween()
			{
				progress = time;
				UpdateTween(0);

				tween = null;
			}

			public void UpdateTween(float deltaTime)
			{
				tween.UpdateTween(this);

				progress += deltaTime;
			}
		}

		public delegate void TweenOnChangeEvent(float t);
		public delegate void TweenOnCompleteEvent();
	}

}
