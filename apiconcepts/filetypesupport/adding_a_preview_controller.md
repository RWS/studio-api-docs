# Adding a preview controller

The last step is to implement a class that controls the preview control element that you added in the previous step.

## Add a preview controller class

The preview control element alone cannot provide the real-time preview functionality. You also need a class that controls the preview UI, for example **InternalPreviewController.cs**. This class handles events such as clicking the **Refresh** button, selecting a segment, scrolling to a segment, and confirming a segment in the editor. In response, it calls the functions implemented in the preview control UI. See [Adding a Preview UI Control](adding_a_preview_ui_control.md).

The preview controller class uses the following namespaces:

- **Sdl.FileTypeSupport.Framework**
- **Sdl.FileTypeSupport.Framework.NativeApi**
- **Sdl.FileTypeSupport.Framework.IntegrationApi**

The class must implement the following interfaces:

- [ISingleFilePreviewControl](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISingleFilePreviewControl.yml): Provides access to the preview file generated in the user's temporary files folder.
- [INavigablePreview](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.INavigablePreview.yml): Lets users navigate to segments in the preview window and defines what happens when they do.
- [IPreviewUpdatedViaRefresh](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IPreviewUpdatedViaRefresh.yml): Defines what happens before and after users click **Refresh** in the preview control.
- `IDisposable`: Disposes of the physical preview file stored temporarily under **SDLTempFileManager** when the preview closes or the application exits.

## Declare the global members

Start by declaring the following class members. Create a `_control` object for the preview UI. Add members to store the file ID and track whether the preview file is already open.

# [C#](#tab/tabid-1)
```cs
InternalPreviewControl _control; // the actual control object
private bool disposed = false; // used for properly disposing of the control        
FileId _fileId; // the actual file ID

// indicates whether a file has already been opened in the preview, 
// so we know if we should close it during a Refresh()  
bool _isFileOpen = false;
```

## Initialize the preview control UI

Next, add the following members to initialize the preview control and dispose of it correctly when the preview or application closes.

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

## Implement the members of the abstract preview control interface

The [IAbstractPreviewControl](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IAbstractPreviewControl.yml) interface lets you control preview behavior, including refresh operations. Add the following members to your preview controller class:

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

## Provide access to the temporary preview file

Add the following member to provide programmatic access to the physical preview file that is generated temporarily on the user's machine:

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

## Implement the preview document navigation

Next, implement the following members of the [INavigablePreview](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.INavigablePreview.yml) interface. These functions control what happens when the user scrolls to or selects a segment in the preview control.

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

## Dispose of the preview file

Add the following members of the `IDisposable` interface to clean up and remove temporary preview files, for example after the application closes:

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

## Implement the refresh functionality

Add the following members of the [IPreviewUpdatedViaRefresh](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IPreviewUpdatedViaRefresh.yml) interface to control what happens when the user refreshes the preview control:

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

## Register the preview controller in the file type component builder

Register the preview controller, not the user control itself, in the File Type Component Builder as follows. You can add this object below the control used for the static preview. See [Modifying the File Type Component Builder](static_modifying_the_file_type_component_builder.md).

The object **id** must match the preview set name that you added earlier. See [Modifying the File Type Component Builder](dynamic_modifying_the_file_type_component_builder.md). It must also start with the **PreviewControl_** prefix.

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

## Put it all together

The complete preview controller class looks like this:

# [C#](#tab/tabid-9)
```cs
using System;
using Sdl.FileTypeSupport.Framework;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.FileTypeSupport.Samples.SimpleText.Preview
{
    class InternalPreviewController : ISingleFilePreviewControl, INavigablePreview, IPreviewUpdatedViaRefresh, IDisposable
    {
        InternalPreviewControl _control; // the actual control object
        private bool disposed = false; // used for properly disposing of the control        
        FileId _fileId; // the actual file ID

        // indicates whether a file has already been opened in the preview, 
        // so we know if we should close it during a Refresh()  
        bool _isFileOpen = false;

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

        /// <summary>
        /// access to the temporary preview file
        /// </summary>
        public TempFileManager PreviewFile
        {
            get;
            set;
        }


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
    }
}
```

## See also

- [Adding a Preview UI Control](adding_a_preview_ui_control.md)
- [Modifying the File Type Component Builder](static_modifying_the_file_type_component_builder.md)
- [Modifying the File Type Component Builder](dynamic_modifying_the_file_type_component_builder.md)


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
