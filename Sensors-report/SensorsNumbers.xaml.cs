using System.Windows;
using System.Windows.Controls;

namespace Sensors_report
{
    /// <summary>
    /// Логика взаимодействия для SensorsNumbers.xaml
    /// </summary>
    public partial class SensorsNumbers
    {
        TextBox[] windowBoxes;
        public bool sensorsNumbersBoxesFilled = false;
        public SensorsNumbers(int count, ref TextBox[] sensorsNumbersBoxesParam)
        {
            InitializeComponent();
            TextBox[] boxes = new TextBox[count];
            int marginTop = 20;
            for(int i = 0; i < count; i++)
            {
                boxes[i] = new TextBox();
                boxes[i].VerticalAlignment = VerticalAlignment.Top;
                boxes[i].HorizontalAlignment = HorizontalAlignment.Center;
                boxes[i].Width = 150;
                boxes[i].Height = 20;
                boxes[i].Margin = new Thickness(0, marginTop, 0, 0);
                boxes[i].Name = "Sensors_Numbers_" + i;
                marginTop += 40;

                if(Height < 500) Height += 40; // this window's height

                MainGrid.Children.Add(boxes[i]);
            }
            CreateOrder.Margin = new Thickness(0, marginTop+20, 0, 30);
            sensorsNumbersBoxesParam = windowBoxes = boxes;
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            bool areAllBoxesFilled = true;
            foreach(TextBox box in windowBoxes)
            {
                if(box.Text == null || box.Text == "")
                {
                    areAllBoxesFilled = false;
                    break;
                }
            }
            if (areAllBoxesFilled)
            {
                sensorsNumbersBoxesFilled = true;
                Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля", "", MessageBoxButton.OK);
            }
        }
    }
}
