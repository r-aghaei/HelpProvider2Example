# HelpProvider Popoup with Unicode support

When you use the built-in [HelpProvider](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.helpprovider?view=net-5.0&WT.mc_id=DT-MVP-5003235) or try to show a help popup using [Help.ShowPopup](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.help.showpopup?view=net-5.0&WT.mc_id=DT-MVP-5003235), if you specify a string containing special Unicode characters like Persian or Arabic characters, the popup doesn't show those characters correctly.

This behavior because of the default font which is used by the underlying API of the HelpProvider. If the underlying API uses a right font which supports Unicode, it will work properly. 

In this example I've created a `HelpProvider2` component which supports unicode characters:

![HelpProvider2.png](HelpProvider2.png)


It also exposes `Font`, `ForeColor` and `BackColor` properties:

![HelpProvider2Properties.png](HelpProvider2Properties.png)

The example also contains a `HelpExtensions.ShowPopup2` which can be used instead of `Help.ShowPopoup`:

    HelpExtensions.ShowPopup2(button1, "متن آزمایشی", Control.MousePosition);
