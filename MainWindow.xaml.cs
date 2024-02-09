using Kursova;
using Kursova.New_Window;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using Microsoft.Win32;
using System.Windows.Markup;
using System.Globalization;

namespace Kursova
{
  /// <summary>
  /// Логика взаимодействия для MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public BindingList<AutoRoad> Data { get; private set; } = new BindingList<AutoRoad>();
    public MainWindow()
    {
      InitializeComponent();
      SizeChanged += MainList_SizeChanged;
    }
    private void MainList_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      double totalWidth = MainList.ActualWidth;

      foreach (var column in ((GridView) MainList.View).Columns)
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
    private void StartProgram(object sender, RoutedEventArgs e)
    {
      Data.Add(new AutoRoad("State", 10, 2, true, true));
      Data.Add(new AutoRoad("Regional", 10, 4, true, true));
      Data.Add(new AutoRoad("Provincial", 10, 4, true, true));
      Data.Add(new AutoRoad("Local", 20, 4, true, true));
      Data.Add(new AutoRoad("Regional", 20, 3, true, true));
      Data.Add(new AutoRoad("Regional", 20, 4, true, true));
      Data.Add(new AutoRoad("Provincial", 15, 9, true, true));
      Data.Add(new AutoRoad("Local", 40, 10, true, true));
      Data.Add(new AutoRoad("State", 40, 6, true, true));

      MainList.ItemsSource = Data;
    }
    private bool IsEmpty()
    {
      if (Data.Count == 0)
      {
        MessageBox.Show("Список пустий.");
        return true;
      }
      return false;
    }
    private void Button_AddElement(object sender, RoutedEventArgs e)
    {
      AddElementWindow addElementWindow = new AddElementWindow();
      bool? isOpen = addElementWindow.ShowDialog();

      if (isOpen == true)
      {
        AutoRoad newEntry = addElementWindow.NewElement;
        Data.Add(newEntry);
      }
      else
      {
        MessageBox.Show("Не вдалось відкрити вікно, щоб добавити дані.\n" +
          "Зверніться до системного адміністратора.");
      }
    }
    private void Button_RemoveSelectedElement(object sender, RoutedEventArgs e)
    {
      if (MainList.SelectedItem != null)
      {
        AutoRoad selectedElement = (AutoRoad) MainList.SelectedItem;

        Data.Remove(selectedElement);
        MainList.Items.Refresh();
      }
      else
      {
        MessageBox.Show("Виберіть елемент для видалення.");
      }
    }
    public void SortDataByLengthForEveryType()
    {
      var mergeSort = new MergeSort<AutoRoad>();

      var sortedData = Data
          .GroupBy(x => x.Type)
          .SelectMany(group => group.OrderBy(x => x.Length))
          .ToList();

      Data.Clear();
      sortedData.ForEach(Data.Add);
    }
    private void Button1_MergeSort(object sender, RoutedEventArgs e)
    {
      if (IsEmpty()) { return; }
      SortDataByLengthForEveryType();
    }
    private void ShowMatchingUsers(WindowForTasks obj,
  List<AutoRoad> matchingUsers, int k, string Error)
    {
      if (k == 0) { MessageBox.Show(Error); }
      else
      {
        obj.ShowList(matchingUsers);
      }
    }
    private void Button2_FindShortestRoadWithMostLanes(object sender, RoutedEventArgs e)
    {
      if (IsEmpty()) { return; }

      var maxLanes = Data.Max(d => d.NumberLanes);
      var minLength = Data.Where(x => x.NumberLanes == maxLanes).Min(x => x.Length);

      var result = Data
          .Where(x => x.NumberLanes == maxLanes && x.Length == minLength)
          .ToList();

      ShowMatchingUsers(new WindowForTasks(), result, result.Count, "Дороги не існує");
    }
    private void Button3_FindRoadsWithDividersAndMultipleLanesGroupedByType(object sender, RoutedEventArgs e)
    {
      if (IsEmpty()) { return; }

      var result = Data
          .Where(x => x.Divider && x.NumberLanes > 2)
          .GroupBy(x => x.Type)
          .SelectMany(group => group.ToList())
          .ToList();

      ShowMatchingUsers(new WindowForTasks(), result, result.Count, "Дороги не існує");
    }
    private void Button4_FindRoadTypeWithLongestLengthAndFootpath(object sender, RoutedEventArgs e)
    {
      if (IsEmpty()) { return; }

      var result = Data
          .Where(x => x.Footpath)
          .Where(x => x.Length == Data.Where(d => d.Footpath).Max(d => d.Length))
          .ToList();

      ShowMatchingUsers(new WindowForTasks(), result, result.Count, "Дороги не існує");
    }
    private void Button5_FindRegionalRoadsWithMostLanesAndPedestrianWalkways(object sender, RoutedEventArgs e)
    {
      if (IsEmpty()) { return; }

      var regionalFootpathRoads = Data
          .Where(x => x.Type == "Regional" && x.Footpath)
          .ToList();

      var result = regionalFootpathRoads
          .Where(x => x.NumberLanes == regionalFootpathRoads.Max(d => d.NumberLanes))
          .ToList();

      ShowMatchingUsers(new WindowForTasks(), result, result.Count, "Дороги не існує");
    }
    private void Button6_CalculateTotalRoadLengthByType(object sender, RoutedEventArgs e)
    {
      if (IsEmpty()) { return; }

      var totalLengthByType = Data
          .GroupBy(x => x.Type)
          .Select(group => new
          {
            RoadType = group.Key,
            TotalLength = group.Sum(x => x.Length)
          })
          .ToList();

      var message = new StringBuilder();
      message.AppendLine("Загальна протяжність доріг по кожному типу:");
      message.AppendLine(string.Join(Environment.NewLine, totalLengthByType.Select(roadInfo => $"{roadInfo.RoadType}: {roadInfo.TotalLength} км")));

      MessageBox.Show(message.ToString());
    }

