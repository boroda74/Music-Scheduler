using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public partial class LibraryReportsPlugin : PluginWindowTemplate
    {
        public class LibraryReportsPreset
        {
            public string name = null;
            public string[] groupingNames;
            public string[] functionNames;
            public FunctionType[] functionTypes;
            public string[] parameterNames;
            public string[] parameter2Names;
            public bool totals;

            public override string ToString()
            {
                if (name != null)
                    return name;

                string representation = "";

                if (groupingNames.Length > 0)
                    representation = groupingNames[0];

                for (int i = 1; i < groupingNames.Length; i++)
                {
                    representation += ", " + groupingNames[i];
                }


                if (representation == "" && functionNames.Length > 0)
                    representation = functionNames[0];
                else if (functionNames.Length > 0)
                    representation += ", " + functionNames[0];

                for (int i = 1; i < functionNames.Length; i++)
                {
                    representation += ", " + functionNames[i];
                }


                return representation;
            }
        }

        private class AggregatedTags : SortedDictionary<string, Plugin.ConvertStringsResults[]>
        {
            public void add(string url, string[] groupingValues, string[] functionValues, List<FunctionType> functionTypes, string[] parameter2Values, bool totals)
            {
                string composedGroupings;
                Plugin.ConvertStringsResults[] aggregatedValues;


                if (groupingValues.Length == 0)
                {
                    composedGroupings = "";
                }
                else
                {
                    composedGroupings = String.Join("" + Plugin.MultipleItemsSplitterId, groupingValues);

                    if (totals)
                        for (int i = groupingValues.Length - 1; i >= 0; i--)
                        {
                            groupingValues[i] = Plugin.TotalsString;
                            add(null, groupingValues, functionValues, functionTypes, parameter2Values, false);
                        }
                }


                if (!TryGetValue(composedGroupings, out aggregatedValues))
                {
                    aggregatedValues = new Plugin.ConvertStringsResults[functionValues.Length + 1];

                    for (int i = 0; i < functionValues.Length; i++)
                    {
                        aggregatedValues[i] = new Plugin.ConvertStringsResults(1);

                        if (functionTypes[i] == FunctionType.Minimum)
                            aggregatedValues[i].result1f = double.MaxValue;
                        else if (functionTypes[i] == FunctionType.Maximum)
                            aggregatedValues[i].result1f = double.MinValue;
                    }

                    aggregatedValues[functionValues.Length] = new Plugin.ConvertStringsResults(1);

                    Add(composedGroupings, aggregatedValues);
                }

                Plugin.ConvertStringsResults currentFunctionValue;

                for (int i = 0; i < functionValues.Length + 1; i++)
                {
                    if (i == functionValues.Length) //It are URLs
                    {
                        if (url != null)
                        {
                            object temp = null;

                            if (!aggregatedValues[i].items.TryGetValue(url, out temp))
                                aggregatedValues[i].items.Add(url, null);

                            aggregatedValues[i].type = 1;
                        }
                    }
                    else if (functionTypes[i] == FunctionType.Count)
                    {
                        object temp = null;

                        if (!aggregatedValues[i].items.TryGetValue(functionValues[i], out temp))
                            aggregatedValues[i].items.Add(functionValues[i], null);

                        aggregatedValues[i].type = 1;
                    }
                    else if (functionTypes[i] == FunctionType.Sum)
                    {
                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        aggregatedValues[i].result1f += currentFunctionValue.result1f;
                        aggregatedValues[i].type = currentFunctionValue.type;
                    }
                    else if (functionTypes[i] == FunctionType.Minimum)
                    {
                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        if (aggregatedValues[i].result1f > currentFunctionValue.result1f)
                            aggregatedValues[i].result1f = currentFunctionValue.result1f;

                        aggregatedValues[i].type = currentFunctionValue.type;
                    }
                    else if (functionTypes[i] == FunctionType.Maximum)
                    {
                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        if (aggregatedValues[i].result1f < currentFunctionValue.result1f)
                            aggregatedValues[i].result1f = currentFunctionValue.result1f;

                        aggregatedValues[i].type = currentFunctionValue.type;
                    }
                    else if (functionTypes[i] == FunctionType.Average)
                    {
                        object temp = null;

                        if (!aggregatedValues[i].items.TryGetValue(parameter2Values[i], out temp))
                            aggregatedValues[i].items.Add(parameter2Values[i], null);

                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        aggregatedValues[i].result1f += currentFunctionValue.result1f;
                        aggregatedValues[i].type = currentFunctionValue.type;
                    }
                    else if (functionTypes[i] == FunctionType.AverageCount)
                    {
                        object temp = null;

                        if (!aggregatedValues[i].items.TryGetValue(parameter2Values[i], out temp))
                            aggregatedValues[i].items.Add(parameter2Values[i], null);

                        if (!aggregatedValues[i].items1.TryGetValue(functionValues[i], out temp))
                            aggregatedValues[i].items1.Add(functionValues[i], null);
                    }
                }
            }

            public static string GetField(KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue, int fieldNumber, List<string> groupingNames)
            {
                if (fieldNumber < groupingNames.Count)
                {
                    string field = keyValue.Key.Split(Plugin.MultipleItemsSplitterId)[fieldNumber];

                    if (field == Plugin.TotalsString)
                        field = (Plugin.MsgAllTags + " '" + groupingNames[fieldNumber] + "'").ToUpper();

                    return field;
                }
                else
                {
                    return keyValue.Value[fieldNumber - groupingNames.Count].getResult();
                }
            }

            public static string[] GetGroupings(KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue, List<string> groupingNames)
            {
                if (keyValue.Key == "")
                {
                    return new string[1];
                }
                else
                {
                    string[] fields = keyValue.Key.Split(Plugin.MultipleItemsSplitterId);

                    for (int i = 0; i < fields.Length; i++)
                        if (fields[i] == Plugin.TotalsString)
                            fields[i] = (Plugin.MsgAllTags + " '" + groupingNames[i] + "'").ToUpper();

                    return fields;
                }
            }
        }

        private delegate void AddRowToTable(string[] row);
        private delegate void UpdateTable();
        private AddRowToTable addRowToTable;
        private UpdateTable updateTable;

        private string[] files = new string[0];
        private AggregatedTags tags = new AggregatedTags();

        public static string PresetName = "";
        private List<string> groupingNames = new List<string>();
        private List<string> functionNames = new List<string>();
        private List<FunctionType> functionTypes = new List<FunctionType>();
        private List<string> parameterNames = new List<string>();
        private List<string> parameter2Names = new List<string>();

        private int conditionField = -1;
        private int comparedField = -1;
        private int savedTagField = -1;
        private int artworkField = -1;

        private string conditionListText;
        private string conditionValueListText;

        private bool totals;

        public static Bitmap DefaultArtwork;
        public static string DefaultArtworkHash;
        public SortedDictionary<string, Bitmap> Artworks = new SortedDictionary<string, Bitmap>();

        private Plugin.MetaDataType tagId = 0;

        private bool ignorePresetChangedEvent = false;
        private bool ignoreItemCheckEvent = false;
        private bool completelyIgnoreItemCheckEvent = false;
        private bool completelyIgnoreFunctionChangedEvent = false;

        public LibraryReportsPlugin()
        {
            InitializeComponent();
        }

        public LibraryReportsPlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            functionComboBox.Items.Add(Plugin.GroupingName);
            functionComboBox.Items.Add(Plugin.CountName);
            functionComboBox.Items.Add(Plugin.SumName);
            functionComboBox.Items.Add(Plugin.MinimumName);
            functionComboBox.Items.Add(Plugin.MaximumName);
            functionComboBox.Items.Add(Plugin.AverageName);
            functionComboBox.Items.Add(Plugin.AverageCountName);

            Plugin.FillList(sourceTagList.Items, true, true);
            Plugin.FillListWithProps(sourceTagList.Items);
            sourceTagList.Items.Add(Plugin.SequenceNumberName);

            Plugin.FillList(parameter2ComboBox.Items, true, false);
            Plugin.FillListWithProps(parameter2ComboBox.Items);

            parameter2ComboBox.SelectedItem = MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Url);

            presetsBox.Items.AddRange(Plugin.SavedSettings.libraryReportsPresets4);

            presetsBox.SelectedIndex = Plugin.SavedSettings.libraryReportsPresetNumber;

            fieldComboBox.Text = Plugin.SavedSettings.savedFieldName;

            Plugin.FillList(destinationTagList.Items);
            destinationTagList.Text = Plugin.SavedSettings.destinationTagOfSavedFieldName;

            conditionCheckBox.Checked = Plugin.SavedSettings.conditionIsChecked;

            conditionFieldList.Text = Plugin.SavedSettings.conditionTagName;

            conditionList.Items.Add(TagToolsPlugin.listItemConditionIs);
            conditionList.Items.Add(TagToolsPlugin.listItemConditionIsNot);
            conditionList.Items.Add(TagToolsPlugin.listItemConditionIsGreater);
            conditionList.Items.Add(TagToolsPlugin.listItemConditionIsLess);
            conditionList.Text = Plugin.SavedSettings.condition;

            comparedFieldList.Text = Plugin.SavedSettings.comparedField;

            resizeArtworkCheckBox.Checked = Plugin.SavedSettings.resizeArtwork;
            xArworkSizeUpDown.Value = Plugin.SavedSettings.xArtworkSize == 0 ? 300 : Plugin.SavedSettings.xArtworkSize;
            yArworkSizeUpDown.Value = Plugin.SavedSettings.yArtworkSize == 0 ? 300 : Plugin.SavedSettings.yArtworkSize;

            try
            {
                DefaultArtwork = new Bitmap(Application.StartupPath + @"\Plugins\" + "Missing Album Artwork.png");
            }
            catch
            {
                DefaultArtwork = new Bitmap(1, 1);
            }

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewList_AddRowToTable;
            updateTable = previewList_updateTable;
        }

        public static string GetColumnName(string tagName, string tag2Name, FunctionType type)
        {
            if (type == FunctionType.Count)
            {
                return Plugin.CountName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Sum)
            {
                return Plugin.SumName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Minimum)
            {
                return Plugin.MinimumName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Maximum)
            {
                return Plugin.MaximumName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Average)
            {
                return Plugin.AverageName + "(" + tagName + "/" + tag2Name + ")";
            }
            else if (type == FunctionType.AverageCount)
            {
                return Plugin.AverageCountName + "(" + tagName + "/" + tag2Name + ")";
            }
            else //Its grouping
            {
                return tagName;
            }
        }

        private void previewList_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);

            if (artworkField != -1)
            {
                //Replace string hashes in the Artwork column with images.
                string stringHash = (string)previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].Value;
                Bitmap pic;

                try
                {
                    pic = Artworks[stringHash];
                }
                catch
                {
                    pic = Artworks[DefaultArtworkHash];
                }

                previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].ValueType = typeof(Bitmap);
                previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].Value = pic;
            }
        }

        private void previewList_updateTable()
        {
            sourceTagList.Enabled = true;
            buttonCheckAll.Enabled = true;
            buttonUncheckAll.Enabled = true;

            if (previewTable.Rows.Count > 0)
            {
                previewTable.CurrentCell = previewTable.Rows[0].Cells[0];
            }
        }

        private bool addColumn(string fieldName, string parameter2Name, FunctionType type)
        {
            DataGridViewColumn column;

            if (fieldName == Plugin.ArtworkName && type != FunctionType.Grouping)
            {
                MessageBox.Show(TagToolsPlugin.msgPleaseUseGroupingFunctionForArtworkTag);
                return false;
            }


            DataGridViewTextBoxColumn textColumnTemplate = new DataGridViewTextBoxColumn();
            column = new DataGridViewColumn(textColumnTemplate.CellTemplate);
            column.SortMode = DataGridViewColumnSortMode.Programmatic;

            column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;


            column.HeaderText = GetColumnName(fieldName, parameter2Name, type);


            conditionFieldList.Items.Add(column.HeaderText);
            if (conditionFieldList.SelectedIndex == -1)
                conditionFieldList.SelectedIndex = 0;

            comparedFieldList.Items.Add(column.HeaderText);


            if (fieldName == Plugin.ArtworkName)
            {
                DataGridViewImageColumn imageColumnTemplate = new DataGridViewImageColumn();
                imageColumnTemplate.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imageColumnTemplate.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                column = new DataGridViewColumn(imageColumnTemplate.CellTemplate);
                column.HeaderText = fieldName;

                previewTable.Columns.Insert(groupingNames.Count, column);
                groupingNames.Add(fieldName);

                artworkField = groupingNames.Count - 1;
            }
            else if (type == FunctionType.Grouping)
            {
                previewTable.Columns.Insert(groupingNames.Count, column);
                groupingNames.Add(fieldName);
            }
            else
            {
                previewTable.Columns.Add(column);
                functionNames.Add(column.HeaderText);
                functionTypes.Add(type);
                parameterNames.Add(fieldName);

                if (type == FunctionType.Average || type == FunctionType.AverageCount)
                    parameter2Names.Add(parameter2Name);
                else
                    parameter2Names.Add(null);


                fieldComboBox.Items.Add(column.HeaderText);
                if (fieldComboBox.SelectedIndex == -1)
                    fieldComboBox.SelectedIndex = 0;
            }


            if(previewTable.Rows.Count > 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonCancel);

            return true;
        }

        private void removeColumn(string fieldName, string parameter2Name, FunctionType type)
        {
            string columnHeader = GetColumnName(fieldName, parameter2Name, type);

            if (groupingNames.Contains(columnHeader))
            {
                groupingNames.Remove(columnHeader);
                if (fieldName == Plugin.ArtworkName)
                    artworkField = -1;
            }
            else
            {
                int index = functionNames.IndexOf(columnHeader);

                functionNames.RemoveAt(index);
                functionTypes.RemoveAt(index);
                parameterNames.RemoveAt(index);
                parameter2Names.RemoveAt(index);

                fieldComboBox.Items.Remove(columnHeader);
                if (fieldComboBox.SelectedIndex == -1 && fieldComboBox.Items.Count > 0)
                    fieldComboBox.SelectedIndex = 0;
            }


            conditionFieldList.Items.Remove(columnHeader);
            if (conditionFieldList.SelectedIndex == -1 && conditionFieldList.Items.Count > 0)
                conditionFieldList.SelectedIndex = 0;


            if (comparedFieldList.Text == (string)comparedFieldList.SelectedValue)
                comparedFieldList.Text = "";

            comparedFieldList.Items.Remove(columnHeader);


            foreach (DataGridViewColumn column in previewTable.Columns)
            {
                if (column.HeaderText == columnHeader)
                {
                    previewTable.Columns.RemoveAt(column.Index);
                    
                    if (previewTable.Columns.Count == 0)
                        clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonCancel);

                    return;
                }
            }

            if (previewTable.Columns.Count == 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonCancel);
        }

        private void clearList()
        {
            completelyIgnoreItemCheckEvent = true;

            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                sourceTagList.SetItemChecked(i, false);
            }

            previewTable.Columns.Clear();

            conditionField = -1;
            comparedField = -1;
            savedTagField = -1;
            artworkField = -1;

            fieldComboBox.Items.Clear();
            conditionFieldList.Items.Clear();
            comparedFieldList.Items.Clear();

            groupingNames.Clear();
            functionNames.Clear();
            functionTypes.Clear();
            parameterNames.Clear();
            parameter2Names.Clear();

            completelyIgnoreItemCheckEvent = false;

            functionComboBox.SelectedIndex = 0;

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonCancel);
        }

        private void addAllTags()
        {
            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                if ((string)sourceTagList.Items[i] != Plugin.ArtworkName)
                    sourceTagList.SetItemChecked(i, true);
            }
        }

        private bool prepareBackgroundPreview()
        {
            if (backgroundTaskIsWorking())
                return true;

            if (previewTable.Columns.Count == 0)
            {
                //MessageBox.Show(tagToolsPlugin.msgNoTagsSelected);
                return false;
            }

            previewTable.Rows.Clear();

            totals = totalsCheckBox.Checked;

            if (MbApiInterface.Library_QueryFiles("domain=DisplayedFiles"))
                files = MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(TagToolsPlugin.msgNoFilesInCurrentView);
                return false;
            }


            sourceTagList.Enabled = false;
            buttonCheckAll.Enabled = false;
            buttonUncheckAll.Enabled = false;

            return true;
        }

        private void previewTrackList()
        {
            string currentFile;
            string tagValue;
            string[] groupingValues;
            string[] functionValues;
            string[] parameter2Values;
            Plugin.FilePropertyType propId;


            tags.Clear();
            Artworks.Clear();

            MD5Cng md5 = new MD5Cng();

            //Lets add default artwork
            Bitmap pic = new Bitmap(DefaultArtwork);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            byte[] hash;

            if (Plugin.SavedSettings.resizeArtwork)
            {
                float xSF = (float)Plugin.SavedSettings.xArtworkSize / (float)pic.Width;
                float ySF = (float)Plugin.SavedSettings.yArtworkSize / (float)pic.Height;
                float SF;

                if (xSF >= ySF)
                    SF = ySF;
                else
                    SF = xSF;


                try
                {
                    Bitmap bm_dest = new Bitmap((int)(pic.Width * SF), (int)(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    Graphics gr_dest = Graphics.FromImage(bm_dest);
                    gr_dest.DrawImage(pic, 0, 0, bm_dest.Width, bm_dest.Height);
                    pic.Dispose();
                    gr_dest.Dispose();

                    pic = bm_dest;
                }
                catch
                {
                    pic = new Bitmap(1, 1);
                }
            }

            try { hash = md5.ComputeHash((byte[])tc.ConvertTo(pic, typeof(byte[]))); }
            catch { hash = md5.ComputeHash(new byte[] { 0x00 }); }

            DefaultArtworkHash = Convert.ToBase64String(hash);

            Artworks.Add(DefaultArtworkHash, pic);


            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                {
                    Invoke(updateTable);
                    return;
                }

                currentFile = files[fileCounter];

                TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.libraryReportsCommandSbText, true, fileCounter, files.Length, currentFile);


                groupingValues = new string[groupingNames.Count];

                for (int i = 0; i < groupingNames.Count; i++)
                {
                    tagId = Plugin.GetTagId(groupingNames[i]);

                    if (groupingNames[i] == Plugin.SequenceNumberName)
                    {
                        tagValue = fileCounter.ToString("D6");
                    }
                    else if (i == artworkField) //Its artwork image. Lets fill cell with hash codes. 
                    {
                        string artworkBase64 = TagToolsPlugin.getFileTag(currentFile, Plugin.MetaDataType.Artwork);

                        try
                        {
                            if (artworkBase64 != "")
                                pic = (Bitmap)tc.ConvertFrom(Convert.FromBase64String(artworkBase64));
                            else
                                pic = new Bitmap(DefaultArtwork);
                        }
                        catch
                        {
                            pic = new Bitmap(DefaultArtwork);
                        }

                        if (Plugin.SavedSettings.resizeArtwork)
                        {
                            float xSF = (float)Plugin.SavedSettings.xArtworkSize / (float)pic.Width;
                            float ySF = (float)Plugin.SavedSettings.yArtworkSize / (float)pic.Height;
                            float SF;

                            if (xSF >= ySF)
                                SF = ySF;
                            else
                                SF = xSF;


                            Bitmap bm_dest = new Bitmap((int)(pic.Width * SF), (int)(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                            Graphics gr_dest = Graphics.FromImage(bm_dest);
                            gr_dest.DrawImage(pic, 0, 0, bm_dest.Width, bm_dest.Height);
                            pic.Dispose();
                            gr_dest.Dispose();

                            pic = bm_dest;
                        }

                        try { hash = md5.ComputeHash((byte[])tc.ConvertTo(pic, typeof(byte[]))); }
                        catch { hash = md5.ComputeHash(new byte[] { 0x00 }); }

                        tagValue = Convert.ToBase64String(hash);

                        try
                        {
                            Artworks.Add(tagValue, pic);
                        }
                        catch { }
                    }
                    else if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Lets make smart conversion of list of artists/composers
                    {
                        tagValue = TagToolsPlugin.getFileTag(currentFile, tagId);
                        tagValue = TagToolsPlugin.getTagRepresentation(tagValue);
                    }
                    else
                    {
                        if (tagId == 0)
                        {
                            propId = TagToolsPlugin.getPropId(groupingNames[i]);
                            tagValue = MbApiInterface.Library_GetFileProperty(currentFile, propId);
                        }
                        else
                        {
                            tagValue = TagToolsPlugin.getFileTag(currentFile, tagId, true);
                        }
                    }


                    if (tagValue == "")
                        tagValue = " ";

                    groupingValues[i] = tagValue;
                }


                functionValues = new string[functionNames.Count];
                parameter2Values = new string[functionNames.Count];

                for (int i = 0; i < functionNames.Count; i++)
                {
                    int parameterIndex = functionNames.IndexOf(parameterNames[i]);

                    tagId = Plugin.GetTagId(parameterNames[i]);

                    if (parameterNames[i] == Plugin.SequenceNumberName)
                    {
                        tagValue = fileCounter.ToString();
                    }
                    else if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Lets make smart conversion of list of artists/composers
                    {

                        tagValue = TagToolsPlugin.getFileTag(currentFile, tagId);
                        tagValue = TagToolsPlugin.getTagRepresentation(tagValue);
                    }
                    else
                    {
                        if (tagId == 0)
                        {
                            propId = TagToolsPlugin.getPropId(parameterNames[i]);
                            tagValue = MbApiInterface.Library_GetFileProperty(currentFile, propId);
                        }
                        else
                        {
                            tagValue = TagToolsPlugin.getFileTag(currentFile, tagId, true);
                        }
                    }

                    functionValues[i] = tagValue;

                    if (functionTypes[i] == FunctionType.Average || functionTypes[i] == FunctionType.AverageCount)
                    {
                        parameterIndex = functionNames.IndexOf(parameter2Names[i]);

                        tagId = Plugin.GetTagId(parameter2Names[i]);

                        if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Lets make smart conversion of list of artists/composers
                        {

                            tagValue = TagToolsPlugin.getFileTag(currentFile, tagId);
                            tagValue = TagToolsPlugin.getTagRepresentation(tagValue);
                        }
                        else
                        {
                            if (tagId == 0)
                            {
                                propId = TagToolsPlugin.getPropId(parameter2Names[i]);
                                tagValue = MbApiInterface.Library_GetFileProperty(currentFile, propId);
                            }
                            else
                            {
                                tagValue = TagToolsPlugin.getFileTag(currentFile, tagId, true);
                            }
                        }

                        parameter2Values[i] = tagValue;
                    }
                    else
                    {
                        parameter2Values[i] = null;
                    }
                }


                tags.add(currentFile, groupingValues, functionValues, functionTypes, parameter2Values, totals);
            }


            int k = 0;
            foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                {
                    Invoke(updateTable);
                    return;
                }

                k++;
                TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.libraryReportsGneratingPreviewCommandSbText, true, k, tags.Count);

                string[] groupingsRow = AggregatedTags.GetGroupings(keyValue, groupingNames);
                string[] row = new string[groupingsRow.Length + keyValue.Value.Length];

                groupingsRow.CopyTo(row, 0);

                for (int i = groupingsRow.Length; i < row.Length; i++)
                {
                    row[i] = AggregatedTags.GetField(keyValue, i, groupingNames);
                }

                Invoke(addRowToTable, new Object[] { row });

                previewIsGenerated = true;
            }

            TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.libraryReportsGneratingPreviewCommandSbText, true, tags.Count - 1, tags.Count, null, true);

            Invoke(updateTable);
        }

        private bool checkCondition(KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue)
        {
            if (conditionField == -1)
                return true;

            string value = AggregatedTags.GetField(keyValue, conditionField, groupingNames);
            string comparedValue;


            if (comparedField == -1)
                comparedValue = conditionValueListText;
            else
                comparedValue = AggregatedTags.GetField(keyValue, comparedField, groupingNames);


            if (conditionListText == TagToolsPlugin.listItemConditionIs)
            {
                if (value == comparedValue)
                    return true;
                else
                    return false;
            }
            else if (conditionListText == TagToolsPlugin.listItemConditionIsNot)
            {
                if (value != comparedValue)
                    return true;
                else
                    return false;
            }
            else if (conditionListText == TagToolsPlugin.listItemConditionIsGreater)
            {
                if (Plugin.CompareStrings(value, comparedValue) == 1)
                    return true;
                else
                    return false;
            }
            else if (conditionListText == TagToolsPlugin.listItemConditionIsLess)
            {
                if (Plugin.CompareStrings(value, comparedValue) == -1)
                    return true;
                else
                    return false;
            }

            return true;
        }

        private void exportTrackList()
        {
            string formats = TagToolsPlugin.exportedFormats;
            System.IO.Stream stream;
            ExportedDocument document;
            SaveFileDialog dialog = new SaveFileDialog();
            int urlField = -1;
            int albumArtistField = -1;
            int albumField = -1;
            Bitmap pic;

            if (!prepareBackgroundTask(false))
                return;

            dialog.FileName = Plugin.SavedSettings.exportedTrackListName;
            dialog.Filter = formats;
            dialog.FilterIndex = Plugin.SavedSettings.filterIndex;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;
            Plugin.SavedSettings.filterIndex = dialog.FilterIndex;



            for (int j = 0; j < groupingNames.Count; j++)
            {
                if (groupingNames[j] == Plugin.UrlTagName)
                    urlField = j;
                else if (groupingNames[j] == Plugin.DisplayedAlbumArtsistName)
                    albumArtistField = j;
                else if (groupingNames[j] == Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Album))
                    albumField = j;
            }


            stream = dialog.OpenFile();

            string fileDirectoryPath;
            string imagesDirectoryName;
            string onlyFileName;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(dialog.FileName);

            fileDirectoryPath = fileInfo.Directory.FullName + @"\";

            onlyFileName = fileInfo.Name;
            onlyFileName += (char)0;
            onlyFileName = onlyFileName.Replace(fileInfo.Extension + (char)0, "");

            Plugin.SavedSettings.exportedTrackListName = onlyFileName;

            imagesDirectoryName = onlyFileName + ".files";

            if (dialog.FilterIndex == 1)
            {
                if (System.IO.Directory.Exists(fileDirectoryPath + imagesDirectoryName))
                    System.IO.Directory.Delete(fileDirectoryPath + imagesDirectoryName, true);

                System.IO.Directory.CreateDirectory(fileDirectoryPath + imagesDirectoryName);
            }
            else if (dialog.FilterIndex == 2 || dialog.FilterIndex == 3)
            {
                if (System.IO.Directory.Exists(fileDirectoryPath + imagesDirectoryName))
                    System.IO.Directory.Delete(fileDirectoryPath + imagesDirectoryName, true);

                if (artworkField != -1)
                    System.IO.Directory.CreateDirectory(fileDirectoryPath + imagesDirectoryName);
            }

            List<int> albumTrackCounts = new List<int>();

            switch (dialog.FilterIndex)
            {
                case 1:
                    if (albumArtistField != 0)
                    {
                        MessageBox.Show(TagToolsPlugin.msgFirstThreeGroupingFieldsInPreviewTableShouldBe);
                        return;
                    }
                    else if (albumField != 1)
                    {
                        MessageBox.Show(TagToolsPlugin.msgFirstThreeGroupingFieldsInPreviewTableShouldBe);
                        return;
                    }
                    else if (artworkField != 2)
                    {
                        MessageBox.Show(TagToolsPlugin.msgFirstThreeGroupingFieldsInPreviewTableShouldBe);
                        return;
                    }


                    string prevAlbum1 = null;
                    string prevAlbumArtist1 = null;

                    int trackCount = 0;

                    foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
                    {
                        string[] groupingsValues1 = AggregatedTags.GetGroupings(keyValue, groupingNames);

                        if (prevAlbumArtist1 != groupingsValues1[albumArtistField] || prevAlbum1 != groupingsValues1[albumField])
                        {
                            prevAlbumArtist1 = groupingsValues1[albumArtistField];
                            prevAlbum1 = groupingsValues1[albumField];

                            albumTrackCounts.Add(trackCount);
                            trackCount = 0;
                        }

                        trackCount++;
                    }
                    albumTrackCounts.Add(trackCount);


                    document = new HtmlDocumentByAlbum(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                    break;
                case 2:
                    document = new HtmlDocument(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                    break;
                case 3:
                    document = new HtmlTable(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                    break;
                case 4:
                    document = new TextDocument(stream, TagToolsPlugin);
                    break;
                case 5:
                    if (urlField == -1)
                    {
                        MessageBox.Show(TagToolsPlugin.msgForExportingPlaylistsURLfieldMustBeIncludedInTagList);
                        return;
                    }

                    document = new M3UDocument(stream, TagToolsPlugin);
                    break;
                default:
                    return;
            }

            document.writeHeader();

            if (dialog.FilterIndex != 5) //Lets write table headers
            {
                for (int j = 0; j < groupingNames.Count; j++)
                {
                    document.addCellToRow(groupingNames[j], groupingNames[j], j == albumArtistField, j == albumField);
                }

                for (int j = 0; j < functionNames.Count; j++)
                {
                    document.addCellToRow(functionNames[j], functionNames[j], false, false);
                }

                document.writeRow(0);
            }


            int height = 0;
            int i = 0;

            string prevAlbum = null;
            string prevAlbumArtist = null;

            foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                    return;

                if (checkCondition(keyValue))
                {
                    string[] groupingsValues = AggregatedTags.GetGroupings(keyValue, groupingNames);

                    if (dialog.FilterIndex == 1)
                    {
                        if (prevAlbumArtist != groupingsValues[albumArtistField])
                        {
                            i++;
                            prevAlbumArtist = groupingsValues[albumArtistField];
                            prevAlbum = groupingsValues[albumField];
                            document.beginAlbumArtist(groupingsValues[albumArtistField], groupingsValues.Length - 2 + functionNames.Count);
                            document.beginAlbum(groupingsValues[albumField], Artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                        else if (prevAlbum != groupingsValues[albumField])
                        {
                            i++;
                            prevAlbum = groupingsValues[albumField];
                            document.beginAlbum(groupingsValues[albumField], Artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                    }


                    for (int j = 0; j < groupingNames.Count; j++)
                    {
                        if (j == artworkField && (dialog.FilterIndex == 1 || dialog.FilterIndex == 2 || dialog.FilterIndex == 3)) //Export images
                        {
                            pic = Artworks[groupingsValues[artworkField]];

                            height = pic.Height;

                            document.addCellToRow(pic, groupingNames[j], groupingsValues[j], pic.Width, pic.Height);
                        }
                        else if (j == artworkField && dialog.FilterIndex != 1 && dialog.FilterIndex != 2 && dialog.FilterIndex != 3) //Export image hashes
                            document.addCellToRow(groupingsValues[artworkField], groupingNames[j], j == albumArtistField, j == albumField);
                        else //Its not the artwork column
                            document.addCellToRow(groupingsValues[j], groupingNames[j], j == albumArtistField, j == albumField);
                    }

                    for (int j = 0; j < functionNames.Count; j++)
                    {
                        document.addCellToRow(AggregatedTags.GetField(keyValue, groupingNames.Count + j, groupingNames), functionNames[j], false, false);
                    }


                    document.writeRow(height);
                    height = 0;
                }
            }

            document.writeFooter();
            document.close();
        }

        private bool prepareBackgroundTask(bool checkForFunctions)
        {
            if (backgroundTaskIsWorking())
                return true;

            if (previewTable.Rows.Count == 0)
            {
                MessageBox.Show(TagToolsPlugin.msgPreviewIsNotGeneratedNothingToSave);
                return false;
            }

            if (checkForFunctions && fieldComboBox.SelectedIndex == -1)
            {
                MessageBox.Show(TagToolsPlugin.msgNoAggregateFunctionNothingToSave);
                return false;
            }


            conditionListText = conditionList.Text;
            conditionValueListText = comparedFieldList.Text;

            tagId = Plugin.GetTagId(destinationTagList.Text);

            conditionField = -1;
            comparedField = -1;


            string columnHeader = "";

            if (conditionCheckBox.Checked)
            {
                for (int i = 0; i < groupingNames.Count; i++)
                {
                    if (groupingNames[i] == conditionFieldList.Text)
                        conditionField = i;

                    if (groupingNames[i] == comparedFieldList.Text)
                        comparedField = i;
                }


                for (int i = 0; i < functionNames.Count; i++)
                {
                    columnHeader = functionNames[i];

                    if (columnHeader == conditionFieldList.Text)
                        conditionField = groupingNames.Count + i;

                    if (columnHeader == comparedFieldList.Text)
                        comparedField = groupingNames.Count + i;
                }
            }


            for (int i = 0; i < functionNames.Count; i++)
            {
                columnHeader = functionNames[i];

                if (columnHeader == fieldComboBox.Text)
                    savedTagField = groupingNames.Count + i;
            }


            if (checkForFunctions)
            {
                sourceTagList.Enabled = false;
                buttonCheckAll.Enabled = false;
                buttonUncheckAll.Enabled = false;
            }

            return true;
        }

        private void saveField()
        {
            SortedDictionary<string, object> urls;
            int i = 0;

            foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
            {
                i++;

                if (backgroundTaskIsCanceled)
                    return;

                if (checkCondition(keyValue))
                {
                    urls = keyValue.Value[keyValue.Value.Length - 1].items;

                    foreach (KeyValuePair<string, object> keyValueUrls in urls)
                    {
                        TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.libraryReportsCommandSbText, false, i, tags.Count, keyValueUrls.Key);

                        TagToolsPlugin.setFileTag(keyValueUrls.Key, tagId, AggregatedTags.GetField(keyValue, savedTagField, groupingNames));
                        TagToolsPlugin.commitTagsToFile(keyValueUrls.Key);
                    }
                }
            }

            TagToolsPlugin.refreshPanels(true);

            TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.libraryReportsCommandSbText, false, tags.Count - 1, tags.Count, null, true);

            Invoke(updateTable);
        }

        private void saveSettings()
        {
            if (presetsBox.SelectedIndex == 0 || presetsBox.Text == "")
            {
                LibraryReportsPreset libraryReportsPreset = new LibraryReportsPreset();

                libraryReportsPreset.groupingNames = new string[groupingNames.Count];
                libraryReportsPreset.functionNames = new string[functionNames.Count];
                libraryReportsPreset.functionTypes = new FunctionType[functionTypes.Count];
                libraryReportsPreset.parameterNames = new string[parameterNames.Count];
                libraryReportsPreset.parameter2Names = new string[parameter2Names.Count];
                libraryReportsPreset.totals = totalsCheckBox.Checked;

                groupingNames.CopyTo(libraryReportsPreset.groupingNames, 0);
                functionNames.CopyTo(libraryReportsPreset.functionNames, 0);
                functionTypes.CopyTo(libraryReportsPreset.functionTypes, 0);
                parameterNames.CopyTo(libraryReportsPreset.parameterNames, 0);
                parameter2Names.CopyTo(libraryReportsPreset.parameter2Names, 0);

                ignorePresetChangedEvent = true;
                Plugin.SetItemInComboBox(presetsBox, libraryReportsPreset);
                ignorePresetChangedEvent = false;

                presetsBox.Items.CopyTo(Plugin.SavedSettings.libraryReportsPresets4, 0);

                PresetName = TagToolsPlugin.libraryReport;
            }
            else if (presetsBox.SelectedIndex <= 2)
            {
                PresetName = presetsBox.Text;
            }
            else
            {
                PresetName = TagToolsPlugin.libraryReport;
            }

            Plugin.SavedSettings.savedFieldName = fieldComboBox.Text;
            Plugin.SavedSettings.destinationTagOfSavedFieldName = destinationTagList.Text;
            Plugin.SavedSettings.conditionIsChecked = conditionCheckBox.Checked;
            Plugin.SavedSettings.conditionTagName = conditionFieldList.Text;
            Plugin.SavedSettings.condition = conditionList.Text;
            Plugin.SavedSettings.comparedField = comparedFieldList.Text;

            Plugin.SavedSettings.libraryReportsPresetNumber = presetsBox.SelectedIndex;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            exportTrackList();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveSettings();
            if (prepareBackgroundTask(true))
                switchOperation(saveField, (Button)sender, buttonPreview, buttonCancel);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, (Button)sender, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, String.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = true;
            buttonPreview.Enabled = true;
            buttonSave.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonPreview.Enabled = false;
            buttonSave.Enabled = false;
        }

        private void sourceTagList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (completelyIgnoreItemCheckEvent)
                return;


            ignorePresetChangedEvent = true;

            if (!ignoreItemCheckEvent)
                presetsBox.SelectedItem = TagToolsPlugin.libraryReportsPreset;

            if (e.NewValue == CheckState.Checked)
            {
                if (!addColumn((string)sourceTagList.Items[e.Index], (string)parameter2ComboBox.SelectedItem, (FunctionType)functionComboBox.SelectedIndex))
                {
                    e.NewValue = CheckState.Unchecked;
                }
            }
            else
            {
                removeColumn((string)sourceTagList.Items[e.Index], (string)parameter2ComboBox.SelectedItem, (FunctionType)functionComboBox.SelectedIndex);
            }

            ignorePresetChangedEvent = false;
        }

        private void buttonUncheckAll_Click(object sender, EventArgs e)
        {
            clearList();
            presetsBox.SelectedIndex = 0;
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            addAllTags();
        }

        private void presetsBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ignorePresetChangedEvent)
                return;

            ignoreItemCheckEvent = true;

            LibraryReportsPreset libraryReportsPreset = (LibraryReportsPreset)presetsBox.SelectedItem;

            clearList();

            completelyIgnoreFunctionChangedEvent = true;

            //Groupings
            functionComboBox.SelectedIndex = 0;
            for (int i = 0; i < libraryReportsPreset.groupingNames.Length; i++)
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (libraryReportsPreset.groupingNames[i] == (string)sourceTagList.Items[j])
                        sourceTagList.SetItemChecked(j, true);
                }
            }

            //Functions
            for (int i = 0; i < libraryReportsPreset.functionNames.Length; i++)
            {
                functionComboBox.SelectedIndex = (int)libraryReportsPreset.functionTypes[i];

                parameter2ComboBox.SelectedItem = libraryReportsPreset.parameter2Names[i];

                completelyIgnoreItemCheckEvent = true; //Lets clear items list which were set in previous iteration
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    sourceTagList.SetItemChecked(j, false);
                }
                completelyIgnoreItemCheckEvent = false;


                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (libraryReportsPreset.parameterNames[i] == (string)sourceTagList.Items[j])
                    {
                        sourceTagList.SetItemChecked(j, true);
                    }
                }
            }

            totalsCheckBox.Checked = libraryReportsPreset.totals;

            completelyIgnoreFunctionChangedEvent = false;
            ignoreItemCheckEvent = false;

            functionComboBox.SelectedIndex = 0;
        }

        private void functionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (completelyIgnoreFunctionChangedEvent)
                return;

            completelyIgnoreItemCheckEvent = true;


            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                sourceTagList.SetItemChecked(i, false);
            }


            if (functionComboBox.SelectedIndex == 0) //Groupings
            {
                for (int i = 0; i < groupingNames.Count; i++)
                {
                    for (int j = 0; j < sourceTagList.Items.Count; j++)
                    {
                        if (groupingNames[i] == (string)sourceTagList.Items[j])
                            sourceTagList.SetItemChecked(j, true);
                    }
                }
            }
            else //Functions
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    for (int i = 0; i < functionNames.Count; i++)
                    {
                        if (functionTypes[i] == FunctionType.Average || functionTypes[i] == FunctionType.AverageCount)
                        {
                            if (functionComboBox.SelectedIndex == (int)functionTypes[i] && parameterNames[i] == (string)sourceTagList.Items[j] && parameter2Names[i] == (string)parameter2ComboBox.SelectedItem)
                                sourceTagList.SetItemChecked(j, true);
                        }
                        else
                        {
                            if (functionComboBox.SelectedIndex == (int)functionTypes[i] && parameterNames[i] == (string)sourceTagList.Items[j])
                                sourceTagList.SetItemChecked(j, true);
                        }
                    }
                }
            }

            if (functionComboBox.SelectedIndex >= functionComboBox.Items.Count - 2)
            {
                label6.Visible = true;
                parameter2ComboBox.Visible = true;
            }
            else
            {
                label6.Visible = false;
                parameter2ComboBox.Visible = false;
            }

            completelyIgnoreItemCheckEvent = false;
        }

        private void checkBoxCondition_CheckedChanged(object sender, EventArgs e)
        {
            conditionFieldList.Enabled = conditionCheckBox.Checked;
            conditionList.Enabled = conditionCheckBox.Checked;
            comparedFieldList.Enabled = conditionCheckBox.Checked;
        }

        private void previewList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void previewTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.ColumnIndex == artworkField)
                    return;

                Plugin.DataGridViewCellComparator comparator = new Plugin.DataGridViewCellComparator();

                comparator.comparedColumnIndex = e.ColumnIndex;

                for (int i = 0; i < previewTable.Columns.Count; i++)
                    previewTable.Columns[i].HeaderCell.SortGlyphDirection = SortOrder.None;

                previewTable.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;

                TagToolsPlugin.setStatusbarText(TagToolsPlugin.libraryReportsCommandSbText + " (" + TagToolsPlugin.sbSorting + ")");
                previewTable.Sort(comparator);
                TagToolsPlugin.setStatusbarText("");
            }
        }

        private void resizeArtworkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Plugin.SavedSettings.resizeArtwork = resizeArtworkCheckBox.Checked;

            xArworkSizeUpDown.Enabled = resizeArtworkCheckBox.Checked;
            yArworkSizeUpDown.Enabled = resizeArtworkCheckBox.Checked;
        }

        private void xArworkSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            Plugin.SavedSettings.xArtworkSize = (ushort)xArworkSizeUpDown.Value;
        }

        private void yArworkSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            Plugin.SavedSettings.yArtworkSize = (ushort)yArworkSizeUpDown.Value;
        }
    }

    public abstract class ExportedDocument
    {
        protected Plugin tagToolsPlugin;
        protected System.IO.Stream stream;
        protected Encoding unicode = Encoding.UTF8;
        protected string text = "";
        protected byte[] buffer;

        protected virtual void getHeader()
        {
            text = "";
        }

        protected virtual void getFooter()
        {
            text = "";
        }

        public ExportedDocument(System.IO.Stream newStream, Plugin tagToolsPluginParam)
        {
            tagToolsPlugin = tagToolsPluginParam;
            stream = newStream;
            stream.SetLength(0);

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stream.Write(buffer, 0, buffer.Length);
        }

        public string getImageName(string hash)
        {
            hash = Regex.Replace(hash, @"\\", "_");
            hash = Regex.Replace(hash, @"/", "_");
            hash = Regex.Replace(hash, @"\:", "_");

            return hash;
        }

        public virtual void writeHeader()
        {
            getHeader();
            buffer = unicode.GetBytes(text);
            text = "";
            stream.Write(buffer, 0, buffer.Length);
        }

        public void writeFooter()
        {
            getFooter();
            buffer = unicode.GetBytes(text);
            text = "";
            stream.Write(buffer, 0, buffer.Length);
        }

        public abstract void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2);
        public abstract void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height);

        protected abstract void getRow(int height);

        public virtual void writeRow(int height)
        {
            getRow(height);
            buffer = unicode.GetBytes(text);
            text = "";
            stream.Write(buffer, 0, buffer.Length);
        }

        public void close()
        {
            stream.Close();
        }

        public virtual void beginAlbumArtist(string albumArtist, int columnsCount)
        {
            return;
        }

        public virtual void beginAlbum(string album, Bitmap artwork, string imageHash, int albumTrackCount)
        {
            return;
        }
    }

    public class TextDocument : ExportedDocument
    {
        public TextDocument(System.IO.Stream newStream, Plugin tagToolsPluginParam)
            : base(newStream, tagToolsPluginParam)
        {
        }

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            if (!(cellName == Plugin.LyricsName || cellName == Plugin.SynchronisedLyricsName || cellName == Plugin.UnsynchronisedLyricsName))
            {
                if (text != "")
                {
                    text += (char)9 + cell;
                }
                else
                {
                    text += cell;
                }
            }
        }

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            if (text != "")
            {
                text += (char)9 + "ARTWORK";
            }
            else
            {
                text += "ARTWORK";
            }
        }

        protected override void getRow(int height)
        {
            text += "" + (char)13 + (char)10;
        }
    }

    public class M3UDocument : TextDocument
    {
        public M3UDocument(System.IO.Stream newStream, Plugin tagToolsPluginParam)
            : base(newStream, tagToolsPluginParam)
        {
        }

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            if (cellName == Plugin.UrlTagName)
            {
                base.addCellToRow(cell, cellName, dontOutput1, dontOutput2);
            }
        }
    }

    public class HtmlTable : ExportedDocument
    {
        protected string fileDirectoryPath;
        protected string imagesDirectoryName;
        protected const string defaultImageName = "Missing Album Artwork.png";
        protected bool defaultImageWasExported = false;

        public HtmlTable(System.IO.Stream newStream, Plugin tagToolsPluginParam, string fileDirectoryPathParam, string imagesDirectoryNameParam)
            : base(newStream, tagToolsPluginParam)
        {
            fileDirectoryPath = fileDirectoryPathParam;
            imagesDirectoryName = imagesDirectoryNameParam;
        }

        protected override void getHeader()
        {
            text = "<html>" + (char)13 + (char)10 + "<head>" + (char)13 + (char)10 + "</head>" + (char)13 + (char)10 + "<body>" + (char)13 + (char)10 + "<table border=1>" + (char)13 + (char)10;
        }

        protected override void getFooter()
        {
            text = "</table>" + (char)13 + "</body>" + (char)13 + (char)10 + "</html>" + (char)13 + (char)10;
        }

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            text += (char)9 + "<td>" + cell + "</td>" + (char)13 + (char)10;
        }

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            string imageName = getImageName(imageHash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += (char)9 + "<td height=" + height + " width=" + width + ">" + " <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > " + "</td>" + (char)13 + (char)10;
        }

        protected override void getRow(int height)
        {
            text = "<tr>" + (char)13 + (char)10 + text + "</tr>" + (char)13 + (char)10;
        }
    }

    public class HtmlDocument : HtmlTable
    {
        protected bool? alternateRow  = null;

        public HtmlDocument(System.IO.Stream newStream, Plugin tagToolsPluginParam, string fileDirectoryPathParam, string imagesDirectoryNameParam)
            : base(newStream, tagToolsPluginParam, fileDirectoryPathParam, imagesDirectoryNameParam)
        {
        }

        public override void writeHeader()
        {
            System.IO.FileStream stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stylesheet.Write(buffer, 0, buffer.Length);

            buffer = unicode.GetBytes("td {color:#050505;font-size:11.0pt;font-weight:400;font-style:normal;font-family:Arial, sans-serif;white-space:nowrap;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl1 {color:#0070C0;font-size:16.0pt;font-weight:700;font-family:Arial, sans-serif;	}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl2 {color:white;font-size:12.0pt;border-top:1.0pt solid windowtext;border-right:1pt solid windowtext;border-bottom:1pt solid windowtext;border-left:1pt solid windowtext;background:#95B3D7;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl3 {font-size:12.0pt;border-top:1pt solid windowtext;border-right:1pt solid windowtext;border-bottom:1.0pt solid windowtext;border-left:1.0pt solid windowtext;background:#DCE6F1;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl0 {color:white;font-size:12.0pt;font-weight:700;border-top:1.0pt solid windowtext;border-right:1pt solid windowtext;border-bottom:1pt solid windowtext;border-left:1.0pt solid windowtext;background:#4F81BD;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl5 {color:#0070A0;font-size:16.0pt;font-weight:700;border-top:0pt solid windowtext;border-right:0pt solid windowtext;border-bottom:2pt solid windowtext;border-left:0pt solid windowtext}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl6 {color:black;font-size:12.0pt;font-weight:400;padding-top:1px;vertical-align:top;border-top:1.0pt solid windowtext;border-right:0pt solid windowtext;border-bottom:0pt solid windowtext;border-left:0pt solid windowtext}");
            stylesheet.Write(buffer, 0, buffer.Length);

            stylesheet.Close();

            base.writeHeader();
            
        }

        protected override void getHeader()
        {
            text = "<html>" + (char)13 + (char)10 + "<head>" + (char)13 + (char)10 + "<link rel=Stylesheet href=\"" + imagesDirectoryName + "\\stylesheet.css\">" 
                + (char)13 + (char)10 + "</head>" + (char)13 + (char)10 + "<body>" + (char)13 + (char)10 +
                "<table> <tr> <td class=xl1>" + LibraryReportsPlugin.PresetName + "</td> </tr> </table> <table style='border-collapse:collapse'>" 
                + (char)13 + (char)10;
        }

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if ((bool)alternateRow)
                rowClass = "xl2";
            else
                rowClass = "xl3";


            text += (char)9 + "<td class=" + rowClass + ">" + cell + "</td>" + (char)13 + (char)10;
        }

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if ((bool)alternateRow)
                rowClass = "xl2";
            else
                rowClass = "xl3";

            string imageName = getImageName(imageHash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += (char)9 + "<td class=" + rowClass + " height=" + height + " width=" + width + ">" + " <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > " + "</td>" + (char)13 + (char)10;
        }

        public override void writeRow(int height)
        {
            if (alternateRow == null)
                alternateRow = false;
            else
                alternateRow = !alternateRow;

            base.writeRow(height);
        }
    }

    public class HtmlDocumentByAlbum : HtmlDocument
    {
        public HtmlDocumentByAlbum(System.IO.Stream newStream, Plugin tagToolsPluginParam, string fileDirectoryPathParam, string imagesDirectoryNameParam)
            : base(newStream, tagToolsPluginParam, fileDirectoryPathParam, imagesDirectoryNameParam)
        {
        }

        public override void beginAlbumArtist(string albumArtist, int columnsCount)
        {
            text = "" + (char)9 + "<tr> <td colspan=" + columnsCount + " class=xl5>" + albumArtist + "</td> </tr>" + (char)13 + (char)10;
        }

        public override void beginAlbum(string album, Bitmap artwork, string imageHash, int albumTrackCount)
        {
            string imageName = getImageName(imageHash) + ".jpg";
            artwork.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
            
            text += "" + (char)9 + "<td rowspan=" + albumTrackCount + " class=xl6>" + " <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > <br>" + album + "</td>" + (char)13 + (char)10;
            //text += "" + (char)9 + "<td class=xl6>" + " <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > <br>" + album + "</td>" + (char)13 + (char)10;
        }

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            if (dontOutput1 || dontOutput2)
                return;

            base.addCellToRow(cell, cellName, dontOutput1, dontOutput2);
        }

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
        }
    }
}
