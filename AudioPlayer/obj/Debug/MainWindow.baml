   M S B A M L   `   `   `  ����F  BAudioPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AudioPlayerAudioPlayer  5   	   d `PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35jMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.ClassicMicrosoft.Windows.Themes 5   	       AudioPlayer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:AudioPlayer     5   	   `ThemesMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Classic     5   	   -   5          � Title$  Audio Player��5
   	   $	��520��6   $	��350��6+    � 
ResizeMode$ NoResize=�67    � Topmost .   � WindowState$ Normal=�6]    � WindowStyle$
 None=�6r    � AllowsTransparency .  � 	Resources 5         ListViewCustomStyle �� 
TargetType �� Property	 �� Value5  /Microsoft.Windows.Themes.ClassicBorderDecorator
   ClassicBorderBrush �� 
TargetType  Name   BorderBrush   BorderThickness   BorderStyle   
Background J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName ]� 
Conditions �� Property �� Value��%�  &	        �� 5   
   " ��6,   .�5      ��  � ��#	 � ��6+   �� 5       � ��#	 � ��6+   �� 5       � ��#	 Z
 6,   �� 5       � ��$	 2q�60   �� 5       � ��$
	 Auto=�6K   �� 5       � ��$
	 Auto=�6I   �� 5       � ��	 . 6>   �� 5       �@��PanningMode$
	 Both=�69   �� 5       �@��IsFlicksEnabled	 .  67   �� 5       � ��$	 Center=�69   �� 5       � ��	 5      �� 5      " ��6&   .R�5        $ Bd��68   # z��6D   # z��6p   $ Sunken=�6�   # z��6�    . ._�5      �� |�.  #��z��6=   .��5   "   �� # z 61   5      5       5      P� 5        � {� .  . �5!   "   �� $ Bd��6?   #	 � ��6O    �@ 
Background5"      ]� 5#       5$   "   �� 5%   &    �@��
IsGrouping . 6F   �� 5&   &   " �@%�IsVirtualizingWhenGrouping .  6h   ./�5(   "   ��  � ��	 .  6R   5)      5+      5-      5.      5/      ��50      �� 52   
    �� ImageSource$ Background.Png��6    j� Stretch$
 Fill=�62   .��56      � -   5         �Name$ MainGrid��56      .+�57   
   �� -   5         ��Name$ Poweroff��57        RoundPlayButtonTemplate#��[ 6:   $��23,10,301.4,485q�6k   ��58      �� 59      " ��6"   .R�5:      �� .+�5;      ��  ��Name$ PoweroffImage1��6    $��25��68   $��25��6D   $��Poweroff.png��6O   $
��Fill=�6e   $y�Visible=�6t   $��	1,0,0.2,0q�6�   �� 5<      $ PoweroffImage2��6    $��25��68   $��25��6D   $��PoweroffDark.png��6O   $
��Fill=�6i   $y�Visible=�6x   $��
0,0,-0.4,0q�6�   5=       5>      P� 5?       � �� . . �5@      �� $ PoweroffImage1��6%    � y�$	 	Collapsed=�6W   �� 5A      $ PoweroffImage2��6%    � y�$	 Visible=�6W   5B      5D      5F      �� 5G   
   $��0,0,0,0q�6   .+�5H      �� -   5         ��Name$ Playlist��5H      $	��296��6*   $��22,40,27.4,10q�67   $��0q�6N    �� SelectedIndex$ 0��6b   ���#444444
����� �#���   6�   6�   �� 5I      -   5        $ 	LoadFiles��5I      $��12(�6(   $��20��66   $	��100��6B   $��Center=�6N   $��125,5,125.4,5q�6�     RoundLoadButtonTemplate#��[ 6�   $��
Load Files��6�   �� 5J      -   5          ��Name$  ProgressSlider��5J      $��50,10,50.4,0q�6-   ! � Minimum$! 0Q�6f   " � Maximum$	" 100Q�6r   6�   �� 5K      -   5        # ��Name$# PlayButtons��5K      $ �� Orientation$$ 
Horizontal=�6.   $��Center=�6G   $��Bottom=�6d   $��25,0,27.4,0q�6   $��62��6�   $	��298��6�   .+�5L      �� -   5        $
 Prev��5L      $��49��6'   $��37��62   $��5,13,5,12.4q�6>   #��[ 6f   ��5M      �� 5N      " ��6*   .R�5O      �� .+�5P   "   �� $ 
PrevImage1��6(   $��37��6<   $��37��6H   $��PrevButton.png��6S   $
��Fill=�6k   $y�Visible=�6z   $��
0,0,-0.4,0q�6�   �� 5Q   "   $ 
PrevImage2��6(   $��37��6<   $��37��6H   $��PrevButtonDark.png��6S   $
��Fill=�6o   $y�Visible=�6~   $��
0,0,-0.4,0q�6�   5R       5S      P� 5T   "    � �� . . �5U   &   �� $ 
PrevImage1��6-    � y�$	 	Collapsed=�6[   �� 5V   &   $ 
PrevImage2��6-    � y�$	 Visible=�6[   5W   #   5Y      5[      �� 5\      -	   5        $ 	PlayPause��5\      $��39��6,   $��39��67   $��5q�6C   #��[ 6f   .��5]      �� -
   5        $ PrevButtonDark_png��5]      $��36��68   $��PlayButton.png��6D   $
��Fill=�6\   $��36��6k   5^      �� 5_      -   5        $
 Next��5_      $��38��6'   $��38��62   $��5q�6>   % J� RenderTransformOrigin$% 	0.504,0.5&�6I   #��[ 6~   ��5`      �� 5a      " ��6*   .R�5b      �� .+�5c   "   �� $ 
PrevImage1��6(   $��38��6<   $��38��6H   $��NextButton.png��6S   $
��Fill=�6k   $y�Visible=�6z   $��
0,0,-0.4,0q�6�   �� 5d   "   $ 
PrevImage2��6(   $��38��6<   $��38��6H   $��NextButtonDark.png��6S   $
��Fill=�6o   $y�Visible=�6~   $��
0,0,-0.4,0q�6�   5e       5f      P� 5g   "    � �� . . �5h   &   �� $ 
PrevImage1��6-    � y�$	 	Collapsed=�6[   �� 5i   &   $ 
PrevImage2��6-    � y�$	 Visible=�6[   5j   #   5l      5n      �� 5o      -   5        $ VolumeButton��5o      $��35��6/   $��35��6:   $��42,0,0,0q�6F   #��[ 6s   .��5p      �� -   5        $ VolumeButton_png��5p      $��30��66   $��VolumeButton.png��6B   $
��Fill=�6\   $��30��6k   5q      �� 5r      -   5        $  VolumeSlider��5r      $��50��60   $��20��6;   $��Center=�6G   $��4,0,0,0q�6b   $! 0Q�6�   $	" 100Q�6�   & � Value$& 80Q�6�   5s      5t      5u      5v      