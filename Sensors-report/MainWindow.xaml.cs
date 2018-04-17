using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DocumentFormat.OpenXml;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


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
        }

        private void paintName_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            TextBox[] textBoxes = { Department, Stand, TestNumber, Unit, NumberOfSensors };
            if (AreAllBoxesFilled(textBoxes, Date))
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
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(@"C:\Users\bezim\Desktop\1.docx", WordprocessingDocumentType.Document))
                {
                    // Main
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    // Head
                    Paragraph paragraph = body.AppendChild(new Paragraph());
                    Paragraph empty = body.AppendChild(new Paragraph());
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
                    
                    // Table
                    Table table_1 = new Table();
                    var Value = new EnumValue<BorderValues>(BorderValues.Birds);

                    TableProperties tablePr = new TableProperties(
                        new TableBorders(new TopBorder() { Val = Value, Size = 2 },
                            new BottomBorder() { Val = Value, Size = 2 },
                            new LeftBorder() { Val = Value, Size = 2 },
                            new RightBorder() { Val = Value, Size = 2 },
                            new InsideHorizontalBorder() { Val = Value, Size = 2 },
                            new InsideVerticalBorder() { Val = Value, Size = 2 }),
                        new TableWidth() { Width = "10000" }
                    );
                    Value = null;

                    table_1.Append(tablePr);

                    TableRow[] tableRows = new TableRow[2];
                    tableRows[0] = new TableRow();
                    tableRows[1] = new TableRow();
                    TableCell[] tableCells = new TableCell[5];
                    Paragraph[] paragraphs = new Paragraph[5];
                    Run[] runs = new Run[5];
                    Text[] texts = { new Text("Отдел: "), new Text("Стенд: "), new Text("Дата: "),
                new Text("Номер испытания: "), new Text("Агрегат: ")};
                    string[] values = { Department.Text, Stand.Text,
                    Date.Text, TestNumber.Text, Unit.Text};

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
                    table_1.Append(tableRows[0]);
                    table_1.Append(tableRows[1]);

                    Table table_2 = new Table();
                    table_2.Append((TableProperties)tablePr.Clone());

                    body.Append(table_1);

                    mainPart.Document.Save();
                    wordDocument.Close();
                }
            }
            else
            {
                MessageBox.Show("Вы не заполнили поля, поэтому отчет не будет создан", "", MessageBoxButton.OK);
            }
        }
    }
}