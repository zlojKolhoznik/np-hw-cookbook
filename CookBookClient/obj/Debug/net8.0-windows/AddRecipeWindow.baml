   M S B A M L   `   `   `  ����I  ECookBookClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CookBookClientCookBookClient  5   	   $    CookBookClient.AddRecipeWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   .localclr-namespace:CookBookClient     5   	     � Title$  AddRecipeWindow��5   	   $	��600��6!   $	��800��6.   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��Auto �6   �� 5       � RowDefinitions 5   
   �� 5      �� 5      $��2* �6   �� 5      $��* �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6    $
��Name��6-   $��Center=�5      $��Right=�6+   �� 5   
   -   5         ��Name$ NameTextBox��5   /   $��1��6   $��0��6"   $��Center=�5      $��20(�6-   $��15q�6;   �� 5   
   $��0��6   $��1��6    $��Preparation method��6-   $��Right=�5      $��0 15 0 0q�6,   �� 5   
   -   5        $ PreparationMethodTextBox��5      $��1��5      $��1��6"   " �� VerticalScrollBarVisibility$
 Auto=�6/   $��20(�5   4   $��15q�5       �� AcceptsReturn$
 True��6   �� 5   
   $��0��6   $��2��6    $��Ingredients��6-   $��Right=�5      $��0 10 0 0q�6,   �� 5    
   $
��Auto=�6   $��Hidden=�6:   $��1��6a   $��2��6q   .��5!      �� -   5         ��Name$ IngredientsInputs��5!      .+�5"      �� $��15q�6   $��5q�6*   $��20(�66   5#      5$      �� 5%   
   -   5        $��3��5%      $��0��6   $��Select image��6.   $��Right=�5&      $��Center=�6-   $��0 5 0 0q�5'      $��10q�6"   5(      �� 5)   
   -   5         ��Name$ Image��5)   -   $��3��6   $��1��6   $	��256��5*      $	��256��6   $��5q�6)   �� 5+   
   $��4��6   $��2��6"   $��0��66    �� Orientation$ 
Horizontal=�6F   $��Center=�5,      $��Center=�62   $��10q�5-      .+�5.      �� -   5        $��10q�5.      $��10q�6!   $��Cancel��6.   5/      �� 50      -   5        $��10q�50      $��10q�6!   $��
Add recipe��6.   51      �� 52      -   5        $��10q�52      $��10q�6!   $��Add ingredient��6.   53      54      55      56      