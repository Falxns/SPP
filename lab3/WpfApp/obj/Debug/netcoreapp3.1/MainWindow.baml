   M S B A M L   `   `   `  ����A  =WpfApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WpfAppWpfApp  5   	       WpfApp.MainWindow   5   	   e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:WpfApp     5   	     � Title$
  Lab3��5   	   $	��800��6   $	��800��6"    � 	Resources 5	       �� 
TargetType �� Property �� Value��%�   'R�       'Q� H     �� 5
   
   " R�6   .�5      ��  � ��$ 14(�6)   5      �� 5   
   " Q�6   .�5      ��  � ��$ 14(�6)   5      5      .��5      � -   5         �Name$ grid1��5       � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��0.15* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��1��6"   ��6/   �� *Dlls+ �� SelectedItem 5      �� *SelectedDll+��5      �� 5      .R�5      �� $��5q�6!   .+�5      �� $��14(�6$   ��62   �� 	 �� Path$
	 Name��� 5       ��6$   �� $
	 Path�5!      5"      5$      �� 5%   
   $��0��6   $��0��6   $��2��6.   $��50(�6B   ��6P   �� *
AddCommand+.��6o   ADD6t   �� 5&   
   $��1��6   $��1��6$   
 � DataContext
 64   �� *SelectedDll+.��5'      R� -   5        $��AssemblyTreeView��5'      ��6/   �� *AssemblyInfo.Children+��5(      �� 5)       �� ItemsSource 6/   �� *
Children+.R�5*      �� ��6$   �� *Name+5+      5-      5.      5/      50      