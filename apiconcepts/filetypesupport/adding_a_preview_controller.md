Adding a Preview Controller
===

Last, you need to implement a class for controlling the actual preview control element, which you added in the previous step.

Add a Preview Controller Class
--

The preview control element alone is not sufficient for implementing the real-time preview functionality. You also need to add a class that controls the preview UI, which you can call e.g. **InternalPreviewController.cs**. This class is used to catch events such as the user clicking the **Refresh** button in the preview window, the user clicking a segment, scrolling to a segment, confirming a segment in the editor, etc. When those events are caught by the controller class, it triggers the corresponding functions that have been implemented in the preview control UI (see chapter [Adding a Preview UI Control](adding_a_preview_ui_control.md)).

The preview controller class needs to use the following namespaces:

* **Sdl.FileTypeSupport.Framework**
* **Sdl.FileTypeSupport.Framework.NativeApi**
* **Sdl.FileTypeSupport.Framework.IntegrationApi**

The class must then implement the following interfaces:

* [ISingleFilePreviewControl](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISingleFilePreviewControl.yml): Provides access to the preview file, which is generated in the user's temporary files folder.
* [INavigablePreview](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.INavigablePreview.yml): Users can navigate to particular segments in the preview window. This interface provides the functionality to control what should happen when a user, e.g. clicks a particular segment in the preview.
* [IPreviewUpdatedViaRefresh](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IPreviewUpdatedViaRefresh.yml): Users can click a **Refresh** button in the preview control. This interface implements functions for controlling the preview behavior before and after a refresh operation.
* `IDisposable`: The preview generates a physical preview file in the user's temporary folder under **SDLTempFileManager**. This temporary file needs to be deleted when the preview is closed or when the user, for example, exits the application. This interface provides the functionality for disposing of the preview file when it is no longer required.

Declare the Global Members
--

First, declare the following global class members. Create a `_control` object for controlling the actual preview UI and other members used to store the file id and to determine whether the preview file has already been opened or not.

# [C#](#tab/tabid-1)
```cs
InternalPreviewControl _control; // the actual control object
private bool disposed = false; // used for properly disposing of the control        
FileId _fileId; // the actual file ID

// indicates whether a file has already been opened in the preview, 
// so we know if we should close it during a Refresh()  
bool _isFileOpen = false;
```
***

Initialize the Preview Control UI
--

Then add the following members to initialize the preview control and to properly dispose of it, e.g. when the preview or the application is closed by the user.

# [C#](#tab/tabid-2)
```cs
/// <summary>
/// initialize the preview controller and preview control
/// </summary>
public InternalPreviewController()
{
    _control = new InternalPreviewControl();
}

/// <summary>
/// used for disposing of the control
/// </summary>
~InternalPreviewController()
{
    Dispose(false);
}
```
***

Implement the Members of the Abstract Preview Control Interface
--

The [IAbstractPreviewControl](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IAbstractPreviewControl.yml) interface provides the functionality that allows you to control, e.g. what happens when the preview control is refreshed. Add the following members to your preview controller class:

# [C#](#tab/tabid-3)
```cs
/// <summary>
/// return a preview control
/// </summary>
public System.Windows.Forms.Control Control
{
    get { return _control; }
}

/// <summary>
/// handler for the WindowSelectionChanged event from the preview control,
/// raises the corresponding event on the INavigablePreview interface.
/// </summary>
/// <param name="component"></param>
void _Control_WindowSelectionChanged(IInteractivePreviewComponent component)
{
    var marker = _control.GetSelectedSegment();
    SegmentReference selectedSegment = new SegmentReference(_fileId, marker.ParagraphUnitId, marker.SegmentId);

    // raise the event
    OnSegmentSelected(this, new SegmentSelectedEventArgs(this, selectedSegment));
}

/// <summary>
/// refresh the file in the preview control
/// </summary>
public void Refresh()
{
    if (_isFileOpen)
    {
        _control.WindowSelectionChanged -= new PreviewControlHandler(_Control_WindowSelectionChanged);
        _control.Close();
    }

    // show the preview file in the control
    _control.OpenTarget(PreviewFile.FilePath);

    // attach event handler
    _control.WindowSelectionChanged += new PreviewControlHandler(_Control_WindowSelectionChanged);

    _isFileOpen = true;
}
```
***

