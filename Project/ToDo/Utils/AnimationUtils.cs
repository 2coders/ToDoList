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

        public static void ItemsTranslation(ExpanderView expander, ToDoItem deleteItem, int from, int to)
        {

            if (expander == null || expander.Items.Count == 0)
            {
                return;
            }

            FrameworkElement first = expander.ItemContainerGenerator.ContainerFromIndex(0) as FrameworkElement;
            double height = first.ActualHeight;

            Storyboard storyBoard = new Storyboard();
            IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
            int initialKeyTime = InitialKeyTime;
            int finalKeyTime = FinalKeyTime;

            TranslateTransform translation = new TranslateTransform();
            DoubleAnimationUsingKeyFrames transAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame transKeyFrame_1 = new EasingDoubleKeyFrame();
            transKeyFrame_1.EasingFunction = quadraticEase;
            transKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
            transKeyFrame_1.Value = 0.0;

            EasingDoubleKeyFrame transKeyFrame_2 = new EasingDoubleKeyFrame();
            transKeyFrame_2.EasingFunction = quadraticEase;
            transKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime);
            transKeyFrame_2.Value = 0.0;

            EasingDoubleKeyFrame transKeyFrame_3 = new EasingDoubleKeyFrame();
            transKeyFrame_3.EasingFunction = quadraticEase;
            transKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
            transKeyFrame_3.Value = -50;

            transAnimation.KeyFrames.Add(transKeyFrame_1);
            transAnimation.KeyFrames.Add(transKeyFrame_2);
            transAnimation.KeyFrames.Add(transKeyFrame_3);

            Storyboard.SetTarget(transAnimation, translation);
            Storyboard.SetTargetProperty(transAnimation, new PropertyPath(TranslateTransform.YProperty));


            DoubleAnimationUsingKeyFrames opacityAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame opacityKeyFrame_1 = new EasingDoubleKeyFrame();
            opacityKeyFrame_1.EasingFunction = quadraticEase;
            opacityKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
            opacityKeyFrame_1.Value = 1.0;

            EasingDoubleKeyFrame opacityKeyFrame_2 = new EasingDoubleKeyFrame();
            opacityKeyFrame_2.EasingFunction = quadraticEase;
            opacityKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime - 150);
            opacityKeyFrame_2.Value = 0.5;

            EasingDoubleKeyFrame opacityKeyFrame_3 = new EasingDoubleKeyFrame();
            opacityKeyFrame_3.EasingFunction = quadraticEase;
            opacityKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
            opacityKeyFrame_3.Value = 0;

            opacityAnimation.KeyFrames.Add(opacityKeyFrame_1);
            opacityAnimation.KeyFrames.Add(opacityKeyFrame_2);
            opacityAnimation.KeyFrames.Add(opacityKeyFrame_3);

            Storyboard.SetTarget(opacityAnimation, first);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));

            storyBoard.Children.Add(opacityAnimation);
            storyBoard.Children.Add(transAnimation);

            for (int i = from; i < to; i++)
            {
                FrameworkElement item = expander.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                item.RenderTransform = translation;
            }
            storyBoard.Begin();

            storyBoard.Completed += delegate(object sender, EventArgs e)
            {
                App.ViewModel.DeleteToDoItem(deleteItem);
            };
        }

        public static void Split(ExpanderView expander, FrameworkElement currentView)
        {
            if (expander == null)
            {
                return;
            }

            int count = expander.Items.Count;
            for (int i = 1; i < count; i++)
            {
                FrameworkElement container = expander.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;

                if (container == null)
                {
                    break;
                }

                Storyboard itemDropDown = new Storyboard();
                IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
                int initialKeyTime = 250;
                int finalKeyTime = 500;

                TranslateTransform translation = new TranslateTransform();
                container.RenderTransform = translation;

                DoubleAnimationUsingKeyFrames transAnimation = new DoubleAnimationUsingKeyFrames();

                EasingDoubleKeyFrame transKeyFrame_1 = new EasingDoubleKeyFrame();
                transKeyFrame_1.EasingFunction = quadraticEase;
                transKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
                transKeyFrame_1.Value = 0.0;

                EasingDoubleKeyFrame transKeyFrame_2 = new EasingDoubleKeyFrame();
                transKeyFrame_2.EasingFunction = quadraticEase;
                transKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime);
                transKeyFrame_2.Value = 0.0;

                EasingDoubleKeyFrame transKeyFrame_3 = new EasingDoubleKeyFrame();
                transKeyFrame_3.EasingFunction = quadraticEase;
                transKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
                transKeyFrame_3.Value = 100.0;

                transAnimation.KeyFrames.Add(transKeyFrame_1);
                transAnimation.KeyFrames.Add(transKeyFrame_2);
                transAnimation.KeyFrames.Add(transKeyFrame_3);

                Storyboard.SetTarget(transAnimation, translation);
                Storyboard.SetTargetProperty(transAnimation, new PropertyPath(TranslateTransform.YProperty));
                itemDropDown.Children.Add(transAnimation);

                itemDropDown.Begin();

            }
            currentView.Visibility = Visibility.Visible;
            
        }

        public static void Expand(object sender)
        {
            FrameworkElement container = sender as FrameworkElement;
            if (container == null || container.Visibility == Visibility.Visible)
            {
                return;
            }

            container.Visibility = Visibility.Visible;

            Storyboard itemDropDown = new Storyboard();
            IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
            int initialKeyTime = InitialKeyTime;
            int finalKeyTime = FinalKeyTime;

            TranslateTransform translation = new TranslateTransform();
            container.RenderTransform = translation;

            DoubleAnimationUsingKeyFrames transAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame transKeyFrame_1 = new EasingDoubleKeyFrame();
            transKeyFrame_1.EasingFunction = quadraticEase;
            transKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
            transKeyFrame_1.Value = -150.0;

            EasingDoubleKeyFrame transKeyFrame_2 = new EasingDoubleKeyFrame();
            transKeyFrame_2.EasingFunction = quadraticEase;
            transKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime);
            transKeyFrame_2.Value = 0.0;

            EasingDoubleKeyFrame transKeyFrame_3 = new EasingDoubleKeyFrame();
            transKeyFrame_3.EasingFunction = quadraticEase;
            transKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
            transKeyFrame_3.Value = 0.0;

            transAnimation.KeyFrames.Add(transKeyFrame_1);
            transAnimation.KeyFrames.Add(transKeyFrame_2);
            transAnimation.KeyFrames.Add(transKeyFrame_3);

            Storyboard.SetTarget(transAnimation, translation);
            Storyboard.SetTargetProperty(transAnimation, new PropertyPath(TranslateTransform.YProperty));
            itemDropDown.Children.Add(transAnimation);

            DoubleAnimationUsingKeyFrames opacityAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame opacityKeyFrame_1 = new EasingDoubleKeyFrame();
            opacityKeyFrame_1.EasingFunction = quadraticEase;
            opacityKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
            opacityKeyFrame_1.Value = 0.0;

            EasingDoubleKeyFrame opacityKeyFrame_2 = new EasingDoubleKeyFrame();
            opacityKeyFrame_2.EasingFunction = quadraticEase;
            opacityKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime - 150);
            opacityKeyFrame_2.Value = 0.0;

            EasingDoubleKeyFrame opacityKeyFrame_3 = new EasingDoubleKeyFrame();
            opacityKeyFrame_3.EasingFunction = quadraticEase;
            opacityKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
            opacityKeyFrame_3.Value = 1.0;

            opacityAnimation.KeyFrames.Add(opacityKeyFrame_1);
            opacityAnimation.KeyFrames.Add(opacityKeyFrame_2);
            opacityAnimation.KeyFrames.Add(opacityKeyFrame_3);

            Storyboard.SetTarget(opacityAnimation, container);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            itemDropDown.Children.Add(opacityAnimation);

            itemDropDown.Begin();
            
        }

        public static void Fold(object sender)
        {
            FrameworkElement container = sender as FrameworkElement;
            if (container == null || container.Visibility == Visibility.Collapsed)
            {
                return;
            }

            Storyboard itemFold = new Storyboard();
            IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
            int initialKeyTime = InitialKeyTime;
            int finalKeyTime = FinalKeyTime;

            TranslateTransform translation = new TranslateTransform();
            container.RenderTransform = translation;

            DoubleAnimationUsingKeyFrames transAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame transKeyFrame_1 = new EasingDoubleKeyFrame();
            transKeyFrame_1.EasingFunction = quadraticEase;
            transKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
            transKeyFrame_1.Value = 0.0;

            EasingDoubleKeyFrame transKeyFrame_2 = new EasingDoubleKeyFrame();
            transKeyFrame_2.EasingFunction = quadraticEase;
            transKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime);
            transKeyFrame_2.Value = 0.0;

            EasingDoubleKeyFrame transKeyFrame_3 = new EasingDoubleKeyFrame();
            transKeyFrame_3.EasingFunction = quadraticEase;
            transKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
            transKeyFrame_3.Value = -150.0;

            transAnimation.KeyFrames.Add(transKeyFrame_1);
            transAnimation.KeyFrames.Add(transKeyFrame_2);
            transAnimation.KeyFrames.Add(transKeyFrame_3);

            Storyboard.SetTarget(transAnimation, translation);
            Storyboard.SetTargetProperty(transAnimation, new PropertyPath(TranslateTransform.YProperty));
            itemFold.Children.Add(transAnimation);

            DoubleAnimationUsingKeyFrames opacityAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame opacityKeyFrame_1 = new EasingDoubleKeyFrame();
            opacityKeyFrame_1.EasingFunction = quadraticEase;
            opacityKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
            opacityKeyFrame_1.Value = 1.0;

            EasingDoubleKeyFrame opacityKeyFrame_2 = new EasingDoubleKeyFrame();
            opacityKeyFrame_2.EasingFunction = quadraticEase;
            opacityKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime - 150);
            opacityKeyFrame_2.Value = 0.5;

            EasingDoubleKeyFrame opacityKeyFrame_3 = new EasingDoubleKeyFrame();
            opacityKeyFrame_3.EasingFunction = quadraticEase;
            opacityKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
            opacityKeyFrame_3.Value = 0.0;

            opacityAnimation.KeyFrames.Add(opacityKeyFrame_1);
            opacityAnimation.KeyFrames.Add(opacityKeyFrame_2);
            opacityAnimation.KeyFrames.Add(opacityKeyFrame_3);

            Storyboard.SetTarget(opacityAnimation, container);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            itemFold.Children.Add(opacityAnimation);

            itemFold.Begin();
            // 动画结束时隐藏
            itemFold.Completed += delegate(object o, EventArgs e)
            {
                container.Visibility = Visibility.Collapsed;
            };
        }

        public static void LineTranslate(object sender)
        {
            Line container = sender as Line;
            if (container == null)
            {
                return;
            }

            Storyboard storyBoard = new Storyboard();
            IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
            int initialKeyTime = InitialKeyTime;
            int finalKeyTime = 500;

            TranslateTransform translation = new TranslateTransform();
            container.RenderTransform = translation;

            DoubleAnimationUsingKeyFrames transAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame transKeyFrame_1 = new EasingDoubleKeyFrame();
            transKeyFrame_1.EasingFunction = quadraticEase;
            transKeyFrame_1.KeyTime = TimeSpan.FromMilliseconds(0.0);
            transKeyFrame_1.Value = -300.0;

            EasingDoubleKeyFrame transKeyFrame_2 = new EasingDoubleKeyFrame();
            transKeyFrame_2.EasingFunction = quadraticEase;
            transKeyFrame_2.KeyTime = TimeSpan.FromMilliseconds(initialKeyTime);
            transKeyFrame_2.Value = 0.0;

            EasingDoubleKeyFrame transKeyFrame_3 = new EasingDoubleKeyFrame();
            transKeyFrame_3.EasingFunction = quadraticEase;
            transKeyFrame_3.KeyTime = TimeSpan.FromMilliseconds(finalKeyTime);
            transKeyFrame_3.Value = 0.0;

            transAnimation.KeyFrames.Add(transKeyFrame_1);
            transAnimation.KeyFrames.Add(transKeyFrame_2);
            transAnimation.KeyFrames.Add(transKeyFrame_3);

            Storyboard.SetTarget(transAnimation, translation);
            Storyboard.SetTargetProperty(transAnimation, new PropertyPath(TranslateTransform.XProperty));
            storyBoard.Children.Add(transAnimation);

            storyBoard.Begin();
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
    }
}
