   M S B A M L   `   `   `  ����D  @Validator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Validator	Validator  5   	   "    Validator.MotionScanSettings   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Validator     5   	     � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � Title$ MotionScanSettings��5
   	   $	��300��6$   $	��300��61   
����d�.��5      � -   5         �Name$ settingsGrid��5       � ColumnDefinitions 5   
   �� 5      $��5* �6   �� 5      $��2* �6    � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��2* �6   .+�5   
   �� $+��%MIN. diff. from nodes avg. (multiple)��6   
�������$
��Left=�6S   $��Center=�6n   6�   �� 5   
   $��1��6   $+��%MAX. diff. from nodes avg. (multiple)��6   
�������$
��Left=�6`   $��Center=�6{   6�   �� 5   
   $��2��6   $��Correct Pixel limit��6   
�������$
��Left=�6N   $��Center=�6i   6�   �� 5    
   $��3��6   $*��$Correct pixel (%) to be Event (Main)��6   
�������$
��Left=�6_   $��Center=�6z   6�   �� 5!   
   $��4��6   $+��%Correct pixel (%) to be Event (Nodes)��6   
�������$
��Left=�6`   $��Center=�6{   6�   �� 5#   
   -   5        $��5��5#      
���� ��
�������$��10,10,10,10q�6E   $�� Cancel ��6Z   6�   �� 5$   
   -   5        $��5��5$      $��1��6   
����� �
�������$��10,10,10,10q�6\   $
�� OK ��6q   6�   �� 5&   
   -   5         ��Name$ motionMinDiffAvgBox��5&      $��1��6/   $��0��6?   $��Center=�6M   $��00��6j   $��Center=�6t   
����d�6�   �� 5'   
   -   5        $ motionMaxDiffAvgBox��5'      $��1��6/   $��1��6?   $��Center=�6M   $��00��6j   $��Center=�6t   
����d�6�   �� 5(   
   -   5        $  motionPixelCorrectLimitBox��5(      $��1��66   $��2��6F   $��Center=�6T   $��00��6q   $��Center=�6{   
����d�6�   �� 5)   
   -   5        $) #motionMainCorrectPixelPercentageBox��5)      $��1��6?   $��3��6O   $��Center=�6]   $��00��6z   $��Center=�6�   
����d�6�   �� 5*   
   -   5        $) #motionNodeCorrectPixelPercentageBox��5*      $��1��6?   $��4��6O   $��Center=�6]   $��00��6z   $��Center=�6�   
����d�6�   5,      5-      