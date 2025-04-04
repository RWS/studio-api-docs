Creating the UI Editor Control for your Display Filter
====
To apply your implementation of the [IDisplayFilter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilter.yml), get a reference to the active document and from there, apply the filter through the `ApplyFilterOnSegments` method. This will invoke a filter action on the document and while iterating over each of the segments, the API will call back into your implementation where you can assert whether or not the segment should be visible in the Var:ProductName Editor.

For a more complete solution, it would be useful to make this type of functionality available from the Var:ProductName Editor, so that the end-user can choose which criteria they would like to include in the filter.

Add two files to the project that permit the user to view the control in the Studio Editor and then select and apply the filter criteria on the document:

* User Control - The user control will contain the code related to the filter criteria that will be applied on the document
* Controller - The controller is a simple class that is decorated with the extensions that are required by the API to add the user control to the studio editor; for this example, there is very little work involved with the controller.

Add a new user control to the project and name it `DisplayFilterControl`, as shown in the example below:

Include UI design elements to the user control that permit the user to choose the appropriate criteria settings for the filter that will be applied on the document. Make reference to the following images an example of how this might look in your control. For the purpose of this example we will be concentrating more on how to apply the display filter as opposed to how the UI design elements are presented in the control.

> [!NOTE]
> 
> The code related to the design controls is available in the SDK so that you can eventually make reference to it.

Once you have added the UI design elements, open the user control in code view. In order to apply your implementation of the [IDisplayFilter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilter.yml) on the active document, you will first need to get a reference to the [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml), and then subscribe to the [ActiveDocumentChanged](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_EditorController_ActiveDocumentChanged) event to ensure that you are always working with the active document from the editor. Making reference to the example code underneath, you can see that we subscribe to this event in the constructor to ensure that we have a handle on this when the control is instantiated in the Editor.

Once you have a reference to the active document, you only need to call the method [ApplyFilterOnSegments](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_ApplyFilterOnSegments_Sdl_TranslationStudioAutomation_IntegrationApi_DisplayFilters_IDisplayFilter_) with your implementation of the [IDisplayFilter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilter.yml), as follows: `ActiveDocument.ApplyFilterOnSegments(new DisplayFilter(filterSettings););` This will then invoke a filter action on the document and while iterating over each of the segments, the API will call back into to the method `EvaluateRow` of your implementation.

