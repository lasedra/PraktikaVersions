   M S B A M L   `   `   `  ���� K  GPraktikaVersions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:PraktikaVersionsPraktikaVersions  !    PraktikaVersions.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:PraktikaVersions     -     � Title$
  Home��$	��450��$	��500��$	��500��$	��700�� � WindowStartupLocation$ CenterScreen=�.���  � RowDefinitions �� $��70 ��� .+��� -   $��FailTextBox��$��CONNECTION FAILED��$
��Bold"�$��16(�$��Center=�$��Center=�$y�Hidden=��� -   $��VersionsPanel��.+��� -   $��VersionTextBox��$��Current version - ��$
��Bold"�$��3q�$��30��]� $��3q�$��30��.+��� $��Available versions: ��$
��Bold"�$��Center=��� -   $��VersionsComboBox��$��3q�$	��450������ .R��� .+��� $
��Bold"����� *	TagName+�� $��Italic$����� *Body+�� -   $��
UpdateBttn��$��3q�$��Right=�$��60��.��Update�� -   $��UserListView��$��1��
����������� .R���  �� CornerRadius$ 5�� �� Padding$ 15q�
�������._���  �� Orientation$ 
Horizontal=�$��7q�.+��� $��2q����� *	surname+�� $��2q����� *name+�� $��2q����� *
patronymic+�� $��2q����� *email+