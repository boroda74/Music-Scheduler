using System;
using System.Windows.Forms;
using System.Text;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public void fillTagNames()
        {
            ReadonlyTagsNames = new string[27];

            ReadonlyTagsNames[0] = GenreCategoryName;
            ReadonlyTagsNames[1] = SynchronisedLyricsName;
            ReadonlyTagsNames[2] = UnsynchronisedLyricsName;
            ReadonlyTagsNames[3] = DisplayedAlbumArtsistName;
            ReadonlyTagsNames[4] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.HasLyrics);
            ReadonlyTagsNames[5] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual1);
            ReadonlyTagsNames[6] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual2);
            ReadonlyTagsNames[7] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual3);
            ReadonlyTagsNames[8] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual4);
            ReadonlyTagsNames[9] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual5);
            ReadonlyTagsNames[10] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual6);
            ReadonlyTagsNames[11] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual7);
            ReadonlyTagsNames[12] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual8);
            ReadonlyTagsNames[13] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual9);

            ReadonlyTagsNames[14] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual10);
            ReadonlyTagsNames[15] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual11);
            ReadonlyTagsNames[16] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual12);
            ReadonlyTagsNames[17] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual13);
            ReadonlyTagsNames[18] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual14);
            ReadonlyTagsNames[19] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual15);
            ReadonlyTagsNames[20] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual16);

            ReadonlyTagsNames[21] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genres);
            ReadonlyTagsNames[22] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Artists);
            ReadonlyTagsNames[23] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithArtistRole);
            ReadonlyTagsNames[24] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithPerformerRole);
            ReadonlyTagsNames[25] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithGuestRole);
            ReadonlyTagsNames[26] = MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithRemixerRole);
            

            //Tags
            bool wereErrors = false;
            Encoding unicode1 = Encoding.UTF8;
            System.IO.FileStream stream1 = System.IO.File.Open(System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.TagNamesErrorLog.txt"), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file1 = new System.IO.StreamWriter(stream1, unicode1);

            TagNamesIds.Clear();
            TagIdsNames.Clear();

            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackTitle) + " / " + Plugin.MetaDataType.TrackTitle);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackTitle), Plugin.MetaDataType.TrackTitle);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackTitle) + " / " + Plugin.MetaDataType.TrackTitle);
            }
            try
            {
                file1.WriteLine("Adding " + AlbumTagName + " / " + Plugin.MetaDataType.Album);
                TagNamesIds.Add(AlbumTagName, Plugin.MetaDataType.Album);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + AlbumTagName + " / " + Plugin.MetaDataType.Album);
            }
            try
            {
                file1.WriteLine("Adding " + DisplayedAlbumArtsistName + " / " + Plugin.MetaDataType.AlbumArtist);
                TagNamesIds.Add(DisplayedAlbumArtsistName, Plugin.MetaDataType.AlbumArtist);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + DisplayedAlbumArtsistName + " / " + Plugin.MetaDataType.AlbumArtist);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.AlbumArtist) + " / " + Plugin.MetaDataType.AlbumArtistRaw);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.AlbumArtist), Plugin.MetaDataType.AlbumArtistRaw);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.AlbumArtist) + " / " + Plugin.MetaDataType.AlbumArtistRaw);
            }
            try
            {
                file1.WriteLine("Adding " + DisplayedArtistName + " / " + Plugin.DisplayedArtistId);
                TagNamesIds.Add(DisplayedArtistName, Plugin.DisplayedArtistId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + DisplayedArtistName + " / " + Plugin.DisplayedArtistId);
            }
            try
            {
                file1.WriteLine("Adding " + ArtistArtistsName + " / " + Plugin.ArtistArtistsId);
                TagNamesIds.Add(ArtistArtistsName, Plugin.ArtistArtistsId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + ArtistArtistsName + " / " + Plugin.ArtistArtistsId);
            }
            try
            {
                file1.WriteLine("Adding " + ArtworkName + " / " + Plugin.MetaDataType.Artwork);
                TagNamesIds.Add(ArtworkName, Plugin.MetaDataType.Artwork);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + ArtworkName + " / " + Plugin.MetaDataType.Artwork);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.BeatsPerMin) + " / " + Plugin.MetaDataType.BeatsPerMin);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.BeatsPerMin), Plugin.MetaDataType.BeatsPerMin);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.BeatsPerMin) + " / " + Plugin.MetaDataType.BeatsPerMin);
            }
            try
            {
                file1.WriteLine("Adding " + DisplayedComposerName + " / " + Plugin.DisplayedComposerId);
                TagNamesIds.Add(DisplayedComposerName, Plugin.DisplayedComposerId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + DisplayedComposerName + " / " + Plugin.DisplayedComposerId);
            }
            try
            {
                file1.WriteLine("Adding " + ComposerComposersName + " / " + Plugin.ComposerComposersId);
                TagNamesIds.Add(ComposerComposersName, Plugin.ComposerComposersId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + ComposerComposersName + " / " + Plugin.ComposerComposersId);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Comment) + " / " + Plugin.MetaDataType.Comment);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Comment), Plugin.MetaDataType.Comment);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Comment) + " / " + Plugin.MetaDataType.Comment);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Conductor) + " / " + Plugin.MetaDataType.Conductor);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Conductor), Plugin.MetaDataType.Conductor);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Conductor) + " / " + Plugin.MetaDataType.Conductor);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscNo) + " / " + Plugin.MetaDataType.DiscNo);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscNo), Plugin.MetaDataType.DiscNo);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscNo) + " / " + Plugin.MetaDataType.DiscNo);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscCount) + " / " + Plugin.MetaDataType.DiscCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscCount), Plugin.MetaDataType.DiscCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscCount) + " / " + Plugin.MetaDataType.DiscCount);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Encoder) + " / " + Plugin.MetaDataType.Encoder);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Encoder), Plugin.MetaDataType.Encoder);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Encoder) + " / " + Plugin.MetaDataType.Encoder);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genre) + " / " + Plugin.MetaDataType.Genre);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genre), Plugin.MetaDataType.Genre);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genre) + " / " + Plugin.MetaDataType.Genre);
            }
            try
            {
                file1.WriteLine("Adding " + GenreCategoryName + " / " + Plugin.MetaDataType.GenreCategory);
                TagNamesIds.Add(GenreCategoryName, Plugin.MetaDataType.GenreCategory);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + GenreCategoryName + " / " + Plugin.MetaDataType.GenreCategory);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Grouping) + " / " + Plugin.MetaDataType.Grouping);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Grouping), Plugin.MetaDataType.Grouping);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Grouping) + " / " + Plugin.MetaDataType.Grouping);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Keywords) + " / " + Plugin.MetaDataType.Keywords);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Keywords), Plugin.MetaDataType.Keywords);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Keywords) + " / " + Plugin.MetaDataType.Keywords);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Lyricist) + " / " + Plugin.MetaDataType.Lyricist);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Lyricist), Plugin.MetaDataType.Lyricist);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Lyricist) + " / " + Plugin.MetaDataType.Lyricist);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Mood) + " / " + Plugin.MetaDataType.Mood);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Mood), Plugin.MetaDataType.Mood);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Mood) + " / " + Plugin.MetaDataType.Mood);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Occasion) + " / " + Plugin.MetaDataType.Occasion);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Occasion), Plugin.MetaDataType.Occasion);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Occasion) + " / " + Plugin.MetaDataType.Occasion);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Origin) + " / " + Plugin.MetaDataType.Origin);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Origin), Plugin.MetaDataType.Origin);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Origin) + " / " + Plugin.MetaDataType.Origin);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Publisher) + " / " + Plugin.MetaDataType.Publisher);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Publisher), Plugin.MetaDataType.Publisher);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Publisher) + " / " + Plugin.MetaDataType.Publisher);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Quality) + " / " + Plugin.MetaDataType.Quality);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Quality), Plugin.MetaDataType.Quality);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Quality) + " / " + Plugin.MetaDataType.Quality);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Rating) + " / " + Plugin.MetaDataType.Rating);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Rating), Plugin.MetaDataType.Rating);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Rating) + " / " + Plugin.MetaDataType.Rating);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingAlbum) + " / " + Plugin.MetaDataType.RatingAlbum);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingAlbum), Plugin.MetaDataType.RatingAlbum);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingAlbum) + " / " + Plugin.MetaDataType.RatingAlbum);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingLove) + " / " + Plugin.MetaDataType.RatingLove);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingLove), Plugin.MetaDataType.RatingLove);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingLove) + " / " + Plugin.MetaDataType.RatingLove);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Tempo) + " / " + Plugin.MetaDataType.Tempo);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Tempo), Plugin.MetaDataType.Tempo);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Tempo) + " / " + Plugin.MetaDataType.Tempo);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackNo) + " / " + Plugin.MetaDataType.TrackNo);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackNo), Plugin.MetaDataType.TrackNo);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackNo) + " / " + Plugin.MetaDataType.TrackNo);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackCount) + " / " + Plugin.MetaDataType.TrackCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackCount), Plugin.MetaDataType.TrackCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackCount) + " / " + Plugin.MetaDataType.TrackCount);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Year) + " / " + Plugin.MetaDataType.Year);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Year), Plugin.MetaDataType.Year);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Year) + " / " + Plugin.MetaDataType.Year);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.HasLyrics) + " / " + Plugin.MetaDataType.HasLyrics);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.HasLyrics), Plugin.MetaDataType.HasLyrics);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.HasLyrics) + " / " + Plugin.MetaDataType.HasLyrics);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual1) + " / " + Plugin.MetaDataType.Virtual1);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual1), Plugin.MetaDataType.Virtual1);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual1) + " / " + Plugin.MetaDataType.Virtual1);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual2) + " / " + Plugin.MetaDataType.Virtual2);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual2), Plugin.MetaDataType.Virtual2);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual2) + " / " + Plugin.MetaDataType.Virtual2);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual3) + " / " + Plugin.MetaDataType.Virtual3);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual3), Plugin.MetaDataType.Virtual3);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual3) + " / " + Plugin.MetaDataType.Virtual3);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual4) + " / " + Plugin.MetaDataType.Virtual4);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual4), Plugin.MetaDataType.Virtual4);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual4) + " / " + Plugin.MetaDataType.Virtual4);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual5) + " / " + Plugin.MetaDataType.Virtual5);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual5), Plugin.MetaDataType.Virtual5);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual5) + " / " + Plugin.MetaDataType.Virtual5);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual6) + " / " + Plugin.MetaDataType.Virtual6);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual6), Plugin.MetaDataType.Virtual6);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual6) + " / " + Plugin.MetaDataType.Virtual6);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual7) + " / " + Plugin.MetaDataType.Virtual7);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual7), Plugin.MetaDataType.Virtual7);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual7) + " / " + Plugin.MetaDataType.Virtual7);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual8) + " / " + Plugin.MetaDataType.Virtual8);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual8), Plugin.MetaDataType.Virtual8);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual8) + " / " + Plugin.MetaDataType.Virtual8);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual9) + " / " + Plugin.MetaDataType.Virtual9);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual9), Plugin.MetaDataType.Virtual9);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual9) + " / " + Plugin.MetaDataType.Virtual9);
            }

            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual10) + " / " + Plugin.MetaDataType.Virtual10);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual10), Plugin.MetaDataType.Virtual10);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual10) + " / " + Plugin.MetaDataType.Virtual10);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual11) + " / " + Plugin.MetaDataType.Virtual11);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual11), Plugin.MetaDataType.Virtual11);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual11) + " / " + Plugin.MetaDataType.Virtual11);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual12) + " / " + Plugin.MetaDataType.Virtual12);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual12), Plugin.MetaDataType.Virtual12);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual12) + " / " + Plugin.MetaDataType.Virtual12);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual13) + " / " + Plugin.MetaDataType.Virtual13);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual13), Plugin.MetaDataType.Virtual13);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual13) + " / " + Plugin.MetaDataType.Virtual13);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual14) + " / " + Plugin.MetaDataType.Virtual14);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual14), Plugin.MetaDataType.Virtual14);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual14) + " / " + Plugin.MetaDataType.Virtual14);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual15) + " / " + Plugin.MetaDataType.Virtual15);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual15), Plugin.MetaDataType.Virtual15);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual15) + " / " + Plugin.MetaDataType.Virtual15);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual16) + " / " + Plugin.MetaDataType.Virtual16);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual16), Plugin.MetaDataType.Virtual16);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual16) + " / " + Plugin.MetaDataType.Virtual16);
            }


            try
            {
                file1.WriteLine("Adding " + LyricsName + " / " + Plugin.LyricsId);
                TagNamesIds.Add(LyricsName, Plugin.LyricsId);
            }
            catch (ArgumentException)
            {
                file1.WriteLine("Cant add " + LyricsName + " / " + Plugin.LyricsId);

                try
                {
                    file1.WriteLine("Retry: adding " + LyricsName + LyricsNamePostfix + " / " + Plugin.LyricsId);
                    TagNamesIds.Add(LyricsName + LyricsNamePostfix, Plugin.LyricsId);
                }
                catch (ArgumentException)
                {
                    wereErrors = true;
                    file1.WriteLine("Retry: cant add " + LyricsName + LyricsNamePostfix + " / " + Plugin.LyricsId);
                }
            }
            try
            {
                file1.WriteLine("Adding " + SynchronisedLyricsName + " / " + Plugin.SynchronisedLyricsId);
                TagNamesIds.Add(SynchronisedLyricsName, Plugin.SynchronisedLyricsId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + SynchronisedLyricsName + " / " + Plugin.SynchronisedLyricsId);
            }
            try
            {
                file1.WriteLine("Adding " + UnsynchronisedLyricsName + " / " + Plugin.UnsynchronisedLyricsId);
                TagNamesIds.Add(UnsynchronisedLyricsName, Plugin.UnsynchronisedLyricsId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + UnsynchronisedLyricsName + " / " + Plugin.UnsynchronisedLyricsId);
            }
            try
            {
                file1.WriteLine("Adding " + NullTagName + " / " + Plugin.NullTagId);
                TagNamesIds.Add(NullTagName, Plugin.NullTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + NullTagName + " / " + Plugin.NullTagId);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom1) + " / " + Plugin.MetaDataType.Custom1);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom1), Plugin.MetaDataType.Custom1);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom1) + " / " + Plugin.MetaDataType.Custom1);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom2) + " / " + Plugin.MetaDataType.Custom2);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom2), Plugin.MetaDataType.Custom2);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom2) + " / " + Plugin.MetaDataType.Custom2);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom3) + " / " + Plugin.MetaDataType.Custom3);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom3), Plugin.MetaDataType.Custom3);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom3) + " / " + Plugin.MetaDataType.Custom3);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom4) + " / " + Plugin.MetaDataType.Custom4);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom4), Plugin.MetaDataType.Custom4);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom4) + " / " + Plugin.MetaDataType.Custom4);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom5) + " / " + Plugin.MetaDataType.Custom5);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom5), Plugin.MetaDataType.Custom5);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom5) + " / " + Plugin.MetaDataType.Custom5);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom6) + " / " + Plugin.MetaDataType.Custom6);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom6), Plugin.MetaDataType.Custom6);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom6) + " / " + Plugin.MetaDataType.Custom6);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom7) + " / " + Plugin.MetaDataType.Custom7);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom7), Plugin.MetaDataType.Custom7);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom7) + " / " + Plugin.MetaDataType.Custom7);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom8) + " / " + Plugin.MetaDataType.Custom8);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom8), Plugin.MetaDataType.Custom8);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom8) + " / " + Plugin.MetaDataType.Custom8);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom9) + " / " + Plugin.MetaDataType.Custom9);
                TagNamesIds.Add(Custom9TagName, Plugin.MetaDataType.Custom9);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom9) + " / " + Plugin.MetaDataType.Custom9);
            }

            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom10) + " / " + Plugin.MetaDataType.Custom10);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom10), Plugin.MetaDataType.Custom10);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom10) + " / " + Plugin.MetaDataType.Custom10);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom11) + " / " + Plugin.MetaDataType.Custom11);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom11), Plugin.MetaDataType.Custom11);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom11) + " / " + Plugin.MetaDataType.Custom11);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom12) + " / " + Plugin.MetaDataType.Custom12);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom12), Plugin.MetaDataType.Custom12);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom12) + " / " + Plugin.MetaDataType.Custom12);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom13) + " / " + Plugin.MetaDataType.Custom13);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom13), Plugin.MetaDataType.Custom13);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom13) + " / " + Plugin.MetaDataType.Custom13);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom14) + " / " + Plugin.MetaDataType.Custom14);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom14), Plugin.MetaDataType.Custom14);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom14) + " / " + Plugin.MetaDataType.Custom14);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom15) + " / " + Plugin.MetaDataType.Custom15);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom15), Plugin.MetaDataType.Custom15);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom15) + " / " + Plugin.MetaDataType.Custom15);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom16) + " / " + Plugin.MetaDataType.Custom16);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom16), Plugin.MetaDataType.Custom16);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom16) + " / " + Plugin.MetaDataType.Custom16);
            }


            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genres) + " / " + Plugin.MetaDataType.Genres);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genres), Plugin.MetaDataType.Genres);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genres) + " / " + Plugin.MetaDataType.Genres);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Artists) + " / " + Plugin.MetaDataType.Artists);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Artists), Plugin.MetaDataType.Artists);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Artists) + " / " + Plugin.MetaDataType.Artists);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithArtistRole) + " / " + Plugin.MetaDataType.ArtistsWithArtistRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithArtistRole), Plugin.MetaDataType.ArtistsWithArtistRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithArtistRole) + " / " + Plugin.MetaDataType.ArtistsWithArtistRole);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithPerformerRole) + " / " + Plugin.MetaDataType.ArtistsWithPerformerRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithPerformerRole), Plugin.MetaDataType.ArtistsWithPerformerRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithPerformerRole) + " / " + Plugin.MetaDataType.ArtistsWithPerformerRole);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithGuestRole) + " / " + Plugin.MetaDataType.ArtistsWithGuestRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithGuestRole), Plugin.MetaDataType.ArtistsWithGuestRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithGuestRole) + " / " + Plugin.MetaDataType.ArtistsWithGuestRole);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithRemixerRole) + " / " + Plugin.MetaDataType.ArtistsWithRemixerRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithRemixerRole), Plugin.MetaDataType.ArtistsWithRemixerRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithRemixerRole) + " / " + Plugin.MetaDataType.ArtistsWithRemixerRole);
            }

            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.LastPlayed) + " / " + Plugin.FilePropertyType.LastPlayed);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.LastPlayed), (Plugin.MetaDataType)Plugin.FilePropertyType.LastPlayed);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.LastPlayed) + " / " + Plugin.FilePropertyType.LastPlayed);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.PlayCount) + " / " + Plugin.FilePropertyType.PlayCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.PlayCount), (Plugin.MetaDataType)Plugin.FilePropertyType.PlayCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.PlayCount) + " / " + Plugin.FilePropertyType.PlayCount);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SkipCount) + " / " + Plugin.FilePropertyType.SkipCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SkipCount), (Plugin.MetaDataType)Plugin.FilePropertyType.SkipCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SkipCount) + " / " + Plugin.FilePropertyType.SkipCount);
            }

            if (wereErrors)
            {
                MessageBox.Show("Some tag names are duplicated. See '" + System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.TagNamesErrorLog.txt") + "' file for details. Plugin is not properly initialized.");
            }

            file1.Close();


            TagIdsNames.Add(Plugin.MetaDataType.TrackTitle, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackTitle));
            TagIdsNames.Add(Plugin.MetaDataType.Album, AlbumTagName);
            TagIdsNames.Add(Plugin.MetaDataType.AlbumArtist, DisplayedAlbumArtsistName);
            TagIdsNames.Add(Plugin.MetaDataType.AlbumArtistRaw, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.AlbumArtist));
            TagIdsNames.Add(Plugin.DisplayedArtistId, DisplayedArtistName);
            TagIdsNames.Add(Plugin.ArtistArtistsId, ArtistArtistsName);
            TagIdsNames.Add(Plugin.MetaDataType.Artwork, ArtworkName);
            TagIdsNames.Add(Plugin.MetaDataType.BeatsPerMin, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.BeatsPerMin));
            TagIdsNames.Add(Plugin.DisplayedComposerId, DisplayedComposerName);
            TagIdsNames.Add(Plugin.ComposerComposersId, ComposerComposersName);
            TagIdsNames.Add(Plugin.MetaDataType.Comment, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Comment));
            TagIdsNames.Add(Plugin.MetaDataType.Conductor, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Conductor));
            TagIdsNames.Add(Plugin.MetaDataType.DiscNo, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscNo));
            TagIdsNames.Add(Plugin.MetaDataType.DiscCount, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.DiscCount));
            TagIdsNames.Add(Plugin.MetaDataType.Encoder, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Encoder));
            TagIdsNames.Add(Plugin.MetaDataType.Genre, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genre));
            TagIdsNames.Add(Plugin.MetaDataType.GenreCategory, GenreCategoryName);
            TagIdsNames.Add(Plugin.MetaDataType.Grouping, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Grouping));
            TagIdsNames.Add(Plugin.MetaDataType.Keywords, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Keywords));
            TagIdsNames.Add(Plugin.MetaDataType.Lyricist, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Lyricist));
            TagIdsNames.Add(Plugin.MetaDataType.Mood, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Mood));
            TagIdsNames.Add(Plugin.MetaDataType.Occasion, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Occasion));
            TagIdsNames.Add(Plugin.MetaDataType.Origin, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Origin));
            TagIdsNames.Add(Plugin.MetaDataType.Publisher, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Publisher));
            TagIdsNames.Add(Plugin.MetaDataType.Quality, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Quality));
            TagIdsNames.Add(Plugin.MetaDataType.Rating, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Rating));
            TagIdsNames.Add(Plugin.MetaDataType.RatingAlbum, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingAlbum));
            TagIdsNames.Add(Plugin.MetaDataType.RatingLove, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingLove));
            TagIdsNames.Add(Plugin.MetaDataType.Tempo, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Tempo));
            TagIdsNames.Add(Plugin.MetaDataType.TrackNo, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackNo));
            TagIdsNames.Add(Plugin.MetaDataType.TrackCount, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackCount));
            TagIdsNames.Add(Plugin.MetaDataType.Year, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Year));
            TagIdsNames.Add(Plugin.MetaDataType.HasLyrics, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.HasLyrics));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual1, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual1));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual2, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual2));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual3, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual3));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual4, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual4));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual5, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual5));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual6, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual6));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual7, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual7));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual8, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual8));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual9, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual9));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual10, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual10));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual11, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual11));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual12, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual12));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual13, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual13));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual14, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual14));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual15, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual15));
            TagIdsNames.Add(Plugin.MetaDataType.Virtual16, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Virtual16));

            TagIdsNames.Add(Plugin.LyricsId, LyricsName);
            TagIdsNames.Add(Plugin.SynchronisedLyricsId, SynchronisedLyricsName);
            TagIdsNames.Add(Plugin.UnsynchronisedLyricsId, UnsynchronisedLyricsName);

            TagIdsNames.Add(Plugin.NullTagId, NullTagName);

            TagIdsNames.Add(Plugin.MetaDataType.Custom1, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom1));
            TagIdsNames.Add(Plugin.MetaDataType.Custom2, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom2));
            TagIdsNames.Add(Plugin.MetaDataType.Custom3, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom3));
            TagIdsNames.Add(Plugin.MetaDataType.Custom4, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom4));
            TagIdsNames.Add(Plugin.MetaDataType.Custom5, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom5));
            TagIdsNames.Add(Plugin.MetaDataType.Custom6, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom6));
            TagIdsNames.Add(Plugin.MetaDataType.Custom7, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom7));
            TagIdsNames.Add(Plugin.MetaDataType.Custom8, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom8));
            TagIdsNames.Add(Plugin.MetaDataType.Custom9, Custom9TagName);
            TagIdsNames.Add(Plugin.MetaDataType.Custom10, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom10));
            TagIdsNames.Add(Plugin.MetaDataType.Custom11, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom11));
            TagIdsNames.Add(Plugin.MetaDataType.Custom12, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom12));
            TagIdsNames.Add(Plugin.MetaDataType.Custom13, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom13));
            TagIdsNames.Add(Plugin.MetaDataType.Custom14, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom14));
            TagIdsNames.Add(Plugin.MetaDataType.Custom15, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom15));
            TagIdsNames.Add(Plugin.MetaDataType.Custom16, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom16));


            TagIdsNames.Add(Plugin.MetaDataType.Genres, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Genres));
            TagIdsNames.Add(Plugin.MetaDataType.Artists, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Artists));
            TagIdsNames.Add(Plugin.MetaDataType.ArtistsWithArtistRole, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithArtistRole));
            TagIdsNames.Add(Plugin.MetaDataType.ArtistsWithPerformerRole, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithPerformerRole));
            TagIdsNames.Add(Plugin.MetaDataType.ArtistsWithGuestRole, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithGuestRole));
            TagIdsNames.Add(Plugin.MetaDataType.ArtistsWithRemixerRole, MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.ArtistsWithRemixerRole));

            TagIdsNames.Add((Plugin.MetaDataType)Plugin.FilePropertyType.LastPlayed, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.LastPlayed));
            TagIdsNames.Add((Plugin.MetaDataType)Plugin.FilePropertyType.PlayCount, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.PlayCount));
            TagIdsNames.Add((Plugin.MetaDataType)Plugin.FilePropertyType.SkipCount, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SkipCount));


            wereErrors = false;
            Encoding unicode2 = Encoding.UTF8;
            System.IO.FileStream stream2 = System.IO.File.Open(System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.PropNamesErrorLog.txt"), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(stream2, unicode2);

            PropNamesIds.Clear();
            PropIdsNames.Clear();

            try
            {
                file2.WriteLine("Adding " + UrlTagName + " / " + Plugin.FilePropertyType.Url);
                PropNamesIds.Add(UrlTagName, Plugin.FilePropertyType.Url);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + UrlTagName + " / " + Plugin.FilePropertyType.Url);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Kind) + " / " + Plugin.FilePropertyType.Kind);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Kind), Plugin.FilePropertyType.Kind);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Kind) + " / " + Plugin.FilePropertyType.Kind);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Format) + " / " + Plugin.FilePropertyType.Format);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Format), Plugin.FilePropertyType.Format);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Format) + " / " + Plugin.FilePropertyType.Format);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Size) + " / " + Plugin.FilePropertyType.Size);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Size), Plugin.FilePropertyType.Size);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Size) + " / " + Plugin.FilePropertyType.Size);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Channels) + " / " + Plugin.FilePropertyType.Channels);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Channels), Plugin.FilePropertyType.Channels);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Channels) + " / " + Plugin.FilePropertyType.Channels);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SampleRate) + " / " + Plugin.FilePropertyType.SampleRate);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SampleRate), Plugin.FilePropertyType.SampleRate);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SampleRate) + " / " + Plugin.FilePropertyType.SampleRate);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Bitrate) + " / " + Plugin.FilePropertyType.Bitrate);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Bitrate), Plugin.FilePropertyType.Bitrate);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Bitrate) + " / " + Plugin.FilePropertyType.Bitrate);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateModified) + " / " + Plugin.FilePropertyType.DateModified);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateModified), Plugin.FilePropertyType.DateModified);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateModified) + " / " + Plugin.FilePropertyType.DateModified);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateAdded) + " / " + Plugin.FilePropertyType.DateAdded);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateAdded), Plugin.FilePropertyType.DateAdded);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateAdded) + " / " + Plugin.FilePropertyType.DateAdded);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Duration) + " / " + Plugin.FilePropertyType.Duration);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Duration), Plugin.FilePropertyType.Duration);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Duration) + " / " + Plugin.FilePropertyType.Duration);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainTrack) + " / " + Plugin.FilePropertyType.ReplayGainTrack);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainTrack), Plugin.FilePropertyType.ReplayGainTrack);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainTrack) + " / " + Plugin.FilePropertyType.ReplayGainTrack);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainAlbum) + " / " + Plugin.FilePropertyType.ReplayGainAlbum);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainAlbum), Plugin.FilePropertyType.ReplayGainAlbum);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainAlbum) + " / " + Plugin.FilePropertyType.ReplayGainAlbum);
            }

            if (wereErrors)
            {
                MessageBox.Show("Some track property names are duplicated. See '" + System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.PropNamesErrorLog.txt") + "' file for details. Plugin is not properly initialized.");
            }

            file2.Close();


            PropIdsNames.Add(Plugin.FilePropertyType.Url, UrlTagName);
            PropIdsNames.Add(Plugin.FilePropertyType.Kind, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Kind));
            PropIdsNames.Add(Plugin.FilePropertyType.Format, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Format));
            PropIdsNames.Add(Plugin.FilePropertyType.Size, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Size));
            PropIdsNames.Add(Plugin.FilePropertyType.Channels, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Channels));
            PropIdsNames.Add(Plugin.FilePropertyType.SampleRate, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.SampleRate));
            PropIdsNames.Add(Plugin.FilePropertyType.Bitrate, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Bitrate));
            PropIdsNames.Add(Plugin.FilePropertyType.DateModified, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateModified));
            PropIdsNames.Add(Plugin.FilePropertyType.DateAdded, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.DateAdded));
            PropIdsNames.Add(Plugin.FilePropertyType.Duration, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Duration));
            PropIdsNames.Add(Plugin.FilePropertyType.ReplayGainTrack, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainTrack));
            PropIdsNames.Add(Plugin.FilePropertyType.ReplayGainAlbum, MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.ReplayGainAlbum));
        }
    }
}
