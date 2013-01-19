using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Shapes;
using ToDo.Model;

namespace ToDo.Utils
{
    public class AnimationUtils
    {
        private const int InitialKeyTime = 225;

        private const int FinalKeyTime = 250;

        public const double AnimationHeightHide = 0;

        public static Storyboard GetStoryboard()
        {
            return new Storyboard();
        }


        public static void SetOpacityAnimation(Storyboard storyboard, FrameworkElement container, double to, double clock)
        {
            if (storyboard == null || container == null)
            {
                return;
            }
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            Storyboard.SetTarget(opacityAnimation, container);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            
            Duration opacityDuration = TimeSpan.FromSeconds(clock);
            opacityAnimation.Duration = opacityDuration;
            
            IEasingFunction opacityEasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut, Exponent = 4 };
            opacityAnimation.EasingFunction = opacityEasingFunction;

            opacityAnimation.From = container.Opacity;
            opacityAnimation.To = to;
            
            storyboard.Children.Add(opacityAnimation);
        }

        public static void SetAnyAnimation(Storyboard storyboard, FrameworkElement container, DependencyProperty property,
            double from, double to, double clock)
        {
            if (storyboard == null || container == null || property == null)
            {
                return;
            }

            DoubleAnimation anyAnimation = new DoubleAnimation();
            Storyboard.SetTarget(anyAnimation, container);
            Storyboard.SetTargetProperty(anyAnimation, new PropertyPath(property));

            Duration duration = TimeSpan.FromSeconds(clock);
            anyAnimation.Duration = duration;

            IEasingFunction easingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut, Exponent = 4 };
            anyAnimation.EasingFunction = easingFunction;

            anyAnimation.From = from;
            anyAnimation.To = to;

            storyboard.Children.Add(anyAnimation);

        }

        public static void SetTranslateAnimation(Storyboard storyboard, FrameworkElement container, double from, double to, double clock)
        {
            if (storyboard == null || container == null)
            {
                return;
            }

            TranslateTransform translation = new TranslateTransform();
            container.RenderTransform = translation;

            DoubleAnimation anyAnimation = new DoubleAnimation();
            Storyboard.SetTarget(anyAnimation, translation);
            Storyboard.SetTargetProperty(anyAnimation, new PropertyPath(TranslateTransform.YProperty));

            Duration duration = TimeSpan.FromSeconds(clock);
            anyAnimation.Duration = duration;

            IEasingFunction easingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut, Exponent = 4 };
            anyAnimation.EasingFunction = easingFunction;

            anyAnimation.From = from;
            anyAnimation.To = to;

            storyboard.Children.Add(anyAnimation);
        }

        public static void SetWidthAnimation(Storyboard storyboard, FrameworkElement container, double to, double clock)
        {
            if (storyboard == null || container == null)
            {
                return;
            }

            DoubleAnimation widthAnimation = new DoubleAnimation();
            Storyboard.SetTarget(widthAnimation, container);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));

            Duration duration = TimeSpan.FromSeconds(clock);
            widthAnimation.Duration = duration;

            IEasingFunction easingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut, Exponent = 4 };
            widthAnimation.EasingFunction = easingFunction;

            widthAnimation.From = container.ActualWidth;
            widthAnimation.To = to;

            storyboard.Children.Add(widthAnimation);

            if (to == AnimationHeightHide)
            {
                storyboard.Completed += delegate(object sender, EventArgs e)
                {
                    container.Visibility = Visibility.Collapsed;
                };
            }
            else
            {
                container.Visibility = Visibility.Visible;
            }
        }

        public static void SetHeightAnimation(Storyboard storyboard, FrameworkElement container, double to, double clock)
        {
            if (storyboard == null || container == null)
            {
                return;
            }

            DoubleAnimation heightAnimation = new DoubleAnimation();
            Storyboard.SetTarget(heightAnimation, container);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
            
            Duration duration = TimeSpan.FromSeconds(clock);
            heightAnimation.Duration = duration;
            
            IEasingFunction easingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut, Exponent = 4 };
            heightAnimation.EasingFunction = easingFunction;

            heightAnimation.From = container.ActualHeight;
            heightAnimation.To = to;
            
            storyboard.Children.Add(heightAnimation);

            //if (to == AnimationHeightHide)
            //{
            //    storyboard.Completed += delegate(object sender, EventArgs e)
            //    {
            //        container.Visibility = Visibility.Collapsed;
            //    };
            //}
            //else
            //{
            //    container.Visibility = Visibility.Visible;
            //}
        }
    }
}
