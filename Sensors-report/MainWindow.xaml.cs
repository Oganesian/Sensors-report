using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;

namespace Sensors_report
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string connectStr;
        TextBox[] sensorsNumbersBoxes;
        SensorsNumbers window;
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
            if (sensors_data_grid.SelectedItem != null)
            {
                tabControl.SelectedIndex = 1;
                AddOrChangeComboBox.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("Выберите строку, которую хотите редактировать", "", MessageBoxButton.OK);
            }
        }

        private void Table_Delete_Column(object sender, RoutedEventArgs e)
        {
            if (sensors_data_grid.SelectedItem != null)
            {
                string key = ((DataRowView)sensors_data_grid.SelectedItems[0]).Row["number_of_sensor"].ToString();
                string SQLQuery = "DELETE FROM `sensors_info`.`sensors_data` ";
                SQLQuery += "WHERE `sensors_data`.`number_of_sensor` = '" + key + "'";
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите выполнить запрос?\n" + SQLQuery,
                    "", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.OK)
                {
                    MySqlLib.MySqlData.MySqlExecute.SqlNoneQuery(SQLQuery, connectStr);
                    SensorsGridUpdate();
                }
            }
            else
            {
                MessageBox.Show("Выберите строку, которую хотите удалить", "", MessageBoxButton.OK);
            }
        }

        private void Add_Or_Change_Column(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "Добавить")
            {
                AddData();
            }
            else
            {
                EditData();
            }
        }

        private bool AreAllBoxesFilled(TextBox[] textBoxes, DatePicker datePicker)
        {
            bool areAllBoxesFilled = true;

            if (datePicker.Text != null && datePicker.Text != "")
            {
                foreach (TextBox box in textBoxes)
                {
                    if (box.Text == null || box.Text == "")
                    {
                        areAllBoxesFilled = false;
                        break;
                    }
                }
            }
            else
            {
                areAllBoxesFilled = false;
            }
            return areAllBoxesFilled;
        }

        private void AddData()
        {
            TextBox[] textBoxes = {NumberOfSensor, NameOfSensor, TypeOfSensor,
                                  Value_A, Value_B, Value_C, Value_D,
                                  Zero_value };

            if (AreAllBoxesFilled(textBoxes, Expiration_date))
            {
                string SQLQuery = "INSERT INTO sensors_data VALUES(";
                foreach (TextBox box in textBoxes)
                {
                    SQLQuery += "'" + box.Text + "', ";
                }
                SQLQuery += "'" + CSharpDateToSQLDate(Expiration_date.Text) + "')";

                SqlNoneQuery(SQLQuery);

                foreach (TextBox box in textBoxes) box.Text = null;
                Expiration_date.Text = null;

            }
            else
            {
                MessageBox.Show("Заполните все поля", "Некоторые поля не заполнены", MessageBoxButton.OK);
            }
        }

        private void SqlNoneQuery(string query)
        {
            MySqlLib.MySqlData.MySqlExecute.MyResult result =
                MySqlLib.MySqlData.MySqlExecute.SqlNoneQuery(query, connectStr);

            if (result.HasError == false)
            {
                SensorsGridUpdate();
            }
            else
            {
                MessageBox.Show(result.ErrorText);
            }
        }

        public string CSharpDateToSQLDate(string CSharpDate) // CSharpDate looks like 'dd.mm.yyyy'
        {                                                    // to enter into the table, 
                                                             // it is necessary to parse it to the SQL format
            return CSharpDate.Substring(6) + "-" +           // 'yyyy-mm-dd'
                CSharpDate.Substring(3, 2) + "-" +
                CSharpDate.Substring(0, 2);
        }

        private void EditData()
        {
            string SQLQuery = "UPDATE sensors_info.sensors_data SET ";
            TextBox[] textBoxes = {NumberOfSensor, NameOfSensor, TypeOfSensor,
                                  Value_A, Value_B, Value_C, Value_D,
                                  Zero_value };
            string[] attributesHeaders = { "number_of_sensor", "name_of_sensor", "value_a", "value_b",
                "value_c", "value_d", "zero_value", "expiration_date"};
            int index = 0;

            foreach (TextBox box in textBoxes)
            {
                if (box.Text != null && box.Text != "")
                {
                    SQLQuery += attributesHeaders[index] + " = '" + box.Text + "', ";
                }
                index++;
            }
            if (Expiration_date.Text != null && Expiration_date.Text != "")
            {
                SQLQuery += attributesHeaders[index] + " = '" + Expiration_date.Text + "', ";
            }
            SQLQuery.Remove(SQLQuery.Length - 1 - ", ".Length);
            SQLQuery += " WHERE sensors_data.number_of_sensor = " + KeyNumberOfSensor.Text;
            MessageBox.Show(SQLQuery);
            SqlNoneQuery(SQLQuery);

            foreach (TextBox box in textBoxes) box.Text = null;
            Expiration_date.Text = null;
        }

        private void AddOrChangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddOrChange != null)
            {
                switch (((ComboBox)sender).SelectedIndex)
                {
                    case 0: // Add
                        AddOrChange.Content = "Добавить";
                        KeyNumberOfSensor.Visibility = Visibility.Collapsed;
                        AddOrChangeTab.Header = "Добавить запись";
                        break;
                    case 1: // Edit
                        AddOrChange.Content = "Изменить";
                        KeyNumberOfSensor.Visibility = Visibility.Visible;
                        AddOrChangeTab.Header = "Изменить запись";
                        break;
                }
            }

        }

        private void PreviewTextInputDigit(object sender, TextCompositionEventArgs e)
        {
            if (!System.Char.IsDigit(e.Text, 0)) e.Handled = true; // Check if the input char is digit
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] textBoxes = { Department, Stand, TestNumber, Unit, NumberOfSensors, FolderPath };
            if (AreAllBoxesFilled(textBoxes, DateP))
            {
                int.TryParse(NumberOfSensors.Text, out int sensorsNumbers);
                window = new SensorsNumbers(sensorsNumbers, ref sensorsNumbersBoxes);
                window.Closed += Window_Closed;
                window.Show();
            }
            else
            {
                MessageBox.Show("Заполните все поля", "", MessageBoxButton.OK);
            }
        }

        private void Window_Closed(object sender, System.EventArgs e) // SensorsNumbers window
        {
            if (window.sensorsNumbersBoxesFilled)
            {
                if (!AreThereIncorrectNumbers())
                {
                    using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Create(FolderPathAndName(), WordprocessingDocumentType.Document))
                    {
                        // Main
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        // Head
                        Paragraph paragraph = body.AppendChild(new Paragraph());
                        body.AppendChild(new Paragraph()); // new line
                        ParagraphProperties paragraphPr = new ParagraphProperties();
                        paragraphPr.Justification = new Justification() { Val = JustificationValues.Center };
                        paragraph.Append(paragraphPr);
                        Run run = paragraph.AppendChild(new Run());
                        RunProperties runPr = new RunProperties();
                        runPr.Bold = new Bold();
                        runPr.FontSize = new FontSize() { Val = "32" };
                        runPr.RunFonts = new RunFonts()
                        {
                            Ascii = "Times New Roman",
                            ComplexScript = "Times New Roman",
                            HighAnsi = "Times New Roman"
                        };
                        run.Append(runPr);
                        run.AppendChild(new Text("Задание на обработку"));

                        // Table_1
                        Table table_1 = CustomTable_1();
                        body.Append(table_1);
                        body.AppendChild(new Paragraph()); // new line

                        // Table_2
                        Table table_2 = CustomTable_2();
                        body.Append(table_2);
                        body.AppendChild(new Paragraph()); // new line

                        // Paragraph
                        body.AppendChild(SensorsCoeffitientsP());

                        // Table_3
                        Table table_3 = CustomTable_3();
                        body.Append(table_3);

                        // End
                        mainPart.Document.Save();
                        wordDocument.Close();

                    }
                    Department.Text = Stand.Text = DateP.Text = TestNumber.Text =
                        NumberOfSensors.Text = FolderPath.Text = Unit.Text = null;
                }
                else
                {
                    MessageBox.Show("Среди указанных вами номеров датчиков есть номер, "+
                        "которого не удалось найти в базе данных", "Указан неверный номер", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Вы не заполнили поля, поэтому отчет не будет создан", "", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Checks if there are incorrect numbers among the sensor numbers
        /// </summary>
        private bool AreThereIncorrectNumbers()
        {
            int.TryParse(NumberOfSensors.Text, out int numberOfSensors);
            MySqlLib.MySqlData.MySqlExecuteData.MyResultData result =
                new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
            for (int i = 0; i < numberOfSensors; i++)
            {
                result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(
                    "SELECT number_of_sensor FROM sensors_data WHERE number_of_sensor = '" +
                        sensorsNumbersBoxes[i].Text + "'", connectStr);
                try
                {
                    bool isResultEmpty = result.ResultData.Rows[0] == null;
                }
                catch (System.IndexOutOfRangeException)
                {
                    return true;
                }
            }
            return false;
        }

        private string FolderPathAndName()
        {
            string folderPathAndName = "";
            folderPathAndName += FolderPath.Text;
            if(!folderPathAndName.EndsWith(@"\")) folderPathAndName += @"\";
            folderPathAndName += "Report of " + DateP.Text + ".docx";
            return folderPathAndName;
        }

        private TableProperties StandardTableProperties()
        {
            var Value = new EnumValue<BorderValues>(BorderValues.Birds);

            return new TableProperties(
                new TableBorders(new TopBorder() { Val = Value, Size = 2 },
                    new BottomBorder() { Val = Value, Size = 2 },
                    new LeftBorder() { Val = Value, Size = 2 },
                    new RightBorder() { Val = Value, Size = 2 },
                    new InsideHorizontalBorder() { Val = Value, Size = 2 },
                    new InsideVerticalBorder() { Val = Value, Size = 2 }),
                new TableWidth() { Width = "10000" },
                new Justification() { Val = JustificationValues.Center }
            );
        }

        private Table CustomTable_1()
        {
            Table table = new Table();
            table.Append(StandardTableProperties());
            TableRow[] tableRows = new TableRow[2];
            tableRows[0] = new TableRow();
            tableRows[1] = new TableRow();
            TableCell[] tableCells = new TableCell[5];
            Paragraph[] paragraphs = new Paragraph[5];
            Run[] runs = new Run[5];
            Text[] texts = { new Text("Отдел: "), new Text("Стенд: "), new Text("Дата: "),
                new Text("Номер испытания: "), new Text("Агрегат: ")};
            string[] values = { Department.Text, Stand.Text,
                    DateP.Text, TestNumber.Text, Unit.Text};

            for (int i = 0; i < 5; i++)
            {
                runs[i] = new Run();
                paragraphs[i] = new Paragraph();
                tableCells[i] = new TableCell();
                texts[i].Text += values[i];
                runs[i].Append(texts[i]);
                paragraphs[i].Append(runs[i]);
                tableCells[i].Append(paragraphs[i]);
                if (i < 3)
                    tableRows[0].Append(tableCells[i]);
                else
                    tableRows[1].Append(tableCells[i]);
            }
            table.Append(tableRows[0]);
            table.Append(tableRows[1]);

            return table;
        }

        private Table CustomTable_2()
        {
            Table table = new Table();
            const int CELLS_IN_A_ROW = 6;
            table.Append(StandardTableProperties());
            int.TryParse(NumberOfSensors.Text, out int numberOfSensors);

            TableRow[] tableRows = new TableRow[numberOfSensors];
            TableCell[,] tableCells = new TableCell[numberOfSensors, CELLS_IN_A_ROW]; // 6 cells in a row
            Paragraph[,] paragraphs = new Paragraph[numberOfSensors, CELLS_IN_A_ROW];
            Run[,] runs = new Run[numberOfSensors, CELLS_IN_A_ROW];

            string[] headers = { "№", "Номер датчика", "Имя датчика", "Тип датчика", "Срок годности", "F0" };
            table.Append(Table_Headers(ref headers)); // set headers (1st row) to the table

            string[] columnsNames = {"number_of_sensor", "name_of_sensor",
            "type_of_sensor", "expiration_date", "zero_value" };

            for (int i = 0; i < numberOfSensors; i++)
            {
                MySqlLib.MySqlData.MySqlExecuteData.MyResultData result =
                    new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
                result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(
                    "SELECT number_of_sensor, name_of_sensor, type_of_sensor, expiration_date, zero_value " +
                    "FROM sensors_data WHERE number_of_sensor = '" + sensorsNumbersBoxes[i].Text + "'", connectStr);

                if (result.HasError == false)
                {
                    tableRows[i] = new TableRow();

                    string[] values = {
                        result.ResultData.Rows[0].Field<string>(0),
                        result.ResultData.Rows[0].Field<string>(1),
                        result.ResultData.Rows[0].Field<string>(2)
                    };

                    DateTime dateTime = result.ResultData.Rows[0].Field<DateTime>(3);
                    double zero_value = result.ResultData.Rows[0].Field<double>(4);

                    for (int k = 0; k < CELLS_IN_A_ROW; k++)
                    {
                        runs[i, k] = new Run();
                        paragraphs[i, k] = new Paragraph();
                        tableCells[i, k] = new TableCell();
                        if (k == 0) runs[i, k].Append(new Text((k + 1).ToString()));
                        else if (k == 4) runs[i, k].Append(new Text(dateTime.ToString().Substring(0, 10)));
                        else if (k == 5) runs[i, k].Append(new Text(zero_value.ToString()));
                        else runs[i, k].Append(new Text(values[k - 1]));

                        paragraphs[i, k].Append(runs[i, k]);
                        tableCells[i, k].Append(paragraphs[i, k]);
                        tableRows[i].Append(tableCells[i, k]);
                    }
                    table.Append(tableRows[i]);
                }
                else
                {
                    MessageBox.Show(result.ErrorText);
                }
            }
            return table;
        }

        private Table CustomTable_3()
        {
            Table table = new Table();
            const int CELLS_IN_A_ROW = 5;
            table.Append(StandardTableProperties());
            int.TryParse(NumberOfSensors.Text, out int numberOfSensors);

            TableRow[] tableRows = new TableRow[numberOfSensors];
            TableCell[,] tableCells = new TableCell[numberOfSensors, CELLS_IN_A_ROW]; // 5 cells in a row
            Paragraph[,] paragraphs = new Paragraph[numberOfSensors, CELLS_IN_A_ROW];
            Run[,] runs = new Run[numberOfSensors, CELLS_IN_A_ROW];

            string[] headers = { "№", "Значение A", "Значение B", "Значение C", "Значение D"};
            table.Append(Table_Headers(ref headers)); // set headers (1st row) to the table

            string[] columnsNames = {"value_a", "value_b", "value_c", "value_d" };

            for (int i = 0; i < numberOfSensors; i++)
            {
                MySqlLib.MySqlData.MySqlExecuteData.MyResultData result = 
                    new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
                result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(
                    "SELECT value_a, value_b, value_c, value_d FROM " +
                    "sensors_data WHERE number_of_sensor = '" + sensorsNumbersBoxes[i].Text + "'", connectStr);

                if (result.HasError == false)
                {
                    tableRows[i] = new TableRow();

                    double[] values = {
                        result.ResultData.Rows[0].Field<double>(0),
                        result.ResultData.Rows[0].Field<double>(1),
                        result.ResultData.Rows[0].Field<double>(2),
                        result.ResultData.Rows[0].Field<double>(3)
                    };

                    for (int k = 0; k < CELLS_IN_A_ROW; k++)
                    {
                        runs[i, k] = new Run();
                        paragraphs[i, k] = new Paragraph();
                        tableCells[i, k] = new TableCell();
                        if(k == 0) runs[i, k].Append(new Text((k+1).ToString()));
                        else runs[i, k].Append(new Text(values[k-1].ToString()));

                        paragraphs[i, k].Append(runs[i, k]);
                        tableCells[i, k].Append(paragraphs[i, k]);
                        tableRows[i].Append(tableCells[i, k]);
                    }
                    table.Append(tableRows[i]);
                }
                else
                {
                    MessageBox.Show(result.ErrorText);
                }

            }
            return table;
        }

        private TableRow Table_Headers(ref string[] headers)
        {
            TableRow tableRow = new TableRow();

            for (int i = 0; i < headers.Length; i++)
            {
                TableCell cell = new TableCell();
                Paragraph par = new Paragraph();
                Run runHeader = new Run();
                Text header = new Text(headers[i]);
                runHeader.Append(header);
                par.Append(runHeader);
                cell.Append(par);
                tableRow.Append(cell);
            }
            return tableRow;
        }

        private Paragraph SensorsCoeffitientsP()
        {
            Paragraph sensor_coefficients_p = new Paragraph();
            Run sensor_coefficients_r = sensor_coefficients_p.AppendChild(new Run());
            RunProperties sensor_coefficients_rPr = new RunProperties();
            sensor_coefficients_rPr.FontSize = new FontSize() { Val = "22" };
            sensor_coefficients_rPr.RunFonts = new RunFonts()
            {
                Ascii = "Times New Roman",
                ComplexScript = "Times New Roman",
                HighAnsi = "Times New Roman"
            };
            sensor_coefficients_r.Append(sensor_coefficients_rPr);
            sensor_coefficients_r.AppendChild(new Text("Коэффициенты датчиков"));
            return sensor_coefficients_p;
        }
    }
}
 