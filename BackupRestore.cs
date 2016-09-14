using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public static string GetDefaultBackupFilename(string prefix)
        {
            DateTime now = DateTime.Now;
            return prefix + now.Year.ToString("D4") + "-" + now.Month.ToString("D2") + "-" + now.Day.ToString("D2") + " " + now.Hour.ToString("D2") + "." + now.Minute.ToString("D2") + "." + now.Second.ToString("D2");
        }

        public static string GetBackupFilenameWithoutExtension(string fullFilename)
        {
            return fullFilename.Substring(0, fullFilename.Length - 4);
        }
    }

    public class BackupDateType
    {
        public DateTime creationDate;
        public string guid;

        public BackupDateType()
        {
            creationDate = DateTime.UtcNow;
            guid = Guid.NewGuid().ToString();
        }

        public BackupDateType(BackupDateType source)
        {
            creationDate = source.creationDate;
            guid = source.guid;
        }

        public virtual void save(string fileName)
        {
            System.IO.Stream stream = System.IO.File.Open(fileName + ".mbd", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupDateType));

            serializer.Serialize(file, this);

            file.Close();
        }

        public virtual void load(string fileName)
        {
            System.IO.FileStream stream = System.IO.File.Open(fileName + ".mbd", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, Encoding.UTF8);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupDateType));

            BackupDateType backupDate;
            try
            {
                backupDate = (BackupDateType)serializer.Deserialize(file);
            }
            catch
            {
                MessageBox.Show("Backup '" + fileName + "' is corrupted or is not valid MusicBee backup!");

                file.Close();
                return;
            }

            file.Close();

            
            creationDate = backupDate.creationDate;
            guid = backupDate.guid;
        }
    }

    public class BackupType : BackupDateType
    {
        public int xSize;
        public int ySize;
        public string[] values;

        public BackupType()
        {
            return;
        }

        public BackupType(int xSizeParam, int ySizeParam)
        {
            guid = Guid.NewGuid().ToString();
            xSize = xSizeParam;
            ySize = ySizeParam;
            values = new string[xSizeParam * ySizeParam];
        }

        public override void save(string fileName)
        {
            System.IO.Stream stream = System.IO.File.Open(fileName + ".xml", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupType));

            serializer.Serialize(file, this);

            file.Close();
        }

        public override void load(string fileName)
        {
            System.IO.FileStream stream = System.IO.File.Open(fileName + ".xml", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, Encoding.UTF8);

            System.Xml.Serialization.XmlSerializer backupSerializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupType));

            BackupType backup;
            try
            {
                backup = (BackupType)backupSerializer.Deserialize(file);
            }
            catch
            {
                MessageBox.Show("Backup '" + fileName + "' is corrupted or is not valid MusicBee backup!");

                file.Close();
                return;
            }

            file.Close();

            creationDate = backup.creationDate;
            guid = backup.guid;
            xSize = backup.xSize;
            ySize = backup.ySize;
            values = backup.values;
        }

        public string getValue(int xParam, int yParam)
        {
            string value = values[xParam * ySize + yParam];
            if (value != null)
            {
                value = Regex.Replace(value, "\ufffa", "\u0000");
                value = Regex.Replace(value, "\ufffb", "\u0001");
                value = Regex.Replace(value, "\ufffc", "\u0002");
                value = Regex.Replace(value, "\ufffd", "\u0003");
            }

            return "" + value;
        }

        public string findValue(string trackId, Plugin.MetaDataType tagId)
        {
            for (int i = 0; i < xSize; i++)
            {
                if (getValue(i, 0) == trackId)
                {
                    for (int j = 1; j < ySize; j += 2)
                    {
                        if (getValue(i, j) == ((int)tagId).ToString())
                        {
                            return getValue(i, j + 1);
                        }
                    }
                }
            }

            return null;
        }

        public void setValue(string valueParam, int xParam, int yParam)
        {
            if (valueParam != null)
            {
                valueParam = Regex.Replace(valueParam, "\u0000", "\ufffa");
                valueParam = Regex.Replace(valueParam, "\u0001", "\ufffb");
                valueParam = Regex.Replace(valueParam, "\u0002", "\ufffc");
                valueParam = Regex.Replace(valueParam, "\u0003", "\ufffd");
            }

            values[xParam * ySize + yParam] = valueParam;
        }

        public int getBound(int i)
        {
            if (i == 0)
                return xSize;
            else
                return ySize;
        }
    }

    public class BackupIndexType
    {
        public int numberOfTracks;
        public List<List<string>> backups;

        public BackupIndexType()
        {
            numberOfTracks = 0;
            backups = new List<List<string>>();
        }

        public BackupIndexType(BackupIndexDictionary dictionary)
        {
            backups = new List<List<string>>();
            numberOfTracks = dictionary.Count;

            if (dictionary.Count == 0)
                return;

            foreach (var trackBackupsWithIds in dictionary)
            {
                List<string> trackBackupsList = new List<string>();

                bool trackIdIsNotAdded = true;
                foreach (var trackBackups in trackBackupsWithIds.Value)
                {
                    if (trackIdIsNotAdded)
                    {
                        trackBackupsList.Add(trackBackupsWithIds.Key);
                        trackIdIsNotAdded = false;
                    }

                    trackBackupsList.Add(trackBackups.Key);
                }

                backups.Add(trackBackupsList);
            }
        }

        public BackupIndexDictionary copyToBackupIndexDictionary()
        {
            BackupIndexDictionary dictionary = new BackupIndexDictionary();

            foreach (var trackBackups in backups)
            {
                SortedDictionary<string, bool> trackBackupIndex = new SortedDictionary<string, bool>();

                string trackId = null;
                foreach (var trackBackup in trackBackups)
                {
                    if (trackId == null)
                    {
                        trackId = trackBackup;
                    }
                    else
                    {
                        trackBackupIndex.Add(trackBackup, true);
                    }
                }

                dictionary.Add(trackId, trackBackupIndex);
            }

            return dictionary;
        }
    }

    public class BackupIndexDictionary : SortedDictionary<string, SortedDictionary<string, bool>>
    {
        public void saveBackup(string backupName)
        {
            if (Plugin.MbApiInterface.Library_QueryFiles("domain=Library"))
            {
                string currentFile;
                string trackId;

                string[] files = Plugin.MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);


                List<string> tagNames = new List<string>();
                Plugin.FillList(tagNames, false, true, false);

                List<Plugin.MetaDataType> tagIds = new List<Plugin.MetaDataType>();
                for (int i = 0; i < tagNames.Count; i++)
                    tagIds.Add(Plugin.GetTagId(tagNames[i]));

                BackupType backup = new BackupType(files.Length, tagIds.Count * 2 + 1);

                for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
                {
                    currentFile = files[fileCounter];
                    trackId = ""; //******

                    backup.setValue(trackId, fileCounter, 0);
                    for (int i = 0; i < tagIds.Count; i++)
                    {
                        backup.setValue(((int)tagIds[i]).ToString(), fileCounter, 2 * i + 1);
                        backup.setValue(Plugin.MbApiInterface.Library_GetFileTag(currentFile, tagIds[i]), fileCounter, 2 * i + 2);
                    }

                    SortedDictionary<string, bool> trackBackups;
                    if (!TryGetValue(trackId, out trackBackups))
                    {
                        trackBackups = new SortedDictionary<string, bool>();
                        Add(trackId, trackBackups);
                    }

                    bool placeholder;
                    if (!trackBackups.TryGetValue(backup.guid, out placeholder))
                        trackBackups.Add(backup.guid, true);
                }

                backup.save(backupName);

                BackupDateType backupDate = new BackupDateType(backup);
                backupDate.save(backupName);
            }


            BackupIndexType backupIndex = new BackupIndexType(this);

            System.IO.FileStream stream = System.IO.File.Open(Plugin.SavedSettings.autobackupDirectory + @"\" + Plugin.BackupIndexFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

            System.Xml.Serialization.XmlSerializer backupIndexSerializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupIndexType));

            backupIndexSerializer.Serialize(file, backupIndex);

            file.Close();
        }

        public void loadBackup(string backupName, bool restoreForEntireLibrary)
        {
            string query;

            if (restoreForEntireLibrary)
                query = "domain=Library";
            else
                query = "domain=SelectedFiles";


            BackupType backup = new BackupType();
            backup.load(backupName);


            if (Plugin.MbApiInterface.Library_QueryFiles(query))
            {
                string currentFile;
                string trackId;

                string[] files = Plugin.MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);


                List<string> tagNames = new List<string>();
                Plugin.FillList(tagNames, false, true, false);

                List<Plugin.MetaDataType> tagIds = new List<Plugin.MetaDataType>();
                for (int i = 0; i < tagNames.Count; i++)
                    tagIds.Add(Plugin.GetTagId(tagNames[i]));

                for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
                {
                    currentFile = files[fileCounter];
                    trackId = ""; //****

                    bool tagsWereSet = false;
                    for (int i = 0; i < tagIds.Count; i++)
                    {
                        string tagValue = backup.findValue(trackId, tagIds[i]);

                        if (tagValue != null)
                        {
                            Plugin.MbApiInterface.Library_SetFileTag(currentFile, tagIds[i], tagValue);
                            tagsWereSet = true;
                        }
                    }

                    if (tagsWereSet)
                        Plugin.MbApiInterface.Library_CommitTagsToFile(currentFile);
                }
            }
        }

        public void deleteBackup(BackupDateType backup)
        {
            string backupGuid = null;
            KeyValuePair<string, SortedDictionary<string, bool>> foundTrackBackups = new KeyValuePair<string, SortedDictionary<string, bool>>();

            foreach (var trackBackups in this)
            {
                bool placeholder;
                if (trackBackups.Value.TryGetValue(backup.guid, out placeholder))
                {
                    backupGuid = backup.guid;
                    foundTrackBackups = trackBackups;
                    break;
                }
            }

            if (backupGuid != null)
            {
                foundTrackBackups.Value.Remove(backupGuid);

                if (foundTrackBackups.Value.Count == 0)
                    Remove(foundTrackBackups.Key);
            }


            BackupIndexType backupIndex = new BackupIndexType(this);

            System.IO.FileStream stream = System.IO.File.Open(Plugin.BackupIndexFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

            System.Xml.Serialization.XmlSerializer backupIndexSerializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupIndexType));

            backupIndexSerializer.Serialize(file, backupIndex);

            file.Close();
        }
    }
}
