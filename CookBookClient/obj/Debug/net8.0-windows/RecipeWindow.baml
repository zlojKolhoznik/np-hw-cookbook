   M S B A M L   `   `   `  ����I  ECookBookClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CookBookClientCookBookClient  5   	   !    CookBookClient.RecipeWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   .localclr-namespace:CookBookClient     5   	     � Title$  RecipeWindow��5   	   $	��450��6   $	��800��6+   .��5	      �  � ColumnDefinitions 5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      .+�5   
   �� -   5        $
��Left=�5   $   $��10q�6?   $��5q�5      .��6   Back to menu6*   �� 5   
   -   5         ��Name$ NameTextBlock��5   =   $��0��5      $��2��6!   $��0��65   $��Center=�6E   $��24(�6b   
���""��$
��Bold"�5   +   6=   �� 5   
   -   5         ��Name$ Image��5   -   $��1��6   $��0��6   6-   �� 5   
   -   5        $ IngredientsTextBlock��5   ?   $��1��6   $��1��6!   $��16(�61   6?   �� 5   
   -   5        $ PreparationTextBlock��5   S   $��2��6   $��0��6!   $��2��61   $��16(�6E   6S   5      5      