Provide Access to the Temporary Preview File
--

Add the following member, which provides programmatic access to the physical preview file, which is going to be temporarily generated on the user's hard disk:

# [C#](#tab/tabid-4)
```cs
/// <summary>
/// access to the temporary preview file
/// </summary>
public TempFileManager PreviewFile
{
    get;
    set;
}
```
***

Implement the Preview Document Navigation
--

In the next step, implement the following members of the [INavigablePreview](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.INavigablePreview.yml) interface. These functions control what should happen when the user, for example, scrolls to or selects a particular segment in the preview control.

# [C#](#tab/tabid-5)
```cs
/// <summary>
/// reference to the current segment
/// </summary>
/// <param name="segment"></param>
public void NavigateToSegment(SegmentReference segment)
{
    _fileId = segment.FileId;
    _control.ScrollToSegment(segment);
}

/// <summary>
/// communicates the preferred highlighting in the control
/// not used in this implementation
/// </summary>
public System.Drawing.Color PreferredHighlightColor
{
    get;
    set;
}

public event EventHandler<SegmentSelectedEventArgs> SegmentSelected;

/// <summary>
/// custom implementaton - raise the <see cref="SegmentSelected"/> event.
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public virtual void OnSegmentSelected(object sender, SegmentSelectedEventArgs args)
{
    if (SegmentSelected != null)
    {
        SegmentSelected(sender, args);
    }
}
```
***

Dispose of the Preview File
--

Add the following members of the `IDisposable` interface, which takes care of cleaning up and removing the temporary preview files, e.g. after the application is closed by the user:

# [C#](#tab/tabid-6)
```cs
/// <summary>
/// deletes the preview file, if it exists.
/// </summary>
public void Dispose()
{
    Dispose(true);
    GC.SuppressFinalize(this);
}


/// <summary>
/// implementation of the recommended dispose protocol
/// deletes the file if possible.
/// </summary>
/// <param name="disposing">true if this method is called from IDisposable.Dispose() and false if called from Finalizer</param>
protected virtual void Dispose(bool disposing)
{
    if (!this.disposed)
    {
        try
        {
            if (disposing)
            {
                PreviewFile.Dispose();
            }
            // release the native unmanaged resources you added
            // in this derived class here.

            this.disposed = true;
        }
        finally
        {
        }
    }
}
```
***

Implement the Refresh Functionality
--

Add the following members of the [IPreviewUpdatedViaRefresh](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IPreviewUpdatedViaRefresh.yml) interface, which provides the functionality to control what happens when the user refreshes the preview control:

# [C#](#tab/tabid-7)
```cs
public void AfterFileRefresh()
{
    Refresh();
    ((InternalPreviewControl)Control).JumpToActiveElement();
}

public void BeforeFileRefresh()
{
    // no action required here
}


/// <summary>
/// returns the file for preview
/// </summary>
public TempFileManager TargetFilePath
{
    get;
    set;
}
```
***

Register the Preview Controller in the File Type Component Builder
---

Do not forget to register the preview controller (i.e. NOT the actual user control item) in the File Type Component Builder as follows. You can add this object below the control used for the static preview (see chapter [Modifying the File Type Component Builder](static_modifying_the_file_type_component_builder.md)). Note that the **id** of the object corresponds to the name of the preview set that you added previously (see chapter [Modifying the File Type Component Builder](dynamic_modifying_the_file_type_component_builder.md)) and must be preceded by a ***PreviewControl_*** prefix to be properly recognized.

# [C#](#tab/tabid-8)
```cs
/// <summary>
/// Creates a new instance of the preview control with the specified name.
/// </summary>
/// <remarks>
/// <para>
/// Should only be called from the main thread, as controls must always be
/// instantiated on the same thread as the application message pump.
/// </para>
/// </remarks>
/// <param name="name">not used here</param>
/// <returns>not implemented</returns>
public virtual IAbstractPreviewControl BuildPreviewControl(string name)
{
    if (name == "PreviewControl_InternalStaticPreviewControl")
    {
        return new InternalPreviewController();
    }
    else if (name == "PreviewControl_InternalNavigablePreview")
    {
        return new InternalPreviewController();
    }
    else
    {
        return null;
    }
}
```
***

Putting it All Together
--

All put together, the preview controller class will look as shown below:

