   M S B A M L   `   `   `  ���� U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   Sensors-report   Sensors_report.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro       � 
ResizeMode$  CanMinimize=� � WindowStartupLocation$ CenterScreen=� � Title$. (Информация с датчиков��$	��570��$	��890��   	BlueBrush(   "MahApps.Metro.Controls.MetroWindow   WindowTitleBrush# �   .��� .+�� -   $��AuthGrid��$y�Visible=�.+��� -   $��loginBox��$	��Top=�$��320,174,320,0q�$	��242��$��40��*   $MahApps.Metro.Controls.TextBoxHelper   IsWaitingForData .    UseFloatingWatermark .    	Watermark$ 
Логин��  LoginTextBox#��[    ClearTextButton ��  �� RelativeSource �� *Self+	 �� Path$&	  (Controls:TextBoxHelper.HasText)�
 �� Mode$
 OneWay=�.��G� -   $��passwordBox��$	��Top=�$��320,224,320,0q�$	��242��$��40�� .  . $ Пароль�� " MetroButtonRevealedPasswordBox#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -   $��hostBox��$	��Top=�$��320,274,320,0q�$	��242��$��40�� .  . $
 Host��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.���� -   $��ВХОД��$��35��$��320,322,320,0q�$	��242��$	��Top=�
����������	#FF2D2D30� -   $��MainGrid��$y�Hidden=�.+��� -   $��
tabControl��$	��539��$	��Top=�$��0,0,-5,0q�-   'MahApps.Metro.Controls.TabControlHelper   IsUnderlined . $
��Left=�$	��887��   
Underlined$ SelectedTabItem=�.@��� $&�� Данные с датчиков��.��� .+�&   System.Windows.Controls.DataGrid  -   $��sensors_data_grid��$��1q�
�������$	��390��$	��Top=�$��
10,10,10,0q�$	��860��   
IsReadOnly .    SelectionMode$ Single=�   GridLinesVisibility$	 All=��� -	   $��Обновить��$
��Left=�$��50��$��110,420,0,0q�$	��Top=�$	��209��$��20(�  AccentColorBrush#���    SearchTextForeground#���  �� -
   $��Изменить��$��Center=�$��50��$��	0,420,0,0q�$	��Top=�$	��209��$��20(�#���  #���  �� -   $��Удалить��$��Right=�$��50��$��0,420,110,0q�$	��Top=�$	��209��$��20(�#���  #���  �� -   $��AddOrChangeTab��$#��Добавить запись��.��� .+��� -    ��Name$ AddOrChangeComboBox��$	��Top=�$��Center=�$��0,50,0,0q�$	��210��$��40�� �� SelectedIndex$ 0��.@��� .��Добавить запись�� .��Изменить запись�� -   $��KeyNumberOfSensor��$y�	Collapsed=�$	��Top=�$��Right=�$��	0,50,50,0q�$	��242��$��40�� .  . $ Номер датчика��#��[ ���	#33096400 ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -   $��NumberOfSensor��$	��Top=�$
��Left=�$��
50,120,0,0q�$	��242��$��40�� .  . $ Номер датчика��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -   $��NameOfSensor��$	��Top=�$��Center=�$��320,120,319,0q�$	��242��$��40�� .  . $- 'Наименование датчика��#��[ $��2�� ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.���� -   $��TypeOfSensor��$	��Top=�$��Right=�$��
0,120,50,0q�$	��242��$��40�� .  . $ Тип датчика��#��[ $��1�� ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.���� -   $��Value_A��$	��Top=�$
��Left=�$��
50,200,0,0q�$	��242��$��40�� .  . $ Значение А��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.���� -   $��Value_B��$	��Top=�$��Center=�$��320,200,319,0q�$	��242��$��40�� .  . $ Значение B��#��[ $��2�� ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.���� -   $��Value_C��$	��Top=�$��Right=�$��
0,200,50,0q�$	��242��$��40�� .  . $ Значение C��#��[ $��1�� ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.���� -   $��Value_D��$	��Top=�$
��Left=�$��
50,280,0,0q�$	��242��$��40�� .  . $ Значение D��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.���� -   $��
Zero_value��$	��Top=�$��Center=�$��320,280,319,0q�$	��242��$��40�� .  . $% Нулевое значение��#��[ $��2�� ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�.��(  "System.Windows.Controls.DatePicker  -     Name$ Expiration_date��$��Right=�$	��Top=�$��
0,280,50,0q�$	��242��$��40�� . $4 .Окончание сркоа годности��   SelectedDateFormat$ Short=�$��1���� -   $��AddOrChange��$��Добавить��$��Center=�$��45��$��
0,400,12,0q�$	��Top=�$	��200��$��18(�#���  #���  �� $��Создать отчет��.��� .+��� -   $��
Department��$	��Top=�$
��Left=�$��
150,64,0,0q�$	��242��$��40�� .  . $ Номер отдела��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -   $��Stand��$	��Top=�$��Right=�$��
0,64,150,0q�$	��242��$��40�� .  . $ Номер стенда��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -   $��
TestNumber��$	��Top=�$
��Left=�$��150,154,0,0q�$	��242��$��40�� .  . $# Номер испытания��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -   $
��Unit��$	��Top=�$��Right=�$��0,154,150,0q�$	��242��$��40�� .  . $ Агрегат��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=�  -   $ DateP��$
��Left=�$	��Top=�$��150,244,0,0q�$	��242��$��40�� . $ Дата��$ Short=��� -   $��NumberOfSensors��$	��Top=�$��Right=�$��0,244,150,0q�$	��242��$��40�� .  . $+ %Количество датчиков��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -   $��
FolderPath��$	��Top=�$��Right=�$��0,324,151,0q�$	��580��$��40�� .  . $b \Путь к папке, в которой необходимо сохранить отчет��#��[  ��  �� *Self+$&	  (Controls:TextBoxHelper.HasText)�$
 OneWay=��� -    $
��Next��$��
Далее��$��Center=�$��45��$��334,400,347,0q�$	��Top=�$	��200��$��18(�#���  #���  