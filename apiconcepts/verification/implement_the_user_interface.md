Implement the User Interface
======

In this chapter you will learn how to implement the user interface of your plug-in.

Add a User Control
----

Implement the graphical user interface by adding a user control, which you can name, e.g. `IdenticalVerifierUI.cs`. This is the interface that users will see when configuring the verifier in <Var:ProductName> through, e.g. **Tools** > **Options** > **Verification**. Our global verifier will only implement one text field. Here, users can enter the display code of the context (e.g. **H** for headline) to which the identical check should be applied. Add a text field to the user control, which you call e.g. `txt_Context`.

<img style="display:block; " src="images/ui_identical_check.jpg"/>

Implement the User Control Code
------

Switch to the code view of the user control, and add the following property to the class. This property is used for data binding of the value that is entered into text field (or the value that is retrieved from it):

# [C#](#tab/tabid-1)
```cs
// Data binding for the text field control
public string ContextToCheck
{
    get
    {
        return this.txt_Context.Text;
    }
    set
    {
        txt_Context.Text = value;
    }
}
```
***

Putting it All Together
-----
The complete `IdenticalVerifierUI` class should now look as shown below:

# [C#](#tab/tabid-2)
```cs
using System.Windows.Forms;

namespace Verification.Sdk.IdenticalCheck
{
    public partial class IdenticalVerifierUI : UserControl
    {
        #region text field control
        // Data binding for the text field control
        public string ContextToCheck
        {
            get
            {
                return this.txt_Context.Text;
            }
            set
            {
                txt_Context.Text = value;
            }
        }
        #endregion

        public IdenticalVerifierUI()
        {
            InitializeComponent();
        }
    }
}
```
***