# [C#](#tab/tabid-9)
```cs
using System;
using Sdl.FileTypeSupport.Framework;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Sdk.FileTypeSupport.Samples.SimpleText.Preview
{
    class InternalPreviewController : ISingleFilePreviewControl, INavigablePreview, IPreviewUpdatedViaRefresh, IDisposable
    {
        #region "global"
        InternalPreviewControl _control; // the actual control object
        private bool disposed = false; // used for properly disposing of the control        
        FileId _fileId; // the actual file ID

        // indicates whether a file has already been opened in the preview, 
        // so we know if we should close it during a Refresh()  
        bool _isFileOpen = false;
        #endregion

        #region "init"
        /// <summary>
        /// initialize the preview controller and preview control
        /// </summary>
        public InternalPreviewController()
        {
            _control = new InternalPreviewControl();
        }

        /// <summary>
        /// used for disposing of the control
        /// </summary>
        ~InternalPreviewController()
        {
            Dispose(false);
        }
        #endregion


        #region "IAbstractPreviewControl Members"
        /// <summary>
        /// return a preview control
        /// </summary>
        public System.Windows.Forms.Control Control
        {
            get { return _control; }
        }

        /// <summary>
        /// handler for the WindowSelectionChanged event from the preview control,
        /// raises the corresponding event on the INavigablePreview interface.
        /// </summary>
        /// <param name="component"></param>
        void _Control_WindowSelectionChanged(IInteractivePreviewComponent component)
        {
            var marker = _control.GetSelectedSegment();
            SegmentReference selectedSegment = new SegmentReference(_fileId, marker.ParagraphUnitId, marker.SegmentId);

            // raise the event
            OnSegmentSelected(this, new SegmentSelectedEventArgs(this, selectedSegment));
        }

        /// <summary>
        /// refresh the file in the preview control
        /// </summary>
        public void Refresh()
        {
            if (_isFileOpen)
            {
                _control.WindowSelectionChanged -= new PreviewControlHandler(_Control_WindowSelectionChanged);
                _control.Close();
            }

            // show the preview file in the control
            _control.OpenTarget(PreviewFile.FilePath);

            // attach event handler
            _control.WindowSelectionChanged += new PreviewControlHandler(_Control_WindowSelectionChanged);

            _isFileOpen = true;
        }
        #endregion

        #region "Temp File Manager"
        /// <summary>
        /// access to the temporary preview file
        /// </summary>
        public TempFileManager PreviewFile
        {
            get;
            set;
        }


        #endregion


        #region "INavigablePreview Members"
        /// <summary>
        /// reference to the current segment
        /// </summary>
        /// <param name="segment"></param>
        public void NavigateToSegment(SegmentReference segment)
        {
            _fileId = segment.FileId;
            _control.ScrollToSegment(segment);
        }

        /// <summary>
        /// communicates the preferred highlighting in the control
        /// not used in this implementation
        /// </summary>
        public System.Drawing.Color PreferredHighlightColor
        {
            get;
            set;
        }

        public event EventHandler<SegmentSelectedEventArgs> SegmentSelected;

        /// <summary>
        /// custom implementaton - raise the <see cref="SegmentSelected"/> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public virtual void OnSegmentSelected(object sender, SegmentSelectedEventArgs args)
        {
            if (SegmentSelected != null)
            {
                SegmentSelected(sender, args);
            }
        }
        #endregion

        #region "IDisposable Members"
        /// <summary>
        /// deletes the preview file, if it exists.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// implementation of the recommended dispose protocol
        /// deletes the file if possible.
        /// </summary>
        /// <param name="disposing">true if this method is called from IDisposable.Dispose() and false if called from Finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        PreviewFile.Dispose();
                    }
                    // release the native unmanaged resources you added
                    // in this derived class here.

                    this.disposed = true;
                }
                finally
                {
                }
            }
        }
        #endregion

        #region "IPreviewUpdatedViaRefresh Members"
        public void AfterFileRefresh()
        {
            Refresh();
            ((InternalPreviewControl)Control).JumpToActiveElement();
        }

        public void BeforeFileRefresh()
        {
            // no action required here
        }


        /// <summary>
        /// returns the file for preview
        /// </summary>
        public TempFileManager TargetFilePath
        {
            get;
            set;
        }
        #endregion
    }
}
```
***


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.