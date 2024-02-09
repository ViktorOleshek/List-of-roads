using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Kursova
{
  /// <summary>
  /// Логика взаимодействия для AddElementWindow.xaml
  /// </summary>
  public partial class AddElementWindow : Window
  {
    public AutoRoad NewElement
    {
      get; private set;
    }
    public AddElementWindow()
    {
      InitializeComponent();
    }
    private bool? GetComboBoxValue(ComboBox comboBox)
    {
      ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

      if (selectedItem != null)
      {
        string content = selectedItem.Content.ToString();

        // Перевірка на True або False та повернення відповідного значення bool.
        return content.Equals("True", StringComparison.OrdinalIgnoreCase);
      }

      return null; //якщо нічого не вибрано.
    }
    private string GetSelectedType(ComboBox comboBox)
    {
      if (comboBox.SelectedItem != null)
      {
        ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

        if (selectedItem != null)
        {
          return selectedItem.Content.ToString();
        }
      }

      return string.Empty;
    }
    private void Button_SaveData(object sender, RoutedEventArgs e)
    {
      string type = GetSelectedType(comboBox_Type);
      string lengthText = TextBox_Length.Text;
      string numberLanesText = TextBox_NumberLanes.Text;
      bool? NullableFootpath = GetComboBoxValue(comboBox_Footpath);
      bool? NullableDivider = GetComboBoxValue(comboBox_Diveder);

      if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(lengthText) ||
  string.IsNullOrEmpty(numberLanesText) || NullableFootpath == null ||
  NullableDivider == null)
      {
        MessageBox.Show("Всі поля повинні бути заповнені");
        return;
      }

      bool footpath = NullableFootpath ?? false;
      bool divider = NullableDivider ?? false;
      if (double.TryParse(lengthText, out double length) &&
          int.TryParse(numberLanesText, out int numberLines))
      {
        NewElement = new AutoRoad(type, length, numberLines, footpath, divider);
        DialogResult = true;
      }
      else
      {
        MessageBox.Show("Некоректний номер телефону");
      }
    }
  }
}
