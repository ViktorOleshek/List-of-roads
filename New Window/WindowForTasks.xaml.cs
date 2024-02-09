using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Kursova.New_Window
{
  /// <summary>
  /// Логика взаимодействия для Task2.xaml
  /// </summary>
  public partial class WindowForTasks : Window
  {
    public WindowForTasks()
    {
      InitializeComponent();
      SizeChanged += Task2_SizeChanged;
    }
    public void ShowList(List<AutoRoad> data)
    {
      ListView.ItemsSource = data;
      ShowDialog();
      Close();
    }
    private void Task2_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      double totalWidth = ListView.ActualWidth;

      foreach (var column in ((GridView) ListView.View).Columns)
      {
        if (column.Header.ToString() == "Type")
        {
          column.Width = totalWidth * 0.2;
        }
        else if (column.Header.ToString() == "Length")
        {
          column.Width = totalWidth * 0.3;
        }
        else if (column.Header.ToString() == "Number lanes")
        {
          column.Width = totalWidth * 0.2;
        }
        else if (column.Header.ToString() == "Footpath")
        {
          column.Width = totalWidth * 0.14;
        }
        else if (column.Header.ToString() == "Divider")
        {
          column.Width = totalWidth * 0.14;
        }
      }
    }
    private void Exit(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
