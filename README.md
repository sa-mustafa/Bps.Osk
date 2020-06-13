# Bps.Osk
WPF on-screen keyboard &amp; keypad

![](https://raw.githubusercontent.com/sa-mustafa/Bps.Osk/master/screenshot.gif)

The underlying code is implemented using MVVM pattern with [Stylet](https://github.com/canton7/stylet/) library. However, your may opt to use any other MVVM frameworks.

To use the library, add a reference to Bps.Osk library. Refer to the library in xaml as usual:
```xaml
<Window ... whatever ...
        xmlns:local="clr-namespace:Bps.Osk.Views;assembly=Bps.Osk"
        ... whatever ...
        >
```

Then tag a textbox with the following code:
```xaml
<TextBox local:KeyboardView.TouchScreen="True">
</TextBox>
```
That's it!
