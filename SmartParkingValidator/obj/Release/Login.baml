   M S B A M L   `   `   `  ���� D  @Validator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Validator	Validator      Validator.Login   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Validator       � 
ResizeMode$  NoResize=� � Icon$ logo.ico�� � Title$ Validator 0.5.3 - [Login]��$	��300��$	��300�� � WindowStartupLocation$ CenterScreen=�.��� -    �Name$ loginMainGrid�� � ColumnDefinitions �� $��2* ��� $��4* � � RowDefinitions �� $��2* ��� �� �� �� .+��� -    ��Name$ epslogo��$��0��$��2���� $��0��$��1��$��User Name: �� �� TextAlignment$ Right=�$��Center=�
����d��� $��0��$��2��$��User Password: ��$ Right=�$��Center=�
����d��� -   	 ��Name$	 	loginName��$��1��$��1��
 �� TextAlignment$

 Left=�$��Center=�$��14(�$��5,5,5,5q�
����d�
����d�G� -    G�Name$ 	loginPass��$��1��$��2��$��Center=�$��14(�$��5,5,5,5q�
����d�
����d��� -    ��Name$ loginButton��$��3��$��1��$��Log in!��$��5,5,5,5q�
����d�
�������
��������� -   $ signinButton��$��3��$��0��$��Sign Up!��$��5,5,5,5q�
����d�
�������
����d�� $��4��$��2�� �� �� �� �� .+�9� -    9�Name$ expander��$��Set Server Name/Password��$��4��$��Center=�$
��Left=�{�.  .����  �� Orientation$ 
Horizontal=�.+��� $�� User Name:    ��$
 Left=�$��Center=�$��0���� -   $	 
serverName��$

 Left=�$��14(�$��Right=�$��1��$��60���� $��   Password:   ��$��2��G� -	   $ 
serverPass��$��3��$��14(�$��4��$��Right=�$��80��