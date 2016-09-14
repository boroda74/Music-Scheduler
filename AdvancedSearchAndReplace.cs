using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace MusicBeePlugin
{
    public partial class AdvancedSearchAndReplacePlugin : PluginWindowTemplate
    {
        public class Preset
        {
            public Preset()
            {
                modifiedByUser = true;
                guid = Guid.NewGuid();

                names = new SortedDictionary<string, string>();
                descriptions = new SortedDictionary<string, string>();
            }

            public Preset(Preset originalPreset, string presetNameSuffix = "")
                : this()
            {
                if (presetNameSuffix == "")
                {
                    modified = originalPreset.modified;
                    modifiedByUser = originalPreset.modifiedByUser;
                    guid = originalPreset.guid;
                }

                foreach (string key in originalPreset.names.Keys)
                {
                    string value;

                    originalPreset.names.TryGetValue(key, out value);
                    names.Add(key, value + presetNameSuffix);
                }

                foreach (string key in originalPreset.descriptions.Keys)
                {
                    string value;

                    originalPreset.descriptions.TryGetValue(key, out value);
                    descriptions.Add(key, value);
                }

                ignoreCase = originalPreset.ignoreCase;
                autoApply = originalPreset.autoApply;
                condition = originalPreset.condition;
                playlist = originalPreset.playlist;

                parameterTagType = originalPreset.parameterTagType;
                parameterTag2Type = originalPreset.parameterTag2Type;
                parameterTag3Type = originalPreset.parameterTag3Type;
                parameterTag4Type = originalPreset.parameterTag4Type;
                parameterTag5Type = originalPreset.parameterTag5Type;
                parameterTag6Type = originalPreset.parameterTag6Type;

                parameterTagId = originalPreset.parameterTagId;
                parameterTag2Id = originalPreset.parameterTag2Id;
                parameterTag3Id = originalPreset.parameterTag3Id;
                parameterTag4Id = originalPreset.parameterTag4Id;
                parameterTag5Id = originalPreset.parameterTag5Id;
                parameterTag6Id = originalPreset.parameterTag6Id;

                customTextChecked = originalPreset.customTextChecked;
                customText = originalPreset.customText;
                customText2Checked = originalPreset.customText2Checked;
                customText2 = originalPreset.customText2;
                customText3Checked = originalPreset.customText3Checked;
                customText3 = originalPreset.customText3;
                customText4Checked = originalPreset.customText4Checked;
                customText4 = originalPreset.customText4;

                searchedTagId = originalPreset.searchedTagId;
                searchedPattern = originalPreset.searchedPattern;
                searchedTag2Id = originalPreset.searchedTag2Id;
                searchedPattern2 = originalPreset.searchedPattern2;
                searchedTag3Id = originalPreset.searchedTag3Id;
                searchedPattern3 = originalPreset.searchedPattern3;
                searchedTag4Id = originalPreset.searchedTag4Id;
                searchedPattern4 = originalPreset.searchedPattern4;
                searchedTag5Id = originalPreset.searchedTag5Id;
                searchedPattern5 = originalPreset.searchedPattern5;

                replacedTagId = originalPreset.replacedTagId;
                replacedPattern = originalPreset.replacedPattern;
                replacedTag2Id = originalPreset.replacedTag2Id;
                replacedPattern2 = originalPreset.replacedPattern2;
                replacedTag3Id = originalPreset.replacedTag3Id;
                replacedPattern3 = originalPreset.replacedPattern3;
                replacedTag4Id = originalPreset.replacedTag4Id;
                replacedPattern4 = originalPreset.replacedPattern4;
                replacedTag5Id = originalPreset.replacedTag5Id;
                replacedPattern5 = originalPreset.replacedPattern5;

                append = originalPreset.append;
                append2 = originalPreset.append2;
                append3 = originalPreset.append3;
                append4 = originalPreset.append4;
                append5 = originalPreset.append5;
            }

            public Preset(SavedPreset savedPreset)
                : this()
            {
                modifiedByUser = savedPreset.modifiedByUser;

                if ("" + savedPreset.modified == "01.01.0001 0:00:00")
                    modified = DateTime.UtcNow;
                else
                    modified = savedPreset.modified;


                if ("" + savedPreset.guid != "00000000-0000-0000-0000-000000000000")
                    guid = savedPreset.guid;

                for (int j = 0; j < savedPreset.names.Length; j += 2)
                {
                    names.Add(savedPreset.names[j], savedPreset.names[j + 1]);
                }

                for (int j = 0; j < savedPreset.descriptions.Length; j += 2)
                {
                    descriptions.Add(savedPreset.descriptions[j], savedPreset.descriptions[j + 1]);
                }

                ignoreCase = savedPreset.ignoreCase;
                autoApply = savedPreset.autoApply;
                condition = savedPreset.condition;
                playlist = savedPreset.playlist;

                parameterTagType = savedPreset.parameterTagType;
                parameterTag2Type = savedPreset.parameterTag2Type;
                parameterTag3Type = savedPreset.parameterTag3Type;
                parameterTag4Type = savedPreset.parameterTag4Type;
                parameterTag5Type = savedPreset.parameterTag5Type;
                parameterTag6Type = savedPreset.parameterTag6Type;

                parameterTagId = savedPreset.parameterTagId;
                parameterTag2Id = savedPreset.parameterTag2Id;
                parameterTag3Id = savedPreset.parameterTag3Id;
                parameterTag4Id = savedPreset.parameterTag4Id;
                parameterTag5Id = savedPreset.parameterTag5Id;
                parameterTag6Id = savedPreset.parameterTag6Id;

                customTextChecked = savedPreset.customTextChecked;
                customText = Regex.Replace(savedPreset.customText == null ? "" : savedPreset.customText, "\uFFFD", " ");
                customText2Checked = savedPreset.customText2Checked;
                customText2 = Regex.Replace(savedPreset.customText2 == null ? "" : savedPreset.customText2, "\uFFFD", " ");
                customText3Checked = savedPreset.customText3Checked;
                customText3 = Regex.Replace(savedPreset.customText3 == null ? "" : savedPreset.customText3, "\uFFFD", " ");
                customText4Checked = savedPreset.customText4Checked;
                customText4 = Regex.Replace(savedPreset.customText4 == null ? "" : savedPreset.customText4, "\uFFFD", " ");

                searchedTagId = savedPreset.searchedTagId;
                searchedPattern = Regex.Replace(savedPreset.searchedPattern == null ? "" : savedPreset.searchedPattern, "\uFFFD", " ");
                searchedTag2Id = savedPreset.searchedTag2Id;
                searchedPattern2 = Regex.Replace(savedPreset.searchedPattern2 == null ? "" : savedPreset.searchedPattern2, "\uFFFD", " ");
                searchedTag3Id = savedPreset.searchedTag3Id;
                searchedPattern3 = Regex.Replace(savedPreset.searchedPattern3 == null ? "" : savedPreset.searchedPattern3, "\uFFFD", " ");
                searchedTag4Id = savedPreset.searchedTag4Id;
                searchedPattern4 = Regex.Replace(savedPreset.searchedPattern4 == null ? "" : savedPreset.searchedPattern4, "\uFFFD", " ");
                searchedTag5Id = savedPreset.searchedTag5Id;
                searchedPattern5 = Regex.Replace(savedPreset.searchedPattern5 == null ? "" : savedPreset.searchedPattern5, "\uFFFD", " ");

                replacedTagId = savedPreset.replacedTagId;
                replacedPattern = Regex.Replace(savedPreset.replacedPattern == null ? "" : savedPreset.replacedPattern, "\uFFFD", " ");
                replacedTag2Id = savedPreset.replacedTag2Id;
                replacedPattern2 = Regex.Replace(savedPreset.replacedPattern2 == null ? "" : savedPreset.replacedPattern2, "\uFFFD", " ");
                replacedTag3Id = savedPreset.replacedTag3Id;
                replacedPattern3 = Regex.Replace(savedPreset.replacedPattern3 == null ? "" : savedPreset.replacedPattern3, "\uFFFD", " ");
                replacedTag4Id = savedPreset.replacedTag4Id;
                replacedPattern4 = Regex.Replace(savedPreset.replacedPattern4 == null ? "" : savedPreset.replacedPattern4, "\uFFFD", " ");
                replacedTag5Id = savedPreset.replacedTag5Id;
                replacedPattern5 = Regex.Replace(savedPreset.replacedPattern5 == null ? "" : savedPreset.replacedPattern5, "\uFFFD", " ");

                append = savedPreset.append;
                append2 = savedPreset.append2;
                append3 = savedPreset.append3;
                append4 = savedPreset.append4;
                append5 = savedPreset.append5;
            }

            public override string ToString()
            {
                return GetDictValue(names, Plugin.Language);
            }

            public DateTime modified;
            public bool modifiedByUser;
            public Guid guid;

            public SortedDictionary<string, string> names;
            public SortedDictionary<string, string> descriptions;

            public bool ignoreCase;
            public bool autoApply;
            public bool condition;
            public string playlist;

            public int parameterTagType;
            public int parameterTag2Type;
            public int parameterTag3Type;
            public int parameterTag4Type;
            public int parameterTag5Type;
            public int parameterTag6Type;

            public int parameterTagId;
            public int parameterTag2Id;
            public int parameterTag3Id;
            public int parameterTag4Id;
            public int parameterTag5Id;
            public int parameterTag6Id;

            public bool customTextChecked;
            public string customText;
            public bool customText2Checked;
            public string customText2;
            public bool customText3Checked;
            public string customText3;
            public bool customText4Checked;
            public string customText4;

            public int searchedTagId;
            public string searchedPattern;
            public int searchedTag2Id;
            public string searchedPattern2;
            public int searchedTag3Id;
            public string searchedPattern3;
            public int searchedTag4Id;
            public string searchedPattern4;
            public int searchedTag5Id;
            public string searchedPattern5;

            public int replacedTagId;
            public string replacedPattern;
            public int replacedTag2Id;
            public string replacedPattern2;
            public int replacedTag3Id;
            public string replacedPattern3;
            public int replacedTag4Id;
            public string replacedPattern4;
            public int replacedTag5Id;
            public string replacedPattern5;

            public bool append;
            public bool append2;
            public bool append3;
            public bool append4;
            public bool append5;
        }

        public struct SavedPreset
        {
            public SavedPreset(Preset originalPreset)
                : this()
            {
                modified = originalPreset.modified;
                modifiedByUser = originalPreset.modifiedByUser;
                guid = originalPreset.guid;

                names = new string[originalPreset.names.Values.Count * 2];
                descriptions = new string[originalPreset.descriptions.Values.Count * 2];

                int j = 0;
                foreach (string language in originalPreset.names.Keys)
                {
                    names[j] = language;
                    names[j + 1] = GetDictValue(originalPreset.names, language);
                    j += 2;
                }

                j = 0;
                foreach (string language in originalPreset.descriptions.Keys)
                {
                    descriptions[j] = language;
                    descriptions[j + 1] = GetDictValue(originalPreset.descriptions, language);
                    j += 2;
                }

                ignoreCase = originalPreset.ignoreCase;
                autoApply = originalPreset.autoApply;
                condition = originalPreset.condition;
                playlist = originalPreset.playlist;

                parameterTagType = originalPreset.parameterTagType;
                parameterTag2Type = originalPreset.parameterTag2Type;
                parameterTag3Type = originalPreset.parameterTag3Type;
                parameterTag4Type = originalPreset.parameterTag4Type;
                parameterTag5Type = originalPreset.parameterTag5Type;
                parameterTag6Type = originalPreset.parameterTag6Type;

                parameterTagId = originalPreset.parameterTagId;
                parameterTag2Id = originalPreset.parameterTag2Id;
                parameterTag3Id = originalPreset.parameterTag3Id;
                parameterTag4Id = originalPreset.parameterTag4Id;
                parameterTag5Id = originalPreset.parameterTag5Id;
                parameterTag6Id = originalPreset.parameterTag6Id;

                customTextChecked = originalPreset.customTextChecked;
                customText = Regex.Replace(originalPreset.customText, " ", "\uFFFD");
                customText2Checked = originalPreset.customText2Checked;
                customText2 = Regex.Replace(originalPreset.customText2, " ", "\uFFFD");
                customText3Checked = originalPreset.customText3Checked;
                customText3 = Regex.Replace(originalPreset.customText3, " ", "\uFFFD");
                customText4Checked = originalPreset.customText4Checked;
                customText4 = Regex.Replace(originalPreset.customText4, " ", "\uFFFD");

                searchedTagId = originalPreset.searchedTagId;
                searchedPattern = Regex.Replace(originalPreset.searchedPattern, " ", "\uFFFD");
                searchedTag2Id = originalPreset.searchedTag2Id;
                searchedPattern2 = Regex.Replace(originalPreset.searchedPattern2, " ", "\uFFFD");
                searchedTag3Id = originalPreset.searchedTag3Id;
                searchedPattern3 = Regex.Replace(originalPreset.searchedPattern3, " ", "\uFFFD");
                searchedTag4Id = originalPreset.searchedTag4Id;
                searchedPattern4 = Regex.Replace(originalPreset.searchedPattern4, " ", "\uFFFD");
                searchedTag5Id = originalPreset.searchedTag5Id;
                searchedPattern5 = Regex.Replace(originalPreset.searchedPattern5, " ", "\uFFFD");

                replacedTagId = originalPreset.replacedTagId;
                replacedPattern = Regex.Replace(originalPreset.replacedPattern, " ", "\uFFFD");
                replacedTag2Id = originalPreset.replacedTag2Id;
                replacedPattern2 = Regex.Replace(originalPreset.replacedPattern2, " ", "\uFFFD");
                replacedTag3Id = originalPreset.replacedTag3Id;
                replacedPattern3 = Regex.Replace(originalPreset.replacedPattern3, " ", "\uFFFD");
                replacedTag4Id = originalPreset.replacedTag4Id;
                replacedPattern4 = Regex.Replace(originalPreset.replacedPattern4, " ", "\uFFFD");
                replacedTag5Id = originalPreset.replacedTag5Id;
                replacedPattern5 = Regex.Replace(originalPreset.replacedPattern5, " ", "\uFFFD");

                append = originalPreset.append;
                append2 = originalPreset.append2;
                append3 = originalPreset.append3;
                append4 = originalPreset.append4;
                append5 = originalPreset.append5;
            }

            public DateTime modified;
            public bool modifiedByUser;
            public Guid guid;

            public string[] names;
            public string[] descriptions;

            public bool ignoreCase;
            public bool autoApply;
            public bool condition;
            public string playlist;

            public int parameterTagType;
            public int parameterTag2Type;
            public int parameterTag3Type;
            public int parameterTag4Type;
            public int parameterTag5Type;
            public int parameterTag6Type;

            public int parameterTagId;
            public int parameterTag2Id;
            public int parameterTag3Id;
            public int parameterTag4Id;
            public int parameterTag5Id;
            public int parameterTag6Id;

            public bool customTextChecked;
            public string customText;
            public bool customText2Checked;
            public string customText2;
            public bool customText3Checked;
            public string customText3;
            public bool customText4Checked;
            public string customText4;

            public int searchedTagId;
            public string searchedPattern;
            public int searchedTag2Id;
            public string searchedPattern2;
            public int searchedTag3Id;
            public string searchedPattern3;
            public int searchedTag4Id;
            public string searchedPattern4;
            public int searchedTag5Id;
            public string searchedPattern5;

            public int replacedTagId;
            public string replacedPattern;
            public int replacedTag2Id;
            public string replacedPattern2;
            public int replacedTag3Id;
            public string replacedPattern3;
            public int replacedTag4Id;
            public string replacedPattern4;
            public int replacedTag5Id;
            public string replacedPattern5;

            public bool append;
            public bool append2;
            public bool append3;
            public bool append4;
            public bool append5;
        }

        public struct SearchedAndReplacedTags
        {
            public string searchedTagValue;
            public string searchedTag2Value;
            public string searchedTag3Value;
            public string searchedTag4Value;
            public string searchedTag5Value;

            public string originalReplacedTagValue;
            public string originalReplacedTag2Value;
            public string originalReplacedTag3Value;
            public string originalReplacedTag4Value;
            public string originalReplacedTag5Value;

            public string replacedTagValue;
            public string replacedTag2Value;
            public string replacedTag3Value;
            public string replacedTag4Value;
            public string replacedTag5Value;
        }

        public class Playlist
        {
            public string playlist;

            public Playlist(string playlistParam)
            {
                playlist = playlistParam;
            }

            public override string ToString()
            {
                return Regex.Replace(playlist, ".*\\\\(.*)\\.[^.]*$", "$1");
            }
        }

        private class Function
        {
            protected string functionName = "null";

            protected virtual string calculate(string parameter0, string parameter1 = null)
            {
                return "\u0000";
            }

            public virtual string evaluate(string tagValue, string searchedPattern, string replacedPattern)
            {
                bool match;

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$0\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$0\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$0"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$0\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$1\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$1\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$1"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$1\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$2\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$2\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$2"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$2\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$3\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$3\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$3"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$3\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$4\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$4\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$4"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$4\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$5\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$5\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$5"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$5\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$6\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$6\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$6"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$6\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$7\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$7\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$7"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$7\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$8\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$8\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$8"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$8\,(.*?)\)", "$1")));

                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\(\$9\,(.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$9\,(.*?)\)", calculate(Regex.Replace(tagValue, searchedPattern, "$9"), Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$9\,(.*?)\)", "$1")));




                if (replacedPattern.Contains(@"\@" + functionName + "($0)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$0\)", calculate(Regex.Replace(tagValue, searchedPattern, "$0")));

                if (replacedPattern.Contains(@"\@" + functionName + "($1)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$1\)", calculate(Regex.Replace(tagValue, searchedPattern, "$1")));

                if (replacedPattern.Contains(@"\@" + functionName + "($2)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$2\)", calculate(Regex.Replace(tagValue, searchedPattern, "$2")));

                if (replacedPattern.Contains(@"\@" + functionName + "($3)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$3\)", calculate(Regex.Replace(tagValue, searchedPattern, "$3")));

                if (replacedPattern.Contains(@"\@" + functionName + "($4)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$4\)", calculate(Regex.Replace(tagValue, searchedPattern, "$4")));

                if (replacedPattern.Contains(@"\@" + functionName + "($5)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$5\)", calculate(Regex.Replace(tagValue, searchedPattern, "$5")));

                if (replacedPattern.Contains(@"\@" + functionName + "($6)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$6\)", calculate(Regex.Replace(tagValue, searchedPattern, "$6")));

                if (replacedPattern.Contains(@"\@" + functionName + "($7)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$7\)", calculate(Regex.Replace(tagValue, searchedPattern, "$7")));

                if (replacedPattern.Contains(@"\@" + functionName + "($8)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$8\)", calculate(Regex.Replace(tagValue, searchedPattern, "$8")));

                if (replacedPattern.Contains(@"\@" + functionName + "($9)"))
                    replacedPattern = Regex.Replace(replacedPattern, @"\\@" + functionName + @"\(\$9\)", calculate(Regex.Replace(tagValue, searchedPattern, "$9")));




                match = Regex.Matches(replacedPattern, @"\\@" + functionName + @"\((.*?)\)", RegexOptions.IgnoreCase).Count > 0 ? true : false;
                if (match)
                    replacedPattern = Regex.Replace(replacedPattern, @"(.*?)\\@" + functionName + @"\((.*?)\)(.*)", "$1" + calculate(Regex.Replace(replacedPattern, @".*?\\@" + functionName + @"\((.*?)\).*", "$1")) + "$3");


                return replacedPattern;
            }
        }

        private class Rg2sc : Function
        {
            public Rg2sc()
            {
                functionName = "rg2sc";
            }

            protected override string calculate(string parameter0, string parameter1 = null)
            {
                parameter0 = Regex.Replace(parameter0, @"(.*)\s.*", "$1");


                double replayGain;

                try
                {
                    replayGain = Convert.ToDouble(parameter0);
                }
                catch
                {
                    replayGain = 0;
                }


                double soundCheck1000d = 1000 * Math.Pow(10.0, (-0.1 * replayGain));
                //if (soundCheck1000d > 65534)
                //    soundCheck1000d = 65534;
                uint soundCheck1000;

                try
                {
                    soundCheck1000 = Convert.ToUInt32(soundCheck1000d);
                }
                catch
                {
                    soundCheck1000 = 0;
                }


                double soundCheck2500d = 2500 * Math.Pow(10.0, (-0.1 * replayGain));
                if (soundCheck2500d > 65534)
                    soundCheck2500d = 65534;
                uint soundCheck2500;

                try
                {
                    soundCheck2500 = Convert.ToUInt32(soundCheck2500d);
                }
                catch
                {
                    soundCheck2500 = 0;
                }


                string ITUNNORM = (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                return ITUNNORM;
            }
        }

        private class Rg2sc4mp3 : Rg2sc
        {
            public Rg2sc4mp3()
            {
                functionName = "rg2sc4mp3";
            }

            protected override string calculate(string parameter0, string parameter1 = null)
            {
                return base.calculate(parameter0) + "\u0000";
            }
        }

        private class Tc : Function
        {
            public Tc()
            {
                functionName = "tc";
            }

            protected override string calculate(string parameter0, string parameter1 = null)
            {
                string[] exceptionWords = parameter1.Split(' ');
                string result = ChangeCasePlugin.ChangeWordsCase(parameter0, ChangeCasePlugin.ChangeCaseOptions.titleCase, exceptionWords, false, null, new string[] { "." }, true, true);

                return result;
            }
        }

        private class Lc : Function
        {
            public Lc()
            {
                functionName = "lc";
            }

            protected override string calculate(string parameter0, string parameter1 = null)
            {
                string[] exceptionWords = parameter1.Split(' ');
                string result = ChangeCasePlugin.ChangeWordsCase(parameter0, ChangeCasePlugin.ChangeCaseOptions.lowerCase, exceptionWords, false, null, new string[] { "." }, true, true);

                return result;
            }
        }

        private class Char1 : Function
        {
            public Char1()
            {
                functionName = "char";
            }

            protected override string calculate(string parameter0, string parameter1 = null)
            {
                int char1;

                if (int.TryParse(parameter0, out char1))
                {
                    return ((char)char1).ToString();
                }
                else
                {
                    return "\u0000";
                }
            }
        }

        private class Mmmss2hhmmss : Function
        {
            public Mmmss2hhmmss()
            {
                functionName = "mmmss2hhmmss";
            }

            protected override string calculate(string parameter0, string parameter1 = null)
            {
                int mins = Convert.ToInt32(Regex.Replace(parameter0, "^(.*)(\\:|\\.)(.*)", "$1"));
                int secs = Convert.ToInt32(Regex.Replace(parameter0, "^(.*)(\\:|\\.)(.*)", "$3"));

                TimeSpan time;

                time = TimeSpan.FromSeconds(secs);
                time += TimeSpan.FromMinutes(mins);

                return time.ToString("T", System.Globalization.CultureInfo.CreateSpecificCulture("ru-ru"));
            }
        }

        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private static Encoding Unicode = Encoding.UTF8;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private Preset preset = null;

        private string[] fileTags;
        private string clipboardText;

        private static SearchedAndReplacedTags searchedAndReplacedTags;
        private static Dictionary<int, string> setTags = new Dictionary<int, string>();

        private static string PresetsPath;
        private static SortedDictionary<Guid, Preset> Presets;

        private string newValueText;

        private bool editButtonEnabled;

        public static string GetDictValue(SortedDictionary<string, string> dict, string language)
        {
            string value;

            dict.TryGetValue(language, out value);

            if (value == null)
                dict.TryGetValue("en", out value);

            return value;
        }

        public static void SetDictValue(SortedDictionary<string, string> dict, string language, string newValue)
        {
            string value;

            dict.TryGetValue(language, out value);

            if (value != null)
                dict.Remove(language);

            dict.Add(language, newValue);


            dict.TryGetValue("en", out value);

            if (value == null)
                dict.Add("en", newValue);
        }

        public static void Init(Plugin tagToolsPluginParam)
        {
            lock (tagToolsPluginParam.autoAppliedPresets)
            {
                Encoding Unicode = Encoding.UTF8;

                setTags = new Dictionary<int, string>();

                PresetsPath = Plugin.MbApiInterface.Setting_GetPersistentStoragePath() + @"\" + Plugin.ASRPresetsPath;
                Presets = new SortedDictionary<Guid, Preset>();
                string[] presetNames;

                tagToolsPluginParam.autoAppliedPresets.Clear();

                if (!System.IO.Directory.Exists(PresetsPath))
                    System.IO.Directory.CreateDirectory(PresetsPath);

                presetNames = System.IO.Directory.GetFiles(PresetsPath, "*" + Plugin.ASRPresetExtension);
                System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedPreset));

                foreach (string presetName in presetNames)
                {
                    System.IO.FileStream stream = System.IO.File.Open(presetName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
                    System.IO.StreamReader file = new System.IO.StreamReader(stream, Unicode);

                    SavedPreset savedPreset;
                    Preset tempPreset;

                    try
                    {
                        savedPreset = (SavedPreset)presetSerializer.Deserialize(file); 
                        tempPreset = new Preset(savedPreset);
                        Presets.Add(tempPreset.guid, tempPreset);

                        if (tempPreset.autoApply)
                            tagToolsPluginParam.autoAppliedPresets.Add(tempPreset);
                    }
                    catch { };

                    file.Close();
                }
            }
        }

        public AdvancedSearchAndReplacePlugin() 
        {
            InitializeComponent();
        }

        public AdvancedSearchAndReplacePlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            presetList.Sorted = false;
            int i = 0;
            foreach (Preset tempPreset in Presets.Values)
            {
                presetList.Items.Add(tempPreset);
                presetList.SetItemChecked(i, tempPreset.autoApply);
                i++;
            }
            presetList.Sorted = true;

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            colCB.HeaderCell = cbHeader;
            colCB.ThreeState = true;

            colCB.FalseValue = "False";
            colCB.TrueValue = "True";
            colCB.IndeterminateValue = "";
            colCB.Width = 25;
            colCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[5].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[7].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[8].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[9].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[10].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[11].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[12].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            newValueText = previewTable.Columns[4].HeaderText;

            MbApiInterface.Playlist_QueryPlaylists();
            string playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

            while (playlist != null)
            {
                //if (mbApiInterface.Playlist_GetType(playlist) == Plugin.PlaylistFormat.Auto)
                {
                    Playlist newPlaylist = new Playlist(playlist);
                    playlistComboBox.Items.Add(newPlaylist);
                }

                playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
            }

            presetList_SelectedIndexChanged(null, null);

            addRowToTable = previewList_AddRowToTable;
            processRowOfTable = previewList_ProcessRowOfTable;
        }

        protected static bool setFileTag(string sourceFileUrl, Plugin.MetaDataType tagId, string value, bool updateOnlyChangedTags, AdvancedSearchAndReplacePlugin plugin)
        {
            if (tagId == Plugin.ClipboardTagId)
            {
                if (plugin == null)
                    return false;

                if (plugin.clipboardText == "")
                {
                    plugin.clipboardText += value;
                    return true;
                }
                else
                {
                    plugin.clipboardText += "" + (char)13 + (char)10 + value;
                    return true;
                }
            }

            return TagToolsPlugin.setFileTag(sourceFileUrl, tagId, value, updateOnlyChangedTags);
        }

        private void previewList_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);
        }

        private void previewList_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = "";
        }

        public static string Replace(string value, string searchedPattern, string replacedPattern, bool ignoreCase, out bool match)
        {
            match = false;
            if (searchedPattern == "")
                return value;


            match = Regex.Matches(value, searchedPattern, RegexOptions.IgnoreCase).Count > 0 ? true : false;

            if (!match)
                return value;


            //Lets evaluate all supported functions
            Function nullFunction = new Function();
            replacedPattern = nullFunction.evaluate(value, searchedPattern, replacedPattern);

            Function rg2sc = new Rg2sc();
            replacedPattern = rg2sc.evaluate(value, searchedPattern, replacedPattern);

            Function rg2sc4mp3 = new Rg2sc4mp3();
            replacedPattern = rg2sc4mp3.evaluate(value, searchedPattern, replacedPattern);

            Function tc = new Tc();
            replacedPattern = tc.evaluate(value, searchedPattern, replacedPattern);

            Function lc = new Lc();
            replacedPattern = lc.evaluate(value, searchedPattern, replacedPattern);

            Function char1 = new Char1();
            replacedPattern = char1.evaluate(value, searchedPattern, replacedPattern);

            Function mmmss2hhmmss = new Mmmss2hhmmss();
            replacedPattern = mmmss2hhmmss.evaluate(value, searchedPattern, replacedPattern);


            if (ignoreCase)
                value = Regex.Replace(value, searchedPattern, replacedPattern, RegexOptions.IgnoreCase);
            else
                value = Regex.Replace(value, searchedPattern, replacedPattern, RegexOptions.None);


            return value;
        }

        public static string GetTag(string currentFile, Plugin tagToolsPluginParam, AdvancedSearchAndReplacePlugin plugin, int tagId)
        {
            if (tagId == (int)Plugin.DateCreatedTagId)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(currentFile);
                if (fileInfo.Exists)
                    return fileInfo.CreationTime.Year.ToString("D4") + "-" + fileInfo.CreationTime.Month.ToString("D2") + "-" + fileInfo.CreationTime.Day.ToString("D2");
                else
                    return "";
            }
            else if (tagId == (int)Plugin.ClipboardTagId)
            {
                if (plugin == null)
                    return "";

                if (plugin.fileTags == null)
                    return "";

                int position = -1;
                for (int i = 0; i < plugin.files.Length; i++)
                {
                    if (plugin.files[i] == currentFile)
                    {
                        position = i;
                        break;
                    }
                }

                return plugin.fileTags[position];
            }


            string tempTag;

            setTags.TryGetValue(tagId, out tempTag);

            if (tempTag == null)
            {
                if ((int)tagId < 1000)
                    return tagToolsPluginParam.getFileTag(currentFile, (Plugin.MetaDataType)tagId);
                else
                    return Plugin.MbApiInterface.Library_GetFileProperty(currentFile, (Plugin.FilePropertyType)(tagId - 1000));
            }
            else
            {
                return tempTag;
            }
        }

        public static void SetTag(string currentFile, Plugin tagToolsPluginParam, int tagId, string tagValue)
        {
            string tempTag;

            setTags.TryGetValue(tagId, out tempTag);
            if (tempTag != null)
                setTags.Remove(tagId);

            setTags.Add(tagId, tagValue);
        }

        public static void SetReplacedTag(string currentFile, Plugin tagToolsPluginParam, AdvancedSearchAndReplacePlugin plugin, int searchedTagId, int replacedTagId, string searchedPattern, string replacedPattern,
            bool ignoreCase, bool append, out string searchedTagValue, out string replacedTagValue, out string originalReplacedTagValue)
        {
            string sourceTagValue;
            string newTagValue;
            string originalNewTagValue;


            if (searchedPattern == "")
            {
                searchedTagValue = "";
                replacedTagValue = "";
                originalReplacedTagValue = "";
                return;
            }

            sourceTagValue = GetTag(currentFile, tagToolsPluginParam, plugin, searchedTagId);
            originalNewTagValue = GetTag(currentFile, tagToolsPluginParam, plugin, replacedTagId);

            bool match;
            newTagValue = Replace(sourceTagValue, searchedPattern, replacedPattern, ignoreCase, out match);


            if (append)
                newTagValue = originalNewTagValue + newTagValue;

            SetTag(currentFile, tagToolsPluginParam, replacedTagId, newTagValue);

            if (match)
                replacedTagValue = newTagValue;
            else
                replacedTagValue = originalNewTagValue;

            searchedTagValue = sourceTagValue;
            originalReplacedTagValue = originalNewTagValue;
        }

        public static string ReplaceVariable(Preset presetParam, string pattern, bool searchedPattern)
        {
            if (presetParam.customTextChecked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@1", Regex.Escape(Regex.Replace(presetParam.customText, "<tab>", "\u0009")));
            else if (presetParam.customTextChecked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@1", Regex.Replace(presetParam.customText, "<tab>", "\u0009"));

            if (presetParam.customText2Checked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@2", Regex.Escape(Regex.Replace(presetParam.customText2, "<tab>", "\u0009")));
            else if (presetParam.customText2Checked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@2", Regex.Replace(presetParam.customText2, "<tab>", "\u0009"));

            if (presetParam.customText3Checked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@3", Regex.Escape(Regex.Replace(presetParam.customText3, "<tab>", "\u0009")));
            else if (presetParam.customText3Checked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@3", Regex.Replace(presetParam.customText3, "<tab>", "\u0009"));

            if (presetParam.customText4Checked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@4", Regex.Escape(Regex.Replace(presetParam.customText4, "<tab>", "\u0009")));
            else if (presetParam.customText4Checked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@4", Regex.Replace(presetParam.customText4, "<tab>", "\u0009"));

            return pattern;
        }

        public static int SubstituteTagId(Preset presetParam, int tagId)
        {
            switch (tagId)
            {
                case -101:
                    tagId = presetParam.parameterTagId;
                    break;
                case -102:
                    tagId = presetParam.parameterTag2Id;
                    break;
                case -103:
                    tagId = presetParam.parameterTag3Id;
                    break;
                case -104:
                    tagId = presetParam.parameterTag4Id;
                    break;
                case -105:
                    tagId = presetParam.parameterTag5Id;
                    break;
                case -106:
                    tagId = presetParam.parameterTag6Id;
                    break;
            }

            return tagId;
        }

        public static void GetReplacedTags(string currentFile, Plugin tagToolsPluginParam, AdvancedSearchAndReplacePlugin plugin, Preset presetParam)
        {
            searchedAndReplacedTags = new SearchedAndReplacedTags();
            setTags.Clear();

            SetReplacedTag(currentFile, tagToolsPluginParam, plugin, SubstituteTagId(presetParam, presetParam.searchedTagId), SubstituteTagId(presetParam, presetParam.replacedTagId), 
                ReplaceVariable(presetParam, presetParam.searchedPattern, true), ReplaceVariable(presetParam, presetParam.replacedPattern, false), presetParam.ignoreCase, presetParam.append, 
                out searchedAndReplacedTags.searchedTagValue, out searchedAndReplacedTags.replacedTagValue, out searchedAndReplacedTags.originalReplacedTagValue);
            SetReplacedTag(currentFile, tagToolsPluginParam, plugin, SubstituteTagId(presetParam, presetParam.searchedTag2Id), SubstituteTagId(presetParam, presetParam.replacedTag2Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern2, true), ReplaceVariable(presetParam, presetParam.replacedPattern2, false), presetParam.ignoreCase, presetParam.append2, 
                out searchedAndReplacedTags.searchedTag2Value, out searchedAndReplacedTags.replacedTag2Value, out searchedAndReplacedTags.originalReplacedTag2Value);
            SetReplacedTag(currentFile, tagToolsPluginParam, plugin, SubstituteTagId(presetParam, presetParam.searchedTag3Id), SubstituteTagId(presetParam, presetParam.replacedTag3Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern3, true), ReplaceVariable(presetParam, presetParam.replacedPattern3, false), presetParam.ignoreCase, presetParam.append3, 
                out searchedAndReplacedTags.searchedTag3Value, out searchedAndReplacedTags.replacedTag3Value, out searchedAndReplacedTags.originalReplacedTag3Value);
            SetReplacedTag(currentFile, tagToolsPluginParam, plugin, SubstituteTagId(presetParam, presetParam.searchedTag4Id), SubstituteTagId(presetParam, presetParam.replacedTag4Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern4, true), ReplaceVariable(presetParam, presetParam.replacedPattern4, false), presetParam.ignoreCase, presetParam.append4, 
                out searchedAndReplacedTags.searchedTag4Value, out searchedAndReplacedTags.replacedTag4Value, out searchedAndReplacedTags.originalReplacedTag4Value);
            SetReplacedTag(currentFile, tagToolsPluginParam, plugin, SubstituteTagId(presetParam, presetParam.searchedTag5Id), SubstituteTagId(presetParam, presetParam.replacedTag5Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern5, true), ReplaceVariable(presetParam, presetParam.replacedPattern5, false), presetParam.ignoreCase, presetParam.append5, 
                out searchedAndReplacedTags.searchedTag5Value, out searchedAndReplacedTags.replacedTag5Value, out searchedAndReplacedTags.originalReplacedTag5Value);
        }

        public static void SaveReplacedTags(string currentFile, Plugin tagToolsPluginParam, Preset presetParam, AdvancedSearchAndReplacePlugin plugin)
        {
            bool wereChanges = false;

            if (presetParam.searchedPattern != "")
            {
                if (searchedAndReplacedTags.originalReplacedTagValue != searchedAndReplacedTags.replacedTagValue)
                {
                    wereChanges = true;
                    setFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTagId), searchedAndReplacedTags.replacedTagValue, true, plugin);
                }

                if (presetParam.searchedPattern2 != "")
                {
                    if (searchedAndReplacedTags.originalReplacedTag2Value != searchedAndReplacedTags.replacedTag2Value)
                    {
                        wereChanges = true;
                        setFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag2Id), searchedAndReplacedTags.replacedTag2Value, true, plugin);
                    }

                    if (presetParam.searchedPattern3 != "")
                    {
                        if (searchedAndReplacedTags.originalReplacedTag3Value != searchedAndReplacedTags.replacedTag3Value)
                        {
                            wereChanges = true;
                            setFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag3Id), searchedAndReplacedTags.replacedTag3Value, true, plugin);
                        }

                        if (presetParam.searchedPattern4 != "")
                        {
                            if (searchedAndReplacedTags.originalReplacedTag4Value != searchedAndReplacedTags.replacedTag4Value)
                            {
                                wereChanges = true;
                                setFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag4Id), searchedAndReplacedTags.replacedTag4Value, true, plugin);
                            }

                            if (presetParam.searchedPattern5 != "")
                            {
                                if (searchedAndReplacedTags.originalReplacedTag5Value != searchedAndReplacedTags.replacedTag5Value)
                                {
                                    wereChanges = true;
                                    setFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag5Id), searchedAndReplacedTags.replacedTag5Value, true, plugin);
                                }
                            }
                        }
                    }
                }
            }


            if (wereChanges)
                tagToolsPluginParam.commitTagsToFile(currentFile, true, true);
        }

        public static void ReplaceTags(string currentFile, Plugin tagToolsPluginParam, AdvancedSearchAndReplacePlugin plugin, Preset presetParam)
        {
            lock (Presets)
            {
                GetReplacedTags(currentFile, tagToolsPluginParam, plugin, presetParam);
                SaveReplacedTags(currentFile, tagToolsPluginParam, presetParam, plugin);
            }
        }

        public static void AutoApply(object currentFileObj, object tagToolsPluginObj)
        {
            string currentFile = (string)currentFileObj;
            Plugin tagToolsPluginParam = (Plugin)tagToolsPluginObj;

            //lock (tagToolsPluginParam.autoAppliedPresets)
            {
                if (tagToolsPluginParam.autoAppliedPresets.Count > 0)
                {
                    tagToolsPluginParam.setStatusbarText(tagToolsPluginParam.sbPresetIsAutoApplied1 + tagToolsPluginParam.sbPresetIsAutoApplied2);

                    foreach (Preset tempPreset in tagToolsPluginParam.autoAppliedPresets)
                    {
                        bool conditionSatisfied = true;

                        if (tempPreset.condition)
                        {
                            conditionSatisfied = false;

                            Plugin.MbApiInterface.Playlist_QueryPlaylists();
                            string playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();

                            while (playlist != null)
                            {
                                if (playlist == tempPreset.playlist)
                                {
                                    conditionSatisfied = Plugin.MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                    break;
                                }

                                playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();
                            }
                        }

                        if (conditionSatisfied)
                        {
                            ReplaceTags(currentFile, tagToolsPluginParam, null, tempPreset);

                            bool englishIsAvailable = false;
                            bool nativeLanguageIsAvailable = false;
                            string firstLanguage = null;

                            foreach (string language in tempPreset.names.Keys)
                            {
                                if (firstLanguage == null)
                                    firstLanguage = language;

                                if (language == Plugin.Language)
                                    nativeLanguageIsAvailable = true;

                                if (language == "en")
                                    englishIsAvailable = true;
                            }

                            string selectedLanguage;

                            if (nativeLanguageIsAvailable)
                                selectedLanguage = Plugin.Language;
                            else if (englishIsAvailable)
                                selectedLanguage = "en";
                            else
                                selectedLanguage = firstLanguage;

                            //tagToolsPluginParam.SetBackgroundTaskMessage(tagToolsPluginParam.sbPresetIsAutoApplied1 + "\"" + AdvancedSearchAndReplacePlugin.GetDictValue(tempPreset.names, selectedLanguage) + "\"" + tagToolsPluginParam.sbPresetIsAutoApplied2);
                        }
                    }

                    //tagToolsPluginParam.SetBackgroundTaskMessage("");
                    tagToolsPluginParam.refreshPanels(true);
                }
            }
        }

        private bool prepareBackgroundPreview()
        {
            if (backgroundTaskIsWorking())
                return true;

            if (presetList.SelectedIndex == -1)
            {
                MessageBox.Show(TagToolsPlugin.msgNoPresetSelected);
                return false;
            }

            clipboardText = "";
            fileTags = null;


            tags.Clear();
            previewTable.Rows.Clear();
            ((DatagridViewCheckBoxHeaderCell)previewTable.Columns[0].HeaderCell).setState(true);


            if (MbApiInterface.Library_QueryFiles("domain=SelectedFiles"))
                files = MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                files = new string[0];



            Preset preset = (Preset)presetList.SelectedItem;
            if (preset.searchedTagId == (int)Plugin.ClipboardTagId || preset.searchedTag2Id == (int)Plugin.ClipboardTagId || preset.searchedTag3Id == (int)Plugin.ClipboardTagId
                || preset.searchedTag4Id == (int)Plugin.ClipboardTagId || preset.searchedTag5Id == (int)Plugin.ClipboardTagId)
            {
                if (!Clipboard.ContainsText())
                {
                    MessageBox.Show(TagToolsPlugin.msgClipboardDesntContainText);
                    return false;
                }

                fileTags = Clipboard.GetText().Split(new string[] { "" + (char)13 + (char)10 }, StringSplitOptions.None);

                if (fileTags.Length != files.Length)
                {
                    MessageBox.Show(TagToolsPlugin.msgNumberOfTagsInClipboard + fileTags.Length + TagToolsPlugin.msgDoesntCorrespondToNumberOfSelectedTracksC + files.Length + TagToolsPlugin.msgMessageEndC);
                    return false;
                }
            }

            
            if (files.Length == 0)
            {
                MessageBox.Show(TagToolsPlugin.msgNoFilesSelected);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            if (previewTable.Rows.Count == 0)
            {
                return prepareBackgroundPreview();
            }
            else
            {
                string[] tag;

                tags.Clear();

                for (int fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    tag = new string[7];

                    tag[0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;
                    tag[1] = (string)previewTable.Rows[fileCounter].Cells[1].Value;
                    tag[2] = (string)previewTable.Rows[fileCounter].Cells[4].Value;
                    tag[3] = (string)previewTable.Rows[fileCounter].Cells[6].Value;
                    tag[4] = (string)previewTable.Rows[fileCounter].Cells[8].Value;
                    tag[5] = (string)previewTable.Rows[fileCounter].Cells[10].Value;
                    tag[6] = (string)previewTable.Rows[fileCounter].Cells[12].Value;

                    tags.Add(tag);
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;

            string track;
            string[] row = { "Checked", "File", "Track", "OriginalTag1", "NewTag1", "OriginalTag2", "NewTag2", "OriginalTag3", "NewTag3", "OriginalTag4", "NewTag4", "OriginalTag5", "NewTag5" };
            string[] tag = { "Checked", "file", "newTag1", "newTag2", "newTag3", "newTag4", "newTag5" };

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.asrCommandSbText, true, fileCounter, files.Length, currentFile);

                track = TagToolsPlugin.getTrackRepresentation(currentFile);

                lock (Presets)
                {
                    GetReplacedTags(currentFile, TagToolsPlugin, this, preset);

                    tag = new string[7];

                    tag[0] = "True";
                    tag[1] = currentFile;
                    tag[2] = searchedAndReplacedTags.replacedTagValue;
                    tag[3] = searchedAndReplacedTags.replacedTag2Value;
                    tag[4] = searchedAndReplacedTags.replacedTag3Value;
                    tag[5] = searchedAndReplacedTags.replacedTag4Value;
                    tag[6] = searchedAndReplacedTags.replacedTag5Value;


                    row = new string[13];

                    row[0] = "True";
                    row[1] = currentFile;
                    row[2] = track;
                    row[3] = searchedAndReplacedTags.originalReplacedTagValue;
                    row[4] = searchedAndReplacedTags.replacedTagValue;
                    row[5] = searchedAndReplacedTags.originalReplacedTag2Value;
                    row[6] = searchedAndReplacedTags.replacedTag2Value;
                    row[7] = searchedAndReplacedTags.originalReplacedTag3Value;
                    row[8] = searchedAndReplacedTags.replacedTag3Value;
                    row[9] = searchedAndReplacedTags.originalReplacedTag4Value;
                    row[10] = searchedAndReplacedTags.replacedTag4Value;
                    row[11] = searchedAndReplacedTags.originalReplacedTag5Value;
                    row[12] = searchedAndReplacedTags.replacedTag5Value;
                }

                Invoke(addRowToTable, new Object[] { row });

                tags.Add(tag);

                previewIsGenerated = true;
            }

            TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.asrCommandSbText, true, files.Length - 1, files.Length, null, true);
        }

        private void applyToSelected()
        {
            string currentFile;
            string isChecked;

            if (tags.Count == 0)
                previewChanges();

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                isChecked = tags[i][0];
                currentFile = tags[i][1];

                if (isChecked == "True")
                {
                    lock (Presets)
                    {
                        tags[i][0] = "";

                        searchedAndReplacedTags.replacedTagValue = tags[i][2];
                        searchedAndReplacedTags.replacedTag2Value = tags[i][3];
                        searchedAndReplacedTags.replacedTag3Value = tags[i][4];
                        searchedAndReplacedTags.replacedTag4Value = tags[i][5];
                        searchedAndReplacedTags.replacedTag5Value = tags[i][6];

                        TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.asrCommandSbText, false, i, tags.Count, currentFile);

                        SaveReplacedTags(currentFile, TagToolsPlugin, preset, this);
                    }

                    Invoke(processRowOfTable, new Object[] { i });
                }
            }


            Preset preset2 = (Preset)presetList.SelectedItem;
            if (preset2.replacedTagId == (int)Plugin.ClipboardTagId || preset2.replacedTag2Id == (int)Plugin.ClipboardTagId || preset2.replacedTag3Id == (int)Plugin.ClipboardTagId
                || preset2.replacedTag4Id == (int)Plugin.ClipboardTagId || preset2.replacedTag5Id == (int)Plugin.ClipboardTagId)
            {
                System.Threading.Thread thread = new System.Threading.Thread(() => Clipboard.SetText(clipboardText));
                thread.SetApartmentState(System.Threading.ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
            }


            TagToolsPlugin.refreshPanels(true);

            TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.asrCommandSbText, false, tags.Count - 1, tags.Count, null, true);
        }

        public string getTagName(int tagId)
        {
            string tagName;

            if (tagId == -101)
                tagName = "<" + Plugin.ParameterTagName + " 1>";
            else if (tagId == -102)
                tagName = "<" + Plugin.ParameterTagName + " 2>";
            else if (tagId == -103)
                tagName = "<" + Plugin.ParameterTagName + " 3>";
            else if (tagId == -104)
                tagName = "<" + Plugin.ParameterTagName + " 4>";
            else if (tagId == -105)
                tagName = "<" + Plugin.ParameterTagName + " 5>";
            else if (tagId == -106)
                tagName = "<" + Plugin.ParameterTagName + " 6>";
            else if (tagId == -201)
                tagName = "<" + Plugin.TempTagName + " 1>";
            else if (tagId == -202)
                tagName = "<" + Plugin.TempTagName + " 2>";
            else if (tagId == -203)
                tagName = "<" + Plugin.TempTagName + " 3>";
            else if (tagId == -204)
                tagName = "<" + Plugin.TempTagName + " 4>";
            else if (tagId == (int)Plugin.ClipboardTagId)
                tagName = Plugin.ClipboardTagName;
            else if (tagId < 1000)
                tagName = Plugin.GetTagName((Plugin.MetaDataType)tagId);
            else
                tagName = TagToolsPlugin.getPropName((Plugin.FilePropertyType)(tagId - 1000));

            return tagName;
        }

        public int getTagId(string tagName)
        {
            int tagId;

            if (tagName == "<" + Plugin.ParameterTagName + " 1>")
                tagId = -101;
            else if (tagName == "<" + Plugin.ParameterTagName + " 2>")
                tagId = -102;
            else if (tagName == "<" + Plugin.ParameterTagName + " 3>")
                tagId = -103;
            else if (tagName == "<" + Plugin.ParameterTagName + " 4>")
                tagId = -104;
            else if (tagName == "<" + Plugin.ParameterTagName + " 5>")
                tagId = -105;
            else if (tagName == "<" + Plugin.ParameterTagName + " 6>")
                tagId = -106;
            else if (tagName == "<" + Plugin.TempTagName + " 1>")
                tagId = -201;
            else if (tagName == "<" + Plugin.TempTagName + " 2>")
                tagId = -202;
            else if (tagName == "<" + Plugin.TempTagName + " 3>")
                tagId = -203;
            else if (tagName == "<" + Plugin.TempTagName + " 4>")
                tagId = -204;
            else if (tagName == Plugin.ClipboardTagName)
                tagId = (int)Plugin.ClipboardTagId;
            else
                tagId = (int)Plugin.GetTagId(tagName);


            if (tagId == 0)
            {
                tagId = (int)TagToolsPlugin.getPropId(tagName);

                if (tagId != 0)
                    tagId += 1000;
            }

            return tagId;
        }

        private void editPreset(Preset tempPreset, Preset originalPreset)
        {
            ASRPresetEditor tagToolsForm = new ASRPresetEditor(TagToolsPlugin, this);
            tagToolsForm.editPreset(ref tempPreset);

            if (tempPreset != null)
            {
                tempPreset.modified = DateTime.UtcNow;

                if (originalPreset != null)
                {
                    Presets.Remove(((Preset)presetList.SelectedItem).guid);
                    Presets.Add(tempPreset.guid, tempPreset);

                    presetList.Items[presetList.SelectedIndex] = tempPreset;
                }
                else
                {
                    Presets.Add(tempPreset.guid, tempPreset);

                    presetList.Items.Add(tempPreset);
                    presetList.SelectedItem = tempPreset;
                }
            }
        }

        private void saveSettings()
        {
            DateTime userPresetDateTime = new DateTime(2100, 01, 01);
            DateTime oldPresetDateTime = new DateTime(2000, 01, 01);

            string[] presetNames = System.IO.Directory.GetFiles(PresetsPath, "*" + Plugin.ASRPresetExtension);

            foreach (string presetName in presetNames)
            {
                System.IO.File.Delete(presetName);
            }


            for (int i = 0; i < presetList.Items.Count; i++)
            {
                Preset tempPreset;

                Presets.TryGetValue(((Preset)presetList.Items[i]).guid, out tempPreset);

                if (presetList.GetItemChecked(i))
                    tempPreset.autoApply = true;
                else
                    tempPreset.autoApply = false;

                Presets.Remove(((Preset)presetList.Items[i]).guid);
                Presets.Add(tempPreset.guid, tempPreset);
            }


            System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedPreset));

            foreach (Preset tempPreset in Presets.Values)
            {
                System.IO.FileStream stream = System.IO.File.Open(PresetsPath + @"\" + tempPreset.guid + Plugin.ASRPresetExtension, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
                System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Unicode);

                SavedPreset savedPreset = new SavedPreset(tempPreset);

                presetSerializer.Serialize(file, savedPreset);

                System.IO.FileInfo fileInfo = new System.IO.FileInfo(PresetsPath + @"\" + tempPreset.guid + Plugin.ASRPresetExtension);

                file.Close();

                if (tempPreset.modifiedByUser)
                    fileInfo.LastWriteTimeUtc = userPresetDateTime;
                else
                {
                    if (tempPreset.modified < oldPresetDateTime)
                        tempPreset.modified = DateTime.Now;

                    fileInfo.LastWriteTimeUtc = tempPreset.modified;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (prepareBackgroundTask())
                switchOperation(applyToSelected, (Button)sender, buttonPreview, buttonCancel);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveSettings();
            Init(TagToolsPlugin);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
            {
                MessageBox.Show(TagToolsPlugin.msgNoPresetSelected);
                return;
            }

            DialogResult result = MessageBox.Show(TagToolsPlugin.msgDeletePresetConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            Presets.Remove(((Preset)presetList.SelectedItem).guid);
            presetList.Items.RemoveAt(presetList.SelectedIndex);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
            {
                MessageBox.Show(TagToolsPlugin.msgNoPresetSelected);
                return;
            }

            editPreset(new Preset(preset, " *"), null);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
            {
                MessageBox.Show(TagToolsPlugin.msgNoPresetSelected);
                return;
            }

            editPreset(new Preset(preset), preset);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            Preset newPreset = new Preset();

            newPreset.names = new SortedDictionary<string, string>();
            newPreset.descriptions = new SortedDictionary<string, string>();

            newPreset.names.Add(Plugin.Language, TagToolsPlugin.defaultASRPresetName + (presetList.Items.Count + 1));
            newPreset.ignoreCase = true;

            editPreset(newPreset, null);
        }


        public void fillParameterTagList(int parameterTagType, ComboBox parameterTagListParam, Label parameterTagLabelParam)
        {
            parameterTagListParam.Items.Clear();

            if (parameterTagType == 0) //Not used
            {
                parameterTagListParam.Text = "";
                parameterTagListParam.Enabled = false;
                parameterTagLabelParam.Enabled = false;
            }
            else if (parameterTagType == 1) //Writable
            {
                Plugin.FillList(parameterTagListParam.Items);
                parameterTagListParam.Enabled = true;
                parameterTagLabelParam.Enabled = true;
            }
            else if (parameterTagType == 2) //Read only
            {
                Plugin.FillList(parameterTagListParam.Items, true);
                Plugin.FillListWithProps(parameterTagListParam.Items);
                parameterTagListParam.Items.Add(Plugin.DateCreatedTagName);
                parameterTagListParam.Enabled = true;
                parameterTagLabelParam.Enabled = true;
            }
        }

        public void nameColumns()
        {
            if (preset.searchedPattern != "")
            {
                previewTable.Columns[3].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTagId));
                previewTable.Columns[4].HeaderText = newValueText;

                previewTable.Columns[3].Visible = (previewTable.Columns[3].HeaderText != "");
                previewTable.Columns[4].Visible = (previewTable.Columns[3].HeaderText != "");

                if (preset.searchedPattern2 != "")
                {
                    previewTable.Columns[5].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag2Id));
                    previewTable.Columns[6].HeaderText = newValueText;

                    previewTable.Columns[5].Visible = (previewTable.Columns[5].HeaderText != "");
                    previewTable.Columns[6].Visible = (previewTable.Columns[5].HeaderText != "");

                    if (previewTable.Columns[5].HeaderText == previewTable.Columns[3].HeaderText)
                    {
                        previewTable.Columns[4].Visible = false;
                        previewTable.Columns[5].Visible = false;
                    }

                    if (preset.searchedPattern3 != "")
                    {
                        previewTable.Columns[7].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag3Id));
                        previewTable.Columns[8].HeaderText = newValueText;

                        previewTable.Columns[7].Visible = (previewTable.Columns[7].HeaderText != "");
                        previewTable.Columns[8].Visible = (previewTable.Columns[7].HeaderText != "");

                        if (previewTable.Columns[7].HeaderText == previewTable.Columns[5].HeaderText)
                        {
                            previewTable.Columns[6].Visible = false;
                            previewTable.Columns[7].Visible = false;
                        }

                        if (preset.searchedPattern4 != "")
                        {
                            previewTable.Columns[9].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag4Id));
                            previewTable.Columns[10].HeaderText = newValueText;

                            previewTable.Columns[9].Visible = (previewTable.Columns[9].HeaderText != "");
                            previewTable.Columns[10].Visible = (previewTable.Columns[9].HeaderText != "");

                            if (previewTable.Columns[9].HeaderText == previewTable.Columns[7].HeaderText)
                            {
                                previewTable.Columns[8].Visible = false;
                                previewTable.Columns[9].Visible = false;
                            }

                            if (preset.searchedPattern5 != "")
                            {
                                previewTable.Columns[11].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag5Id));
                                previewTable.Columns[12].HeaderText = newValueText;

                                previewTable.Columns[11].Visible = (previewTable.Columns[11].HeaderText != "");
                                previewTable.Columns[12].Visible = (previewTable.Columns[11].HeaderText != "");

                                if (previewTable.Columns[11].HeaderText == previewTable.Columns[9].HeaderText)
                                {
                                    previewTable.Columns[10].Visible = false;
                                    previewTable.Columns[11].Visible = false;
                                }
                            }
                            else
                            {
                                previewTable.Columns[11].HeaderText = "";
                                previewTable.Columns[12].HeaderText = "";

                                previewTable.Columns[11].Visible = false;
                                previewTable.Columns[12].Visible = false;
                            }
                        }
                        else
                        {
                            previewTable.Columns[9].HeaderText = "";
                            previewTable.Columns[10].HeaderText = "";
                            previewTable.Columns[11].HeaderText = "";
                            previewTable.Columns[12].HeaderText = "";

                            previewTable.Columns[9].Visible = false;
                            previewTable.Columns[10].Visible = false;
                            previewTable.Columns[11].Visible = false;
                            previewTable.Columns[12].Visible = false;
                        }
                    }
                    else
                    {
                        previewTable.Columns[7].HeaderText = "";
                        previewTable.Columns[8].HeaderText = "";
                        previewTable.Columns[9].HeaderText = "";
                        previewTable.Columns[10].HeaderText = "";
                        previewTable.Columns[11].HeaderText = "";
                        previewTable.Columns[12].HeaderText = "";

                        previewTable.Columns[7].Visible = false;
                        previewTable.Columns[8].Visible = false;
                        previewTable.Columns[9].Visible = false;
                        previewTable.Columns[10].Visible = false;
                        previewTable.Columns[11].Visible = false;
                        previewTable.Columns[12].Visible = false;
                    }
                }
                else
                {
                    previewTable.Columns[5].HeaderText = "";
                    previewTable.Columns[6].HeaderText = "";
                    previewTable.Columns[7].HeaderText = "";
                    previewTable.Columns[8].HeaderText = "";
                    previewTable.Columns[9].HeaderText = "";
                    previewTable.Columns[10].HeaderText = "";
                    previewTable.Columns[11].HeaderText = "";
                    previewTable.Columns[12].HeaderText = "";

                    previewTable.Columns[5].Visible = false;
                    previewTable.Columns[6].Visible = false;
                    previewTable.Columns[7].Visible = false;
                    previewTable.Columns[8].Visible = false;
                    previewTable.Columns[9].Visible = false;
                    previewTable.Columns[10].Visible = false;
                    previewTable.Columns[11].Visible = false;
                    previewTable.Columns[12].Visible = false;
                }
            }
            else
            {
                previewTable.Columns[3].HeaderText = "";
                previewTable.Columns[4].HeaderText = "";
                previewTable.Columns[5].HeaderText = "";
                previewTable.Columns[6].HeaderText = "";
                previewTable.Columns[7].HeaderText = "";
                previewTable.Columns[8].HeaderText = "";
                previewTable.Columns[9].HeaderText = "";
                previewTable.Columns[10].HeaderText = "";
                previewTable.Columns[11].HeaderText = "";
                previewTable.Columns[12].HeaderText = "";

                previewTable.Columns[3].Visible = false;
                previewTable.Columns[4].Visible = false;
                previewTable.Columns[5].Visible = false;
                previewTable.Columns[6].Visible = false;
                previewTable.Columns[7].Visible = false;
                previewTable.Columns[8].Visible = false;
                previewTable.Columns[9].Visible = false;
                previewTable.Columns[10].Visible = false;
                previewTable.Columns[11].Visible = false;
                previewTable.Columns[12].Visible = false;
            }
        }

        private void presetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
            {
                descriptionBox.Text = "";

                parameterTagList.Enabled = false;
                labelTag.Enabled = false;

                parameterTag2List.Enabled = false;
                labelTag2.Enabled = false;

                parameterTag3List.Enabled = false;
                labelTag3.Enabled = false;

                parameterTag4List.Enabled = false;
                labelTag4.Enabled = false;

                parameterTag5List.Enabled = false;
                labelTag5.Enabled = false;

                parameterTag6List.Enabled = false;
                labelTag6.Enabled = false;

                customTextBox.Enabled = false;
                customTextLabel.Enabled = false;

                customText2Box.Enabled = false;
                customText2Label.Enabled = false;

                customText3Box.Enabled = false;
                customText3Label.Enabled = false;

                customText4Box.Enabled = false;
                customText4Label.Enabled = false;

                conditionCheckBox.Checked = false;
                conditionCheckBox.Enabled = false;
            }
            else
            {
                Presets.TryGetValue(((Preset)presetList.SelectedItem).guid, out preset);

                descriptionBox.Text = AdvancedSearchAndReplacePlugin.GetDictValue(preset.descriptions, Plugin.Language);

                if (!preset.modifiedByUser && !TagToolsPlugin.developerMode)
                    editButtonEnabled = false;
                else
                    editButtonEnabled = true;

                buttonEdit.Enabled = editButtonEnabled;
                buttonSubmit.Enabled = editButtonEnabled;

                fillParameterTagList(preset.parameterTagType, parameterTagList, labelTag);
                parameterTagList.Text = getTagName(preset.parameterTagId);
                fillParameterTagList(preset.parameterTag2Type, parameterTag2List, labelTag2);
                parameterTag2List.Text = getTagName(preset.parameterTag2Id);
                fillParameterTagList(preset.parameterTag3Type, parameterTag3List, labelTag3);
                parameterTag3List.Text = getTagName(preset.parameterTag3Id);
                fillParameterTagList(preset.parameterTag4Type, parameterTag4List, labelTag4);
                parameterTag4List.Text = getTagName(preset.parameterTag4Id);
                fillParameterTagList(preset.parameterTag5Type, parameterTag5List, labelTag5);
                parameterTag5List.Text = getTagName(preset.parameterTag5Id);
                fillParameterTagList(preset.parameterTag6Type, parameterTag6List, labelTag6);
                parameterTag6List.Text = getTagName(preset.parameterTag6Id);

                customTextBox.Text = preset.customTextChecked ? preset.customText : "";
                customTextBox.Enabled = preset.customTextChecked;
                customTextLabel.Enabled = preset.customTextChecked;
                customText2Box.Text = preset.customText2Checked ? preset.customText2 : "";
                customText2Box.Enabled = preset.customText2Checked;
                customText2Label.Enabled = preset.customText2Checked;
                customText3Box.Text = preset.customText3Checked ? preset.customText3 : "";
                customText3Box.Enabled = preset.customText3Checked;
                customText3Label.Enabled = preset.customText3Checked;
                customText4Box.Text = preset.customText4Checked ? preset.customText4 : "";
                customText4Box.Enabled = preset.customText4Checked;
                customText4Label.Enabled = preset.customText4Checked;

                conditionCheckBox.Enabled = true;
                conditionCheckBox.Checked = preset.condition;

                bool playlistFound = false;
                foreach (Playlist tempPreset in playlistComboBox.Items)
                {
                    if (tempPreset.playlist == preset.playlist)
                    {
                        playlistComboBox.SelectedItem = tempPreset;
                        playlistFound = true;
                        break;
                    }
                }
                if(!playlistFound)
                    playlistComboBox.SelectedIndex = -1;



                //Lets deal with preview table
                nameColumns();
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
            {
                MessageBox.Show(TagToolsPlugin.msgNoPresetSelected);
                return;
            }

            DialogResult result = MessageBox.Show(TagToolsPlugin.msgSendingEmailConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            saveSettings();

            
            MailMessage message = new MailMessage();
            message.From = new MailAddress(Plugin.ASRPresetMailFromAddress);
            message.Subject = Plugin.ASRPresetMailSubject;
            message.To.Add(Plugin.ASRPresetMailRecipient);

            Attachment a = new Attachment(PresetsPath + @"\" + preset.guid + Plugin.ASRPresetExtension);
            message.Attachments.Add(a);

            SmtpClient smtp = new SmtpClient(Plugin.ASRPresetMailServerName, Plugin.ASRPresetMailServerPort);
            smtp.Send(message);
            a.Dispose();
        }


        public void import(bool importAll)
        {
            string newPresetsPath = Application.StartupPath + @"\Plugins\" + Plugin.ASRPresetsPath;
            string[] presetNames;
            int numberOfImportedPresets = 0;
            int numberOfErrors = 0;

            DialogResult result = MessageBox.Show(TagToolsPlugin.msgImportingConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            //saveSettings();


            if (!System.IO.Directory.Exists(PresetsPath))
                System.IO.Directory.CreateDirectory(PresetsPath);

            if (System.IO.Directory.Exists(newPresetsPath))
            {
                presetNames = System.IO.Directory.GetFiles(newPresetsPath, "*" + Plugin.ASRPresetExtension);
                foreach (string presetName in presetNames)
                {
                    System.IO.FileInfo sourcefileInfo = new System.IO.FileInfo(presetName);
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(PresetsPath + @"\" + sourcefileInfo.Name);

                    try
                    {
                        if (fileInfo.Exists)
                        {
                            if ((sourcefileInfo.LastWriteTimeUtc > fileInfo.LastWriteTimeUtc && sourcefileInfo.LastWriteTimeUtc > Plugin.SavedSettings.lastImportDateUtc) || importAll)
                            {
                                System.IO.File.Delete(fileInfo.FullName);
                                System.IO.File.Copy(presetName, fileInfo.FullName);
                                numberOfImportedPresets++;
                            }
                        }
                        else
                        {
                            if (sourcefileInfo.LastWriteTimeUtc > Plugin.SavedSettings.lastImportDateUtc || importAll)
                            {
                                System.IO.File.Copy(presetName, fileInfo.FullName);
                                numberOfImportedPresets++;
                            }
                        }
                    }
                    catch
                    {
                        numberOfErrors++;
                    }
                }
            }

            if (numberOfImportedPresets > 0)
                Plugin.SavedSettings.lastImportDateUtc = DateTime.UtcNow;


            string message = "";

            if (numberOfImportedPresets == 0 && numberOfErrors == 0)
                message = TagToolsPlugin.msgNoPresetsImported;
            else
                message += numberOfImportedPresets + TagToolsPlugin.msgPresetsWereImported;

            if (numberOfErrors > 0)
                message += (char)10 + (char)10 + TagToolsPlugin.msgFailedToImport + numberOfErrors + TagToolsPlugin.msgPresets;


            Init(TagToolsPlugin);

            presetList.Items.Clear();
            presetList.Sorted = false;
            int i = 0;
            foreach (Preset tempPreset in Presets.Values)
            {
                presetList.Items.Add(tempPreset);
                presetList.SetItemChecked(i, tempPreset.autoApply);
                i++;
            }
            presetList.Sorted = true;

            MessageBox.Show(message);
        }

        public void deleteAll()
        {
            DateTime userPresetDateTime = new DateTime(2100, 01, 01);
            string[] presetNames;
            int numberOfDeletedPresets = 0;
            int numberOfErrors = 0;

            DialogResult result = MessageBox.Show(TagToolsPlugin.msgDeletingConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            saveSettings();


            if (!System.IO.Directory.Exists(PresetsPath))
                return;

            presetNames = System.IO.Directory.GetFiles(PresetsPath, "*" + Plugin.ASRPresetExtension);
            foreach (string presetName in presetNames)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(presetName);

                try
                {
                    if (fileInfo.LastWriteTimeUtc != userPresetDateTime)
                    {
                        System.IO.File.Delete(presetName);
                        numberOfDeletedPresets++;
                    }
                }
                catch
                {
                    numberOfErrors++;
                }
            }


            string message = "";

            if (numberOfDeletedPresets == 0 && numberOfErrors == 0)
                message = TagToolsPlugin.msgNoPresetsDeleted;
            else
                message += numberOfDeletedPresets + TagToolsPlugin.msgPresetsWereDeleted;

            if (numberOfErrors > 0)
                message += (char)10 + (char)10 + TagToolsPlugin.msgFailedToDelete + numberOfErrors + TagToolsPlugin.msgPresets;


            MessageBox.Show(message);

            Init(TagToolsPlugin);

            presetList.Items.Clear();
            presetList.Sorted = false;
            int i = 0;
            foreach (Preset tempPreset in Presets.Values)
            {
                presetList.Items.Add(tempPreset);
                presetList.SetItemChecked(i, tempPreset.autoApply);
                i++;
            }
            presetList.Sorted = true;
        }

        private void buttonImportNew_Click(object sender, EventArgs e)
        {
            import(false);
        }

        private void buttonImportAll_Click(object sender, EventArgs e)
        {
            import(true);
        }

        private void buttonDownloadNew_Click(object sender, EventArgs e)
        {

        }

        private void buttonDownloadAll_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            deleteAll();
        }

        private void parameterTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTagId = getTagId(parameterTagList.Text);
            nameColumns();
        }

        private void parameterTag2_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag2Id = getTagId(parameterTag2List.Text);
            nameColumns();
        }

        private void parameterTag3_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag3Id = getTagId(parameterTag3List.Text);
            nameColumns();
        }

        private void parameterTag4List_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag4Id = getTagId(parameterTag4List.Text);
            nameColumns();
        }

        private void parameterTag5List_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag5Id = getTagId(parameterTag5List.Text);
            nameColumns();
        }

        private void parameterTag6List_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag6Id = getTagId(parameterTag6List.Text);
            nameColumns();
        }

        private void customText_TextChanged(object sender, EventArgs e)
        {
            preset.customText = customTextBox.Text;
            nameColumns();
        }

        private void customText2Box_TextChanged(object sender, EventArgs e)
        {
            preset.customText2 = customText2Box.Text;
            nameColumns();
        }

        private void customText3Box_TextChanged(object sender, EventArgs e)
        {
            preset.customText3 = customText3Box.Text;
            nameColumns();
        }

        private void customText4Box_TextChanged(object sender, EventArgs e)
        {
            preset.customText4 = customText4Box.Text;
            nameColumns();
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            playlistComboBox.Enabled = conditionCheckBox.Checked;
            preset.condition = playlistComboBox.Enabled;
        }

        private void playlistComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playlistComboBox.SelectedIndex >= 0)
                preset.playlist = ((Playlist)(playlistComboBox.SelectedItem)).playlist;
            else
                preset.playlist = "";
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if ((string)row.Cells[0].Value == "")
                    continue;

                if (state)
                    row.Cells[0].Value = "False";
                else
                    row.Cells[0].Value = "True";

                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0, row.Index);
                previewList_CellContentClick(null, e);
            }
        }

        private void previewList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sourceTag1Value;
                string sourceTag2Value;
                string sourceTag3Value;
                string sourceTag4Value;
                string sourceTag5Value;
                string newTag1Value;
                string newTag2Value;
                string newTag3Value;
                string newTag4Value;
                string newTag5Value;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "False";

                    sourceTag1Value = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    sourceTag2Value = (string)previewTable.Rows[e.RowIndex].Cells[5].Value;
                    sourceTag3Value = (string)previewTable.Rows[e.RowIndex].Cells[7].Value;
                    sourceTag4Value = (string)previewTable.Rows[e.RowIndex].Cells[9].Value;
                    sourceTag5Value = (string)previewTable.Rows[e.RowIndex].Cells[11].Value;

                    previewTable.Rows[e.RowIndex].Cells[4].Value = sourceTag1Value;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = sourceTag2Value;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = sourceTag3Value;
                    previewTable.Rows[e.RowIndex].Cells[10].Value = sourceTag4Value;
                    previewTable.Rows[e.RowIndex].Cells[12].Value = sourceTag5Value;
                }
                else if (isChecked == "False")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "True";

                    sourceTag1Value = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    sourceTag2Value = (string)previewTable.Rows[e.RowIndex].Cells[5].Value;
                    sourceTag3Value = (string)previewTable.Rows[e.RowIndex].Cells[7].Value;
                    sourceTag4Value = (string)previewTable.Rows[e.RowIndex].Cells[9].Value;
                    sourceTag5Value = (string)previewTable.Rows[e.RowIndex].Cells[11].Value;

                    lock (Presets)
                    {
                        GetReplacedTags((string)previewTable.Rows[e.RowIndex].Cells[1].Value, TagToolsPlugin, this, preset);

                        newTag1Value = searchedAndReplacedTags.replacedTagValue;
                        newTag2Value = searchedAndReplacedTags.replacedTag2Value;
                        newTag3Value = searchedAndReplacedTags.replacedTag3Value;
                        newTag4Value = searchedAndReplacedTags.replacedTag4Value;
                        newTag5Value = searchedAndReplacedTags.replacedTag5Value;
                    }

                    previewTable.Rows[e.RowIndex].Cells[4].Value = newTag1Value;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = newTag2Value;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTag3Value;
                    previewTable.Rows[e.RowIndex].Cells[10].Value = newTag4Value;
                    previewTable.Rows[e.RowIndex].Cells[12].Value = newTag5Value;
                }
            }
        }

        private void previewTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                if ((string)previewTable.Rows[e.RowIndex].Cells[3].Value == (string)previewTable.Rows[e.RowIndex].Cells[4].Value)
                    previewTable.Rows[e.RowIndex].Cells[4].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[4].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 6)
            {
                if ((string)previewTable.Rows[e.RowIndex].Cells[5].Value == (string)previewTable.Rows[e.RowIndex].Cells[6].Value)
                    previewTable.Rows[e.RowIndex].Cells[6].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[6].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 8)
            {
                if ((string)previewTable.Rows[e.RowIndex].Cells[7].Value == (string)previewTable.Rows[e.RowIndex].Cells[8].Value)
                    previewTable.Rows[e.RowIndex].Cells[8].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[8].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 10)
            {
                if ((string)previewTable.Rows[e.RowIndex].Cells[9].Value == (string)previewTable.Rows[e.RowIndex].Cells[10].Value)
                    previewTable.Rows[e.RowIndex].Cells[10].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[10].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 12)
            {
                if ((string)previewTable.Rows[e.RowIndex].Cells[11].Value == (string)previewTable.Rows[e.RowIndex].Cells[12].Value)
                    previewTable.Rows[e.RowIndex].Cells[12].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[12].Style = Plugin.ChangedStyle;
            }
        }

        private void AdvancedSearchAndReplacePlugin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Init(TagToolsPlugin);
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            presetList.Enabled = enable;

            if (enable)
            {
                presetList_SelectedIndexChanged(null, null);
            }
            else
            {
                parameterTagList.Enabled = enable;
                parameterTag2List.Enabled = enable;
                parameterTag3List.Enabled = enable;
                parameterTag4List.Enabled = enable;
                parameterTag5List.Enabled = enable;
                parameterTag6List.Enabled = enable;

                customTextBox.Enabled = enable;
                customText2Box.Enabled = enable;
                customText3Box.Enabled = enable;
                customText4Box.Enabled = enable;
            }

            buttonSubmit.Enabled = enable && editButtonEnabled;
            buttonImportNew.Enabled = enable;
            buttonImportAll.Enabled = enable;
            buttonDownloadNew.Enabled = enable;
            buttonDownloadAll.Enabled = enable;

            buttonCreate.Enabled = enable;
            buttonCopy.Enabled = enable;
            buttonEdit.Enabled = enable && editButtonEnabled;
            buttonDelete.Enabled = enable;
            buttonDeleteAll.Enabled = enable;
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
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonPreview.Enabled = false;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            presetList.Items.Clear();
            presetList.Sorted = false;
            int i = 0;
            foreach (Preset tempPreset in Presets.Values)
            {
                if (Regex.IsMatch(tempPreset.ToString(), Regex.Escape(searchTextBox.Text), RegexOptions.IgnoreCase))
                {
                    presetList.Items.Add(tempPreset);
                    presetList.SetItemChecked(i, tempPreset.autoApply);
                    i++;
                }
            }
            presetList.Sorted = true;
        }

        private void clearSearchButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "";
            presetList.Items.Clear();

            presetList.Sorted = false;
            int i = 0;
            foreach (Preset tempPreset in Presets.Values)
            {
                presetList.Items.Add(tempPreset);
                presetList.SetItemChecked(i, tempPreset.autoApply);
                i++;
            }
            presetList.Sorted = true;
        }
    }
}
