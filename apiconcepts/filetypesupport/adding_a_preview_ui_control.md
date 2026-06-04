# Adding a preview UI control

Your filter needs a control that can display the document preview.

## Add a web browser control

The static internal preview uses the built-in web browser control in Var:ProductName. See [Modifying the File Type Component Builder](static_modifying_the_file_type_component_builder.md). Other native file formats may require different controls. For example, DOC files use a Microsoft Word Viewer control.

This sample uses a web browser control again, but this time it adds and configures a custom control instead of reusing the built-in one.

Start by adding a user control such as **InternalPreviewControl.cs** to your project. Then add a web browser control from the Visual Studio toolbox and name it `webBrowserControl`.

## Implement the preview control functionality

To respond to events such as clicking a segment in the editor or scrolling to a segment, implement the following code in your preview control:

# [C#](#tab/tabid-1)
```cs
using System;
using System.Windows.Forms;
using System.Security.Permissions;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.FileTypeSupport.Samples.SimpleText.Preview
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class InternalPreviewControl : UserControl
    {
        string _activeSegId = String.Empty;
        string _jumpparagraphID = String.Empty;
        string _jumpsegmentID = String.Empty;
        bool _segmentSelectedFromBrowser = false;


        public event PreviewControlHandler WindowSelectionChanged;

        public InternalPreviewControl()
        {
            InitializeComponent();
            //set the properties of the webbrowser component
            webBrowserControl.AllowWebBrowserDrop = false;
            webBrowserControl.IsWebBrowserContextMenuEnabled = false;
            webBrowserControl.WebBrowserShortcutsEnabled = false;
            webBrowserControl.ScriptErrorsSuppressed = true;
            webBrowserControl.AllowNavigation = false;
            webBrowserControl.ObjectForScripting = this;
            webBrowserControl.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowserControl_DocumentCompleted);
        }

        void webBrowserControl_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ScrollToElement(_activeSegId);

            //set the CSS style for the curently selected segment
            webBrowserControl.Document.InvokeScript("setActiveStyle", new String[] { _activeSegId });
        }

        protected void FireWindowSelectionChanged()
        {
            if (WindowSelectionChanged != null)
            {
                WindowSelectionChanged(null);
            }
        }

        /// <summary>
        /// open file for preview
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenTarget(string fileName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action<string>(OpenTarget), fileName);
            }
            else
            {
                webBrowserControl.Navigate(fileName);
                webBrowserControl.Refresh();
            }
        }

        public void Close()
        {
            // The Filter Framework takes care of cleaning up temporary files.
        }

        /// <summary>
        /// construct a segment reference from _jumpparagraphID and _jumpsegmentID, 
        /// which is returned when user clicks the corresponding segment in the preview control
        /// </summary>
        /// <returns></returns>
        public SegmentReference GetSelectedSegment()
        {
            if (_jumpsegmentID != null && _jumpsegmentID != String.Empty)
            {
                SegmentReference segRef = new SegmentReference(default(FileId), new ParagraphUnitId(_jumpparagraphID), new SegmentId(_jumpsegmentID));
                return segRef;
            }
            return null;
        }

        /// <summary>
        /// public method that is called from the preview control 
        /// when a segment has been selected
        /// </summary>
        /// <param name="segmentId"></param>
        public void SelectSegment(string paragraphUnitID, string segmentID)
        {
            // set global variables for jumping into clicked segment
            _jumpparagraphID = paragraphUnitID;
            _jumpsegmentID = segmentID;

            _segmentSelectedFromBrowser = true;
            FireWindowSelectionChanged();
        }

        /// <summary>
        /// scroll to the active segment inside the control
        /// </summary>
        /// <param name="elemName"></param>
        private void ScrollToElement(String elemName)
        {
            if (webBrowserControl.Document != null)
            {
                HtmlDocument doc = webBrowserControl.Document;
                HtmlElementCollection elems = doc.All.GetElementsByName(elemName);
                if (elems != null && elems.Count > 0)
                {
                    HtmlElement elem = elems[0];

                    elem.ScrollIntoView(true);
                }
            }
        }


        /// <summary>
        /// called when segment is confirmed and Trados Studio jumps into next segment
        /// </summary>
        public void JumpToActiveElement()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(JumpToActiveElement));
            }
        }


        /// <summary>
        /// scroll to and highlight active segment in the preview control
        /// </summary>
        /// <param name="segment"></param>
        public void ScrollToSegment(SegmentReference segment)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action<SegmentReference>(ScrollToSegment), segment);
            }
            else
            {
                if (!_segmentSelectedFromBrowser)
                {
                    ScrollToElement(segment.SegmentId.Id);

                    // handle situations in which the document was opened 
                    // and no active segment has been set yet.
                    if (_activeSegId == null || _activeSegId == "")
                    {
                        _activeSegId = segment.SegmentId.Id;
                        // select the CSS style for the curently selected segment
                        webBrowserControl.Document.InvokeScript("setActiveStyle", new String[] { segment.SegmentId.Id });
                    }
                }

                if (_activeSegId != segment.SegmentId.Id)
                {
                    // reset the CSS style back from active to normal for the previously selected segment
                    if (_activeSegId != null || _activeSegId == "")
                    {
                        webBrowserControl.Document.InvokeScript("setNormalStyle", new String[] { _activeSegId });
                    }
                    // set the CSS style for the curently selected segment
                    webBrowserControl.Document.InvokeScript("setActiveStyle", new String[] { segment.SegmentId.Id });
                }

                // set the active segment id
                _activeSegId = segment.SegmentId.Id;

                if (_segmentSelectedFromBrowser)
                {
                    _segmentSelectedFromBrowser = false;
                }
            }

        }
    }
}
```

## See also

- [Modifying the File Type Component Builder](static_modifying_the_file_type_component_builder.md)
- [Adding a Preview Controller](adding_a_preview_controller.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

