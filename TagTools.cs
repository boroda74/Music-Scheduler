using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public struct SizePositionType
    {
        public SizePositionType(int xParam, int yParam, int wParam, int hParam, bool maxParam)
        {
            x = xParam;
            y = yParam;
            w = wParam;
            h = hParam;
            max = maxParam;
        }

        public int x;
        public int y;
        public int w;
        public int h;
        public bool max;
    }

    public enum FunctionType
    {
        Grouping = 0,
        Count,
        Sum,
        Minimum,
        Maximum,
        Average,
        AverageCount
    }

    public partial class Plugin
    {
        public const int NumberOfCommandWindows = 14;

        public bool developerMode = false;
        private bool uninstalled = false;

        public static MusicBeeApiInterface MbApiInterface;
        private PluginInfo about = new PluginInfo();

        public Form mbForm;
        public List<PluginWindowTemplate> openedForms = new List<PluginWindowTemplate>();
        public int numberOfNativeMbBackgroundTasks = 0;

        private bool prioritySet = false; //Plugin's background thread priority must be set to 'lower'

        public int numberOfTagChanges = 0;

        private string lastMessage = "";

        public static SortedDictionary<string, MetaDataType> TagNamesIds = new SortedDictionary<string, MetaDataType>();
        public static Dictionary<MetaDataType, string> TagIdsNames = new Dictionary<MetaDataType, string>();

        public static SortedDictionary<string, FilePropertyType> PropNamesIds = new SortedDictionary<string, FilePropertyType>();
        public static Dictionary<FilePropertyType, string> PropIdsNames = new Dictionary<FilePropertyType, string>();

        private delegate void AutoApplyDelegate(object currentFileObj, object tagToolsPluginObj);
        private AutoApplyDelegate autoApplyDelegate = AdvancedSearchAndReplacePlugin.AutoApply;

        private List<string> filesUpdatedByPlugin = new List<string>();

        private List<string> tagsWereChanged = new List<string>();

        private System.Threading.Timer periodicUI_RefreshTimer = null;
        private bool uiRefreshingIsNeeded = false;
        private TimeSpan refreshUI_Delay = new TimeSpan(0, 0, 5);
        private DateTime lastUI_Refresh = DateTime.MinValue;
        private object lastUI_RefreshLocker = new object();

        public System.Threading.Timer periodicAutobackupTimer = null;
        public object autobackupLocker = new object();

        public Button emptyButton = new Button();

        public static string[] ReadonlyTagsNames;
        private string[] conditions;

        public static string Language;
        private static string PluginSettingsFileName;
        public const string BackupIndexFileName = ".master tag index";
        public static string BackupDefaultPrefix = "Tag Backup ";
        private BackupIndexDictionary backupIndexDictionary;

        public const int StatusbarTextUpdateInterval = 50;
        public static char[] FilesSeparators = { '\0' };

        public List<AdvancedSearchAndReplacePlugin.Preset> autoAppliedPresets = new List<AdvancedSearchAndReplacePlugin.Preset>();
        public const string ASRPresetsPath = @"ASR Presets";
        public const string ASRPresetExtension = @".ASR Preset.xml";
        public const string ASRPresetMailSubject = @"New ASR preset";
        public const string ASRPresetMailRecipient = @"rusmusicbee@gmail.com";
        public const string ASRPresetMailServerName = @"smtp.post.ru";
        public const int ASRPresetMailServerPort = 25;
        public const string ASRPresetMailFromAddress = @"musicbee_user@anywhere.org";

        public const char MultipleItemsSplitterId = (char)0;
        public const char GuestId = (char)1;
        public const char PerformerId = (char)2;
        public const char RemixerId = (char)4;
        public const char EndOfStringId = (char)8;

        public const string TotalsString = "\u0001";
        public const int NumberOfPredefinedPresets = 3;

        public static string[] DurationTemplates = { "mmm:ss" };

        private string lastCommandSbText;
        private bool lastPreview;
        private int lastFileCounter;
        private MB_RefreshPanelsDelegate MB_RefreshPanels;

        public LibraryReportsPlugin.LibraryReportsPreset libraryReportsPreset = new LibraryReportsPlugin.LibraryReportsPreset();

        //Some workarounds
        public const MetaDataType DisplayedArtistId = (MetaDataType)(-1);
        public const MetaDataType ArtistArtistsId = (MetaDataType)(-2);
        public const MetaDataType DisplayedComposerId = (MetaDataType)(-3);
        public const MetaDataType ComposerComposersId = (MetaDataType)(-4);
        public const MetaDataType LyricsId = (MetaDataType)(-5);
        public const MetaDataType SynchronisedLyricsId = (MetaDataType)(-6);
        public const MetaDataType UnsynchronisedLyricsId = (MetaDataType)(-7);

        public const MetaDataType NullTagId = (MetaDataType)(-20);
        public const MetaDataType DateCreatedTagId = (MetaDataType)(-21);
        public const MetaDataType ClipboardTagId = (MetaDataType)(-150);

        public static string DisplayedArtistName;
        public static string ArtistArtistsName;
        public static string DisplayedComposerName;
        public static string ComposerComposersName;
        public static string DisplayedAlbumArtsistName;
        public static string ArtworkName;
        public static string LyricsName;
        public static string LyricsNamePostfix;
        public static string SynchronisedLyricsName;
        public static string UnsynchronisedLyricsName;

        public static string NullTagName;


        //Defaults for controls
        public static DataGridViewCellStyle UnchangedStyle = new DataGridViewCellStyle();
        public static DataGridViewCellStyle ChangedStyle = new DataGridViewCellStyle();

        public class SavedSettingsType
        {
            public int menuPlacement;
            public bool contextMenu;

            public bool dontShowCopyTag;
            public bool dontShowSwapTags;
            public bool dontShowChangeCase;
            public bool dontShowRencodeTag;
            public bool dontShowLibraryReports;
            public bool dontShowAutorate;
            public bool dontShowASR;
            public bool dontShowCAR;
            public bool dontShowShowHiddenWindows;
            public bool dontShowBackupRestore;

            public bool useSkinColors;
            public int closeShowHiddenWindows;

            public bool dontPlayCompletedSound;
            public bool playStartedSound;
            public bool playCanceledSound;

            public string copySourceTagName;
            public string changeCaseSourceTagName;
            public string reencodeTagSourceTagName;
            public string swapTagsSourceTagName;

            public string copyDestinationTagName;
            public string swapTagsDestinationTagName;

            public string initialEncodingName;
            public string usedEncodingName;

            public bool onlyIfDestinationIsEmpty;
            public bool smartOperation;
            public bool appendSource;
            public bool addSource;
            public string[] customText;
            public string[] appendedText;
            public string[] addedText;

            public int changeCaseFlag;
            public bool useExceptionWords;
            public bool useOnlyWords;
            public string[] exceptionWords;
            public bool useExceptionChars;
            public string exceptionChars;
            public bool useWordSplitters;
            public string wordSplitters;
            public bool alwaysCapitalize1stWord;
            public bool alwaysCapitalizeLastWord;

            public LibraryReportsPlugin.LibraryReportsPreset[] libraryReportsPresets4;
            public int libraryReportsPresetNumber;
            public string savedFieldName;
            public string destinationTagOfSavedFieldName;
            public bool conditionIsChecked;
            public string conditionTagName;
            public string condition;
            public string comparedField;
            public int filterIndex;

            public AutoLibraryReportsPlugin.AutoLibraryReportsPreset[] autoLibraryReportsPresets;
            public bool recalculateOnNumberOfTagsChanges;
            public decimal numberOfTagsToRecalculate;

            public string unitK;
            public string unitM;
            public string unitG;

            public string multipleItemsSplitterChar1;
            public string multipleItemsSplitterChar2;

            public string exportedTrackListName;

            public MetaDataType autoRateTagId;
            public bool storePlaysPerDay;
            public MetaDataType playsPerDayTagId;

            public bool autoRateAtStartUp;
            public bool notifyWhenAutoratingCompleted;
            public bool calculateThresholdsAtStartUp;
            public bool autoRateOnTrackProperties;
            public bool sinceAdded;

            public bool calculateAlbumRatingAtStartUp;
            public bool calculateAlbumRatingAtTagsChanged;
            public bool notifyWhenCalculationCompleted;
            public bool considerUnrated;
            public int defaultRating;
            public string trackRatingTagName;
            public string albumRatingTagName;

            public bool resizeArtwork;
            public ushort xArtworkSize;
            public ushort yArtworkSize;

            public bool checkBox5;
            public double threshold5;
            public bool checkBox45;
            public double threshold45;
            public bool checkBox4;
            public double threshold4;
            public bool checkBox35;
            public double threshold35;
            public bool checkBox3;
            public double threshold3;
            public bool checkBox25;
            public double threshold25;
            public bool checkBox2;
            public double threshold2;
            public bool checkBox15;
            public double threshold15;
            public bool checkBox1;
            public double threshold1;
            public bool checkBox05;
            public double threshold05;

            public decimal perCent5;
            public decimal perCent45;
            public decimal perCent4;
            public decimal perCent35;
            public decimal perCent3;
            public decimal perCent25;
            public decimal perCent2;
            public decimal perCent15;
            public decimal perCent1;
            public decimal perCent05;

            public decimal actualPerCent5;
            public decimal actualPerCent45;
            public decimal actualPerCent4;
            public decimal actualPerCent35;
            public decimal actualPerCent3;
            public decimal actualPerCent25;
            public decimal actualPerCent2;
            public decimal actualPerCent15;
            public decimal actualPerCent1;
            public decimal actualPerCent05;

            public DateTime lastImportDateUtc;

            public string[] forms;
            public SizePositionType[] sizesPositions;

            public int[] copyTagsSourceTagIds;

            public string autobackupDirectory;
            public string autobackupPrefix;
            public decimal autobackupInterval;
            public decimal autodeleteKeepNumberOfDays;
            public decimal autodeleteKeepNumberOfFiles;
        }

        public static SavedSettingsType SavedSettings;


        //Localizable strings

        //Supported exported file formats
        public string exportedFormats;

        //Plugin localizable strings
        public string pluginName;
        private string description;

        private string tagToolsMenuSectionName;
        private string backupRestoreMenuSectionName;

        public string copyTagCommandName;
        public string swapTagsCommandName;
        public string changeCaseCommandName;
        public string reencodeTagCommandName;
        public string libraryReportsCommandName;
        public string autoLibraryReportsCommandName;
        public string autoRateCommandName;
        public string asrCommandName;
        public string carCommandName;
        public string copyTagsToClipboardCommandName;
        public string pasteTagsFromClipboardCommandName;
        public string showHiddenCommandName;

        private string copyTagCommandDescription;
        private string swapTagsCommandDescription;
        private string changeCaseCommandDescription;
        private string reencodeTagCommandDescription;
        private string libraryReportsCommandDescription;
        private string autoLibraryReportsCommandDescription;
        private string autoRateCommandDescription;
        private string asrCommandDescription;
        private string carCommandDescription;
        private string copyTagsToClipboardCommandDescription;
        private string pasteTagsFromClipboardCommandDescription;
        private string showHiddenCommandDescription;

        public string backupTagsCommandName;
        public string restoreTagsCommandName;
        public string restoreTagsForEntireLibraryCommandName;
        public string renameBackupCommandName;
        public string deleteBackupCommandName;
        public string autoBackupSettingsCommandName;
        
        private string backupTagsCommandDescription;
        private string restoreTagsCommandDescription;
        private string restoreTagsForEntireLibraryCommandDescription;
        private string renameBackupCommandDescription;
        private string deleteBackupCommandDescription;
        private string autoBackupSettingsCommandDescription;

        public string copyTagCommandSbText;
        public string swapTagsCommandSbText;
        public string changeCaseCommandSbText;
        public string reencodeTagCommandSbText;
        public string libraryReportsCommandSbText;
        public string libraryReportsGneratingPreviewCommandSbText;
        public string autoRateCommandSbText;
        public string asrCommandSbText;
        public string carCommandSbText;

        //Other localizable strings
        public static string AlbumTagName;
        public static string Custom9TagName;
        public static string UrlTagName;
        public static string GenreCategoryName;

        public static string GroupingName;
        public static string CountName;
        public static string SumName;
        public static string MinimumName;
        public static string MaximumName;
        public static string AverageName;
        public static string AverageCountName;

        public string libraryReport;

        public static string DateCreatedTagName;
        public static string EmptyValueTagName;
        public static string ClipboardTagName;
        public static string TextFileTagName;
        public static string SequenceNumberName;

        public static string ParameterTagName;
        public static string TempTagName;

        public string customTagsPresetName;
        public string libraryTotalsPresetName;
        public string libraryAveragesPresetName;

        public string emptyPresetName;

        //Displayed text
        public string listItemConditionIs;
        public string listItemConditionIsNot;
        public string listItemConditionIsGreater;
        public string listItemConditionIsLess;

        public string stopButtonName;
        public string cancelButtonName;
        public string hideButtonName;
        public string previewButtonName;
        public string clearButtonName;

        public string defaultASRPresetName;

        public string tableCellError;

        public string sbSorting;
        public string sbUpdating;
        public string sbReading;
        public string sbUpdated;
        public string sbRead;
        public string sbFiles;
        public string sbPresetIsAutoApplied1;
        public string sbPresetIsAutoApplied2;

        public string sbAutobackuping;
        public string sbMovingBackupsToNewFolder;

        public string msgFileNotFound;
        public string msgNoFilesSelected;
        public string msgSourceAndDestinationTagsAreTheSame;
        public string msgSwapTagsSourceAndDestinationTagsAreTheSame;
        public string msgNoTagsSelected;
        public string msgNoFilesInCurrentView;
        public string msgTracklistIsEmpty;
        public string msgForExportingPlaylistsURLfieldMustBeIncludedInTagList;
        public string msgPreviewIsNotGeneratedNothingToSave;
        public string msgPreviewIsNotGeneratedNothingToChange;
        public string msgNoAggregateFunctionNothingToSave;
        public string msgPleaseUseGroupingFunctionForArtworkTag;
        public static string MsgAllTags;
        public string msgNoURLcolumnUnableToSave;
        public string msgEmptyURL;
        public string msgUnableToSave;
        public string msgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag;
        public string msgBackgroundTaskIsCompleted;
        public string msgThresholdsDescription;
        public string msgAutoCalculationOfThresholdsDescription;

        public string msgNumberOfPlayedTracks;
        public string msgIncorrectSumOfWeights;
        public string msgSum;
        public string msgNumberOfNotRatedTracks;
        public string msgTracks;
        public string msgActualPercent;

        public string msgNoPresetSelected;
        public string msgIncorrectPresetName;
        public string msgSendingEmailConfirmation;
        public string msgDeletePresetConfirmation;
        public string msgImportingConfirmation;
        public string msgNoPresetsImported;
        public string msgPresetsWereImported;
        public string msgFailedToImport;
        public string msgPresets;
        public string msgDeletingConfirmation;
        public string msgNoPresetsDeleted;
        public string msgPresetsWereDeleted;
        public string msgFailedToDelete;

        public string msgNumberOfTagsInTextFile;
        public string msgDoesntCorrespondToNumberOfSelectedTracks;
        public string msgMessageEnd;

        public string msgClipboardDesntContainText;

        public string msgNumberOfTagsInClipboard;
        public string msgNumberOfTracksInClipboard;
        public string msgDoesntCorrespondToNumberOfSelectedTracksC;
        public string msgDoesntCorrespondToNumberOfCopiedTagsC;
        public string msgMessageEndC;
        public string msgDoYouWantToPasteAnyway;

        public string msgFirstThreeGroupingFieldsInPreviewTableShouldBe;

        public string msgBackgroundAutoLibraryReportIsExecuted;

        public string msgSaveBackupTitle;
        public string msgRestoreBackupTitle;
        public string msgRenameSelectBackupTitle;
        public string msgRenameSaveBackupTitle;
        public string msgMusicBeeBackupType;
        public string msgDeleteBackupTitle;
        public string msgMasterBackupIndexIsCorrupted;

        public string ctlDirtyError1sf;
        public string ctlDirtyError1mf;
        public string ctlDirtyError2sf;
        public string ctlDirtyError2mf;

        public struct SwappedTags
        {
            public string newDestinationTagValue;
            public string newDestinationTagTValue;
            public string destinationTagTValue;
            public string newSourceTagValue;
            public string newSourceTagTValue;
            public string sourceTagTValue;
        }

        public struct ConvertStringsResults
        {
            public int type; // 3 - date-time/timespan, 2 - double, 1 - items count, 0 - unknown/string

            public double result1f;
            public double result2f;
            public string result1s;
            public string result2s;

            public SortedDictionary<string, object> items;
            public SortedDictionary<string, object> items1;

            public ConvertStringsResults(int typeParam)
            {
                type = typeParam;
                result1f = 0;
                result2f = 0;
                result1s = null;
                result2s = null;
                items = new SortedDictionary<string, object>();
                items1 = new SortedDictionary<string, object>();
            }

            public string getResult()
            {
                if (items1.Count != 0) //Its 'average count' function
                    return "" + Math.Round((double) items1.Count / items.Count, 2);
                else if (type == 0)
                {
                    if (items.Count == 0)
                        return "" + result1s;
                    else //Its 'average' function
                        return "" + Math.Round(result1f / items.Count, 2);
                }
                else if (type == 1)
                {
                    return "" + items.Count;
                }
                else if (type == 2)
                {
                    if (items.Count == 0)
                        return "" + result1f;
                    else //Its 'average' function
                        return "" + Math.Round(result1f / items.Count, 2);
                }
                else //if (type == 3)
                {
                    if (items.Count == 0)
                        return "" + TimeSpan.FromSeconds(result1f);
                    else //Its 'average' function
                        return "" + TimeSpan.FromSeconds((int)(result1f / items.Count));
                }
            }
        }

        public static ConvertStringsResults ConvertStrings(string xstring, string ystring = null, bool replacements = false)
        {
            ConvertStringsResults results = new ConvertStringsResults();

            if (ystring == null)
                ystring = xstring;

            if (replacements)
            {
                string additionalUnitK = "";
                string additionalUnitM = "";
                string additionalUnitG = "";

                if (Plugin.SavedSettings.unitK != "")
                    additionalUnitK = "|" + Plugin.SavedSettings.unitK;

                if (Plugin.SavedSettings.unitM != "")
                    additionalUnitM = "|" + Plugin.SavedSettings.unitM;

                if (Plugin.SavedSettings.unitG != "")
                    additionalUnitG = "|" + Plugin.SavedSettings.unitG;

                xstring = Regex.Replace(xstring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(g|г" + additionalUnitG + ").*$", "$1`$3$4$5~000000000", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(g|г" + additionalUnitG + ").*$", "$1`$3$4$5~000000000", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"~", "", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"~", "", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"^(.*?)`(.{9}).*$", "$1$2", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(.*?)`(.{9}).*$", "$1$2", RegexOptions.IgnoreCase);


                xstring = Regex.Replace(xstring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(m|м" + additionalUnitM + ").*$", "$1`$3$4$5~000000", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(m|м" + additionalUnitM + ").*$", "$1`$3$4$5~000000", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"~", "", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"~", "", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"^(.*?)`(.{6}).*$", "$1$2", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(.*?)`(.{6}).*$", "$1$2", RegexOptions.IgnoreCase);


                xstring = Regex.Replace(xstring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(k|к" + additionalUnitK + ").*$", "$1`$3$4$5~000", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(k|к" + additionalUnitK + ").*$", "$1`$3$4$5~000", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"~", "", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"~", "", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"^(.*?)`(.{3}).*$", "$1$2", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(.*?)`(.{3}).*$", "$1$2", RegexOptions.IgnoreCase);


                xstring = Regex.Replace(xstring, @"^(\d+).*$", "$1", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+).*$", "$1", RegexOptions.IgnoreCase);
            }


            try
            {
                results.result1f = Convert.ToDouble(xstring);
                results.result2f = Convert.ToDouble(ystring);
                results.type = 2;

                return results;
            }
            catch
            {
                try
                {
                    //results.result1ts = DateTime.ParseExact(xstring, DurationTemplates, null, DateTimeStyles.AssumeLocal) - DateTime.Now.Date;
                    //results.result2ts = DateTime.ParseExact(xstring, DurationTemplates, null, DateTimeStyles.AssumeLocal) - DateTime.Now.Date;

                    string[] ts1 = xstring.Split(':');
                    TimeSpan time;

                    time = TimeSpan.FromSeconds(Convert.ToInt32(ts1[ts1.Length - 1]));
                    if (ts1.Length > 1)
                        time += TimeSpan.FromMinutes(Convert.ToInt32(ts1[ts1.Length - 2]));
                    if (ts1.Length > 2)
                        time += TimeSpan.FromHours(Convert.ToInt32(ts1[ts1.Length - 3]));

                    results.result1f = time.TotalSeconds;


                    string[] ts2 = ystring.Split(':');

                    time = TimeSpan.FromSeconds(Convert.ToInt32(ts2[ts2.Length - 1]));
                    if (ts2.Length > 1)
                        time += TimeSpan.FromMinutes(Convert.ToInt32(ts2[ts2.Length - 2]));
                    if (ts2.Length > 2)
                        time += TimeSpan.FromHours(Convert.ToInt32(ts2[ts2.Length - 3]));

                    results.result2f = time.TotalSeconds;


                    results.type = 3;

                    return results;
                }
                catch
                {
                    if (!replacements)
                    {
                        return ConvertStrings(xstring, ystring, true);
                    }
                    else
                    {
                        results.result1s = xstring;
                        results.result2s = ystring;
                        results.type = 0;

                        return results;
                    }
                }
            }
        }

        public static int CompareStrings(string xstring, string ystring)
        {
            ConvertStringsResults results;

            results = ConvertStrings(xstring, ystring);

            switch (results.type)
            {
                case 1:
                    if (results.result1f > results.result2f)
                        return 1;
                    else if (results.result1f < results.result2f)
                        return -1;
                    else
                        return 0;

                case 2:
                    if (results.result1f > results.result2f)
                        return 1;
                    else if (results.result1f < results.result2f)
                        return -1;
                    else
                        return 0;

                case 3:
                    if (results.result1f > results.result2f)
                        return 1;
                    else if (results.result1f < results.result2f)
                        return -1;
                    else
                        return 0;

                default:
                    return String.Compare(xstring, ystring);
            }
        }

        public class DataGridViewCellComparator : System.Collections.IComparer
        {
            public int comparedColumnIndex = -1;

            public int Compare(object x, object y)
            {
                if (comparedColumnIndex != -1)
                {
                    int comparation = 0;

                    comparation = CompareStrings((string)(((DataGridViewRow)x).Cells[comparedColumnIndex].Value), (string)(((DataGridViewRow)y).Cells[comparedColumnIndex].Value));

                    if (comparation > 0)
                        return 1;
                    else if (comparation < 0)
                        return -1;
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }


        public class TextTableComparator : System.Collections.Generic.IComparer<string[]>
        {
            public int tagCounterIndex = -1;

            public int Compare(string[] x, string[] y)
            {
                if (tagCounterIndex != -1)
                {
                    int comparation = 0;

                    for (int i = 0; i < tagCounterIndex; i++)
                    {
                        comparation = CompareStrings(x[i], y[i]);

                        if (comparation > 0)
                            return 1;
                        else if (comparation < 0)
                            return -1;
                    }

                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string getTagRepresentation(string tag)
        {
            return replaceMSIds(removeMSIdAtTheEndOfString(removeRoleIds(tag)));
        }

        public string getTrackRepresentation(string currentFile)
        {
            string trackRepresentation = "";
            string displayedArtist;
            string album;
            string title;
            string diskNo;
            string trackNo;

            displayedArtist = getFileTag(currentFile, DisplayedArtistId);
            album = getFileTag(currentFile, MetaDataType.Album);
            title = getFileTag(currentFile, MetaDataType.TrackTitle);
            diskNo = getFileTag(currentFile, MetaDataType.DiscNo);
            trackNo = getFileTag(currentFile, MetaDataType.TrackNo);

            trackRepresentation += diskNo;
            trackRepresentation += (trackRepresentation == "") ? (trackNo) : ("-" + trackNo);
            trackRepresentation += (trackRepresentation == "") ? (displayedArtist) : (". " + displayedArtist);
            trackRepresentation += (trackRepresentation == "") ? (album) : (" - " + album);
            trackRepresentation += (trackRepresentation == "") ? (title) : (" - " + title);

            return trackRepresentation;
        }

        public SwappedTags swapTags(string sourceTagValue, string destinationTagValue, MetaDataType sourceTagId, MetaDataType destinationTagId,
            bool smartOperation, bool appendSourceToDestination = false, string appendedText = "", bool addSourceToDestination = false, string addedText = "")
        {
            SwappedTags swappedTags = new SwappedTags();
            string appendedValue;

            if (smartOperation)
            {
                if (appendSourceToDestination)
                {
                    if (destinationTagId == ArtistArtistsId)
                        appendedValue = replaceMSChars(appendedText) + removeMSIdAtTheEndOfString(sourceTagValue);
                    else if (destinationTagId == ComposerComposersId)
                        appendedValue = replaceMSChars(appendedText) + removeMSIdAtTheEndOfString(removeRoleIds(sourceTagValue));
                    else
                        appendedValue = appendedText + getTagRepresentation(sourceTagValue);

                    swappedTags.newDestinationTagValue = removeMSIdAtTheEndOfString(destinationTagValue) + appendedValue;
                }
                else if (addSourceToDestination)
                {
                    if (destinationTagId == ArtistArtistsId)
                        appendedValue = removeMSIdAtTheEndOfString(sourceTagValue) + replaceMSChars(addedText);
                    else if (destinationTagId == ComposerComposersId)
                        appendedValue = removeMSIdAtTheEndOfString(removeRoleIds(sourceTagValue)) + replaceMSChars(addedText);
                    else
                        appendedValue = getTagRepresentation(sourceTagValue) + addedText;

                    swappedTags.newDestinationTagValue = appendedValue + removeMSIdAtTheEndOfString(destinationTagValue);
                }
                else
                {
                    if (destinationTagId == ArtistArtistsId)
                        appendedValue = removeMSIdAtTheEndOfString(sourceTagValue);
                    else if (destinationTagId == ComposerComposersId)
                        appendedValue = removeMSIdAtTheEndOfString(removeRoleIds(sourceTagValue));
                    else
                        appendedValue = getTagRepresentation(sourceTagValue);

                    swappedTags.newDestinationTagValue = appendedValue;
                }
                swappedTags.newDestinationTagTValue = replaceMSIds(removeRoleIds(swappedTags.newDestinationTagValue));
                swappedTags.destinationTagTValue = getTagRepresentation(destinationTagValue);

                if (sourceTagId == destinationTagId) //Smart conversion of multiple items of one tag
                {
                    if (replaceMSIds(sourceTagValue) == sourceTagValue) //No MS ids, its a single item
                    {
                        appendedValue = replaceMSChars(sourceTagValue);
                    }
                    else //Multiple items
                    {
                        appendedValue = getTagRepresentation(sourceTagValue);
                    }
                }
                else //Normal swapping
                {
                    if (sourceTagId == ArtistArtistsId)
                        appendedValue = removeMSIdAtTheEndOfString(destinationTagValue);
                    else if (sourceTagId == ComposerComposersId)
                        appendedValue = removeMSIdAtTheEndOfString(removeRoleIds(destinationTagValue));
                    else
                        appendedValue = getTagRepresentation(destinationTagValue);
                }

                swappedTags.newSourceTagValue = appendedValue;
                swappedTags.newSourceTagTValue = replaceMSIds(removeRoleIds(swappedTags.newSourceTagValue));
                swappedTags.sourceTagTValue = getTagRepresentation(sourceTagValue);
            }
            else
            {
                if (appendSourceToDestination)
                {
                    appendedValue = appendedText + sourceTagValue;
                    swappedTags.newDestinationTagValue = destinationTagValue + appendedValue;
                }
                else if (addSourceToDestination)
                {
                    appendedValue = sourceTagValue + addedText;
                    swappedTags.newDestinationTagValue = appendedValue + destinationTagValue;
                }
                else
                {
                    appendedValue = sourceTagValue;
                    swappedTags.newDestinationTagValue = appendedValue;
                }
                swappedTags.newDestinationTagTValue = swappedTags.newDestinationTagValue;
                swappedTags.destinationTagTValue = destinationTagValue;


                appendedValue = destinationTagValue;
                swappedTags.newSourceTagValue = appendedValue;
                swappedTags.newSourceTagTValue = swappedTags.newSourceTagValue;
                swappedTags.sourceTagTValue = sourceTagValue;
            }


            return swappedTags;
        }

        public static void ComboBoxLeave(ComboBox comboBox)
        {
            if (comboBox.Text == "")
                return;

            string comboBoxText = comboBox.Text;

            if (comboBox.Items.Contains(comboBoxText))
                comboBox.Items.Remove(comboBoxText);
            else
                comboBox.Items.RemoveAt(9);

            comboBox.Items.Insert(0, comboBoxText);

            comboBox.Text = comboBoxText;
        }

        public static void SetItemInComboBox(ComboBox comboBox, object item)
        {
            bool itemIsFound = false;

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (("" + comboBox.Items[i]) == ("" + item))
                {
                    comboBox.Items.RemoveAt(i);
                    itemIsFound = true;
                    break;
                }
            }

            if (!itemIsFound)
                comboBox.Items.RemoveAt(9);

            comboBox.Items.Insert(NumberOfPredefinedPresets, item);
            comboBox.SelectedItem = item;
        }

        public string getFileTag(string sourceFileUrl, MetaDataType tagId, bool autoAlbumArtist = false, bool normalizeTrackRatingTo0_100Range = false)
        {
            string tag;
            string rawArtist = "";
            string multiArtist = "";
            string rawComposer = "";
            string multiComposer = "";

            switch (tagId)
            {
                case (MetaDataType)0:
                    tag = "";
                    break;

                case NullTagId:
                    tag = "";
                    break;

                case DisplayedArtistId:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                    break;

                case ArtistArtistsId:
                    rawArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                    multiArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiArtist);

                    if (multiArtist != "")
                        tag = multiArtist;
                    else
                        tag = rawArtist;
                    break;

                case DisplayedComposerId:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Composer);
                    break;

                case ComposerComposersId:
                    rawComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Composer);
                    multiComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiComposer);

                    if (multiComposer != "")
                        tag = multiComposer;
                    else
                        tag = rawComposer;
                    break;

                case MetaDataType.Artwork:
                    tag = MbApiInterface.Library_GetArtwork(sourceFileUrl, 0);
                    break;

                case LyricsId:
                    tag = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.NotSpecified);
                    break;

                case SynchronisedLyricsId:
                    tag = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.Synchronised);
                    break;

                case UnsynchronisedLyricsId:
                    tag = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.UnSynchronised);
                    break;

                case MetaDataType.Rating:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Rating);

                    if (normalizeTrackRatingTo0_100Range)
                    {
                        if (tag != "")
                        {
                            double rating = Plugin.ConvertStrings(tag).result1f;

                            if (rating <= 5)
                                rating *= 20;

                            tag = rating.ToString();
                        }
                    }

                    break;

                default:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, tagId);
                    break;
            }


            if (tag == null)
                tag = "";


            return tag;
        }

        public bool setFileTag(string sourceFileUrl, MetaDataType tagId, string value, bool updateOnlyChangedTags = false)
        {
            string multiArtist = "";
            string multiComposer = "";
            bool result1;
            bool result2;


            if (updateOnlyChangedTags)
            {
                if (tagId == (MetaDataType)(-201) || tagId == (MetaDataType)(-202) || tagId == (MetaDataType)(-203) || tagId == (MetaDataType)(-204))
                    return true;
                else if (getFileTag(sourceFileUrl, tagId, true) == value)
                    return true;
                else
                    lock (tagsWereChanged)
                    {
                        if (!tagsWereChanged.Contains(sourceFileUrl))
                            tagsWereChanged.Add(sourceFileUrl);
                    }
            }

            switch (tagId)
            {
                case NullTagId:
                    return true;

                case DisplayedArtistId:
                    multiArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiArtist);
                    result1 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Artist, value);
                    result2 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiArtist, multiArtist);
                    return result1 && result2;

                case ArtistArtistsId:
                    if (replaceMSIds(value) == value) //No MS Ids, single artist
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Artist, value);
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiArtist, value);

                case DisplayedComposerId:
                    multiComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiComposer);
                    result1 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Composer, value);
                    result2 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiComposer, multiComposer);
                    return result1 && result2;

                case ComposerComposersId:
                    if (replaceMSIds(value) == value) //No MS Ids, single composer
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Composer, value);
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiComposer, value);

                case MetaDataType.Artwork:
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Bitmap));
                    Bitmap pic = (Bitmap)tc.ConvertFrom(Convert.FromBase64String(value));
                    byte[] imageData = (byte[])tc.ConvertTo(pic, typeof(byte[]));

                    return MbApiInterface.Library_SetArtworkEx(sourceFileUrl, 0, imageData);

                case MetaDataType.AlbumArtistRaw:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.AlbumArtist, value);

                case LyricsId:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Lyrics, value);

                case MetaDataType.Rating:
                    if (value.ToLower() == "no rating")
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Rating, "");
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Rating, value);

                case MetaDataType.RatingAlbum:
                    if (value.ToLower() == "no rating")
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.RatingAlbum, "");
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.RatingAlbum, value);

                default:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, tagId, value);
            }
        }

        public bool commitTagsToFile(string sourceFileUrl, bool ignoreTagsChanged = false, bool updateOnlyChangedTags = false)
        {
            bool result = false;

            if (updateOnlyChangedTags)
            {
                lock (tagsWereChanged)
                {
                    if (!tagsWereChanged.Contains(sourceFileUrl))
                        result = true;
                }

                if (result)
                    return true;
            }

            if (ignoreTagsChanged)
            {
                lock (filesUpdatedByPlugin)
                {
                    filesUpdatedByPlugin.Add(sourceFileUrl);
                }
            }

            result = MbApiInterface.Library_CommitTagsToFile(sourceFileUrl);

            refreshPanels();

            lock (tagsWereChanged)
            {
                if (tagsWereChanged.Contains(sourceFileUrl))
                    tagsWereChanged.Remove(sourceFileUrl);
            }

            return result;
        }

        public void refreshPanels(bool immediateRefresh = false)
        {
            if (immediateRefresh)
            {
                if (uiRefreshingIsNeeded)
                {
                    uiRefreshingIsNeeded = false;
                    lock (lastUI_RefreshLocker)
                    {
                        lastUI_Refresh = DateTime.Now;
                    }

                    MbApiInterface.MB_RefreshPanels();
                    MbApiInterface.MB_SetBackgroundTaskMessage(lastMessage);
                }
            }
            else
            {
                bool refresh = false;

                lock (lastUI_RefreshLocker)
                {
                    if (DateTime.Now - lastUI_Refresh >= refreshUI_Delay)
                    {
                        lastUI_Refresh = DateTime.Now;
                        refresh = true;
                    }
                }

                
                if (refresh)
                {
                    uiRefreshingIsNeeded = false;
                    MbApiInterface.MB_RefreshPanels();
                    MbApiInterface.MB_SetBackgroundTaskMessage(lastMessage);
                }
                else
                {
                    uiRefreshingIsNeeded = true;
                }
            }
        }

        public void setStatusbarText(string newMessage)
        {
            if (lastMessage != newMessage)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(newMessage);
                lastMessage = newMessage;
            }
        }

        public string removeMSIdAtTheEndOfString(string value)
        {
            value += EndOfStringId;
            value = value.Replace("" + MultipleItemsSplitterId + EndOfStringId, "");
            value = value.Replace("" + EndOfStringId, "");

            return value;
        }

        public string replaceMSChars(string value)
        {
            value = value.Replace(SavedSettings.multipleItemsSplitterChar2, "" + MultipleItemsSplitterId);
            value = value.Replace(SavedSettings.multipleItemsSplitterChar1, "" + MultipleItemsSplitterId);

            return value;
        }

        public string replaceMSIds(string value)
        {
            value = value.Replace("" + MultipleItemsSplitterId, SavedSettings.multipleItemsSplitterChar2);

            return value;
        }

        public string removeRoleIds(string value)
        {
            value = value.Replace("" + GuestId, "");
            value = value.Replace("" + PerformerId, "");
            value = value.Replace("" + RemixerId, "");

            return value;
        }

        public static MetaDataType GetTagId(string tagName)
        {
            MetaDataType tagId;

            if(tagName == null) tagName = "";

            if (tagName == DateCreatedTagName)
                return DateCreatedTagId;

            if (TagNamesIds.TryGetValue(tagName, out tagId))
            {
                return tagId;
            }
            else
            {
                return 0;
            }
        }

        public static string GetTagName(MetaDataType tagId)
        {
            if (tagId == DateCreatedTagId)
                return DateCreatedTagName;

            string tagName;
            if (TagIdsNames.TryGetValue(tagId, out tagName))
            {
                return tagName;
            }
            else
            {
                return "";
            }
        }

        public static void FillList(System.Collections.IList list, bool addReadOnlyTagsAlso = false, bool addArtworkAlso = false, bool addNullAlso = true)
        {
            foreach (string element in TagNamesIds.Keys)
            {
                if (element == ArtworkName)
                {
                    if (addArtworkAlso)
                        list.Add(element);
                }
                else
                {
                    if (addNullAlso || element != NullTagName)
                        if (addReadOnlyTagsAlso || !ChangeCasePlugin.IsItemContainedInList(element, ReadonlyTagsNames))
                            list.Add(element);
                }
            }
        }

        public FilePropertyType getPropId(string propName)
        {
            FilePropertyType propId;
            if (PropNamesIds.TryGetValue(propName, out propId))
            {
                return propId;
            }
            else
            {
                return 0;
            }
        }

        public string getPropName(FilePropertyType propId)
        {
            string propName;
            if (PropIdsNames.TryGetValue(propId, out propName))
            {
                return propName;
            }
            else
            {
                return "";
            }
        }

        public static void FillListWithProps(System.Collections.IList list)
        {
            foreach (string element in PropNamesIds.Keys)
            {
                list.Add(element);
            };
        }

        public void initializeSbText()
        {
            lastCommandSbText = "";
            lastPreview = true;
            lastFileCounter = 0;

        }

        public void setResultingSbText()
        {
            if (lastCommandSbText == "")
            {
                setStatusbarText("");
                return;
            }

            string sbText;

            //if (lastPreview)
            //    sbText = lastCommandSbText + ": " + lastFileCounter + " " + sbFiles + " " + sbRead;
            //else
            //    sbText = lastCommandSbText + ": " + lastFileCounter + " " + sbFiles + " " + sbUpdated;

            if (lastPreview)
                sbText = lastCommandSbText + ": 100% " + sbRead;
            else
                sbText = lastCommandSbText + ": 100% " + sbUpdated;

            setStatusbarText(sbText);
        }

        public void setStatusbarTextForFileOperations(string commandSbText, bool preview, int fileCounter, int filesTotal, string currentFile = null, bool immediateDisplaying = false)
        {
            string sbText;

            lastCommandSbText = commandSbText;
            lastPreview = preview;
            lastFileCounter = fileCounter + 1;

            fileCounter++;

            if (immediateDisplaying || fileCounter % Plugin.StatusbarTextUpdateInterval == 0)
            {
                if (preview)
                    sbText = commandSbText + " (" + sbReading + "): " + Math.Round((double)100*lastFileCounter/filesTotal, 0) + "%";
                else
                    sbText = commandSbText + " (" + sbUpdating + "): " + Math.Round((double)100*lastFileCounter / filesTotal, 0) + "%";

                if (currentFile != null)
                    sbText += " (" + currentFile + ")";

                setStatusbarText(sbText);
            }
        }

        private void periodicUI_Refresh(object state)
        {
            if (uiRefreshingIsNeeded)
            {
                lock (lastUI_RefreshLocker)
                {
                    lastUI_Refresh = DateTime.Now;
                }

                uiRefreshingIsNeeded = false;
                MbApiInterface.MB_RefreshPanels();
                MbApiInterface.MB_SetBackgroundTaskMessage(lastMessage);
            }
        }

        public void periodicAutobackup(object state)
        {
            lock (autobackupLocker)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(sbAutobackuping);
                backupIndexDictionary.saveBackup(GetDefaultBackupFilename(SavedSettings.autobackupPrefix));
            }

            MbApiInterface.MB_SetBackgroundTaskMessage("");
        }

        public void copyTagEventHandler(object sender, EventArgs e)
        {
            CopyTagPlugin tagToolsForm = new CopyTagPlugin(this);
            tagToolsForm.display();
        }

        public void swapTagsEventHandler(object sender, EventArgs e)
        {
            SwapTagsPlugin tagToolsForm = new SwapTagsPlugin(this);
            tagToolsForm.display();
        }

        public void changeCaseEventHandler(object sender, EventArgs e)
        {
            ChangeCasePlugin tagToolsForm = new ChangeCasePlugin(this);
            tagToolsForm.display();
        }

        public void reencodeTagEventHandler(object sender, EventArgs e)
        {
            ReencodeTagPlugin tagToolsForm = new ReencodeTagPlugin(this);
            tagToolsForm.display();
        }

        public void libraryReportsEventHandler(object sender, EventArgs e)
        {
            LibraryReportsPlugin tagToolsForm = new LibraryReportsPlugin(this);
            tagToolsForm.display();
        }

        public void autoLibraryReportsEventHandler(object sender, EventArgs e)
        {
            AutoLibraryReportsPlugin tagToolsForm = new AutoLibraryReportsPlugin(this);
            tagToolsForm.display();
        }

        public void autoRateEventHandler(object sender, EventArgs e)
        {
            AutoRatePlugin tagToolsForm = new AutoRatePlugin(this);
            tagToolsForm.display();
        }

        public void asrEventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplacePlugin tagToolsForm = new AdvancedSearchAndReplacePlugin(this);
            tagToolsForm.display();
        }

        public void carEventHandler(object sender, EventArgs e)
        {
            CalculateAverageAlbumRatingPlugin tagToolsForm = new CalculateAverageAlbumRatingPlugin(this);
            tagToolsForm.display();
        }

        public void copyTagsToClipboardEventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardPlugin tagToolsForm = new CopyTagsToClipboardPlugin(this);
            tagToolsForm.display();
        }

        public void pasteTagsFromClipboardEventHandler(object sender, EventArgs e)
        {
            PasteTagsFromClipboardPlugin tagToolsForm = new PasteTagsFromClipboardPlugin(this);
            tagToolsForm.display();
        }

        public void showHiddenEventHandler(object sender, EventArgs e)
        {
            lock (openedForms)
            {
                foreach (PluginWindowTemplate form in openedForms)
                {
                    if (!form.Visible)
                        form.Visible = true;
                }
            }
        }

        public void backupTagsEventHandler(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = msgSaveBackupTitle;
            dialog.Filter = msgMusicBeeBackupType;
            dialog.InitialDirectory = SavedSettings.autobackupDirectory;
            dialog.FileName = GetDefaultBackupFilename(BackupDefaultPrefix);

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            if (System.IO.File.Exists(dialog.FileName))
                System.IO.File.Delete(dialog.FileName);
            if (System.IO.File.Exists(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbd"))
                System.IO.File.Delete(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbd");

            backupIndexDictionary.saveBackup(GetBackupFilenameWithoutExtension(dialog.FileName));
        }

        public void restoreTagsEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = msgRestoreBackupTitle;
            dialog.Filter = msgMusicBeeBackupType;
            dialog.InitialDirectory = SavedSettings.autobackupDirectory;

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            backupIndexDictionary.loadBackup(GetBackupFilenameWithoutExtension(dialog.FileName), false);
        }

        public void restoreTagsForEntireLibraryEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = msgRestoreBackupTitle;
            dialog.Filter = msgMusicBeeBackupType;
            dialog.InitialDirectory = SavedSettings.autobackupDirectory;

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            backupIndexDictionary.loadBackup(GetBackupFilenameWithoutExtension(dialog.FileName), true);
        }

        public void renameBackupEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = msgRenameSelectBackupTitle;
            openDialog.Filter = msgMusicBeeBackupType;
            openDialog.InitialDirectory = SavedSettings.autobackupDirectory;

            if (openDialog.ShowDialog() == DialogResult.Cancel) return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = msgRenameSaveBackupTitle;
            saveDialog.Filter = msgMusicBeeBackupType;
            saveDialog.InitialDirectory = SavedSettings.autobackupDirectory;
            saveDialog.FileName = openDialog.SafeFileName;

            if (saveDialog.ShowDialog() == DialogResult.Cancel) return;

            if (System.IO.File.Exists(saveDialog.FileName))
                System.IO.File.Delete(saveDialog.FileName);
            if (System.IO.File.Exists(GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbd"))
                System.IO.File.Delete(GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbd");

            System.IO.File.Move(openDialog.FileName, saveDialog.FileName);
            System.IO.File.Move(GetBackupFilenameWithoutExtension(openDialog.FileName) + ".mbd", GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbd");
        }

        public void deleteBackupEventHandler(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = msgDeleteBackupTitle;
            dialog.Filter = msgMusicBeeBackupType;
            dialog.InitialDirectory = SavedSettings.autobackupDirectory;

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            BackupDateType backupDate = new BackupDateType();
            backupDate.load(GetBackupFilenameWithoutExtension(dialog.FileName));

            backupIndexDictionary.deleteBackup(backupDate);

            System.IO.File.Delete(dialog.FileName);
            System.IO.File.Delete(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbd");
        }

        public void autoBackupSettingsEventHandler(object sender, EventArgs e)
        {
            AutoBackupSettingsPlugin tagToolsForm = new AutoBackupSettingsPlugin(this);
            tagToolsForm.display();
        }

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            MbApiInterface = new MusicBeeApiInterface();
            MbApiInterface.Initialise(apiInterfacePtr);

            MB_RefreshPanels = MbApiInterface.MB_RefreshPanels;


            //Localizable strings

            //Plugin localizable strings
            pluginName = "Additional Tagging Tools";
            description = "Adds some tagging tools to MusicBee";

            tagToolsMenuSectionName = "ADDITIONAL TAGGING";
            backupRestoreMenuSectionName = "BACKUP && RESTORE";

            copyTagCommandName = "Copy tag...";
            swapTagsCommandName = "Swap tags...";
            changeCaseCommandName = "Change case...";
            reencodeTagCommandName = "Reencode tag...";
            libraryReportsCommandName = "Library reports...";
            autoLibraryReportsCommandName = "Auto library reports...";
            autoRateCommandName = "Auto rate tracks...";
            asrCommandName = "Advanced search and replace...";
            carCommandName = "Calculate average album rating...";
            copyTagsToClipboardCommandName = "Copy tags to clipboard...";
            pasteTagsFromClipboardCommandName = "Paste tags from clipboard";
            showHiddenCommandName = "Show hidden plugin windows";

            copyTagCommandDescription = "Tagging tools: Copy tag";
            swapTagsCommandDescription = "Tagging tools: Swap tags";
            changeCaseCommandDescription = "Tagging tools: Change Case";
            reencodeTagCommandDescription = "Tagging tools: Reencode tag";
            libraryReportsCommandDescription = "Tagging tools: Library reports";
            autoLibraryReportsCommandDescription = "Tagging tools: Auto library reports";
            autoRateCommandDescription = "Tagging tools: Auto rate tracks";
            asrCommandDescription = "Tagging tools: Advanced search and replace";
            carCommandDescription = "Tagging tools: Calculate average album rating";
            copyTagsToClipboardCommandDescription = "Tagging tools: Copy tags to clipboard";
            pasteTagsFromClipboardCommandDescription = "Tagging tools: Paste tags from clipboard";
            showHiddenCommandDescription = "Tagging tools: Show hidden plugin windows";

            backupTagsCommandName = "Backup tags for all tracks...";
            restoreTagsCommandName = "Restore tags for selected tracks...";
            restoreTagsForEntireLibraryCommandName = "Restore tags for all tracks...";
            renameBackupCommandName = "Rename or move backup...";
            deleteBackupCommandName = "Delete backup...";
            autoBackupSettingsCommandName = "Autobackup settings...";

            backupTagsCommandDescription = "Tagging tools: Backup tags for all tracks";
            restoreTagsCommandDescription = "Tagging tools: Restore tags for selected tracks";
            restoreTagsForEntireLibraryCommandDescription = "Tagging tools: Restore tags for all tracks";
            renameBackupCommandDescription = "Tagging tools: Rename backup";
            deleteBackupCommandDescription = "Tagging tools: Delete backup";
            autoBackupSettingsCommandDescription = "Tagging tools: Autobackup settings";

            copyTagCommandSbText = "Copying tag";
            swapTagsCommandSbText = "Swapping tags";
            changeCaseCommandSbText = "Changing case";
            reencodeTagCommandSbText = "Reencoding tag";
            libraryReportsCommandSbText = "Generating report";
            libraryReportsGneratingPreviewCommandSbText = "Generating preview";
            autoRateCommandSbText = "Auto rating tracks";
            asrCommandSbText = "Advanced searching and replacing";
            carCommandSbText = "Calculating average album rating";

            //Other localizable strings
            AlbumTagName = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            Custom9TagName = MbApiInterface.Setting_GetFieldName(MetaDataType.Custom9);
            UrlTagName = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            GenreCategoryName = MbApiInterface.Setting_GetFieldName(MetaDataType.GenreCategory);

            GroupingName = "<Grouping>";
            CountName = "Count";
            SumName = "Sum";
            MinimumName = "Minimum";
            MaximumName = "Maximum";
            AverageName = "Average value";
            AverageCountName = "Average count";

            libraryReport = "Library report";


            DateCreatedTagName = "<Date created>";
            EmptyValueTagName = "<Empty value>";
            ClipboardTagName = "<Clipboard>";
            TextFileTagName = "<Text file>";

            ParameterTagName = "Tag";
            TempTagName = "Temp";

            customTagsPresetName = "<Unsaved tags>";
            libraryTotalsPresetName = "Library totals";
            libraryAveragesPresetName = "Library averages";

            emptyPresetName = "<Empty preset>";


            ArtistArtistsName = MbApiInterface.Setting_GetFieldName(MetaDataType.Artist);
            ComposerComposersName = MbApiInterface.Setting_GetFieldName(MetaDataType.Composer);

            DisplayedArtistName = ArtistArtistsName + " (displayed)";
            DisplayedComposerName = ComposerComposersName + " (displayed)";
            DisplayedAlbumArtsistName = MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + " (displayed)";

            ArtworkName = MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork);

            LyricsName = MbApiInterface.Setting_GetFieldName(MetaDataType.Lyrics);
            LyricsNamePostfix = " (any)";
            SynchronisedLyricsName = LyricsName + " (synchronized)";
            UnsynchronisedLyricsName = LyricsName + " (unsynchronized)";


            NullTagName = "<Null>";


            //Supported exported file formats
            exportedFormats = "HTML Document (grouped by albums)|*.htm|HTML Document|*.htm|Simple HTML table|*.htm|Tab delimited text|*.txt|M3U Playlist|*.m3u";
            string exportedTrackList = "Exported Track List";


            //Displayed text
            SequenceNumberName = "#";

            listItemConditionIs = "is";
            listItemConditionIsNot = "is not";
            listItemConditionIsGreater = "is greater than";
            listItemConditionIsLess = "is less than";

            stopButtonName = "Stop";
            cancelButtonName = "Cancel";
            hideButtonName = "Hide";
            previewButtonName = "Preview";
            clearButtonName = "Clear";

            tableCellError = "#Error!";

            sbSorting = "Sorting table...";
            sbUpdating = "updating";
            sbReading = "reading";
            sbUpdated = "updated";
            sbRead = "read";
            sbFiles = "file(s)";
            sbPresetIsAutoApplied1 = "ASR Preset ";
            sbPresetIsAutoApplied2 = " is auto applied";

            sbAutobackuping = "Autosaving tag backup...";
            sbMovingBackupsToNewFolder = "Moving backups to new folder...";

            defaultASRPresetName = "Preset #";

            msgFileNotFound = "File not found!";
            msgNoFilesSelected = "No files selected.";
            msgSourceAndDestinationTagsAreTheSame = "Both tags are the same. Nothing done.";
            msgSwapTagsSourceAndDestinationTagsAreTheSame = "Using the same " +
                "source and destination tags may be useful only for 'Artist'/'Composer' tags for conversion of ';' delimited tag to the list of artists/composers and vice versa. Nothing done.";
            msgNoTagsSelected = "No tags selected. Nothing to preview.";
            msgNoFilesInCurrentView = "No files in current view.";
            msgTracklistIsEmpty = "Track list is empty. Nothing to export. Click 'Preview' first.";
            msgForExportingPlaylistsURLfieldMustBeIncludedInTagList = "For exporting playlists '" + UrlTagName + "' field must be included in tag list.";
            msgPreviewIsNotGeneratedNothingToSave = "Preview is not generated. Nothing to save.";
            msgPreviewIsNotGeneratedNothingToChange = "Preview is not generated. Nothing to change.";
            msgNoAggregateFunctionNothingToSave = "No aggregate function in the table. Nothing to save.";
            msgPleaseUseGroupingFunctionForArtworkTag = "Please use <Grouping> function for artwork tag!";
            MsgAllTags = "ALL TAGS";
            msgNoURLcolumnUnableToSave = "No '" + UrlTagName + "' tag in the table. Unable to save.";
            msgEmptyURL = "Empty '" + UrlTagName + "' in row ";
            msgUnableToSave = "Unable to save. ";
            msgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag = "Using '" + EmptyValueTagName +
                "' as source tag is useful only with option 'Append source tag to the end of destination tag' for adding some static text to the destination tag. Nothing done.";
            msgBackgroundTaskIsCompleted = "Additional tagging tools background task is completed.";
            msgThresholdsDescription = "Auto ratings are based on the average number of times track is played every day " + (char)10 +
                "or 'plays per day' virtual tag. You should set 'plays per day' threshold values for every rating number " + (char)10 +
                "you want to be assigned to tracks. Thresholds are evaluated for track in descending order: from 5 star rating " + (char)10 +
                "to 0.5 star rating. 'Plays per day' virtual tag can be evaluated for every track independent of rest of library, " + (char)10 +
                "so its possible to automatically update auto rating if playing track is changed. " + (char)10 +
                "Also its possible to update auto rating manually for selected files or automatically for entire library " + (char)10 +
                "on MusicBee startup. ";
            msgAutoCalculationOfThresholdsDescription = "Auto calculation of thresholds is another way to set up auto ratings. This option allows you to set desirable weights " + (char)10 +
                "of auto rating values in your library. Actual weights may differ from desirable values because its not always possible " + (char)10 +
                "to satisfy desirable weights (suppose all your tracks have the same 'plays per day' value, its obvious that there is no way to split " + (char)10 +
                "your library into several parts on the basis of 'plays per day' value). Actual weights are displayed next to desired weights after calculation is made. " + (char)10 +
                "Because calculation of thresholds is time consuming operation it cannot be done automatically on any event except for MusicBee startup. ";

            msgNumberOfPlayedTracks = "Ever played tracks in your library: ";
            msgIncorrectSumOfWeights = "The sum of all weights must be equal or less than 100%!";
            msgSum = "Sum: ";
            msgNumberOfNotRatedTracks = "% of tracks rated as no stars)";
            msgTracks = " tracks)";
            msgActualPercent = "% / Act.: ";
            msgNoPresetSelected = "No preset selected!";
            msgIncorrectPresetName = "Incorrect preset name or duplicated preset names.";
            msgSendingEmailConfirmation = "Do you want e-mail selected preset to plugin developer?";
            msgDeletePresetConfirmation = "Do you want to delete selected preset?";
            msgImportingConfirmation = "Do you want to import presets?";
            msgNoPresetsImported = "No presets were imported. ";
            msgPresetsWereImported = " preset(s) was/were successfully imported. ";
            msgFailedToImport = "Failed to import ";
            msgPresets = " preset(s). ";
            msgDeletingConfirmation = "Do you want to delete all non-user presets?";
            msgNoPresetsDeleted = "No presets were deleted. ";
            msgPresetsWereDeleted = " preset(s) was/were successfully deleted. ";
            msgFailedToDelete = "Failed to delete ";

            msgNumberOfTagsInTextFile = "Number of tags in text file (";
            msgDoesntCorrespondToNumberOfSelectedTracks = ") doesn't correspond to number of selected tracks (";
            msgMessageEnd = ")!";

            msgClipboardDesntContainText = "Clipboard doesn't contain text!";

            msgNumberOfTagsInClipboard = "The number of tags in clipboard (";
            msgNumberOfTracksInClipboard = "The number of tracks in clipboard (";
            msgDoesntCorrespondToNumberOfSelectedTracksC = ") doesn't correspond to the number of selected tracks (";
            msgDoesntCorrespondToNumberOfCopiedTagsC = ") doesn't correspond to the number of copied tags (";
            msgMessageEndC = ")!";
            msgDoYouWantToPasteAnyway = " Do you want to paste tags anyway?";

            msgFirstThreeGroupingFieldsInPreviewTableShouldBe = "First three grouping fields in preview table should be '" + DisplayedAlbumArtsistName + "', '" + AlbumTagName + "' and '" + ArtworkName + "' to export to HTML Document (grouped by album)";

            msgBackgroundAutoLibraryReportIsExecuted = "Background auto library report is executed! Please wait until it is finished!";

            msgMusicBeeBackupType = "MusicBee Tag Backup|*.xml";
            msgSaveBackupTitle = "NAVIGATE TO DESIRED FOLDER, TYPE BACKUP NAME AT THE BOTTOM OF THE WINDOW AND CLICK 'SAVE'";
            msgRestoreBackupTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP AND CLICK 'OPEN'";
            msgRenameSelectBackupTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP TO BE RENAMED AND CLICK 'OPEN'";
            msgRenameSaveBackupTitle = "NAVIGATE TO DESIRED FOLDER, TYPE NEW BACKUP NAME AT THE BOTTOM OF THE WINDOW AND CLICK 'SAVE'";
            msgDeleteBackupTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP TO DELETE AND CLICK 'SAVE'";
            msgMasterBackupIndexIsCorrupted = "Master tag backup index is corrupted! All existing at the moment backups are not available any more in 'Tag history' command.";

            ctlDirtyError1sf = "";
            ctlDirtyError1mf = "";
            ctlDirtyError2sf = " background file updating operation is running/scheduled. " + (char)10 +
                "Preview results may be not accurate. ";
            ctlDirtyError2mf = " background file updating operations is running/scheduled. " + (char)10 +
                "Preview results may be not accurate. ";


            //Defaults for controls
            UnchangedStyle.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            UnchangedStyle.SelectionForeColor = Color.FromKnownColor(KnownColor.HighlightText);

            ChangedStyle.ForeColor = Color.FromKnownColor(KnownColor.Highlight);
            ChangedStyle.SelectionForeColor = Color.FromArgb(255 - Color.FromKnownColor(KnownColor.Highlight).R, 255 - Color.FromKnownColor(KnownColor.Highlight).G, 255 - Color.FromKnownColor(KnownColor.Highlight).B);


            SavedSettings = new SavedSettingsType();

            //Lets set initial defaults
            SavedSettings.menuPlacement = 2;

            SavedSettings.libraryReportsPresets4 = null;

            SavedSettings.smartOperation = true;
            SavedSettings.appendSource = false;

            SavedSettings.changeCaseFlag = 1;
            SavedSettings.useExceptionWords = false;

            SavedSettings.useExceptionChars = false;
            SavedSettings.exceptionChars = "\" ' ( { [ /";
            SavedSettings.useWordSplitters = false;
            SavedSettings.wordSplitters = "& .";

            SavedSettings.comparedField = "1";


            //Lets try to read defaults for controls from settings file
            PluginSettingsFileName = System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "mb_TagTools.Settings.xml");

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(PluginSettingsFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = null;
            try
            {
                controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                SavedSettings = (SavedSettingsType)controlsDefaultsSerializer.Deserialize(file);
            }
            catch
            {
                //Ignore...
            };

            file.Close();


            if (SavedSettings.forms == null || SavedSettings.forms.Length < NumberOfCommandWindows)
            {
                SavedSettings.forms = new string[NumberOfCommandWindows];
                SavedSettings.forms[0] = typeof(MusicBeePlugin.AdvancedSearchAndReplacePlugin).FullName;
                SavedSettings.forms[1] = typeof(MusicBeePlugin.ASRPresetEditor).FullName;
                SavedSettings.forms[2] = typeof(MusicBeePlugin.AutoRatePlugin).FullName;
                SavedSettings.forms[3] = typeof(MusicBeePlugin.CalculateAverageAlbumRatingPlugin).FullName;
                SavedSettings.forms[4] = typeof(MusicBeePlugin.ChangeCasePlugin).FullName;
                SavedSettings.forms[5] = typeof(MusicBeePlugin.CopyTagPlugin).FullName;
                SavedSettings.forms[6] = typeof(MusicBeePlugin.LibraryReportsPlugin).FullName;
                SavedSettings.forms[7] = typeof(MusicBeePlugin.AutoLibraryReportsPlugin).FullName;
                SavedSettings.forms[8] = typeof(MusicBeePlugin.ReencodeTagPlugin).FullName;
                SavedSettings.forms[9] = typeof(MusicBeePlugin.CopyTagsToClipboardPlugin).FullName;
                SavedSettings.forms[10] = typeof(MusicBeePlugin.PasteTagsFromClipboardPlugin).FullName;
                SavedSettings.forms[11] = typeof(MusicBeePlugin.SettingsPlugin).FullName;
                SavedSettings.forms[NumberOfCommandWindows - 1] = typeof(MusicBeePlugin.SwapTagsPlugin).FullName;

                SavedSettings.sizesPositions = new SizePositionType[NumberOfCommandWindows];
            }


            if (SavedSettings.exceptionWords == null || SavedSettings.exceptionWords.Length < 10)
            {
                SavedSettings.exceptionWords = new string[10];
                SavedSettings.exceptionWords[0] = "the a an and or not";
                SavedSettings.exceptionWords[1] = "a al an and as at but by de for in la le mix nor of on or remix the to vs. y ze";
                SavedSettings.exceptionWords[2] = "U2 UB40";
                SavedSettings.exceptionWords[3] = "";
                SavedSettings.exceptionWords[4] = "";
                SavedSettings.exceptionWords[5] = "";
                SavedSettings.exceptionWords[6] = "";
                SavedSettings.exceptionWords[7] = "";
                SavedSettings.exceptionWords[8] = "";
                SavedSettings.exceptionWords[9] = "";
            }

            if (SavedSettings.customText == null || SavedSettings.customText.Length < 10)
            {
                SavedSettings.customText = new string[10];
                SavedSettings.customText[0] = "";
                SavedSettings.customText[1] = "";
                SavedSettings.customText[2] = "";
                SavedSettings.customText[3] = "";
                SavedSettings.customText[4] = "";
                SavedSettings.customText[5] = "";
                SavedSettings.customText[6] = "";
                SavedSettings.customText[7] = "";
                SavedSettings.customText[8] = "";
                SavedSettings.customText[9] = "";
            }

            if (SavedSettings.appendedText == null || SavedSettings.appendedText.Length < 10)
            {
                SavedSettings.appendedText = new string[10];
                SavedSettings.appendedText[0] = "";
                SavedSettings.appendedText[1] = "";
                SavedSettings.appendedText[2] = "";
                SavedSettings.appendedText[3] = "";
                SavedSettings.appendedText[4] = "";
                SavedSettings.appendedText[5] = "";
                SavedSettings.appendedText[6] = "";
                SavedSettings.appendedText[7] = "";
                SavedSettings.appendedText[8] = "";
                SavedSettings.appendedText[9] = "";
            }

            if (SavedSettings.addedText == null || SavedSettings.addedText.Length < 10)
            {
                SavedSettings.addedText = new string[10];
                SavedSettings.addedText[0] = "";
                SavedSettings.addedText[1] = "";
                SavedSettings.addedText[2] = "";
                SavedSettings.addedText[3] = "";
                SavedSettings.addedText[4] = "";
                SavedSettings.addedText[5] = "";
                SavedSettings.addedText[6] = "";
                SavedSettings.addedText[7] = "";
                SavedSettings.addedText[8] = "";
                SavedSettings.addedText[9] = "";
            }


            SavedSettings.actualPerCent5 = -1;
            SavedSettings.actualPerCent45 = -1;
            SavedSettings.actualPerCent4 = -1;
            SavedSettings.actualPerCent35 = -1;
            SavedSettings.actualPerCent3 = -1;
            SavedSettings.actualPerCent25 = -1;
            SavedSettings.actualPerCent2 = -1;
            SavedSettings.actualPerCent15 = -1;
            SavedSettings.actualPerCent1 = -1;
            SavedSettings.actualPerCent05 = -1;

            if (SavedSettings.autobackupDirectory == null) SavedSettings.autobackupDirectory = MbApiInterface.Setting_GetPersistentStoragePath() + "Tag Backups";
            if (!System.IO.Directory.Exists(SavedSettings.autobackupDirectory)) System.IO.Directory.CreateDirectory(SavedSettings.autobackupDirectory);


            Language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            if (!System.IO.File.Exists(Application.StartupPath + @"\Plugins\ru\mb_TagTools.resources.dll")) Language = "en"; //For testing

            if (Language == "ru")
            {
                //Lets redefine localizable strings

                //Plugin localizable strings
                pluginName = "Дополнительные инструменты";
                description = "Плагин добавляет дополнительные инструменты для работы с тегами";

                tagToolsMenuSectionName = "ДОПОЛНИТЕЛЬНЫЕ ИНСТРУМЕНТЫ";
                backupRestoreMenuSectionName = "АРХИВАЦИЯ И ВОССТАНОВЛЕНИЕ";

                copyTagCommandName = "Копировать тег...";
                swapTagsCommandName = "Поменять местами два тега...";
                changeCaseCommandName = "Изменить регистр тега...";
                reencodeTagCommandName = "Изменить кодировку тега...";
                libraryReportsCommandName = "Отчеты по библиотеке...";
                autoLibraryReportsCommandName = "Автоматические отчеты по библиотеке...";
                autoRateCommandName = "Автоматически установить рейтинги...";
                asrCommandName = "Дополнительные поиск и замена...";
                carCommandName = "Рассчитать средний рейтинг альбомов...";
                copyTagsToClipboardCommandName = "Копировать теги в буфер обмена...";
                pasteTagsFromClipboardCommandName = "Вставить теги из буфера обмена";
                showHiddenCommandName = "Показать скрытые окна плагина";

                copyTagCommandDescription = "Дополнительные инструменты: Копировать тег";
                swapTagsCommandDescription = "Дополнительные инструменты: Поменять местами два тега";
                changeCaseCommandDescription = "Дополнительные инструменты: Изменить регистр тега";
                reencodeTagCommandDescription = "Дополнительные инструменты: Изменить кодировку тега";
                libraryReportsCommandDescription = "Дополнительные инструменты: Отчеты по библиотеке";
                autoLibraryReportsCommandDescription = "Дополнительные инструменты: Автоматические отчеты по библиотеке";
                autoRateCommandDescription = "Дополнительные инструменты: Автоматически установить рейтинги";
                asrCommandDescription = "Дополнительные инструменты: Дополнительные поиск и замена";
                carCommandDescription = "Дополнительные инструменты: Рассчитать средний рейтинг альбомов";
                copyTagsToClipboardCommandDescription = "Дополнительные инструменты: Копировать теги в буфер обмена";
                pasteTagsFromClipboardCommandDescription = "Дополнительные инструменты: Вставить теги из буфера обмена";
                showHiddenCommandDescription = "Дополнительные инструменты: Показать скрытые окна плагина";

                backupTagsCommandName = "Архивировать теги для всех дорожек...";
                restoreTagsCommandName = "Восстановить теги для выбранных дорожек...";
                restoreTagsForEntireLibraryCommandName = "Восстановить теги для всех дорожек...";
                renameBackupCommandName = "Переименовать или переместить архив...";
                deleteBackupCommandName = "Удалить архив...";
                autoBackupSettingsCommandName = "Настройки автоархивации...";

                backupTagsCommandDescription = "Tagging tools: Backup tags for all tracks";
                restoreTagsCommandDescription = "Tagging tools: Restore tags for selected tracks";
                restoreTagsForEntireLibraryCommandDescription = "Tagging tools: Restore tags for all tracks";
                renameBackupCommandDescription = "Tagging tools: Rename backup";
                deleteBackupCommandDescription = "Tagging tools: Delete backup";
                autoBackupSettingsCommandDescription = "Tagging tools: Autobackup settings";

                copyTagCommandSbText = "Копирование тегов";
                swapTagsCommandSbText = "Обмен тегов местами";
                changeCaseCommandSbText = "Изменение регистра тега";
                reencodeTagCommandSbText = "Изменение кодировки тега";
                libraryReportsCommandSbText = "Формирование отчета";
                libraryReportsGneratingPreviewCommandSbText = "Формирование предварительного просмотра";
                autoRateCommandSbText = "Автоматическая установка рейтингов";
                asrCommandSbText = "Дополнительные поиск и замена";
                carCommandSbText = "Расчет среднего рейтинга альбомов";

                GroupingName = "<Группировка>";
                CountName = "Количество";
                SumName = "Сумма";
                MinimumName = "Минимум";
                MaximumName = "Максимум";
                AverageName = "Среднее значение";
                AverageCountName = "Среднее количество";

                libraryReport = "Отчет по библиотеке";


                //Other localizable strings
                DateCreatedTagName = "<Создано>";
                EmptyValueTagName = "<Пустое значение>";
                ClipboardTagName = "<Буфер обмена>";
                TextFileTagName = "<Текстовый файл>";

                ParameterTagName = "Тег";
                //tempTagName = "Врем";

                customTagsPresetName = "<Несохраненные теги>";
                libraryTotalsPresetName = "Итоги по библиотеке";
                libraryAveragesPresetName = "В среднем по библиотеке";

                emptyPresetName = "<Пустой шаблон>";


                DisplayedArtistName = ArtistArtistsName + " (отображаемый)";
                DisplayedComposerName = ComposerComposersName + " (отображаемый)";
                DisplayedAlbumArtsistName = MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + " (отображаемый)";

                LyricsNamePostfix = " (любые)";
                SynchronisedLyricsName = LyricsName + " (синхронные)";
                UnsynchronisedLyricsName = LyricsName + " (несинхронные)";
                SequenceNumberName = "№ п/п";


                //Supported exported file formats
                exportedFormats = "Документ HTML (по альбомам)|*.htm|Документ HTML|*.htm|Простая таблица HTML|*.htm|Текст, разделенный табуляциями|*.txt|Плейлист M3U|*.m3u";
                exportedTrackList = "Список экпортированных дорожек";

                //Displayed text
                listItemConditionIs = "равен";
                listItemConditionIsNot = "не равен";
                listItemConditionIsGreater = "больше чем";
                listItemConditionIsLess = "меньше чем";

                stopButtonName = "Остановить";
                cancelButtonName = "Отменить";
                hideButtonName = "Скрыть";
                previewButtonName = "Просмотр";
                clearButtonName = "Очистить";

                tableCellError = "#Ошибка!";

                sbSorting = "Сортировка таблицы...";
                sbUpdating = "запись";
                sbReading = "чтение";
                sbUpdated = "записан(о)";
                sbRead = "прочитан(о)";
                sbFiles = "файл(ов)";
                sbPresetIsAutoApplied1 = "Автоматически применен шаблон дополнительного поиска и замены ";
                sbPresetIsAutoApplied2 = "";

                sbAutobackuping = "Автосохранение архива тегов...";
                sbMovingBackupsToNewFolder = "Идет перемещение архивов в новую папку...";

                defaultASRPresetName = "Шаблон №";

                msgFileNotFound = "Файл не найден!";
                msgNoFilesSelected = "Файлы не выбраны.";
                msgSourceAndDestinationTagsAreTheSame = "Оба тега одинаковые. Обработка не выполнена.";
                msgSwapTagsSourceAndDestinationTagsAreTheSame = "Использовать один и тот же " +
                    "тег-источник и тег-получатель имеет смысл только для тегов 'Исполнитель'/'Композитор' для преобразования строки, разделенной символами ';', в список исполнителей/композиторов и " +
                    "наоборот. Обработка не выполнена.";
                msgNoTagsSelected = "Теги не выбраны. Обработка не выполнена.";
                msgNoFilesInCurrentView = "Нет файлов в текущем режиме отображения.";
                msgTracklistIsEmpty = "Список дорожек пуст. Сначала нажмите кнопку 'Просмотр'.";
                msgForExportingPlaylistsURLfieldMustBeIncludedInTagList = "Для экпортирования плейлистов тег '" + UrlTagName + "' должен быть обязательно включен в таблицу.";
                msgPreviewIsNotGeneratedNothingToSave = "Таблица не сгенерирована. Нечего сохранять.";
                msgPreviewIsNotGeneratedNothingToChange = "Таблица не сгенерирована. Нечего изменять.";
                msgNoAggregateFunctionNothingToSave = "В таблице нет ни одной агрегатной функции. Нечего сохранять.";
                msgPleaseUseGroupingFunctionForArtworkTag = "Пожалуйста, используйте функцию <Группировка> для тега 'Обложка'!";
                MsgAllTags = "ВСЕ ТЕГИ";
                msgNoURLcolumnUnableToSave = "В таблице нет тега '" + UrlTagName + "'. Невозможно сохранить результаты.";
                msgEmptyURL = "Пустой тег '" + UrlTagName + "' в строке ";
                msgUnableToSave = "Невозможно сохранить результаты. ";
                msgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag = "Использование псевдотега '" + EmptyValueTagName +
                    "' в качестве тега-источника имеет смысл только при включенной опции 'Добавить тег источника в конец тега получателя' для добавления некоторого " +
                    "текста  к тегу-получателю. Обработка не выполнена.";
                msgBackgroundTaskIsCompleted = "Фоновая задача плагина 'Дополнительные инструменты' завершена.";
                msgThresholdsDescription = "Автоматическая установка рейтингов основана на среднем числе воспроизведений композиции в день " + (char)10 +
                    "(на виртуальном теге 'число воспроизведений в день'). Вам следует установить пороговые значения 'числа воспроизведений в день' для каждого значения рейтинга, " + (char)10 +
                    "которое может быть назаначено композициям. Пороговые значения оцениваются в убывающем порядке: от рейтинга 5 звездочек " + (char)10 +
                    "до рейтинга 0.5 звездочки. Тег 'число воспроизведений в день' может быть рассчитан для каждой композиции независимо от остальной библиотеки, " + (char)10 +
                    "так что возможно автоматическое обновление рейтинга при смене композиции. Также возможно рассчитать рейтинги для выбранных композиций " + (char)10 +
                    "коммандой 'Установить рейтинги' или для всех композиций библиотеки при запуске MusicBee. ";
                msgAutoCalculationOfThresholdsDescription = "Автоматический расчет пороговых значений 'числа воспроизведений в день' - это еще один способ установки авто-рейтингов. " + (char)10 +
                    "Эта опция позволяет вам установить желаемый процент каждого значения рейтинга в библиотеке. Действительные проценты значений рейтингов могут отличаться " + (char)10 +
                    "от желаемых, поскольку не всегда можно разбить композиции библиотеки на несколько групп (предположим все композиции библитеки имеют одинаковое " + (char)10 +
                    "значение 'числа воспроизведений в день', очевидно не существует способа разбить все композиции на группы, исходя из значений 'числа воспроизведений в день'. " + (char)10 +
                    "Действительные проценты отображаются справа от желаемых процентов после вычисления пороговых значений. Поскольку вычисление пороговых значений требует заметного " + (char)10 +
                    "времени, то оно не может производиться автоматически, кроме как при запуске MusicBee. ";

                msgNumberOfPlayedTracks = "Число когда либо воспроизводившихся композиций: ";
                msgIncorrectSumOfWeights = "Сумма всех процентов должна быть равна 100% или меньше!";
                msgSum = "Сумма: ";
                msgNumberOfNotRatedTracks = "% композиций с нулевым рейтингом)";
                msgTracks = " композиций)";
                msgActualPercent = "% / Действ.: ";
                msgNoPresetSelected = "Шаблон не выбран!";
                msgIncorrectPresetName = "Некорректное название шаблона или шаблон с таким названием уже существует.";
                msgSendingEmailConfirmation = "Вы хотите отправить выбранный шаблон разаработчику плагина по электронной почте?";
                msgDeletePresetConfirmation = "Вы действительно хотите удалить выбранный шаблон?";
                msgImportingConfirmation = "Вы действительно хотите импортировать шаблоны?";
                msgNoPresetsImported = "Шаблоны не были импортированы.";
                msgPresetsWereImported = " шаблон(ов) было импортировано.";
                msgFailedToImport = "Не удалось импортировать ";
                msgPresets = " шаблон(ов). ";
                msgDeletingConfirmation = "Вы действительно хотите удалить все непользовательские шаблоны?";
                msgNoPresetsDeleted = "Шаблоны не были удалены.";
                msgPresetsWereDeleted = " шаблон(ов) было удалено.";
                msgFailedToDelete = "Не удалось удалить ";

                msgNumberOfTagsInTextFile = "Количество тегов в текстовом файле (";
                msgDoesntCorrespondToNumberOfSelectedTracks = ") не соответствует количеству выбранных дорожек (";
                msgMessageEnd = ")!";

                msgClipboardDesntContainText = "Буфер обмена не содержит текст!";

                msgNumberOfTagsInClipboard = "Количество тегов в буфере обмена (";
                msgNumberOfTracksInClipboard = "Количество дорожек в буфере обмена (";
                msgDoesntCorrespondToNumberOfSelectedTracksC = ") не соответствует количеству выбранных дорожек (";
                msgDoesntCorrespondToNumberOfCopiedTagsC = ") не соответствует количеству скопированных тегов (";
                msgMessageEndC = ")!";
                msgDoYouWantToPasteAnyway = " Вставить теги все равно?";

                msgFirstThreeGroupingFieldsInPreviewTableShouldBe = "Первые три поля группировок в таблице должны быть '" + DisplayedAlbumArtsistName + "', '" + AlbumTagName + "' and '" + ArtworkName + "' для того, чтобы экспортировать теги в формат 'Документ HTML (по альбомам)'";

                msgBackgroundAutoLibraryReportIsExecuted = "Исполняется фоновая задача автоматических отчетов по библиотеке! Подождите пока она завершится!";


                msgMusicBeeBackupType = "Архив тегов MusicBee|*.xml";
                msgSaveBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, НАПИШИТЕ НАЗВАНИЕ АРХИВА ВНИЗУ ОКНА И НАЖМИТЕ 'СОХРАНИТЬ'";
                msgRestoreBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ И НАЖМИТЕ 'ОТКРЫТЬ'";
                msgRenameSelectBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ, КОТОРЫЙ ХОТИТЕ ПЕРЕИМЕНОВАТЬ И НАЖМИТЕ 'ОТКРЫТЬ'";
                msgRenameSaveBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, НАПИШИТЕ НОВОЕ НАЗВАНИЕ АРХИВА ВНИЗУ ОКНА И НАЖМИТЕ 'СОХРАНИТЬ'";
                msgDeleteBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ ДЛЯ УДАЛЕНИЯ И НАЖМИТЕ 'СОХРАНИТЬ'";
                msgMasterBackupIndexIsCorrupted = "Основной индекс архива тегов поврежден! Все существующие на данный момент архивы будут не доступны в команде 'История тегов'.";

                ctlDirtyError1sf = "Работает/запланирована ";
                ctlDirtyError1mf = "Работают/запланированы ";
                ctlDirtyError2sf = " фоновая операция обновления файлов. " + (char)10 +
                    "Результаты предварительного просмотра могут быть не точны. ";
                ctlDirtyError2mf = " фоновых операций обновления файлов. " + (char)10 +
                    "Результаты предварительного просмотра могут быть не точны. ";

                ////Defaults for controls
                //charBox = "";

                //exceptions = "the a an and or not";
                //exceptionChars = "\" ' ( { [ /";
                //wordSplitters = "&";

                BackupDefaultPrefix = "Архив тегов ";

                if (SavedSettings.autobackupPrefix == null) SavedSettings.autobackupPrefix = "Автоматический архив тегов ";
            }
            else if (Language == "fr")
            {
                //Lets redefine localizable strings

                //Plugin localizable strings
                pluginName = "Outils de tagging supplémentaires";
                description = "Ajoute des outils de tagging à MusicBee";

                tagToolsMenuSectionName = pluginName.ToUpper();

                copyTagCommandName = "Copier le tag...";
                swapTagsCommandName = "Échanger les tags...";
                changeCaseCommandName = "Changer la casse...";
                reencodeTagCommandName = "Réencoder le tag...";
                libraryReportsCommandName = "Comptes-rendus de la collection...";
                autoLibraryReportsCommandName = "Comptes-rendus automatiques de la collection...";
                autoRateCommandName = "Noter automatiquement les pistes...";
                asrCommandName = "Recherche avancée et remplacement...";
                carCommandName = "Calculer la note moyenne d'un album...";
                showHiddenCommandName = "Afficher les fenêtres cachées du plugin";

                copyTagCommandDescription = "Outils de tagging : Copier le tag";
                swapTagsCommandDescription = "Outils de tagging : Échanger les tags";
                changeCaseCommandDescription = "Outils de tagging : Changer la casse";
                reencodeTagCommandDescription = "Outils de tagging : Réencoder le tag";
                libraryReportsCommandDescription = "Outils de tagging : Comptes-rendus de la collection";
                autoLibraryReportsCommandDescription = "Outils de tagging : Comptes-rendus automatiques de la collection";
                autoRateCommandDescription = "Outils de tagging : Noter automatiquement les pistes";
                asrCommandDescription = "Outils de tagging : Recherche avancée et remplacement";
                carCommandDescription = "Outils de tagging : Calculer la note moyenne d'un album";
                showHiddenCommandDescription = "Outils de tagging : Afficher les fenêtres cachées du plugin";

                copyTagCommandSbText = "Copie du tag";
                swapTagsCommandSbText = "Échange des tags";
                changeCaseCommandSbText = "Changement de la casse";
                reencodeTagCommandSbText = "Réencodage du tag";
                libraryReportsCommandSbText = "Génération du compte-rendu";
                libraryReportsGneratingPreviewCommandSbText = "Génération preview";
                autoRateCommandSbText = "Notation automatique des pistes";
                asrCommandSbText = "Recherche avancée et remplacement";
                carCommandSbText = "Calcul de la note moyenne de l'album";

                GroupingName = "<Groupement>";
                CountName = "Compteur";
                SumName = "Somme";
                MinimumName = "Minimum";
                MaximumName = "Maximum";
                AverageName = "Valeur moyenne";
                AverageCountName = "Moyenne du compteur";

                libraryReport = "Rapport de la collection";


                //Other localizable strings
                DateCreatedTagName = "<Date de création>";
                EmptyValueTagName = "<Aucune valeur>";
                ClipboardTagName = "<Presse-papiers>";
                TextFileTagName = "<Fichier texte>";

                ParameterTagName = "Tag";
                //tempTagName = "Temp";

                customTagsPresetName = "<Tags non sauvegardés>";
                libraryTotalsPresetName = "Totaux de la collection";
                libraryAveragesPresetName = "Moyennes de la collection";


                DisplayedArtistName = ArtistArtistsName + " (Affiché)";
                DisplayedComposerName = ComposerComposersName + " (Affiché)";
                DisplayedAlbumArtsistName = MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + " (Affiché)";

                LyricsName = "Paroles de la chanson";
                //lyricsNamePostfix = " (any)";
                SynchronisedLyricsName = LyricsName + " (synchronisées)";
                UnsynchronisedLyricsName = LyricsName + " (non synchronisées)";

                //Supported exported file formats
                exportedFormats = "Document HTML (regroupées par albums)|*.htm|Document HTML|*.htm|Simple tableau HTML|*.htm|Texte Tab délimité|*.txt|Playlist M3U|*.m3u";
                exportedTrackList = "Tracklist explortée";

                //Displayed text
                listItemConditionIs = "est";
                listItemConditionIsNot = "n'est pas";
                listItemConditionIsGreater = "est supérieur à";
                listItemConditionIsLess = "est inférieur à";

                stopButtonName = "Arrêter";
                cancelButtonName = "Annuler";
                hideButtonName = "Masquer";
                previewButtonName = "Aperçu";
                clearButtonName = "Vider";

                tableCellError = "#Erreur !";

                sbSorting = "Tri de la table...";
                sbUpdating = "Mise à jour";
                sbReading = "Lecture";
                sbUpdated = "mis à jour";
                sbRead = "lu(s)";
                sbFiles = "fichier(s)";
                sbPresetIsAutoApplied1 = "Le préréglage ASR ";
                sbPresetIsAutoApplied2 = " est automatiquement appliqué.";

                defaultASRPresetName = "Préréglage #";

                msgFileNotFound = "Fichier non trouvé!";
                msgNoFilesSelected = "Aucun fichier sélectionné.";
                msgSourceAndDestinationTagsAreTheSame = "Les deux tags sont identiques. Aucune action effectuée.";
                msgSwapTagsSourceAndDestinationTagsAreTheSame = "Utiliser les mêmes " +
                    "tags de source et de destination peut s'avérer utile uniquemeny pour la conversion des tags 'Artiste'/'Compositeur' délimités par ';' en une " +
                    "liste d'artistes et de compositeurs et inversement. Aucune action effectuée.";
                msgNoTagsSelected = "Aucun tag sélectionné. Rien à afficher en aperçu.";
                msgNoFilesInCurrentView = "Aucun fichier dans l'affichage actuel.";
                msgTracklistIsEmpty = "La tracklist est vide. Rien à exporter. Cliquer d'abord sur 'Aperçu'.";
                msgForExportingPlaylistsURLfieldMustBeIncludedInTagList = "Pour exporter des playlists le champ '" + UrlTagName + "' doit être inclus dans la liste de tag.";
                msgPreviewIsNotGeneratedNothingToSave = "L'aperçu n'a pas été généré. Rien à sauvegarder.";
                msgPreviewIsNotGeneratedNothingToChange = "L'aperçu n'a pas été généré. Rien à modifier.";
                msgNoAggregateFunctionNothingToSave = "Aucune fonction d'aggrégat dans le tableau. Rien à enregistrer.";
                msgPleaseUseGroupingFunctionForArtworkTag = "Veuillez utiliser la fonction <Groupement> por le tag pochette !";
                msgNoURLcolumnUnableToSave = "Aucun tag '" + UrlTagName + "' dans le tableau. Sauvegarde impossible.";
                msgEmptyURL = "'" + UrlTagName + "' vide dans le rang ";
                msgUnableToSave = "Sauvegarde impossible. ";
                msgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag = "L'utilisation de '" + EmptyValueTagName +
                    "'comme tag source n'est utile qu'avec l'option 'Ajouter le tag source à la fin du tag de destination' pour ajouter du texte fixe au tag de destination. Aucune action effectuée.";
                msgBackgroundTaskIsCompleted = "La tâche de fond de tagging supplémentaire est terminée.";
                msgThresholdsDescription = "Les notes automatiques sont basées sur le nombre moyen de lectures d'une piste par jour " + (char)10 +
                    "ou sur le tag virtuel 'lectures par jour'. Vous devriez définir les seuils de 'lectures par jour' pour chaque nombre de notes " + (char)10 +
                    "que vous voulez assigner aux pistes. Les seuils sont évalués pour les pistes dans l'ordre décroissant: de cinq étoiles " + (char)10 +
                    "à 0.5. Le tag virtuel 'lectures par jour' peut être évalué pour chaque piste indépendemment du reste de votre collection, " + (char)10 +
                    "il est donc possible de mettre à jour automatiquement les notes automatiques le nombre de lectures change. " + (char)10 +
                    "Il est également possible de mettre à jour les notes automatiques manuellement pour des fichiers sélectionnés ou automatiquement pour toute la collection " + (char)10 +
                    "au démarrage de MusicBee. ";
                msgAutoCalculationOfThresholdsDescription = "Le calcul automatique des seuils est une autre manière de configurer les notes automatiques. Cette option vous permet de définir les nombres voulus " + (char)10 +
                    "des valeurs de la notation automatique dans votre collection. Les nombres effectifs peuvent différer de ceux voulus car il n'est pas toujours possible " + (char)10 +
                    "de satisfaire les nombres souhaités (supposez que toutes vos pistes aient la même valeur 'lectures par piste', il est évident qu'il est impossible de séparer " + (char)10 +
                    "votre collection en plusieurs parties sur cette base). Les nombres effectifs sont affichés à côté de ceux souhaités après calcul. " + (char)10 +
                    "Le calcul des seuils étant une opération coûteuse en temps, il ne peut être effectué automatiquement pour un évènement sauf le démarrage de MusicBee. ";

                msgNumberOfPlayedTracks = "Pistes jouées dans votre collection: ";
                msgIncorrectSumOfWeights = "La somme de tous les nombres doit être inférieure ou égale à 100% !";
                msgSum = "Somme: ";
                msgNumberOfNotRatedTracks = "% de pistes avec aucune étoile pour note)";
                msgTracks = " pistes)";
                msgActualPercent = "% / Act.: ";
                msgNoPresetSelected = "Aucun préréglage sélectionné !";
                msgIncorrectPresetName = "Nom de préréglage incorrect ou déjà utilisé.";
                msgSendingEmailConfirmation = "Voulez-vous envoyer par e-mail le préréglage sélectionné au développeur du plugin ?";
                msgDeletePresetConfirmation = "Voulez-vous supprimer le préréglage sélectionné ?";
                msgImportingConfirmation = "Voulez-vous importer des préréglages ?";
                msgNoPresetsImported = "Aucun préréglage importé. ";
                msgPresetsWereImported = "préréglages(s) a/ont été importés avec succès. ";
                msgFailedToImport = "Echec de l'importation ";
                msgPresets = " préréglage(s). ";
                msgDeletingConfirmation = "Voulez-vous supprimer tous les préréglages par défaut ?";
                msgNoPresetsDeleted = "Aucun préréglage supprimé. ";
                msgPresetsWereDeleted = " préréglages(s) a/ont été supprimés avec succès. ";
                msgFailedToDelete = "Echec de la suppression ";

                msgNumberOfTagsInTextFile = "Nombre de mots-clés dans le fichier texte (";
                msgDoesntCorrespondToNumberOfSelectedTracks = ") ne correspond pas au nombre de titres sélectionnés (";
                msgMessageEnd = ")!";

                msgClipboardDesntContainText = "Presse-papiers ne contient pas de texte !";

                msgNumberOfTagsInClipboard = "Nombre de mots-clés dans le presse-papiers (";
                msgDoesntCorrespondToNumberOfSelectedTracksC = ") ne correspond pas au nombre de titres sélectionnés (";
                msgMessageEndC = ")!";

                msgFirstThreeGroupingFieldsInPreviewTableShouldBe = "Tout d'abord trois champs de groupements dans Aperçu de la table doivent être '" + DisplayedAlbumArtsistName + "', '" + AlbumTagName + "' and '" + ArtworkName + "' pour exporter au format 'HTML Document (regroupé par albums)'";

                ctlDirtyError1sf = "";
                ctlDirtyError1mf = "";
                ctlDirtyError2sf = " une opération de mise à jour du fichier en tâche de fond est en cours ou programmé. " + (char)10 +
                    "Il se peut que les aperçus soient incorrects. ";
                ctlDirtyError2mf = " une opération de mise à jour du fichier en tâche de fond est en cours ou programmé. " + (char)10 +
                    "Il se peut que les aperçus soient incorrects. ";

                ////Defaults for controls
                //charBox = "";

                //exceptions = "the a an and or not";
                //exceptionChars = "\" ' ( { [ /";
                //wordSplitters = "&";
            }
            else if (Language == "de")
            {
                //Lets redefine localizable strings

                //Plugin localizable strings
                pluginName = "Zusätzliche Tagging-Werkzeuge";
                description = "Einige Tagging-Werkzeuge zu MusicBee hinzufügen";

                tagToolsMenuSectionName = pluginName.ToUpper();

                copyTagCommandName = "Kopiere Tag...";
                swapTagsCommandName = "Tausche Tags...";
                changeCaseCommandName = "Ändere Schreibweise...";
                reencodeTagCommandName = "Rekodiere Tag...";
                libraryReportsCommandName = "Bibliothek-Berichte...";
                autoLibraryReportsCommandName = "Auto Bibliothek-Bericht...";
                autoRateCommandName = "Auto-Bewertung von Songs...";
                asrCommandName = "Erweitertes Suchen und Ersetzen...";
                carCommandName = "Berechne durchschnittl. Album-Bewertung...";
                showHiddenCommandName = "Zeige versteckte Plugin-Fenster";

                copyTagCommandDescription = "Tagging-Werkzeuge: Kopiere Tag";
                swapTagsCommandDescription = "Tagging-Werkzeuge: Tausche Tags";
                changeCaseCommandDescription = "Tagging-Werkzeuge: Ändere Schreibweise";
                reencodeTagCommandDescription = "Tagging-Werkzeuge: Rekodiere Tag";
                libraryReportsCommandDescription = "Tagging-Werkzeuge: Bibliothek-Berichte";
                autoLibraryReportsCommandDescription = "Tagging-Werkzeuge: Auto Bibliothek-Bericht";
                autoRateCommandDescription = "Tagging-Werkzeuge: Auto-Bewertung Songs";
                asrCommandDescription = "Tagging-Werkzeuge: Erweiteres Suchen und Ersetzen";
                carCommandDescription = "Tagging-Werkzeuge: Berechne durchschnittl. Album-Bewertung";
                showHiddenCommandDescription = "Tagging-Werkzeuge: Zeige versteckte Plugin-Fenster";

                copyTagCommandSbText = "kopiert Tag";
                swapTagsCommandSbText = "tauscht Tags untereinander";
                changeCaseCommandSbText = "ändert die Schreibweise";
                reencodeTagCommandSbText = "Rekodiert Tag";
                libraryReportsCommandSbText = "Erstelle Bericht";
                libraryReportsGneratingPreviewCommandSbText = "Vorschau wird erstellt";
                autoRateCommandSbText = "bewertet automatisch Songs";
                asrCommandSbText = "erweitert Suche und Ersetzen";
                carCommandSbText = "berechnet durchschnittl. Album-Bewertung";

                GroupingName = "<Gruppierung>";
                CountName = "Anzahl";
                SumName = "Summe";
                MinimumName = "Minimum";
                MaximumName = "Maximum";
                AverageName = "Durchschnittswerte";
                AverageCountName = "durchschnittl. Anzahl";

                libraryReport = "Bibliothek Bericht";


                //Other localizable strings
                DateCreatedTagName = "<Erstellungsdatum>";
                EmptyValueTagName = "<Leerwert>";
                ClipboardTagName = "<Zwischenablage>";
                TextFileTagName = "<Text-Datei>";

                ParameterTagName = "Tag";
                //tempTagName = "Vorübergehend";

                customTagsPresetName = "<Ungespeicherte Tags>";
                libraryTotalsPresetName = "Bibliothek Summen";
                libraryAveragesPresetName = "Bibliothek Durchschnittswerte";

                emptyPresetName = "<Leeres Preset>";


                DisplayedArtistName = ArtistArtistsName + " (angezeigt)";
                DisplayedComposerName = ComposerComposersName + " (angezeigt)";
                DisplayedAlbumArtsistName = MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + " (angezeigt)";

                LyricsName = "Song-Texte";
                //lyricsNamePostfix = " (any)";
                SynchronisedLyricsName = LyricsName + " (synchronisiert)";
                UnsynchronisedLyricsName = LyricsName + " (unsynchronisiert)";

                //Supported exported file formats
                exportedFormats = "HTML-Dokument (gruppiert nach Alben)|*.htm|HTML Dokument|*.htm|Simple HTML table|*.htm|durch Tab getrennter Text|*.txt|M3U Playlist|*.m3u";
                exportedTrackList = "Exportierte Song-Liste";

                //Displayed text
                listItemConditionIs = "ist";
                listItemConditionIsNot = "ungleich";
                listItemConditionIsGreater = "größer als";
                listItemConditionIsLess = "kleiner als";

                stopButtonName = "Anhalten";
                cancelButtonName = "Abbrechen";
                hideButtonName = "Verstecken";
                previewButtonName = "Vorschau";
                clearButtonName = "Löschen";

                tableCellError = "#Fehler!";

                sbSorting = "Sortiere Tabelle...";
                sbUpdating = "Aktualisierung";
                sbReading = "Lesen";
                sbUpdated = "aktualisiert";
                sbRead = "gelesen";
                sbFiles = "Datei(en)";
                sbPresetIsAutoApplied1 = "ASR Preset ";
                sbPresetIsAutoApplied2 = " wird automatisch angewendet.";

                defaultASRPresetName = "Preset #";

                msgFileNotFound = "Datei nicht gefunden!";
                msgNoFilesSelected = "Keine Dateien ausgewählt.";
                msgSourceAndDestinationTagsAreTheSame = "Beide Tags sind gleich. Keine Ausführung möglich.";
                msgSwapTagsSourceAndDestinationTagsAreTheSame = "Die Benutzung gleicher " +
                    "Quell- und Ziel-Tags kann nur nützlich sein für 'Künstler'/'Komponist' Tags, um einen durch ';' getrennten Tag-Wert zur Liste von Künstlern/Komponisten zu konvertieren und umgekehrt. Es wurde nichts ausgeführt.";
                msgNoTagsSelected = "Keine Tags ausgewählt. Keine Vorschau möglich.";
                msgNoFilesInCurrentView = "Aucun fichier dans l'affichage actuel.";
                msgTracklistIsEmpty = "Song-Liste ist leer. Export nicht möglich. Klicke zuerst auf 'Vorschau'.";
                msgForExportingPlaylistsURLfieldMustBeIncludedInTagList = "Um Playlists zu exportieren muß das Feld '" + UrlTagName + " in der Tag-Liste enthalten sein.";
                msgPreviewIsNotGeneratedNothingToSave = "Vorschau wurde nicht erstellt. Speichern nicht möglich.";
                msgPreviewIsNotGeneratedNothingToChange = "Vorschau wurde nicht erstellt. Ändern nicht möglich.";
                msgNoAggregateFunctionNothingToSave = "Es befindet sich keine Summen-Funktion in der Tabelle. Speichern nicht möglich.";
                msgPleaseUseGroupingFunctionForArtworkTag = "Bitte benutze die <Gruppierung>-Funktion für Artwork-Tag!";
                msgNoURLcolumnUnableToSave = "Aucun tag '" + UrlTagName + "' Tag in der Tabelle. Speichern nicht möglich.";
                msgEmptyURL = "Aucun tag '" + UrlTagName + "' Tag in der Reihe ";
                msgUnableToSave = "Speichern nicht möglich. ";
                msgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag = "Die Benutzung des '" + EmptyValueTagName +
                    "' Tags als Quell-Tag ist nur zusammen mit der Option 'Hänge Quell-Tag an Ziel-Tag-Ende an' sinnvoll, um statischen Text zum Ziel-Tag hinzuzufügen. Kein Ausführen möglich.";
                msgBackgroundTaskIsCompleted = "Die Hintergrund-Aufgabe der Zusätzlichen Tagging-Werkzeuge ist vollständig.";
                msgThresholdsDescription = "Auto-Bewertungen basieren auf der durchschnittlichen Anzahl von Wiedergaben pro Tag " + (char)10 +
                    "oder dem virtuellen Tag 'Wiedergabe pro Tag'. Du solltest die 'Wiedergaben pro Tag'-Schwellenwerte für jede Bewertungsnummer, " + (char)10 +
                    "die Du den Songs zuweisen möchtest, setzen. Schwellenwerte für Songs werden in absteigender Reihenfolge bestimmt: von 5-Sterne-Bewertung " + (char)10 +
                    "nach 0,5-Sterne-Bewertung. Der virtuelle Tag 'Wiedergaben pro Tag' kann für jeden Song bestimmt werden, unabhängig vom Rest der Bibliothek, " + (char)10 +
                    "deshalb ist es möglich, automatisch die Auto-Bewertung zu aktualisieren, wenn der abgespielte Song geändert wird. " + (char)10 +
                    "Außerdem ist es möglich, die Auto-Bewertung manuell für ausgewählte Dateien oder die ganze Bibliothek zu aktualisieren " + (char)10 +
                    "beim Start von MusicBee.  ";
                msgAutoCalculationOfThresholdsDescription = "Die Auto-Berechnung der Schwellenwerte ist eine andere Art, um Auto-Bewertungen zu erzeugen. Diese Option erlaubt Dir, gewünschte Gewichtungen " + (char)10 +
                    "von Auto-Bewertungs-Werten in Deiner Bibliothek zu setzen. Aktuelle Gewichtungen können sich von den gewünschten Werten unterscheiden, da es nicht immer möglich ist, " + (char)10 +
                    "die gewünschte Gewichtung zu erhalten (Stelle Dir vor, alle Deine Songs haben den gleichen 'Wiedergaben pro Song'-Wert, dann ist es klar, dass es keine Möglichkeit gibt, Deine Bibliothek " + (char)10 +
                    "in mehrere Teile auf Basis des 'Wiedergaben pro Tag'-Wertes aufzuteilen). Aktuelle Gewichtungen werden nach der Berechnung neben den gewünschten Gewichtungen angezeigt. " + (char)10 +
                    "Da die Berechnung der Schwellenwerte eine zeitaufwendige Aufgabe ist, kann sie nicht zu jedem Zeitpunkt automatisch ausgeführt werden, außer beim MusicBee-Start. ";

                msgNumberOfPlayedTracks = "Jemals gespielte Songs in Deiner Bibliothek: ";
                msgIncorrectSumOfWeights = "Die Summe aller Gewichtungen muß gleich oder kleiner als 100% sein!";
                msgSum = "Summe: ";
                msgNumberOfNotRatedTracks = "% der unbewerteten Songs)";
                msgTracks = " Songs)";
                msgActualPercent = "% / Akt.: ";
                msgNoPresetSelected = "Kein Preset ausgewählt!";
                msgIncorrectPresetName = "Falscher Preset-Name oder doppelte Preset-Namen.";
                msgSendingEmailConfirmation = "Möchtest Du die ausgewählten Presets per E-Mail an den Entwickler senden?";
                msgDeletePresetConfirmation = "Möchtest Du die ausgewählten Presets löschen?";
                msgImportingConfirmation = "Möchtest Du Presets importieren?";
                msgNoPresetsImported = "Es wurden keine Presets importiert. ";
                msgPresetsWereImported = "Preset(s) wurde/wurden erfolgreich importiert. ";
                msgFailedToImport = "Import ist fehlgeschlagen ";
                msgPresets = " Preset(s). ";
                msgDeletingConfirmation = "Möchtest Du alle Entwickler-Presets löschen?";
                msgNoPresetsDeleted = "Es wurden keine Presets gelöscht. ";
                msgPresetsWereDeleted = " Preset(s) wurde/wurden erfolgreich geloscht. ";
                msgFailedToDelete = "Löschen ist fehlgeschlagen ";

                msgNumberOfTagsInTextFile = "Tag-Anzahl in Text-Datei (";
                msgDoesntCorrespondToNumberOfSelectedTracks = ") stimmt nicht überein mit Anzahl ausgewählter Songs (";
                msgMessageEnd = ")!";

                msgClipboardDesntContainText = "Zwischenablage enthält keinen Text!";

                msgNumberOfTagsInClipboard = "Anzahl der Tags in der Zwischenablage (";
                msgDoesntCorrespondToNumberOfSelectedTracksC = ") stimmt nicht mit der Anzahl der ausgewählten Songs (";
                msgMessageEndC = ") überein!";

                msgFirstThreeGroupingFieldsInPreviewTableShouldBe = "Um ins 'HTML-Dokument-Format (gruppiert nach Alben)' zu exportieren, sollten die ersten drei Gruppierungsfelder in der Vorschau-Tabelle 'Album-Künstler (angezeigt)', 'Album' und 'Cover' sein.";

                msgBackgroundAutoLibraryReportIsExecuted = "Hintergrund-Auto-Bibliothek-Bericht wird ausgeführt! Bitte warte, bis er abgeschlossen ist!";

                ctlDirtyError1sf = "";
                ctlDirtyError1mf = "";
                ctlDirtyError2sf = " Hintergrund-Prozess(e) für Datei-Aktualisierung laufen/sind geplant. " + (char)10 +
                    "Vorschau-Ergebnisse sind möglicherweise ungenau. ";
                ctlDirtyError2mf = " Hintergrund-Prozess(e) für Datei-Aktualisierung laufen/sind geplant. " + (char)10 +
                    "Vorschau-Ergebnisse sind möglicherweise ungenau. ";

                ////Defaults for controls
                //charBox = "";

                //exceptions = "the a an and or not";
                //exceptionChars = "\" ' ( { [ /";
                //wordSplitters = "&";
            }
            else if (Language == "pl")
            {
                //Lets redefine localizable strings

                //Plugin localizable strings
                pluginName = "Dodatkowe narzędzia do tagowania";
                description = "Dodaje do MusicBee niektóre narzędzia do tagowania";

                tagToolsMenuSectionName = pluginName.ToUpper();

                copyTagCommandName = "Kopiuj tag...";
                swapTagsCommandName = "Zamień tagi...";
                changeCaseCommandName = "Zmień wielkość liter...";
                reencodeTagCommandName = "Ponownie koduj tag...";
                libraryReportsCommandName = "Raporty z biblioteki...";
                autoRateCommandName = "Automatycznie oceń utwory...";
                asrCommandName = "Zaawansowane Szukaj i Zastąp...";
                carCommandName = "Oblicz średnią ocenę dla albumu...";
                showHiddenCommandName = "Pokaż ukryte okna wtyczki";

                copyTagCommandDescription = "Narzędzia do tagowania: Kopiuj tag";
                swapTagsCommandDescription = "Narzędzia do tagowania: Zamień tagi";
                changeCaseCommandDescription = "Narzędzia do tagowania: Zmień wielkość liter";
                reencodeTagCommandDescription = "Narzędzia do tagowania: Ponownie koduj tag";
                libraryReportsCommandDescription = "Narzędzia do tagowania: Raporty z biblioteki";
                autoRateCommandDescription = "Narzędzia do tagowania: Automatycznie oceń utwory";
                asrCommandDescription = "Narzędzia do tagowania: Zaawansowane Szukaj i Zastąp";
                carCommandDescription = "Narzędzia do tagowania: Oblicz średnią ocenę dla albumu";
                showHiddenCommandDescription = "Narzędzia do tagowania: Pokaż ukryte okna wtyczki";

                copyTagCommandSbText = "Kopiowanie taga";
                swapTagsCommandSbText = "Zamiana tagów";
                changeCaseCommandSbText = "Zmiana wielkości liter";
                reencodeTagCommandSbText = "Ponowne kodowanie taga";
                libraryReportsCommandSbText = "Tworzenie raportu";
                libraryReportsGneratingPreviewCommandSbText = "Generowanie podglądu";
                autoRateCommandSbText = "Automatyczna ocena utworów";
                asrCommandSbText = "Zaawansowane Szukanie i Zastępowanie";
                carCommandSbText = "Obliczanie średniej oceny dla albumu";

                GroupingName = "<Grupowanie>";
                CountName = "Liczba";
                SumName = "Suma";
                MinimumName = "Minimum";
                MaximumName = "Maximum";
                AverageName = "Średnia wartość";
                AverageCountName = "Średnia liczba";

                libraryReport = "Raport z biblioteki";


                //Other localizable strings
                DateCreatedTagName = "<Data utworzenia>";
                EmptyValueTagName = "<Pusta wartość>";
                ClipboardTagName = "<Schowek>";
                TextFileTagName = "<Plik tekstowy>";

                ParameterTagName = "Tag";
                //tempTagName = "Врем";

                customTagsPresetName = "<Niezapisane tagi>";
                libraryTotalsPresetName = "Biblioteka - ogółem";
                libraryAveragesPresetName = "Biblioteka - średnio";


                DisplayedArtistName = ArtistArtistsName + " (wyświetlany)";
                DisplayedComposerName = ComposerComposersName + " (wyświetlany)";
                DisplayedAlbumArtsistName = MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + " (wyświetlany)";

                //lyricsNamePostfix = " (any)";
                SynchronisedLyricsName = LyricsName + " (zsynchronizowane)";
                UnsynchronisedLyricsName = LyricsName + " (niezsynchronizowane)";
                SequenceNumberName = "#";


                //Supported exported file formats
                exportedFormats = "Dokument HTML (pogrupowany wg albumów)|*.htm|Dokument HTML|*.htm|Prosta tabela HTML|*.htm|Tekst rozdzielany tabulatorami|*.txt|Lista odtwarzania M3U|*.m3u";
                exportedTrackList = "Wyeksportowana lista utworów";

                //Displayed text
                listItemConditionIs = "jest";
                listItemConditionIsNot = "nie jest";
                listItemConditionIsGreater = "jest większy niż";
                listItemConditionIsLess = "jest mniejszy niż";

                stopButtonName = "Stop";
                cancelButtonName = "Anuluj";
                hideButtonName = "Ukryj";
                previewButtonName = "Podgląd";
                clearButtonName = "Wyczyść";

                tableCellError = "#Błąd!";

                sbSorting = "Tabela sortowania...";
                sbUpdating = "aktualizacja";
                sbReading = "odczytywanie";
                sbUpdated = "zaktualizowano";
                sbRead = "odczytano";
                sbFiles = "plik(ów)";
                sbPresetIsAutoApplied1 = "Ustawienie ASR ";
                sbPresetIsAutoApplied2 = " zostało automatycznie zastosowane";

                defaultASRPresetName = "Ustawienie #";

                msgFileNotFound = "Nie znaleziono pliku!";
                msgNoFilesSelected = "Nie wybrano plików.";
                msgSourceAndDestinationTagsAreTheSame = "Oba tagi są idenyczne. Nic nie zrobiono.";
                msgSwapTagsSourceAndDestinationTagsAreTheSame = "Użycie takich samych tagów źródłowych i docelowych " +
                    "może być przydatne tylko dla tagów 'Artysta'/'Kompozytor' przy konwersji tagu oddzielonego za pomocą '; '  " +
                    "do listy Artystów/ Kompozytorów i odwrotnie. Nic nie zrobiono.";
                msgNoTagsSelected = "Nie wybrano tagów. Podgląd niedostępny.";
                msgNoFilesInCurrentView = "Brak plików w bieżącym widoku.";
                msgTracklistIsEmpty = "Lista utworów jest pusta. Nie ma nic do wyeskportowania. Najpierw naciśnij 'Podgląd'.";
                msgForExportingPlaylistsURLfieldMustBeIncludedInTagList = "Aby wyeksportować listy odtwarzania pole '" + UrlTagName + "' musi znajdować się na liście tagów.";
                msgPreviewIsNotGeneratedNothingToSave = "Podgląd nie został wygenerowany. Nie ma nic do zapisania.";
                msgPreviewIsNotGeneratedNothingToChange = "Podgląd nie został wygenerowany. Nie ma nic do zmiany.";
                msgNoAggregateFunctionNothingToSave = "Brak funkcji agregującej w tabeli. Nie ma nic do zapisania.";
                msgPleaseUseGroupingFunctionForArtworkTag = "Proszę użyj funkcji <Grupowanie> dla taga z grafiką!";
                MsgAllTags = "WSZYSTKIE TAGI";
                msgNoURLcolumnUnableToSave = "Brak taga '" + UrlTagName + "' w tabeli. Zapis niemożliwy.";
                msgEmptyURL = "Pusty '" + UrlTagName + "' w rzędzie ";
                msgUnableToSave = "Zapis niemożliwy. ";
                msgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag = "Użycie '" + EmptyValueTagName +
                    "' jako taga źródłowego jest przydatne tylko razem z opcją 'Dołącz tag źródłowy na koniec taga docelowego', " +
                    "aby dodać jakiś niezmienny tekst do taga docelowego. Nic nie zrobiono.";
                msgBackgroundTaskIsCompleted = "Dodatkowe narzędzia do tagowania ukończyły zadanie w tle.";
                msgThresholdsDescription = "Automatyczne oceny bazują na średniej liczbie odtworzeń danego utworu w ciągu dnia " + (char)10 +
                    "lub wirtualnym tagu 'Odtworzeń na dzień'. Powinieneś ustalić progowe wartości 'Odtworzeń na dzień' dla poszczególnych ocen, " + (char)10 +
                    "które chcesz przypisać utworom. Progi są określane dla utworu w kolejności malejącej: od oceny 5 do 0,5 gwiazdki. " + (char)10 +
                    "Wirtualny tag 'Odtworzeń na dzień' może być określony dla każdego utworu niezależnie od pozostałych w bibliotece, " + (char)10 +
                    "zatem możliwa jest aktualizacja automatycznych ocen po zmianie odtwarzanego utworu. Automatyczne oceny mogą też " + (char)10 +
                    "być aktualizowane: ręcznie dla wybranych utworów, albo automatycznie dla całej biblioteki po uruchomieniu MusicBee. ";
                msgAutoCalculationOfThresholdsDescription = "Inny sposób określania automatycznych ocen polega na automatycznym obliczaniu progów. " + (char)10 +
                    "Ta opcja pozwala ustalić pożądane wagi dla automatycznych ocen w twojej bibliotece. Rzeczywiste wagi mogą odbiegać od pożądanych wartości, " + (char)10 +
                    "gdyż nie zawsze można osiągnąć pożądane wagi (zakładając, że wszystkie utwory mają tę samą wartość 'Odtworzeń na dzień', jest oczywiste, " + (char)10 +
                    "że nie można podzielić biblioteki na kilka zbiorów na podstawie wartości 'Odtworzeń na dzień'). Rzeczywiste wagi są wyświetlane obok pożądanych po dokonaniu obliczeń. " + (char)10 +
                    "Operacja obliczania progów zajmuje dość dużo czasu, dlatego czynność ta wykonywana jest automatycznie tylko podczas uruchamiania MusicBee. ";

                msgNumberOfPlayedTracks = "Kiedykolwiek odtworzone utwory w twojej bibliotece: ";
                msgIncorrectSumOfWeights = "Suma wszystkich wag musi być równa lub mniejsza niż 100%!";
                msgSum = "Suma: ";
                msgNumberOfNotRatedTracks = "% utworów ocenionych na 0 gwiazdek)";
                msgTracks = " utworów)";
                msgActualPercent = "% / Fakt.: ";
                msgNoPresetSelected = "Nie wybrano ustawienia!";
                msgIncorrectPresetName = "Niewłaściwa lub zduplikowana nazwa ustawienia.";
                msgSendingEmailConfirmation = "Czy chcesz przesłać przez e-mail wybrane ustawienie do twórcy tej wtyczki?";
                msgDeletePresetConfirmation = "Czy chcesz skasować wybrane ustwwienie?";
                msgImportingConfirmation = "Czy chcesz zaimportować ustawienia?";
                msgNoPresetsImported = "Nie zaimportowano ustawień. ";
                msgPresetsWereImported = " ustawień(-ie,-ia) zaimportowano z powodzeniem. ";
                msgFailedToImport = "Import nieudany ";
                msgPresets = " ustawień(-ie,-ia). ";
                msgDeletingConfirmation = "Czy chcesz skasować wszystkie ustawienia nie ustalone przez użytkownika?";
                msgNoPresetsDeleted = "Żadne ustawienie nie zostało skasowane. ";
                msgPresetsWereDeleted = " ustawień(-ie,-ia) skasowano z powodzeniem. ";
                msgFailedToDelete = "Kasowanie nieudane ";

                msgNumberOfTagsInTextFile = "Liczba tagów w pliku tekstowym (";
                msgDoesntCorrespondToNumberOfSelectedTracks = ") nie odpowiada liczbie wybranych utworów (";
                msgMessageEnd = ")!";

                msgClipboardDesntContainText = "W schowku brak tekstu!";

                msgNumberOfTagsInClipboard = "Liczba tagów w schowku (";
                msgDoesntCorrespondToNumberOfSelectedTracksC = ") nie odpowiada liczbie wybranych utworów (";
                msgMessageEndC = ")!";

                msgFirstThreeGroupingFieldsInPreviewTableShouldBe = "Aby wyeksportować do formatu 'Dokument HTML (pogrupowany wg albumów)', pierwsze trzy pola grupowania w tabeli podglądu powinny być 'Wykonawca albumu (wyświetlany)', 'Album' i 'Grafika'.";

                ctlDirtyError1sf = "";
                ctlDirtyError1mf = "";
                ctlDirtyError2sf = " operacji aktualizacji plików w tle jest w toku/zaplanowanych. " + (char)10 +
                    "Rezulaty podglądu mogą nie być właściwe.";
                ctlDirtyError2mf = " operacji aktualizacji plików w tle jest w toku/zaplanowanych. " + (char)10 +
                    "Rezulaty podglądu mogą nie być właściwe.";

                ////Defaults for controls
                //charBox = "";

                //exceptions = "the a an and or not";
                //exceptionChars = "\" ' ( { [ /";
                //wordSplitters = "&";
            }
            else
            {
                Language = "en";
            }


            if (SavedSettings.autobackupPrefix == null) SavedSettings.autobackupPrefix = "Tag Autobackup ";

            string[] noTags = new string[0];
            FunctionType[] noTypes = new FunctionType[0];
            LibraryReportsPlugin.LibraryReportsPreset tempLibraryReportsPreset;

            if (SavedSettings.libraryReportsPresets4 == null || SavedSettings.libraryReportsPresets4.Length < 10)
            {
                SavedSettings.libraryReportsPresets4 = new LibraryReportsPlugin.LibraryReportsPreset[10];



                tempLibraryReportsPreset = new LibraryReportsPlugin.LibraryReportsPreset();
                tempLibraryReportsPreset.groupingNames = noTags;
                tempLibraryReportsPreset.functionNames = noTags;
                tempLibraryReportsPreset.functionTypes = noTypes;
                tempLibraryReportsPreset.parameterNames = noTags;



                SavedSettings.libraryReportsPresets4[3] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets4[4] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets4[5] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets4[6] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets4[7] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets4[8] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets4[9] = tempLibraryReportsPreset;
            }



            libraryReportsPreset = new LibraryReportsPlugin.LibraryReportsPreset();
            libraryReportsPreset.groupingNames = noTags;
            libraryReportsPreset.functionNames = noTags;
            libraryReportsPreset.functionTypes = noTypes;
            libraryReportsPreset.parameterNames = noTags;
            libraryReportsPreset.name = customTagsPresetName.ToUpper();

            SavedSettings.libraryReportsPresets4[0] = libraryReportsPreset;


            string[] tempGroupings = new string[3];
            string[] tempFunctions = new string[9];
            FunctionType[] tempFunctionTypes = new FunctionType[9];
            string[] tempParameters = new string[9];
            string[] tempParameters2 = new string[9];

            tempGroupings[0] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempGroupings[1] = DisplayedAlbumArtsistName;
            tempGroupings[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);

            tempParameters[0] = DisplayedAlbumArtsistName;
            tempParameters[1] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempParameters[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Year);
            tempParameters[3] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            tempParameters[4] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[5] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration);
            tempParameters[6] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size);
            tempParameters[7] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount);
            tempParameters[8] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount);

            tempFunctionTypes[0] = FunctionType.Count;
            tempFunctionTypes[1] = FunctionType.Count;
            tempFunctionTypes[2] = FunctionType.Count;
            tempFunctionTypes[3] = FunctionType.Count;
            tempFunctionTypes[4] = FunctionType.Count;
            tempFunctionTypes[5] = FunctionType.Sum;
            tempFunctionTypes[6] = FunctionType.Sum;
            tempFunctionTypes[7] = FunctionType.Sum;
            tempFunctionTypes[8] = FunctionType.Sum;

            for (int i = 0; i < tempParameters.Length; i++)
                tempFunctions[i] = LibraryReportsPlugin.GetColumnName(tempParameters[i], null, tempFunctionTypes[i]);

            tempLibraryReportsPreset = new LibraryReportsPlugin.LibraryReportsPreset();
            tempLibraryReportsPreset.groupingNames = tempGroupings;
            tempLibraryReportsPreset.functionNames = tempFunctions;
            tempLibraryReportsPreset.functionTypes = tempFunctionTypes;
            tempLibraryReportsPreset.parameterNames = tempParameters;
            tempLibraryReportsPreset.parameter2Names = tempParameters2;
            tempLibraryReportsPreset.totals = true;
            tempLibraryReportsPreset.name = libraryTotalsPresetName.ToUpper();

            SavedSettings.libraryReportsPresets4[1] = tempLibraryReportsPreset;



            tempGroupings = new string[3];
            tempFunctions = new string[10];
            tempFunctionTypes = new FunctionType[10];
            tempParameters = new string[10];
            tempParameters2 = new string[10];

            tempGroupings[0] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempGroupings[1] = DisplayedAlbumArtsistName;
            tempGroupings[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);

            tempParameters[0] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[1] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[2] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[3] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[4] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate);
            tempParameters[5] = MbApiInterface.Setting_GetFieldName(MetaDataType.Rating);
            tempParameters[6] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration);
            tempParameters[7] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size);
            tempParameters[8] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount);
            tempParameters[9] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount);

            tempParameters2[0] = MbApiInterface.Setting_GetFieldName(MetaDataType.Artist);
            tempParameters2[1] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            tempParameters2[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempParameters2[3] = MbApiInterface.Setting_GetFieldName(MetaDataType.Year);
            tempParameters2[4] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[5] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[6] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[7] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[8] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[9] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);

            tempFunctionTypes[0] = FunctionType.AverageCount;
            tempFunctionTypes[1] = FunctionType.AverageCount;
            tempFunctionTypes[2] = FunctionType.AverageCount;
            tempFunctionTypes[3] = FunctionType.AverageCount;
            tempFunctionTypes[4] = FunctionType.Average;
            tempFunctionTypes[5] = FunctionType.Average;
            tempFunctionTypes[6] = FunctionType.Average;
            tempFunctionTypes[7] = FunctionType.Average;
            tempFunctionTypes[8] = FunctionType.Average;
            tempFunctionTypes[9] = FunctionType.Average;

            for (int i = 0; i < tempParameters.Length; i++)
                tempFunctions[i] = LibraryReportsPlugin.GetColumnName(tempParameters[i], tempParameters2[i], tempFunctionTypes[i]);

            tempLibraryReportsPreset = new LibraryReportsPlugin.LibraryReportsPreset();
            tempLibraryReportsPreset.groupingNames = tempGroupings;
            tempLibraryReportsPreset.functionNames = tempFunctions;
            tempLibraryReportsPreset.functionTypes = tempFunctionTypes;
            tempLibraryReportsPreset.parameterNames = tempParameters;
            tempLibraryReportsPreset.parameter2Names = tempParameters2;
            tempLibraryReportsPreset.totals = true;
            tempLibraryReportsPreset.name = libraryAveragesPresetName.ToUpper();

            SavedSettings.libraryReportsPresets4[NumberOfPredefinedPresets - 1] = tempLibraryReportsPreset;



            conditions = new string[4];
            conditions[0] = listItemConditionIs;
            conditions[1] = listItemConditionIsNot;
            conditions[2] = listItemConditionIsGreater;
            conditions[3] = listItemConditionIsLess;

            //Lets reset invalid defaults for controls
            if (SavedSettings.menuPlacement == 0) SavedSettings.menuPlacement = 2;
            if (SavedSettings.closeShowHiddenWindows == 0) SavedSettings.closeShowHiddenWindows = 1;

            if (SavedSettings.changeCaseFlag == 0) SavedSettings.changeCaseFlag = 1;

            if (SavedSettings.defaultRating == 0) SavedSettings.defaultRating = 5;

            if (SavedSettings.numberOfTagsToRecalculate == 0) SavedSettings.numberOfTagsToRecalculate = 100;
            if (SavedSettings.autoLibraryReportsPresets == null) SavedSettings.autoLibraryReportsPresets = new AutoLibraryReportsPlugin.AutoLibraryReportsPreset[0];

            fillTagNames();

            //Again lets reset invalid defaults for controls
            if (GetTagId(SavedSettings.copySourceTagName) == 0) SavedSettings.copySourceTagName = ArtistArtistsName;
            if (GetTagId(SavedSettings.changeCaseSourceTagName) == 0) SavedSettings.changeCaseSourceTagName = ArtistArtistsName;
            if (GetTagId(SavedSettings.reencodeTagSourceTagName) == 0) SavedSettings.reencodeTagSourceTagName = ArtistArtistsName;
            if (GetTagId(SavedSettings.swapTagsSourceTagName) == 0) SavedSettings.swapTagsSourceTagName = ArtistArtistsName;

            //if (getTagId(SavedSettings.conditionTagName) == 0) SavedSettings.conditionTagName = tagCounterTagName;
            if (!ChangeCasePlugin.IsItemContainedInList(SavedSettings.condition, conditions)) SavedSettings.condition = listItemConditionIsGreater;
            if (GetTagId(SavedSettings.destinationTagOfSavedFieldName) == 0) SavedSettings.destinationTagOfSavedFieldName = Custom9TagName;
            if (SavedSettings.filterIndex == 0) SavedSettings.filterIndex = 1;
            if ("" + SavedSettings.multipleItemsSplitterChar2 == "")
            {
                SavedSettings.multipleItemsSplitterChar1 = ";";
                SavedSettings.multipleItemsSplitterChar2 = "; ";
            }
            SavedSettings.multipleItemsSplitterChar1 = "" + SavedSettings.multipleItemsSplitterChar1;
            if ("" + SavedSettings.exportedTrackListName == "") SavedSettings.exportedTrackListName = exportedTrackList;

            if (GetTagId(SavedSettings.copyDestinationTagName) == 0) SavedSettings.copyDestinationTagName = GetTagName(MetaDataType.TrackTitle);
            if (GetTagId(SavedSettings.swapTagsDestinationTagName) == 0) SavedSettings.swapTagsDestinationTagName = GetTagName(MetaDataType.TrackTitle);

            if (SavedSettings.autoRateTagId == 0) SavedSettings.autoRateTagId = MetaDataType.Custom9;
            if (SavedSettings.playsPerDayTagId == 0) SavedSettings.playsPerDayTagId = MetaDataType.Custom8;
            if (!(SavedSettings.checkBox5 || SavedSettings.checkBox45 || SavedSettings.checkBox4 || SavedSettings.checkBox35 || SavedSettings.checkBox3
                || SavedSettings.checkBox25 || SavedSettings.checkBox2 || SavedSettings.checkBox15 || SavedSettings.checkBox1 || SavedSettings.checkBox05))
            {
                SavedSettings.checkBox5 = true;
                SavedSettings.perCent5 = 100;
            }

            if (GetTagId(SavedSettings.trackRatingTagName) == 0) SavedSettings.trackRatingTagName = GetTagName(MetaDataType.Rating);
            if (GetTagId(SavedSettings.albumRatingTagName) == 0) SavedSettings.albumRatingTagName = GetTagName(MetaDataType.RatingAlbum);

            //Final initialization
            mbForm = (Form)Form.FromHandle(MbApiInterface.MB_GetWindowHandle());


            string tagToolsSubmenu = "mnuTools";
            switch (SavedSettings.menuPlacement)
            {
                case 1:
                    MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + "/[1]" + pluginName, null, null);
                    tagToolsSubmenu += "/[1]" + pluginName + "/";
                    break;
                case 2:
                    tagToolsSubmenu += "/mnuTagTools/";
                    break;
                default:
                    tagToolsSubmenu += "/";
                    break;
            }


            if (!SavedSettings.dontShowBackupRestore) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + tagToolsMenuSectionName, null, null).Enabled = false;

            if (!SavedSettings.dontShowCopyTag) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + copyTagCommandName, copyTagCommandDescription, copyTagEventHandler);
            if (!SavedSettings.dontShowSwapTags) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + swapTagsCommandName, swapTagsCommandDescription, swapTagsEventHandler);
            if (!SavedSettings.dontShowChangeCase) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + changeCaseCommandName, changeCaseCommandDescription, changeCaseEventHandler);
            if (!SavedSettings.dontShowRencodeTag) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + reencodeTagCommandName, reencodeTagCommandDescription, reencodeTagEventHandler);
            if (!SavedSettings.dontShowLibraryReports) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + libraryReportsCommandName, libraryReportsCommandDescription, libraryReportsEventHandler);
            if (!SavedSettings.dontShowLibraryReports) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + autoLibraryReportsCommandName, autoLibraryReportsCommandDescription, autoLibraryReportsEventHandler);
            if (!SavedSettings.dontShowAutorate) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + autoRateCommandName, autoRateCommandDescription, autoRateEventHandler);
            if (!SavedSettings.dontShowASR)
            {
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + asrCommandName, asrCommandDescription, asrEventHandler);
                AdvancedSearchAndReplacePlugin.Init(this);
            }
            if (!SavedSettings.dontShowCAR) MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + carCommandName, carCommandDescription, carEventHandler);
            MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + copyTagsToClipboardCommandName, copyTagsToClipboardCommandDescription, copyTagsToClipboardEventHandler);
            MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + pasteTagsFromClipboardCommandName, pasteTagsFromClipboardCommandDescription, pasteTagsFromClipboardEventHandler);
            if (!SavedSettings.dontShowShowHiddenWindows)
            {
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + "-", null, null);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + showHiddenCommandName, showHiddenCommandDescription, showHiddenEventHandler);
            }

            if (!SavedSettings.dontShowBackupRestore)
            {
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + "-", null, null);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + backupRestoreMenuSectionName, null, null).Enabled = false;
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + backupTagsCommandName, backupTagsCommandDescription, backupTagsEventHandler);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + restoreTagsCommandName, restoreTagsCommandDescription, restoreTagsEventHandler);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + restoreTagsForEntireLibraryCommandName, restoreTagsForEntireLibraryCommandDescription, restoreTagsForEntireLibraryEventHandler);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + renameBackupCommandName, renameBackupCommandDescription, renameBackupEventHandler);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + deleteBackupCommandName, deleteBackupCommandDescription, deleteBackupEventHandler);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + "-", null, null);
                MbApiInterface.MB_AddMenuItem(tagToolsSubmenu + autoBackupSettingsCommandName, autoBackupSettingsCommandDescription, autoBackupSettingsEventHandler);
            }


            if (SavedSettings.contextMenu)
            {
                ToolStripMenuItem mainMenuItem = (ToolStripMenuItem)MbApiInterface.MB_AddMenuItem("context.Main/" + pluginName, null, null);

                if (!SavedSettings.dontShowBackupRestore) mainMenuItem.DropDown.Items.Add(tagToolsMenuSectionName, null, null).Enabled = false;

                mainMenuItem.DropDown.Items.Add(copyTagCommandName, null, copyTagEventHandler);
                mainMenuItem.DropDown.Items.Add(swapTagsCommandName, null, swapTagsEventHandler);
                mainMenuItem.DropDown.Items.Add(changeCaseCommandName, null, changeCaseEventHandler);
                mainMenuItem.DropDown.Items.Add(asrCommandName, null, asrEventHandler);
                mainMenuItem.DropDown.Items.Add(copyTagsToClipboardCommandName, null, copyTagsToClipboardEventHandler);
                mainMenuItem.DropDown.Items.Add(pasteTagsFromClipboardCommandName, null, pasteTagsFromClipboardEventHandler);

                if (!SavedSettings.dontShowBackupRestore)
                {
                    mainMenuItem.DropDown.Items.Add("-", null, null);
                    mainMenuItem.DropDown.Items.Add(backupRestoreMenuSectionName, null, null).Enabled = false;

                    mainMenuItem.DropDown.Items.Add(copyTagCommandName, null, copyTagEventHandler);
                }
            }


            if (System.IO.File.Exists(Application.StartupPath + @"\Plugins\DevMode.txt"))
                developerMode = true;


            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = pluginName;
            about.Description = description;
            about.Author = "boroda74";
            about.TargetApplication = "";   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            about.Type = PluginType.General;
            about.VersionMajor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;  // .net version
            about.VersionMinor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor; // plugin version
            about.Revision = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build; // number of days since 2000-01-01 at build time
            about.MinInterfaceVersion = 20;
            about.MinApiRevision = 39;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            //string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window

            SettingsPlugin tagToolsForm = new SettingsPlugin(this, about);
            tagToolsForm.ShowDialog();
            SaveSettings();

            return true;
        }
       
        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(PluginSettingsFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));

            controlsDefaultsSerializer.Serialize(file, SavedSettings);

            file.Close();
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            periodicUI_RefreshTimer.Dispose();
            periodicUI_RefreshTimer = null;

            if (periodicAutobackupTimer != null)
            {
                periodicAutobackupTimer.Dispose();
                periodicAutobackupTimer = null;
            }

            emptyButton.Dispose();
            emptyButton = null;


            lock (openedForms)
            {
                foreach (PluginWindowTemplate form in openedForms)
                {
                    form.backgroundTaskIsCanceled = true;
                }
            }

            if (!uninstalled) 
                SaveSettings();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            //Delete settings file
            if (System.IO.File.Exists(PluginSettingsFileName))
            {
                System.IO.File.Delete(PluginSettingsFileName);
            }

            //Delete presets files
            string presetsPath = MbApiInterface.Setting_GetPersistentStoragePath() + @"\" + Plugin.ASRPresetsPath;

            if (System.IO.Directory.Exists(presetsPath))
            {
                string[] presetNames = System.IO.Directory.GetFiles(presetsPath, "*" + Plugin.ASRPresetExtension);

                foreach (string presetName in presetNames)
                {
                    System.IO.File.Delete(presetName);
                }


                //Delete presets folder
                System.IO.Directory.Delete(presetsPath);
            }

            uninstalled = true;
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            if (!prioritySet)
            {
                prioritySet = true;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            }

            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    // perform startup initialisation
                    periodicUI_RefreshTimer = new System.Threading.Timer(periodicUI_Refresh, null, refreshUI_Delay, refreshUI_Delay);

                    if (!SavedSettings.dontShowBackupRestore)
                    {
                        if (System.IO.File.Exists(BackupIndexFileName))
                        {
                            System.IO.FileStream stream = System.IO.File.Open(BackupIndexFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
                            System.IO.StreamReader file = new System.IO.StreamReader(stream, Encoding.UTF8);

                            System.Xml.Serialization.XmlSerializer backupSerializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupIndexType));

                            BackupIndexType backupIndex;
                            try
                            {
                                backupIndex = (BackupIndexType)backupSerializer.Deserialize(file);
                                backupIndexDictionary = backupIndex.copyToBackupIndexDictionary();
                            }
                            catch
                            {
                                MessageBox.Show(msgMasterBackupIndexIsCorrupted);
                                backupIndexDictionary = new BackupIndexDictionary();
                            }

                            file.Close();
                        }
                        else
                        {
                            backupIndexDictionary = new BackupIndexDictionary();
                        }
                    }

                    if (SavedSettings.autobackupInterval != 0)
                    {
                        periodicAutobackupTimer = new System.Threading.Timer(periodicAutobackup, null, (int)SavedSettings.autobackupInterval * 1000 * 60, (int)SavedSettings.autobackupInterval * 1000 * 60);
                    }

                    if (SavedSettings.calculateThresholdsAtStartUp || SavedSettings.autoRateAtStartUp)
                    {
                        AutoRatePlugin tagToolsForm = new AutoRatePlugin(this);
                        tagToolsForm.switchOperation(tagToolsForm.onStartup, emptyButton, emptyButton, emptyButton);
                    }
                    if (SavedSettings.calculateAlbumRatingAtStartUp)
                    {
                        CalculateAverageAlbumRatingPlugin tagToolsForm = new CalculateAverageAlbumRatingPlugin(this);
                        tagToolsForm.calculateAlbumRatingForAllTracks();
                    }

                    //Auto library reports
                    AutoLibraryReportsPlugin.AutoCalculate(this);

                    break;
                case NotificationType.PlayCountersChanged:
                    //Play count should be changed, but track is not changed yet
                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (filesUpdatedByPlugin)
                        {
                            if (filesUpdatedByPlugin.Contains(sourceFileUrl))
                                filesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AdvancedSearchAndReplacePlugin.AutoApply(sourceFileUrl, this);
                    }

                    if (SavedSettings.autoRateOnTrackProperties)
                    {
                        AutoRatePlugin.AutoRateLive(this, sourceFileUrl);
                    }

                    numberOfTagChanges++;

                    break;
                case NotificationType.TagsChanged:
                case NotificationType.FileAddedToInbox:
                case NotificationType.FileAddedToLibrary:
                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (filesUpdatedByPlugin)
                        {
                            if (filesUpdatedByPlugin.Contains(sourceFileUrl))
                                filesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AdvancedSearchAndReplacePlugin.AutoApply(sourceFileUrl, this);
                    }

                    numberOfTagChanges++;

                    break;
                case NotificationType.RatingChanged:
                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (filesUpdatedByPlugin)
                        {
                            if (filesUpdatedByPlugin.Contains(sourceFileUrl))
                                filesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AdvancedSearchAndReplacePlugin.AutoApply(sourceFileUrl, this);
                    }

                    if (!SavedSettings.dontShowCAR)
                    {
                        if (SavedSettings.calculateAlbumRatingAtTagsChanged)
                            CalculateAverageAlbumRatingPlugin.CalculateAlbumRatingForAlbum(this, sourceFileUrl);
                    }

                    numberOfTagChanges++;

                    break;
                case NotificationType.ReplayGainChanged:
                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (filesUpdatedByPlugin)
                        {
                            if (filesUpdatedByPlugin.Contains(sourceFileUrl))
                                filesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AdvancedSearchAndReplacePlugin.AutoApply(sourceFileUrl, this);
                    }

                    numberOfTagChanges++;

                    break;
            }


            //Auto library reports
            if (numberOfTagChanges >= SavedSettings.numberOfTagsToRecalculate)
                numberOfTagChanges = -1;

            if (SavedSettings.recalculateOnNumberOfTagsChanges && numberOfTagChanges == -1)
                AutoLibraryReportsPlugin.AutoCalculate(this);
        }
    }
}