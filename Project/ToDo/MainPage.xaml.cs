﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using ToDo.ViewModel;
using ToDo.Model;
using ToDo.Controls;

namespace ToDo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private UIElement shownButtons = null;

        public MainPage()
        {
            InitializeComponent();

            this.DataContext = App.ViewModel;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (newTodoItem.Text.Length > 0)
            {
                ToDoItem item = new ToDoItem();
                item.Title = newTodoItem.Text;
                item.CreateTime = DateTime.Now;
                item.RemindTime = DateTime.Now;
                item.IsCompleted = false;
                item.Note = "";
                item.Priority = 0;

                App.ViewModel.AddToDoItem(item);
                newTodoItem.Text = "";

                TileModel.updateTile();
            }
            
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (shownButtons != null)
            {
                shownButtons.Visibility = System.Windows.Visibility.Collapsed;
            }
            FrameworkElement tbx = sender as FrameworkElement;
            StackPanel parent = tbx.Parent as StackPanel;
            if ((shownButtons != parent.Children[1]) && ((shownButtons = parent.Children[1]) != null))
            {
                shownButtons.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                shownButtons = null;
            }
        }

        private void TodayTitle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (todayToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            {
                todayToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                todayToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void TomorrowTitle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (tomorrowToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            {
                tomorrowToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                tomorrowToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void LaterTitle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (laterToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            {
                laterToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                laterToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ScrollViewer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow.ShowWindow(new NoteControl());
        }

        private void FlagButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel panel = (sender as FrameworkElement).Parent as StackPanel;
            panel = panel.Parent as StackPanel;
            TextBlock flagTextBlock = (panel.Children[0] as StackPanel).Children[0] as TextBlock;
            flagTextBlock.Text = (flagTextBlock.Text == "0") ? "1" : "0";

            App.ViewModel.SaveChangesToDB();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {

        }

        private void showNewNote(object sender, EventArgs e)
        {
            PopupWindow.ShowWindow(new NoteControl());
        }
    }
}