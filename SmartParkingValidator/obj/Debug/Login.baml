   M S B A M L   `   `   `  ����D  @Validator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Validator	Validator  5   	       Validator.Login   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Validator     5   	     � 
ResizeMode$  NoResize=�5   	    � Icon$ logo.ico��5	   	    � Title$ Validator 1.0 - [Login]��5
   	   $	��300��6)   $	��300��66    � WindowStartupLocation$ CenterScreen=�6B   .��5      � -   5         �Name$ loginMainGrid��5       � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��4* �6    � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      �� 5      �� 5      �� 5      .+�5   
   �� -   5         ��Name$
 logo��5      $��0��6   $��1��6+   $��Center=�6;   $��
10,10,10,0q�6X   6n   �� 5   
   $��0��6   $��0��6$   $��	Welcome! ��61    �� TextAlignment$ Right=�6B   $��Center=�6X   
����d�$��0,0,0,0q�6�   6�   �� 5   
   $��0��6   $��1��6$   $��User Name: ��61   $ Right=�6D   $��Center=�6Z   
����d�6�   �� 5    
   $��0��6   $��2��6$   $��User Password: ��61   $ Right=�6H   $��Center=�6^   
����d�6�   �� 5!   
   -   5        	 ��Name$	 	loginName��5!      $��1��6%   $��1��65   
 �� TextAlignment$

 Left=�6B   $��Center=�6W   $��14(�6r   $��5,5,5,5q�6�   
����d�
����d�6�   G� 5"   
   -   5         G�Name$ 	loginPass��5"      $��1��6)   $��2��69   $��Center=�6F   $��14(�6a   $��5,5,5,5q�6o   
����d�
����d�6�   �� 5#   
   -   5         ��Name$ loginButton��5#      $��3��6&   $��1��63   $��Log in!��6C   $��5,5,5,5q�6o   
����d�
�������
�������6�   �� 5$   
   -   5        $ signinButton��5$      $��3��6'   $��0��64   $��Sign Up!��6D   $��5,5,5,5q�6r   
����d�
�������
����d�6�   � 5&   
   $��4��6   $��2��6    5(      �� 5)      �� 5*      �� 5+      �� 5,      .+�50      9� -   5         9�Name$ expander��50      $��Set Server Name/Password��6)   $��4��6K   $��Center=�6_   $
��Left=�6z   {�.  .��51      ��  �� Orientation$ 
Horizontal=�6   .+�52      �� $�� User Name:    ��6    $
 Left=�67   $��Center=�6L   $��0��6g   6y   �� 53      -   5        $	 
serverName��53      $

 Left=�63   $��14(�6H   $��Right=�6V   $��1��6r   $��60��6�   6�   �� 54      $��   Password:   ��6    $��2��67   6I   G� 55      -	   5        $ 
serverPass��55   "   $��3��66   $��14(�6F   $��4��6T   $��Right=�6h   $��80��6�   56      57      58      5:      5<      