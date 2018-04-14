using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sensors_report
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string connectStr;
        public MainWindow()
        {
            connectStr = @"Database = sensors_info; Data Source = localhost; User Id = root; Password =";
            InitializeComponent();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            connectStr = @"Database = sensors_info; Data Source = " + hostBox.Text + ";" +
                "User Id = " + loginBox.Text + "; Password = " + passwordBox.Password;
            SensorsGridUpdate();
        }

        private void SensorsGridUpdate()
        {
            MySqlLib.MySqlData.MySqlExecuteData.MyResultData result = new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
            result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset("SELECT * FROM sensors_data", connectStr);
            if (result.HasError == false)
            {
                sensors_data_grid.ItemsSource = result.ResultData.DefaultView;
            }
            else
            {
                MessageBox.Show(result.ErrorText);
            }
        }

        private void Table_Update(object sender, RoutedEventArgs e)
        {
            SensorsGridUpdate();
        }

        private void Table_Edit_Column(object sender, RoutedEventArgs e)
        {

        }

        private void Table_Delete_Column(object sender, RoutedEventArgs e)
        {
            string key = ((DataRowView)sensors_data_grid.SelectedItems[0]).Row["number_of_sensor"].ToString();
            string SQLQuery = "DELETE FROM `sensors_info`.`sensors_data` ";
            SQLQuery += "WHERE `sensors_data`.`number_of_sensor` = '" + key + "'";
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выполнить запрос?\n" + SQLQuery,
                "", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if(result == MessageBoxResult.OK)
            {
                MySqlLib.MySqlData.MySqlExecute.SqlNoneQuery(SQLQuery, connectStr);
                SensorsGridUpdate();
            }
        }
    }
}
