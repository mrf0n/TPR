   M S B A M L   `   `   `  ????V  RLinearRegressionConstructor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:LinearRegressionConstructor.views!LinearRegressionConstructor.views  5   	   0    *LinearRegressionConstructor.views.Settings   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Alocal/clr-namespace:LinearRegressionConstructor.views     5   	     ? 
ResizeMode$  NoResize=?5   	    ? Title$ Settings??5	   	   $	??500??6   $	??450??6'    ? 	Resources 5
         lblStyle ?? Property ?? Value  tbStyle  	bttnStyle??%?  &	        &	 ?     &	 ?     ?? 5   
   .?5      ??  ? ??$ Center=?6<   ?? 5       ? ??$ 13(?61   ?? 5       ? ??$ Medium"?63   5      ?? 5   
   .?5      ??  ? ??$ 50??6.   ?? 5       ? ??$ 16(?61   5      ?? 5   
   .?5      ??  ? ??$	 100??6.   ?? 5       ? ??$ 30??6/   ?? 5       ? ??
 ?????63   5      5      .??5      ]? $??20q?6   .+?5   
   ?? $	??Top=?6   $	??420??6*   .+?5      ??  ?? Orientation$ Vertical=?6   .+?5      ?? $ 
Horizontal=?6   $??Center=?5      .+?5       ?? $I??CВыбрать другой управляющий параметр??6   $??20??5!      $	??235??5"      
?????????5$      ?? *ChooseParam+5%      ?? 5&      $??Center=?6   $ 
Horizontal=?5'      5)      5*      ?? 5,      $ Vertical=?6   $	??407??60   .+?5-      ?? $v??pВыберите порог для изменения порогового значения корреляции??6   #??[  5.      $	??425??6:   ?? 5/      $ 
Horizontal=?6   $??Center=?50      .+?51      ?? #??[ 6   ??52      ?? *ThresholdPair+53      54      ?? 55      $ Vertical=?6   .+?56      ?? $Z??TПроверка на значимость эмпирическим подходом??6   #??[  57      ?? 58      $ 
Horizontal=?6   $??Center=?59      .+?5:      ?? #??[ 6   ??5;      ?? *ThresholdLinear+5<      5=      ?? 5>      $ 
Horizontal=?6   $??Center=?5?      .+?5@      ?? $<??6Применить критерий Стьюдента??6   ?? 5A      $??Center=?6    d? 	IsChecked 5B      ?? *IsSignChecked+5C      ?? 5D      $ 
Horizontal=?6   $??Center=?5E      .+?5F      ?? $??
Блок 1??6   ?? 5G      $??Center=?6    5H      ?? *IsSignCheckedBlock1+5I      ?? 5J      $ 
Horizontal=?6   $??Center=?5K      .+?5L      ?? $??
Блок 2??6   ?? 5M      $??Center=?6    5N      ?? *IsSignCheckedBlock2+5O      5P      5Q      5R      