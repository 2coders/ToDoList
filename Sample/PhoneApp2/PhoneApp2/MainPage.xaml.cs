using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PhoneApp2
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const int InitialKeyTime = 225;

        private const int FinalKeyTime = 250;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void aa_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //FrameworkElement container = sender as FrameworkElement;
            FrameworkElement container = bb as FrameworkElement;

                if (container == null)
                {
                    return;
                }

                if (container.Visibility == Visibility.Collapsed)
                {
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
                    container.Visibility = Visibility.Visible;
                }
                else
                {
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
                    itemFold.Completed += itemFold_Completed;
                    
                }

                
        }

        void itemFold_Completed(object sender, EventArgs e)
        {
            FrameworkElement container = bb as FrameworkElement;
            container.Visibility = Visibility.Collapsed;
        }


    }
}