    private void DeleteSpaceInString(string [] arrayOfStrings)
    {
      for (int i = 0; i < arrayOfStrings.Length - 3; i++)
      {
        // Видаляємо лише пробіли, якщо це не останній елемент (назва вулиці)
        if (i < arrayOfStrings.Length - 1)
        {
          arrayOfStrings [i] = arrayOfStrings [i].Replace(" ", "");
        }
      }
    }
    private bool IsValidRoadType(string roadType)
    {
      // Перевіряємо, чи тип дороги є допустимим значенням
      string [] validTypes = { "State", "Regional", "Provincial", "Local" };
      return validTypes.Contains(roadType);
    }
    private void MenuItem_Open(object sender, RoutedEventArgs e)
    {
      string path = GetFilePath();
      if (path == null)
      {
        MessageBox.Show("Файл не було вибрано.");
        return;
      }

      BindingList<AutoRoad> tmp = CopyCurrentData();
      Data.Clear();

      try
      {
        ReadDataFromFile(path);
      }
      catch (Exception ex)
      {
        // Відновлення попередніх даних у випадку помилки
        tmp.ToList().ForEach(Data.Add);

        MessageBox.Show($"Помилка при читанні файлу: {ex.Message}\n" +
                        $"Приклад заповнення: State, 120.5, 4, True, False");
      }
    }
    private void ReadDataFromFile(string path)
    {
      if (File.Exists(path) && new FileInfo(path).Length > 0)
      {
        foreach (string line in File.ReadLines(path))
        {
          ProcessFileLine(line);
        }
      }
      else
      {
        MessageBox.Show("Файл порожній або не існує.");
      }
    }
    private void ProcessFileLine(string line)
    {
      string [] parts = line.Split(',');

      if (IsValidRoadData(parts))
      {
        Data.Add(new AutoRoad(parts [0], 
          double.Parse(parts [1], CultureInfo.InvariantCulture), 
          int.Parse(parts [2]), bool.Parse(parts [3]), 
          bool.Parse(parts [4])));
      }
      else
      {
        throw new Exception("Некоректний формат рядка у файлі.");
      }
    }
    private string GetFilePath()
    {
      var fileDialog = new Microsoft.Win32.OpenFileDialog();
      return fileDialog.ShowDialog() == true ? fileDialog.FileName : null;
    }
    private BindingList<AutoRoad> CopyCurrentData()
    {
      var tmp = new BindingList<AutoRoad>();
      Data.ToList().ForEach(tmp.Add);
      return tmp;
    }
    private bool IsValidRoadData(string [] parts)
    {
      return parts.Length == 5 &&
             IsValidRoadType(parts [0]) &&
             double.TryParse(parts [1], NumberStyles.Any, CultureInfo.InvariantCulture, out double length) &&
             int.TryParse(parts [2], out int numberLanes) &&
             bool.TryParse(parts [3], out bool footpath) &&
             bool.TryParse(parts [4], out bool divider);
    }

    private void MenuItem_Save(object sender, RoutedEventArgs e)
    {
      string path = GetFilePath();
      if (path == null)
      {
        MessageBox.Show("Файл не було вибрано.");
        return;
      }

      try
      {
        SaveDataToFile(path);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Помилка при записі у файл: {ex.Message}");
      }
    }

    private void SaveDataToFile(string path)
    {
      File.WriteAllText(path, string.Empty);

      using (StreamWriter file = new StreamWriter(path))
      {
        foreach (var element in Data)
        {
          file.WriteLine($"{element.Type}, {element.Length}, " +
                          $"{element.NumberLanes}, {element.Footpath}, {element.Divider}");
        }
      }
    }
    private void MenuItem_Help(object sender, RoutedEventArgs e)
    {
      MessageBox.Show("Email system admin: Ostap.Tyndyk.PZ.2022@lpnu.ua");
    }
    private void MenuItem_Exit(object sender, RoutedEventArgs e)
    {
      Environment.Exit(0);
    }
  }
}