Subscribing to the [DocumentFilterChanged](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_DocumentFilterChanged) event is quite useful as it permits you to understand when a filter has been applied on the document and then further deduct whether or not that filter is derived from you implementation of the interface.
# [C#](#tab/tabid-1)
```cs
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.TranslationStudio.Plugins.AdvancedDisplayFilter.Models;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace TranslationStudio.Plugins.AdvancedDisplayFilter.Controls
{
    public partial class DisplayFilterControl : UserControl
    {
        #region  |  Delegates  |

        public delegate void OnApplyFilterHandler(IDisplayFilterSettings displayFilterSettings, FilteredCountsCallback result);
        public event OnApplyFilterHandler OnApplyDisplayFilter;

        public delegate void FilteredCountsCallback(int filteredSegments, int totalSegments);
        #endregion

        #region  |  Properties  |

        private static EditorController GetEditorController()
        {
            return SdlTradosStudio.Application.GetController<EditorController>();
        }
        private EditorController EditorController { get; set; }
        private Document ActiveDocument { get; set; }

        public DisplayFilters.DisplayFilter DisplayFilter { get; set; }

        public IList<IContextInfo> ContextInfoList { get; set; }

        private int TotalSegmentPairsCount { get; set; }
        private int FilteredSegmentPairsCount { get; set; }

        private DisplayFilterSettings DisplayFilterSettings
        {
            get
            {
                #region  |  get settings  |
                var settings = new DisplayFilterSettings()
                {
                    IsRegularExpression = checkBox_regularExpression.Checked,
                    IsCaseSensitive = checkBox_caseSensitive.Checked,
                    SourceText = textBox_source.Text.Trim(),
                    TargetText = textBox_target.Text.Trim(),
                    CommentText = textBox_commentText.Text.Trim(),
                    CommentAuthor = textBox_commentAuthor.Text.Trim(),
                    CommentSeverity = comboBox_commentSeverity.SelectedIndex,
                    ShowAllContent = false
                };


                foreach (var contextInfo in listView_contextInfo.SelectedItems
                    .Cast<ListViewItem>().Select(selectedItem => selectedItem.Tag as IContextInfo)
                    .Where(contextInfo => contextInfo != null
                        && !settings.ContextInfoTypes.Contains(contextInfo.ContextType)))
                {
                    settings.ContextInfoTypes.Add(contextInfo.ContextType);
                }

                foreach (ListViewItem item in this.listView_selected.Items)
                {
                    if (item.Group == GroupGeneralSelected
                        && item.Tag.ToString() == StringResources.DisplayFilterControl_ShowAllContent)
                        settings.ShowAllContent = true;
                    else if (item.Group == GroupOriginSelected)
                        settings.OriginTypes.Add(item.Tag.ToString());
                    else if (item.Group == GroupPreviousOriginSelected)
                        settings.PreviousOriginTypes.Add(item.Tag.ToString());
                    else if (item.Group == GroupStatusSelected)
                        settings.ConfirmationLevels.Add(item.Tag.ToString());
                    else if (item.Group == GroupRepetitionTypesSelected)
                        settings.RepetitionTypes.Add(item.Tag.ToString());
                    else if (item.Group == GroupReviewTypesSelected)
                        settings.SegmentReviewTypes.Add(item.Tag.ToString());
                    else if (item.Group == GroupLockingTypesSelected)
                        settings.SegmentLockingTypes.Add(item.Tag.ToString());
                    else if (item.Group == GroupContentTypesSelected)
                        settings.SegmentContentTypes.Add(item.Tag.ToString());
                }
                #endregion
                return settings;
            }
            set
            {
                if (value == null) return;
                #region  |  set settings  |

                #region  |  content panel  |

                textBox_source.Text = value.SourceText;
                textBox_target.Text = value.TargetText;

                checkBox_regularExpression.Checked = value.IsRegularExpression;
                checkBox_caseSensitive.Checked = value.IsCaseSensitive;

                #endregion

                #region  |  filters panel  |

                try
                {
                    listView_available.BeginUpdate();
                    listView_selected.BeginUpdate();

                    MoveAllListViewItems(listView_selected, listView_available);

                    foreach (var item in listView_available.Items.Cast<ListViewItem>()
                        .Where(
                            item =>
                                item.Group == GroupGeneralAvailable
                                &&
                                item.Tag.ToString() ==
                                StringResources.DisplayFilterControl_ShowAllContent
                                && value.ShowAllContent))
                    {

                        MoveListViewItem(listView_available, item, listView_selected);
                    }


                    if (value.RepetitionTypes.Any())
                    {
                        foreach (var item in listView_available.Items.Cast<ListViewItem>()
                            .Where(
                                item =>
                                    item.Group == GroupRepetitionTypesAvailable &&
                                    value.RepetitionTypes.Contains(item.Tag.ToString())))
                        {

                            MoveListViewItem(listView_available, item, listView_selected);
                        }
                    }

                    if (value.SegmentReviewTypes.Any())
                    {
                        foreach (var item in listView_available.Items.Cast<ListViewItem>()
                            .Where(
                                item =>
                                    item.Group == GroupReviewTypesAvailable &&
                                    value.SegmentReviewTypes.Contains(item.Tag.ToString())))
                        {

                            MoveListViewItem(listView_available, item, listView_selected);
                        }
                    }

                    if (value.SegmentLockingTypes.Any())
                    {
                        foreach (var item in listView_available.Items.Cast<ListViewItem>()
                            .Where(
                                item =>
                                    item.Group == GroupLockingTypesAvailable &&
                                    value.SegmentLockingTypes.Contains(item.Tag.ToString())))
                        {

                            MoveListViewItem(listView_available, item, listView_selected);
                        }
                    }

                    if (value.SegmentContentTypes.Any())
                    {
                        foreach (var item in listView_available.Items.Cast<ListViewItem>()
                            .Where(
                                item =>
                                    item.Group == GroupContentTypesAvailable &&
                                    value.SegmentContentTypes.Contains(item.Tag.ToString())))
                        {

                            MoveListViewItem(listView_available, item, listView_selected);
                        }
                    }


                    if (value.ConfirmationLevels.Any())
                    {
                        foreach (var item in listView_available.Items.Cast<ListViewItem>()
                            .Where(
                                item =>
                                    item.Group == GroupStatusAvailable &&
                                    value.ConfirmationLevels.Contains(item.Tag.ToString())))
                        {

                            MoveListViewItem(listView_available, item, listView_selected);
                        }
                    }


                    if (value.OriginTypes.Any())
                    {

                        foreach (var item in listView_available.Items.Cast<ListViewItem>()
                            .Where(
                                item =>
                                    item.Group == GroupOriginAvailable &&
                                    value.OriginTypes.Contains(item.Tag.ToString())))
                        {
                            MoveListViewItem(listView_available, item, listView_selected);
                        }
                    }

                    if (value.PreviousOriginTypes.Any())
                    {

                        foreach (var item in listView_available.Items.Cast<ListViewItem>()
                            .Where(
                                item =>
                                    item.Group == GroupPreviousOriginAvailable &&
                                    value.PreviousOriginTypes.Contains(item.Tag.ToString())))
                        {
                            MoveListViewItem(listView_available, item, listView_selected);
                        }
                    }
                }
                finally
                {
                    listView_available.EndUpdate();
                    listView_selected.EndUpdate();
                }

                #endregion

                #region  |  comments panel  |

                textBox_commentText.Text = value.CommentText;
                textBox_commentAuthor.Text = value.CommentAuthor;
                comboBox_commentSeverity.SelectedIndex = value.CommentSeverity;


                #endregion

                #region  |  context info panel  |


                foreach (ListViewItem item in listView_contextInfo.Items)
                {
                    var contextInfoItem = item.Tag as IContextInfo;
                    if (contextInfoItem != null && value.ContextInfoTypes.Contains(contextInfoItem.ContextType))
                        item.Selected = true;
                    else
                        item.Selected = false;
                }

                #endregion

                #endregion
            }
        }


        #region  |  ListView Groups  |

        internal static ListViewGroup GroupStatusAvailable { get; set; }
        internal static ListViewGroup GroupOriginAvailable { get; set; }
        internal static ListViewGroup GroupPreviousOriginAvailable { get; set; }
        internal static ListViewGroup GroupGeneralAvailable { get; set; }
        internal static ListViewGroup GroupRepetitionTypesAvailable { get; set; }
        internal static ListViewGroup GroupReviewTypesAvailable { get; set; }
        internal static ListViewGroup GroupLockingTypesAvailable { get; set; }
        internal static ListViewGroup GroupContentTypesAvailable { get; set; }


        private static ListViewGroup GroupStatusSelected { get; set; }
        private static ListViewGroup GroupOriginSelected { get; set; }
        private static ListViewGroup GroupPreviousOriginSelected { get; set; }
        private static ListViewGroup GroupGeneralSelected { get; set; }
        private static ListViewGroup GroupRepetitionTypesSelected { get; set; }
        private static ListViewGroup GroupReviewTypesSelected { get; set; }
        private static ListViewGroup GroupLockingTypesSelected { get; set; }
        private static ListViewGroup GroupContentTypesSelected { get; set; }

        #endregion


        #endregion

        #region  |  Constructor  |
        public DisplayFilterControl()
        {
            InitializeComponent();

            AddGroupsToOriginTypeListview();

            InitializeSettings();


            listView_available.ListViewItemSorter = new ListViewItemComparer();
            listView_selected.ListViewItemSorter = new ListViewItemComparer();


            EditorController = GetEditorController();
            EditorController.ActiveDocumentChanged += EditorController_ActiveDocumentChanged;

            ActiveDocument = EditorController.ActiveDocument;

            OnApplyDisplayFilter += ApplyDisplayFilter;


            listView_available.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Collapsed);
            listView_selected.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Normal);

            listView_available.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Normal, listView_available.Groups[1]);
            listView_available.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Normal, listView_available.Groups[2]);

        }



        #endregion


        private void InitializeSettings()
        {
            #region  |  content panel  |

            textBox_source.Text = string.Empty;
            textBox_target.Text = string.Empty;

            checkBox_regularExpression.Checked = false;
            checkBox_caseSensitive.Checked = false;

            #endregion

            #region  |  filters panel  |

            try
            {
                listView_available.BeginUpdate();
                listView_selected.BeginUpdate();


                listView_available.Items.Clear();
                listView_selected.Items.Clear();

                var _item =
                    listView_available.Items.Add(
                        StringResources.DisplayFilterControl_Show_All_Content);
                _item.Group = GroupGeneralAvailable;
                _item.Tag = StringResources.DisplayFilterControl_ShowAllContent;


                foreach (var type in Enum.GetValues(typeof(DisplayFilterSettings.RepetitionType)))
                {
                    var item =
                        listView_available.Items.Add(Helper.GetTypeName((DisplayFilterSettings.RepetitionType)type));

                    item.Group = GroupRepetitionTypesAvailable;
                    item.Tag = type;
                }


                foreach (var type in Enum.GetValues(typeof(DisplayFilterSettings.SegmentReviewType)))
                {
                    var item =
                        listView_available.Items.Add(Helper.GetTypeName((DisplayFilterSettings.SegmentReviewType)type));

                    item.Group = GroupReviewTypesAvailable;
                    item.Tag = type;
                }


                foreach (var type in Enum.GetValues(typeof(DisplayFilterSettings.SegmentLockingType)))
                {
                    var item =
                        listView_available.Items.Add(Helper.GetTypeName((DisplayFilterSettings.SegmentLockingType)type));

                    item.Group = GroupLockingTypesAvailable;
                    item.Tag = type;
                }


                foreach (var type in Enum.GetValues(typeof(DisplayFilterSettings.SegmentContentType)))
                {
                    var item =
                        listView_available.Items.Add(Helper.GetTypeName((DisplayFilterSettings.SegmentContentType)type));

                    item.Group = GroupContentTypesAvailable;
                    item.Tag = type;
                }


                foreach (var type in Enum.GetValues(typeof(DisplayFilterSettings.ConfirmationLevel)))
                {
                    var item =
                        listView_available.Items.Add(Helper.GetTypeName((DisplayFilterSettings.ConfirmationLevel)type));

                    item.Group = GroupStatusAvailable;
                    item.Tag = type;
                }

                foreach (var type in Enum.GetValues(typeof(DisplayFilterSettings.OriginType)))
                {
                    if (type.ToString() == "None")
                        continue;

                    var item =
                        listView_available.Items.Add(Helper.GetTypeName((DisplayFilterSettings.OriginType)type));

                    item.Group = GroupOriginAvailable;

                    item.Tag = type;
                }
                foreach (var type in Enum.GetValues(typeof(DisplayFilterSettings.OriginType)))
                {
                    if (type.ToString() == "None")
                        continue;

                    var item =
                        listView_available.Items.Add(Helper.GetTypeName((DisplayFilterSettings.OriginType)type));

                    item.Group = GroupPreviousOriginAvailable;

                    item.Tag = type;
                }
                listView_available.Items[0].Selected = true;
            }
            finally
            {
                listView_available.EndUpdate();
                listView_selected.EndUpdate();
            }

            #endregion

            #region  |  comments panel  |

            textBox_commentText.Text = string.Empty;
            textBox_commentAuthor.Text = string.Empty;

            comboBox_commentSeverity.Items.Clear();
            foreach (var severity in Enum.GetValues(typeof(DisplayFilterSettings.CommentSeverityType)))
                comboBox_commentSeverity.Items.Add(severity.ToString());

            comboBox_commentSeverity.SelectedIndex = 0;

            #endregion

            #region  |  context info panel  |

            PopulateContextInfoList();

            #endregion

            #region  |  filter status counter  |

            // initialize the filter status counter
            UpdateFilteredCountDisplay(0, 0);

            #endregion

            InitializeTabPageIcons();
        }

        public void ApplyFilter()
        {
            if (OnApplyDisplayFilter != null)
            {
                var result = new FilteredCountsCallback(UpdateFilteredCountDisplay);
                OnApplyDisplayFilter(DisplayFilterSettings, result);
            }
        }

        public void ClearFilter()
        {

            InitializeSettings();

            ApplyFilter();
        }

        public void SaveFilter()
        {
            var saveSettingsDialog = new SaveFileDialog
            {
                Title = StringResources.DisplayFilterControl_SaveFilter_Save_Filter_Settings,
                Filter = StringResources.DisplayFilterControl_Settings_XML_File_sdladfsettings
            };
            if (saveSettingsDialog.ShowDialog() != DialogResult.OK) return;

            var settingsXml = DisplayFilterSerializer.SerializeSettings(DisplayFilterSettings);
            using (var sw = new StreamWriter(saveSettingsDialog.FileName, false, Encoding.UTF8))
            {
                sw.Write(settingsXml);
                sw.Flush();
            }
        }

        public void LoadFilter()
        {
            var loadSettingsDialog = new OpenFileDialog
            {
                Title = StringResources.DisplayFilterControl_LoadFilter_Load_Filter_Settings,
                Filter = StringResources.DisplayFilterControl_Settings_XML_File_sdladfsettings
            };


            if (loadSettingsDialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                // read in the xml content
                string settingsXml;
                using (var sr = new StreamReader(loadSettingsDialog.FileName, Encoding.UTF8))
                    settingsXml = sr.ReadToEnd();

                // deserialize the to the settings xml
                DisplayFilterSettings = DisplayFilterSerializer.DeserializeSettings<DisplayFilterSettings>(settingsXml);

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ActiveDocument_DocumentFilterChanged(object sender, DocumentFilterEventArgs e)
        {
            if (e.DisplayFilter == null
                || e.DisplayFilter.GetType() != typeof(DisplayFilters.DisplayFilter))
                InitializeSettings();


            UpdateFilteredCountDisplay(e.FilteredSegmentPairsCount, e.TotalSegmentPairsCount);
        }
        private void EditorController_ActiveDocumentChanged(object sender, DocumentEventArgs e)
        {
            InitializeSettings();

            if (ActiveDocument != null)
                ActiveDocument.DocumentFilterChanged -= ActiveDocument_DocumentFilterChanged;

            // get a reference to the active document            
            ActiveDocument = e.Document;

            if (ActiveDocument != null)
            {
                ActiveDocument.DocumentFilterChanged += ActiveDocument_DocumentFilterChanged;

                SetContextInfoList();
                PopulateContextInfoList();

                if (ActiveDocument.DisplayFilter != null &&
                    ActiveDocument.DisplayFilter.GetType() == typeof(DisplayFilters.DisplayFilter))
                {
                    //invalidate UI with display settings recovered from the active document
                    DisplayFilterSettings = ((DisplayFilters.DisplayFilter)ActiveDocument.DisplayFilter).Settings as DisplayFilterSettings;
                }

                UpdateFilteredCountDisplay(ActiveDocument.FilteredSegmentPairsCount, ActiveDocument.TotalSegmentPairsCount);
            }
        }
        private void ApplyDisplayFilter(IDisplayFilterSettings displayFilterSettings, FilteredCountsCallback result)
        {
            if (ActiveDocument == null)
                return;

            DisplayFilter = new DisplayFilters.DisplayFilter(displayFilterSettings, ActiveDocument);

            ActiveDocument.ApplyFilterOnSegments(DisplayFilter);

            result.Invoke(ActiveDocument.FilteredSegmentPairsCount, ActiveDocument.TotalSegmentPairsCount);
        }
        private void UpdateFilteredCountDisplay(int filteredSegments, int totalSegments)
        {
            FilteredSegmentPairsCount = filteredSegments;
            TotalSegmentPairsCount = totalSegments;

            label_filterStatusBarMessage.Text =
                string.Format(StringResources.DisplayFilterControl_Filtered_of_rows
                    , filteredSegments, totalSegments);

            UpdateFilterExpression();
            CheckEnabledFilterIcons();
        }
        private void UpdateFilterExpression()
        {
            filterExpressionControl.ClearItems();

            if (DisplayFilterSettings.SourceText != string.Empty
                || DisplayFilterSettings.TargetText != string.Empty)
            {
                if (DisplayFilterSettings.SourceText != string.Empty)
                    filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Source + ":\"" + DisplayFilterSettings.SourceText + "\"");
                if (DisplayFilterSettings.TargetText != string.Empty)
                    filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Target + ":\"" + DisplayFilterSettings.TargetText + "\"");

                if (DisplayFilterSettings.IsRegularExpression)
                    filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Regular_Expression + ":\"" + DisplayFilterSettings.IsRegularExpression + "\"");
                if (DisplayFilterSettings.IsCaseSensitive)
                    filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Case_Sensitive + ":\"" + DisplayFilterSettings.IsCaseSensitive + "\"");
            }

            if (DisplayFilterSettings.ShowAllContent)
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Show_All_Content
                    + ":\"" + DisplayFilterSettings.ShowAllContent + "\"");

            if (DisplayFilterSettings.ConfirmationLevels.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Status + ":"
                    + "(" + DisplayFilterSettings.ConfirmationLevels.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + Helper.GetTypeName((DisplayFilterSettings.ConfirmationLevel)Enum.Parse(
                        typeof(DisplayFilterSettings.ConfirmationLevel), item, true))) + ")");

            if (DisplayFilterSettings.OriginTypes.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Origin + ":"
                    + "(" + DisplayFilterSettings.OriginTypes.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + Helper.GetTypeName((DisplayFilterSettings.OriginType)Enum.Parse(
                        typeof(DisplayFilterSettings.OriginType), item, true))) + ")");

            if (DisplayFilterSettings.PreviousOriginTypes.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Previous_Origin + ":"
                    + "(" + DisplayFilterSettings.PreviousOriginTypes.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + Helper.GetTypeName((DisplayFilterSettings.OriginType)Enum.Parse(
                        typeof(DisplayFilterSettings.OriginType), item, true))) + ")");

            if (DisplayFilterSettings.RepetitionTypes.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Repetitions + ":"
                    + "(" + DisplayFilterSettings.RepetitionTypes.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + Helper.GetTypeName((DisplayFilterSettings.RepetitionType)Enum.Parse(
                        typeof(DisplayFilterSettings.RepetitionType), item, true))) + ")");

            if (DisplayFilterSettings.SegmentReviewTypes.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Segment_Review + ":"
                    + "(" + DisplayFilterSettings.SegmentReviewTypes.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + Helper.GetTypeName((DisplayFilterSettings.SegmentReviewType)Enum.Parse(
                        typeof(DisplayFilterSettings.SegmentReviewType), item, true))) + ")");

            if (DisplayFilterSettings.SegmentLockingTypes.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Segment_Locking + ":"
                    + "(" + DisplayFilterSettings.SegmentLockingTypes.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + Helper.GetTypeName((DisplayFilterSettings.SegmentLockingType)Enum.Parse(
                        typeof(DisplayFilterSettings.SegmentLockingType), item, true))) + ")");

            if (DisplayFilterSettings.SegmentContentTypes.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Segment_Content + ":"
                    + "(" + DisplayFilterSettings.SegmentContentTypes.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + Helper.GetTypeName((DisplayFilterSettings.SegmentContentType)Enum.Parse(
                        typeof(DisplayFilterSettings.SegmentContentType), item, true))) + ")");


            if (DisplayFilterSettings.CommentText != string.Empty)
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Comment_text + ":\"" + DisplayFilterSettings.CommentText + "\"");
            if (DisplayFilterSettings.CommentAuthor != string.Empty)
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Comment_author + ":\"" + DisplayFilterSettings.CommentAuthor + "\"");
            if (DisplayFilterSettings.CommentSeverity > 0)
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Comment_severity + ":\"" + (DisplayFilterSettings.CommentSeverityType)DisplayFilterSettings.CommentSeverity + "\"");


            if (DisplayFilterSettings.ContextInfoTypes.Any())
                filterExpressionControl.AddItem(StringResources.DisplayFilterControl_Document_structure + ":"
                    + "(" + DisplayFilterSettings.ContextInfoTypes.Aggregate(string.Empty, (current, item) => current
                    + (current != string.Empty ? " " + "|" + " " : string.Empty)
                    + ContextInfoList.FirstOrDefault(a => a.ContextType == item).DisplayCode) + ")");
        }



        #region  |  Helpers  |


        #region  |  Tab icons  |

        private void InitializeTabPageIcons()
        {
            tabPage_content.ImageIndex = -1;
            tabPage_filters.ImageIndex = -1;
            tabPage_comments.ImageIndex = -1;
            tabPage_contextInfo.ImageIndex = -1;
        }
        private void CheckEnabledFilterIcons()
        {
            if (ActiveDocument != null)
            {
                if (ActiveDocument.DisplayFilter != null
                    && ActiveDocument.DisplayFilter.GetType() == typeof(DisplayFilters.DisplayFilter))
                {
                    var settings = ((DisplayFilters.DisplayFilter)ActiveDocument.DisplayFilter).Settings;

                    InvalidateIconsFilterApplied_contentTab(settings);
                    InvalidateIconsFilterApplied_filtersTab(settings);
                    InvalidateIconsFilterApplied_commentsTab(settings);
                    InvalidateIconsFilterApplied_contextInfoTab(settings);

                    SetStatusBackgroundColorCode(IsFilterApplied(settings));
                }
                else
                {
                    SetStatusBackgroundColorCode(false);

                    InvalidateIconsFilterApplied(tabPage_content);
                    InvalidateIconsFilterApplied(tabPage_filters);
                    InvalidateIconsFilterApplied(tabPage_comments);
                    InvalidateIconsFilterApplied(tabPage_contextInfo);
                }
            }
        }

        private void SetStatusBackgroundColorCode(bool visible)
        {
            panel_filterStatusBarImage.Visible = visible;
            panel_filterStatusBar.BackColor = visible ? SystemColors.GradientInactiveCaption : Color.Transparent;
        }
        private bool IsFilterApplied(IDisplayFilterSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.SourceText)
                || !string.IsNullOrEmpty(settings.TargetText)
                || settings.ContextInfoTypes.Any()
                || settings.SegmentReviewTypes.Any()
                || settings.ConfirmationLevels.Any()
                || settings.OriginTypes.Any()
                || settings.PreviousOriginTypes.Any()
                || settings.RepetitionTypes.Any()
                || settings.SegmentContentTypes.Any()
                || settings.SegmentLockingTypes.Any()
                || settings.SegmentReviewTypes.Any()
                || settings.ShowAllContent
                || !string.IsNullOrEmpty(settings.CommentText)
                || !string.IsNullOrEmpty(settings.CommentAuthor)
                || settings.CommentSeverity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void InvalidateIconsFilterApplied(TabPage tabPage)
        {
            tabPage.ImageIndex = -1;
        }
        private void InvalidateIconsFilterApplied_contentTab(IDisplayFilterSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.SourceText)
                || !string.IsNullOrEmpty(settings.TargetText))
            {
                tabPage_content.ImageIndex = 0;
            }
            else
            {
                tabPage_content.ImageIndex = -1;
            }

        }
        private void InvalidateIconsFilterApplied_filtersTab(IDisplayFilterSettings settings)
        {

            if (settings.SegmentReviewTypes.Any()
                || settings.ConfirmationLevels.Any()
                || settings.OriginTypes.Any()
                || settings.PreviousOriginTypes.Any()
                || settings.RepetitionTypes.Any()
                || settings.SegmentContentTypes.Any()
                || settings.SegmentLockingTypes.Any()
                || settings.SegmentReviewTypes.Any()
                || settings.ShowAllContent)
            {
                tabPage_filters.ImageIndex = 0;
            }
            else
            {
                tabPage_filters.ImageIndex = -1;
            }

        }
        private void InvalidateIconsFilterApplied_commentsTab(IDisplayFilterSettings settings)
        {

            if (!string.IsNullOrEmpty(settings.CommentText)
                || !string.IsNullOrEmpty(settings.CommentAuthor)
                || settings.CommentSeverity > 0)
            {
                tabPage_comments.ImageIndex = 0;
            }
            else
            {
                tabPage_comments.ImageIndex = -1;
            }

        }
        private void InvalidateIconsFilterApplied_contextInfoTab(IDisplayFilterSettings settings)
        {
            if (settings.ContextInfoTypes.Any())
            {
                tabPage_contextInfo.ImageIndex = 0;
            }
            else
            {
                tabPage_contextInfo.ImageIndex = -1;
            }

        }
        private void InvalidateIconsFilterEdited(TabPage tabPage)
        {

            if (ActiveDocument != null && ActiveDocument.DisplayFilter != null
                && ActiveDocument.DisplayFilter.GetType() == typeof(DisplayFilters.DisplayFilter))
            {
                var settings = ((DisplayFilters.DisplayFilter)ActiveDocument.DisplayFilter).Settings;

                if (tabPage == tabPage_content
                    && (!string.IsNullOrEmpty(settings.SourceText)
                    || !string.IsNullOrEmpty(settings.TargetText)))
                {
                    var item1 = textBox_source.Text + ", " + textBox_target.Text + ", " +
                                checkBox_regularExpression.Checked + ", " + checkBox_caseSensitive.Checked;

                    var item2 = settings.SourceText + ", " + settings.TargetText + ", " +
                                settings.IsRegularExpression + ", " + settings.IsCaseSensitive;

                    tabPage.ImageIndex = string.CompareOrdinal(item1, item2) == 0 ? 0 : 1;
                }
                else if (tabPage == tabPage_filters
                    && (settings.SegmentReviewTypes.Any()
                    || settings.ConfirmationLevels.Any()
                    || settings.OriginTypes.Any()
                    || settings.PreviousOriginTypes.Any()
                    || settings.RepetitionTypes.Any()
                    || settings.SegmentContentTypes.Any()
                    || settings.SegmentLockingTypes.Any()
                    || settings.SegmentReviewTypes.Any()
                    || settings.ShowAllContent))
                {

                    var list1 = new List<string> { DisplayFilterSettings.ShowAllContent.ToString() };
                    list1.AddRange(DisplayFilterSettings.OriginTypes);
                    list1.AddRange(DisplayFilterSettings.PreviousOriginTypes);
                    list1.AddRange(DisplayFilterSettings.ConfirmationLevels);
                    list1.AddRange(DisplayFilterSettings.RepetitionTypes);
                    list1.AddRange(DisplayFilterSettings.SegmentReviewTypes);
                    list1.AddRange(DisplayFilterSettings.SegmentLockingTypes);
                    list1.AddRange(DisplayFilterSettings.SegmentContentTypes);


                    var list2 = new List<string> { settings.ShowAllContent.ToString() };
                    list2.AddRange(settings.OriginTypes);
                    list2.AddRange(settings.PreviousOriginTypes);
                    list2.AddRange(settings.ConfirmationLevels);
                    list2.AddRange(settings.RepetitionTypes);
                    list2.AddRange(settings.SegmentReviewTypes);
                    list2.AddRange(settings.SegmentLockingTypes);
                    list2.AddRange(settings.SegmentContentTypes);

                    tabPage.ImageIndex = string.Join(", ", list1) == string.Join(", ", list2)
                        ? 0
                        : 1;
                }
                else if (tabPage == tabPage_comments
                    && (!string.IsNullOrEmpty(settings.CommentText)
                    || !string.IsNullOrEmpty(settings.CommentAuthor)
                    || settings.CommentSeverity > 0))
                {
                    var item1 = textBox_commentText.Text + ", " + textBox_commentAuthor.Text + ", " +
                                comboBox_commentSeverity.SelectedIndex;

                    var item2 = settings.CommentText + ", " + settings.CommentAuthor + ", " +
                                settings.CommentSeverity;

                    tabPage.ImageIndex = string.CompareOrdinal(item1, item2) == 0 ? 0 : 1;
                }
                else if (tabPage == tabPage_contextInfo && settings.ContextInfoTypes.Any())
                {
                    // check if identical selection
                    var list = new List<string>();
                    foreach (var contextInfo in listView_contextInfo.SelectedItems
                        .Cast<ListViewItem>().Select(selectedItem => selectedItem.Tag as IContextInfo)
                        .Where(contextInfo => contextInfo != null
                                              && !list.Contains(contextInfo.ContextType)))
                    {
                        list.Add(contextInfo.ContextType);
                    }
                    tabPage.ImageIndex = string.Join(", ", list) == string.Join(", ", settings.ContextInfoTypes)
                        ? 0
                        : 1;

                }
                else
                    tabPage.ImageIndex = 1;
            }
            else
            {
                tabPage.ImageIndex = 1;
            }
        }


        #endregion

        #region  |  Filter Attributes group  |

        private void CheckEnabledActionButtons()
        {
            var add = true;
            var remove = true;
            var removeAll = true;

            if (listView_available.SelectedItems.Count == 0)
                add = false;

            if (listView_selected.SelectedItems.Count == 0)
                remove = false;

            if (listView_selected.Items.Count == 0)
                removeAll = false;

            button_add.Enabled = add;
            button_remove.Enabled = remove;
            button_removeAll.Enabled = removeAll;
        }
        private void MoveSelectedListViewItem(ListView from, ListView to)
        {
            if (from.SelectedItems.Count > 0)
            {
                var itemIndex = 0;
                foreach (ListViewItem itemFrom in from.SelectedItems)
                {
                    itemIndex = itemFrom.Index;

                    var itemTo = to.Items.Add((ListViewItem)itemFrom.Clone());

                    AssignOriginTypeListViewItemGroup(itemTo, itemFrom.Group);


                    itemFrom.Remove();
                }

                SelectDefaultItem(from, itemIndex);

                listView_available.Sort();
                listView_selected.Sort();

                CheckEnabledActionButtons();
            }
        }
        private static void SelectDefaultItem(ListView from, int itemIndex)
        {
            if (from.Items.Count > 0)
            {
                foreach (var item in from.Items.Cast<ListViewItem>())
                    item.Selected = false;

                if (from.Items.Count - 1 < itemIndex)
                    itemIndex--;

                from.Items[itemIndex].Selected = true;
            }
        }
        private void MoveListViewItem(ListView from, ListViewItem itemFrom, ListView to)
        {
            var itemIndex = itemFrom.Index;

            var itemTo = to.Items.Add((ListViewItem)itemFrom.Clone());

            AssignOriginTypeListViewItemGroup(itemTo, itemFrom.Group);

            itemFrom.Remove();

            SelectDefaultItem(from, itemIndex);

            listView_available.Sort();
            listView_selected.Sort();

            CheckEnabledActionButtons();

        }
        private void MoveAllListViewItems(ListView from, ListView to)
        {
            foreach (ListViewItem itemFrom in from.Items)
            {
                var itemTo = to.Items.Add((ListViewItem)itemFrom.Clone());
                AssignOriginTypeListViewItemGroup(itemTo, itemFrom.Group);

                itemFrom.Remove();
            }

            listView_available.Sort();
            listView_selected.Sort();

            CheckEnabledActionButtons();
        }
        private void AssignOriginTypeListViewItemGroup(ListViewItem itemTo, ListViewGroup fromGroup)
        {
            if (itemTo.ListView.Equals(listView_available)
                || itemTo.ListView.Equals(listView_selected))
            {
                if (itemTo.ListView.Equals(listView_available))
                {
                    if (fromGroup == GroupOriginSelected)
                        itemTo.Group = GroupOriginAvailable;
                    else if (fromGroup == GroupPreviousOriginSelected)
                        itemTo.Group = GroupPreviousOriginAvailable;
                    else if (fromGroup == GroupStatusSelected)
                        itemTo.Group = GroupStatusAvailable;
                    else if (fromGroup == GroupGeneralSelected)
                        itemTo.Group = GroupGeneralAvailable;
                    else if (fromGroup == GroupRepetitionTypesSelected)
                        itemTo.Group = GroupRepetitionTypesAvailable;
                    else if (fromGroup == GroupReviewTypesSelected)
                        itemTo.Group = GroupReviewTypesAvailable;
                    else if (fromGroup == GroupLockingTypesSelected)
                        itemTo.Group = GroupLockingTypesAvailable;
                    else if (fromGroup == GroupContentTypesSelected)
                        itemTo.Group = GroupContentTypesAvailable;
                }
                else
                {
                    if (fromGroup == GroupOriginAvailable)
                        itemTo.Group = GroupOriginSelected;
                    else if (fromGroup == GroupPreviousOriginAvailable)
                        itemTo.Group = GroupPreviousOriginSelected;
                    else if (fromGroup == GroupStatusAvailable)
                        itemTo.Group = GroupStatusSelected;
                    else if (fromGroup == GroupGeneralAvailable)
                        itemTo.Group = GroupGeneralSelected;
                    else if (fromGroup == GroupRepetitionTypesAvailable)
                        itemTo.Group = GroupRepetitionTypesSelected;
                    else if (fromGroup == GroupReviewTypesAvailable)
                        itemTo.Group = GroupReviewTypesSelected;
                    else if (fromGroup == GroupLockingTypesAvailable)
                        itemTo.Group = GroupLockingTypesSelected;
                    else if (fromGroup == GroupContentTypesAvailable)
                        itemTo.Group = GroupContentTypesSelected;
                }
            }
        }
        private void AddGroupsToOriginTypeListview()
        {

            listView_available.ShowGroups = true;
            listView_selected.ShowGroups = true;

            GroupGeneralAvailable = new ListViewGroup(StringResources.DisplayFilterControl_General);
            GroupStatusAvailable = new ListViewGroup(StringResources.DisplayFilterControl_Status);
            GroupOriginAvailable = new ListViewGroup(StringResources.DisplayFilterControl_Origin);
            GroupPreviousOriginAvailable = new ListViewGroup(StringResources.DisplayFilterControl_Previous_Origin);
            GroupRepetitionTypesAvailable = new ListViewGroup(StringResources.DisplayFilterControl_Repetitions);
            GroupReviewTypesAvailable = new ListViewGroup(StringResources.DisplayFilterControl_Segment_Review);
            GroupLockingTypesAvailable = new ListViewGroup(StringResources.DisplayFilterControl_Segment_Locking);
            GroupContentTypesAvailable = new ListViewGroup(StringResources.DisplayFilterControl_Segment_Content);

            listView_available.Groups.Add(GroupGeneralAvailable);
            listView_available.Groups.Add(GroupStatusAvailable);
            listView_available.Groups.Add(GroupOriginAvailable);
            listView_available.Groups.Add(GroupPreviousOriginAvailable);
            listView_available.Groups.Add(GroupRepetitionTypesAvailable);
            listView_available.Groups.Add(GroupReviewTypesAvailable);
            listView_available.Groups.Add(GroupLockingTypesAvailable);
            listView_available.Groups.Add(GroupContentTypesAvailable);

            GroupGeneralSelected = new ListViewGroup(StringResources.DisplayFilterControl_General);
            GroupStatusSelected = new ListViewGroup(StringResources.DisplayFilterControl_Status);
            GroupOriginSelected = new ListViewGroup(StringResources.DisplayFilterControl_Origin);
            GroupPreviousOriginSelected = new ListViewGroup(StringResources.DisplayFilterControl_Previous_Origin);
            GroupRepetitionTypesSelected = new ListViewGroup(StringResources.DisplayFilterControl_Repetitions);
            GroupReviewTypesSelected = new ListViewGroup(StringResources.DisplayFilterControl_Segment_Review);
            GroupLockingTypesSelected = new ListViewGroup(StringResources.DisplayFilterControl_Segment_Locking);
            GroupContentTypesSelected = new ListViewGroup(StringResources.DisplayFilterControl_Segment_Content);

            listView_selected.Groups.Add(GroupGeneralSelected);
            listView_selected.Groups.Add(GroupStatusSelected);
            listView_selected.Groups.Add(GroupOriginSelected);
            listView_selected.Groups.Add(GroupPreviousOriginSelected);
            listView_selected.Groups.Add(GroupRepetitionTypesSelected);
            listView_selected.Groups.Add(GroupReviewTypesSelected);
            listView_selected.Groups.Add(GroupLockingTypesSelected);
            listView_selected.Groups.Add(GroupContentTypesSelected);
        }

        #endregion

        #region  |  Context info  |
        private void SetContextInfoList()
        {
            ContextInfoList = new List<IContextInfo>();
            foreach (var segmentPair in ActiveDocument.SegmentPairs)
            {
                var contexts = segmentPair.GetParagraphUnitProperties().Contexts;
                if (contexts == null) continue;
                foreach (var contextInfo in contexts.Contexts
                   .Where(a => a.DisplayCode != null)
                   .Where(contextInfo => ContextInfoList.All(a => a.ContextType != contextInfo.ContextType)))
                {
                    ContextInfoList.Add(contextInfo);
                }
            }
        }
        private void PopulateContextInfoList()
        {
            try
            {
                listView_contextInfo.BeginUpdate();
                listView_contextInfo.Items.Clear();

                if (ContextInfoList == null) return;
                foreach (var contextInfo in ContextInfoList)
                {
                    var item = listView_contextInfo.Items.Add(contextInfo.DisplayCode);
                    item.SubItems.Add(contextInfo.DisplayName);
                    item.SubItems.Add(contextInfo.Description ?? contextInfo.ContextType);
                    item.BackColor = contextInfo.DisplayColor;
                    item.UseItemStyleForSubItems = false;
                    item.Tag = contextInfo;
                }
            }
            finally
            {
                listView_contextInfo.EndUpdate();
            }

            UpdatedContextInfoSelectedStatusCount();
        }

        #endregion

        #endregion

        #region  |  ToolbarStrip events  |

        private void toolStripButton_applyFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void toolStripButton_clearFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }

        private void toolStripButton_saveFilter_Click(object sender, EventArgs e)
        {
            SaveFilter();
        }

        private void toolStripButton_loadFilter_Click(object sender, EventArgs e)
        {
            LoadFilter();
        }

        #endregion

        #region  |  Content tab events  |

        private void textBox_source_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ApplyFilter();
        }

        private void textBox_target_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ApplyFilter();
        }


        private void textBox_source_TextChanged(object sender, EventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_content);
        }

        private void textBox_target_TextChanged(object sender, EventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_content);
        }

        private void checkBox_regularExpression_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_content);
        }

        private void checkBox_caseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_content);
        }

        #endregion

        #region  |  Filter Attributes tab events  |


        private void button_add_Click(object sender, EventArgs e)
        {
            MoveSelectedListViewItem(listView_available, listView_selected);
            InvalidateIconsFilterEdited(tabPage_filters);
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            MoveSelectedListViewItem(listView_selected, listView_available);
            InvalidateIconsFilterEdited(tabPage_filters);
        }

        private void button_removeAll_Click(object sender, EventArgs e)
        {
            MoveAllListViewItems(listView_selected, listView_available);
            InvalidateIconsFilterEdited(tabPage_filters);
        }

        private void listView_available_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckEnabledActionButtons();
        }

        private void listView_selected_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckEnabledActionButtons();
        }


        #endregion

        #region  |  Comments tab events  |

        private void textBox_commentText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ApplyFilter();
        }

        private void textBox_commentAuthor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ApplyFilter();
        }



        private void textBox_commentText_TextChanged(object sender, EventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_comments);
        }

        private void textBox_commentAuthor_TextChanged(object sender, EventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_comments);
        }

        private void comboBox_commentSeverity_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_comments);
        }

        #endregion

        #region  |  Contextinfo tab events  |
        private void linkLabel_contextInfoClearSelection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView_contextInfo.BeginUpdate();
            foreach (ListViewItem item in listView_contextInfo.Items)
                item.Selected = false;
            listView_contextInfo.EndUpdate();

            if (ActiveDocument != null && ActiveDocument.DisplayFilter != null
                && ActiveDocument.DisplayFilter.GetType() == typeof(DisplayFilters.DisplayFilter))
            {
                var settings = ((DisplayFilters.DisplayFilter)ActiveDocument.DisplayFilter).Settings;

                if (settings.ContextInfoTypes.Any())
                {
                    tabPage_contextInfo.ImageIndex = 1;
                }
                else
                {
                    tabPage_contextInfo.ImageIndex = -1;
                }
            }
            else
            {
                tabPage_contextInfo.ImageIndex = -1;
            }

            UpdatedContextInfoSelectedStatusCount();
        }

        private void listView_contextInfo_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            InvalidateIconsFilterEdited(tabPage_contextInfo);
            UpdatedContextInfoSelectedStatusCount();
        }

        private void UpdatedContextInfoSelectedStatusCount()
        {
            label_contextInfoSelected.Text = string.Format("Selected: {0}", listView_contextInfo.SelectedItems.Count);
        }

        #endregion



        private void listView_contextInfo_Resize(object sender, EventArgs e)
        {
            var width = ((ListView)sender).Width - 20 - SystemInformation.VerticalScrollBarWidth;

            columnHeader_code.Width = Convert.ToInt32(width * .2);
            columnHeader_name.Width = Convert.ToInt32(width * .4);
            columnHeader_description.Width = Convert.ToInt32(width * .45);
        }

        private void listView_available_Resize(object sender, EventArgs e)
        {
            var width = ((ListView)sender).Width - 20 - SystemInformation.VerticalScrollBarWidth;
            columnHeader_filtersAvailable_name.Width = width;            
        }

        private void listView_selected_Resize(object sender, EventArgs e)
        {
            var width = ((ListView)sender).Width - 20 - SystemInformation.VerticalScrollBarWidth;
            columnHeader_filtersSelected_name.Width = width;
        }


    }




}
```
****

To load the user control in the Var:ProductName Editor, you will need to add a controller and decorate it with the appropriate extensions that will provide the API with enough information to identify and then load it.

Create a new class and name it `DisplayFilterController`.

Add the following code to the `DisplayFilterController.cs` class; it will provide the API with the required information to identify the control and load it in the Editor, docking it at the bottom of the view.

# [C#](#tab/tabid-2)
```cs
using System;
using System.Linq;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;

namespace Community.AdvancedDisplayFilter.Controls
{
    [ViewPart(
    Id = "CommunityAdvancedDisplayFilterViewPart",
    Name = "AdvancedDisplayFilterViewPart_Name",
    Description = "AdvancedDisplayFilterViewPart_Description",
    Icon = "AdvancedDisplayFiltersIcon")]
    [ViewPartLayout(
        typeof(EditorController), Dock = DockType.Right)]
    public class DisplayFilterController : AbstractViewPartController
    {
        private readonly DisplayFilterControl _control = new DisplayFilterControl();

        protected override Control GetContentControl()
        {
            return _control;
        }

        protected override void Initialize()
        {
        }
    }
}
```
*****
