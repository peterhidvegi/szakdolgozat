   M S B A M L   `   `   `  ���� D  @Validator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Validator	Validator      Validator.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Validator     -     �Name$  Focus�� � Icon$ logo.ico�� � Title$ Validator 0.5.3 - ��$	��768��$
��1366�� � WindowStartupLocation$ CenterScreen=�.��� -    �Name$ mainGrid�� � ColumnDefinitions �� $
��106* ��� $
��299* ��� $
��146* ��� $
��615* ��� $
��106* � � RowDefinitions �� $��2* ��� $��2* ��� $��6* ��� $��6* �.+��� $��1��$��1��
�������$��1q�$��4��$��3��._�� -   $ 	container��$��1��$��1��� 
����d�$��1��$��0��$��4��.+��� -    ��Name$ datas��
�������$��1q�._�� -   $ GridRelevantList��$��2��
����d� �� �� ��  �� $��2* ��� �� $��3* �.+��� $��	Interval:��$��Right=�$��0��
��������� $��1��$��0��
�������$��3q�$��5,5,20,5q��� -   $��Check��$��1��$��5,5,20,5q�
�������
����d�
��������� $��Reserved��
���  ��$��0��$��1��$��Right=�	� -    	�Name$ RBRes��	 	� 	GroupName$	 interval��$��1��$��1��$
��Left=�$��Center=�
 � LayoutTransform
 ��  �� ScaleX$ 2Q� �� ScaleY$ 2Q��� $
��Free��
��� d �$��0��$��2��$��Right=�	� -   $ RBFree��$	 interval��$��1��$��2��$
��Left=�$��Center=�
 �� $ 2Q�$ 2Q��� $��
From: (ID)��$��Center=�$��1��$��1��
��������� -	    ��Name$ CBFrom��$��1��$��1��$��Center=�$��Right=�$��0,0,20,0q�
����d��� $��To: (ID)��$��Center=�$��1��$��2��
��������� -
   $
 CBTo��$��1��$��2��$��Center=�$��Right=�$��0,0,20,0q�
����d��� $��Process information:��$��2��
�������� -    �Name$ progressBarForImageTest��$��2��$��0��
�������
����ح�$��	150,5,5,5q�
����ح�$��1q��� -    ��Name$ labelImageComparator��$��2��$��0��$��Center=�$��Image Comparator��
����d��� $��2��$��1��$��2q�
�������$��2��$��2,2,2,2q�� -   $ progressBar��$��2��$��2��$��5,0,5,5q�
�������
����ح��� -   $ labelDownload��$(��"Select date and click on download!��$��2��$��1��$��Center=�
��������� -    ��Name$ 	setMotion��$�� Motion Settings��$��	30,0,30,0q�$��0��$��0��
������ 
�������$��Center=�$��Center=�
��������� -   $ labelDateFrom��$��Date: ��$��0��$��1��$
��Left=�
��������� $"��Name of your Network Drive: ��$��0��$��2��$
��Left=�
��������� -   $ 	drivesBox�� �� SelectedIndex$ 0��$��0��$��2��$��Center=�$��191,0,143,0q�$��54��
����d�(  "System.Windows.Controls.DatePicker  -     Name$ 
datePicker��$��Center=�$��0,0,0,0q�$��1��$��0��$	��Top=�$��2��
����d��� -    ��Name$ motionCheckBox��$��0��
�������$
��Left=�$��Center=�.���� $��LeftToRight=�$��Scan motions���� $��1��$��0q�
�������._��� -   $ button��$��Download��
����d�$��16(�$��2q�
�������� 
����d�$��1��.+��� -   $ senzorLabel��$��	Senzor ID��$��1��$��0��$��Center=�$
��22,0q�
��������� -   $ sensorComboBox��$��1��$��Center=�$��22,32q�
�������
����d�� $��2��
����d�.+��� 
�������$��0 0 0 2q�._�� -   $��1��$��1�� � Points# �   ���  -1 -1������ �� Stretch$ Uniform=�$��Center=�$	��Top=�$��10 50 10 10q�����  �� Color$ 
WhiteSmoke�� �� Opacity$	 0.8Q�� $��2��.+��� -   $ numberOfPage��$��1��$��Center=�$��Bottom=�$��34(�
�������� $��3��
����d�.+�� -   $��1��$��1��' �   ���  -1 -1������ -1$ Uniform=�$��Center=�$��Bottom=�$��10 10 10 50q����� $ 
WhiteSmoke��$	 0.8Q�� $��3��.+��� -   $ maxPage��$��1��$��Center=�$	��Top=�$��34(�
�������