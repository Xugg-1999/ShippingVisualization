   M S B A M L   `   `   `  ����   ShippingVisualization&     ShippingVisualization.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -   $��Microsoft YaHei)�$��
ExtraLight"�  � WindowStartupLocation$  CenterScreen=� � Title$! 成品发货可视化平台�� � WindowStyle$
 None=� � AllowsTransparency$
 True�� � 
ResizeMode$ NoResize=�$	��700��$
��1300����
�  
� Center$ 1,0.1&� 
� GradientOrigin$ 1,0.1&� 
� RadiusX$	 0.9Q� 
� RadiusY$	 0.9Q�.��� 	 � Color$	 	#FF0F4D86��
 � Offset$

 -1.5Q�� $	 #010C1D��$
 2Q� � 	Resources  �� Source�� $% ../Assets/Styles/AppStyles.xaml?�%    .��� $
}�True�� � RowDefinitions �� $��27 ��� $
��auto �$	��236���� .+��� -    ��Name$ CloseImageButton��$��/Icon/Close.ico��$��18��$��18��$
��Left=�$	��Top=�$��	10,10,0,0q� � Cursor$
 Hand��� $��Center=�$��Center=�$��30��$	��382��z���  �� AngleX$	 -30Q� � ColumnDefinitions �� $
��115* ��� $
��210* ��� $	��19* ��� $	��19* ��� $	��19* �.+��� $��30��$	��345��$��4��$��0,0,18,0q�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,0&�.��� $	 	#FF1F3A69��$
 0Q�� $	 #0C192A��$
 1Q��� $��13�����#101E34$��2��$��3,2,3,2q��� $��13�����#101E34$��3��$��3,2,3,2q��� $��13�����#101E34$��4��$��3,2,3,2q��� $!��成品发货可视化平台��$��Center=�$��Center=�$��18(����#1FDCF5$��Normal"�   Dingding#��[  $��20��$	��174���� $��2�� �� Orientation$ 
Horizontal=�$��Center=�$��Right=�$��	10,0,0,-5q�.+��� $��刷新频率(s):��$��Center=�$��12(�#��[  ���	#661FDCF5$��0,0,5,-5q��� -   $��20��$��70�� �� SelectedItem �� *SelectedRefreshFrequency+ �� Mode$ TwoWay=�.@��� $��3���� $��5���� $��10���� $��30���� $��60���� $	��120���� $�� ��$��Center=�$��12(�#��[  ���	#661FDCF5$��0,0,5,0q��� $��日期:��$��Center=�$��12(�#��[  ���	#661FDCF5$��0,0,5,0q�(  "System.Windows.Controls.DatePicker  $��24��$	��120��#��[  $��Center=����#D14646   SelectedDate �� *SelectedDate+$ TwoWay=�� $��1�� �� $
��1.5* ��� .+��� $��当日发货计划��  GroupBoxStyle#��[ ���� .R��� $ 
Horizontal=�$��Center=�.+��� $��Center=����� �� $��/Icon/DropShipping.ico��$��20��$��20��$��5,0,0,0q�.��&   System.Windows.Controls.DataGrid  -     Name$ ShippingDataGrid��   AutoGenerateColumns$ False��   CanUserAddRows$ False��   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ *    	RowHeight$ 20Q�$��10(�$
��Bold"����� *ShippingRecords+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$	 No.��   Width$
 0.4*     
IsReadOnly$
  True��1  +System.Windows.Controls.DataGridBoundColumn!   Binding! �� *FNO+  $ 发货通知单号��$
 1.2* $
  True��! �� *	FBILLNO+  $ 	业务员��$
 0.6* $
  True��! �� *FSALER+  $ 	提单号��$
 1.8* $
  True��! �� *FBLNO+  $ 箱型��$
 0.8* $
  True��! �� *
FBOXTYPE+  $ 装车时间��$
 1.5* $
  True��! �� *FLOADINGTIME+" �� StringFormat$" yy-MM-dd HH:mm:ss��  $ 开始扫描时间��$
 1.5* $
  True��! �� *
FSTARTTIME+$" yy-MM-dd HH:mm:ss��  $ 最后扫描时间��$
 1.5* $
  True��! �� *
FENDTIME+$" yy-MM-dd HH:mm:ss��  $ 用时��$
 0.6* $
  True��! �� *FTOTALDURATION+  $ 状态��$
 0.6* ! �� *	FSTATUS+�� $��次日发货计划��$��1��#��[ ���� .R��� $ 
Horizontal=�$��Center=�.+��� $��Center=����� �� $ ��/Icon/DropShippingNext.ico��$��20��$��20��$��5,0,0,0q�.��  -   $ ShippingDataGridNextDay��$ False��$ False��$ 20Q�$��10(�$
��Bold"�$ * ���� *ShippingRecordsNextDay+   $	 No.��$
 0.3* $
  True��! �� *FNO+  $ 发货通知单号��$ 1* $
  True��! �� *	FBILLNO+  $ 	提单号��$
 1.5* $
  True��! �� *FBLNO+  $ 箱型��$
 0.8* $
  True��! �� *
FBOXTYPE+  $ 预计装车时间��$
 1.3* $
  True��! �� *FLOADINGTIME+$" yy-MM-dd HH:mm:ss��� $��1��$��Center=�$
��1300�� �� $��3* ��� $��2* �� $��2�� �� $��* ��� $��* �.+��� $��	已装柜��#��[ ���� .R��� $ 
Horizontal=�$��Center=�.+��� $��Center=����� �� $��/Icon/List.ico��$��20��$��20��$��5,0,0,0q�.��  -   $ CompletedLoading��$ False��$ False��$ 20Q�$��10(�$
��Bold"�$ * ���� *CompletedLoading+   $ 销售订单��$ 0.85* $
  True��! �� *	FBILLNO+  $ 规格型号��$
 3.1* $
  True��! �� *FSPEC+  $ 物料名称��$
 0.9* $
  True��! �� *FMATERIALNAME+  $ 物料编码��$
 1.3* $
  True��! �� *FMATERIALID+  $ 数量��$
 0.4* $
  True��! �� *FQTY+  $ 单位��$
 0.4* $
  True��! �� *FUNIT+�� $��	待装柜��$��1��#��[ ���� .R��� $ 
Horizontal=�$��Center=�.+��� $��Center=����� �� $��/Icon/UnList.ico��$��20��$��20��$��5,0,0,0q�.��  -   $ UnCompletedLoading��$ False��$ False��$ 20Q�$��10(�$
��Bold"�$ * ���� *UnCompletedLoading+   $ 销售订单��$ 0.85* $
  True��! �� *	FBILLNO+  $ 规格型号��$
 2.8* $
  True��! �� *FSPEC+  $ 物料名称��$
 1.1* $
  True��! �� *FMATERIALNAME+  $ 物料编码��$
 1.4* $
  True��! �� *FMATERIALID+  $ 数量��$
 0.4* $
  True��! �� *FQTY+  $ 单位��$
 0.4* $
  True��! �� *FUNIT+