   M S B A M L   `   `   `  ���� D  @Validator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Validator	Validator  "    Validator.MotionScanSettings   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Validator       � WindowStyle$
  None=� � WindowStartupLocation$ CenterScreen=� � Title$ MotionScanSettings��$	��300��$	��300��
����d�.��� -    �Name$ settingsGrid�� � ColumnDefinitions �� $��5* ��� $��2* � � RowDefinitions �� $��2* ��� $��2* ��� $��2* ��� $��2* ��� $��2* ��� $��2* �.+��� $+��%MIN. diff. from nodes avg. (multiple)��
�������$
��Left=�$��Center=��� $��1��$+��%MAX. diff. from nodes avg. (multiple)��
�������$
��Left=�$��Center=��� $��2��$��Correct Pixel limit��
�������$
��Left=�$��Center=��� $��3��$*��$Correct pixel (%) to be Event (Main)��
�������$
��Left=�$��Center=��� $��4��$+��%Correct pixel (%) to be Event (Nodes)��
�������$
��Left=�$��Center=��� -   $��5��
���� ��
�������$��10,10,10,10q�$�� Cancel ���� -   $��5��$��1��
����� �
�������$��10,10,10,10q�$
�� OK ���� -    ��Name$ motionMinDiffAvgBox��$��1��$��0��$��Center=�$��00��$��Center=�
����d��� -   $ motionMaxDiffAvgBox��$��1��$��1��$��Center=�$��00��$��Center=�
����d��� -   $  motionPixelCorrectLimitBox��$��1��$��2��$��Center=�$��00��$��Center=�
����d��� -   $) #motionMainCorrectPixelPercentageBox��$��1��$��3��$��Center=�$��00��$��Center=�
����d��� -   $) #motionNodeCorrectPixelPercentageBox��$��1��$��4��$��Center=�$��00��$��Center=�
����d