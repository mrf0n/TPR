   M S B A M L   `   `   `  ????V  RLinearRegressionConstructor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:LinearRegressionConstructor.views!LinearRegressionConstructor.views  5   	   0    *LinearRegressionConstructor.views.MainView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Alocal/clr-namespace:LinearRegressionConstructor.views     5   	   $	??250??5   	   $
??1100??5   	     ? Title$  Linear Regression??5
   	   $	??450??6#   $
??1000??60    ? 	Resources 5         	bttnStyle ?? Property ?? Value??%?   &	        ?? 5   
   .?5      ??  ? ??$	 100??6.   ?? 5       ? ??$ 30??6/   ?? 5       ? ??
 ?????63   5      5      .??5      ?  ? RowDefinitions 5   
   ?? 5      $
??auto ?6   ?? 5      $??* ?6   ?? 5      $??30 ?6   .+?5   
   o? $??0??6   $??Center=?5      
???????.@?5      m? $??Настройки??6   .@?5      m? $??Открыть файл??6    m? Command 5      ?? *Open+m? 5      $??Настройки??6    5      ?? *
Settings+5       5!      ]? 5"   
   $??1??6   .+?5#      ?? $??24(?5$      $	??Top=?5%      $??Center=?5&      ??5#      ?? *
Equation+ ?? UpdateSourceTrigger$ PropertyChanged=?5&      ?? 5'      $??16(?5(      $	??Top=?5)      $??Center=?5*      ??5'      ?? *CurrentCount+$ PropertyChanged=? ?? Mode$ TwoWay=?5*      ?? 5+      $??16(?5,      $	??Top=?5-      $??Center=?5.      ??5+      ?? *	ModelInfo+$ PropertyChanged=?5.      ?? 50      $??Bottom=?6    ?? Orientation$ 
Horizontal=?51      $??Center=?52      .+?53      ?? $B??<Вывести регрессионное уравнение??6   #??[  54      $??Bottom=?55      $	??224??57   7   ??56      ?? *	Calculate+{?57      ?? *IsCalcReady+67   58      5:      ?? 5;   
   $??2??5<      $??Center=?5=      ??5;      ?? *Status+5=      5>      5?      