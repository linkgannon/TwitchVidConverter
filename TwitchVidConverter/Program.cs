using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Twitch.Net;
using Twitch.Net.Models;

namespace TwitchVidConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var convertVids = ConvertVideosAsync();
                convertVids.Wait();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static async Task ConvertVideosAsync()
        {
            List<string> vidIds = new List<string>();
            List<string> mp4Files = Directory.GetFiles(Directory.GetCurrentDirectory()).Where(f => Path.GetExtension(f) == ".mp4").ToList();
            foreach (var file in mp4Files)
                    vidIds.Add(Path.GetFileName(file.Split('-')[0]));

            // The first paramter should be substituted with your client id, the 2nd should be your app secret
            var twitchApi = new TwitchApiBuilder("xxxxx").WithClientSecret("xxxxx").Build();
            var result = await twitchApi.Videos.GetVideos(vidIds.ToArray());

            foreach (HelixVideo vid in result.Data)
            {
                string[] vidFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
                if (vidFiles.Where(v => v.Contains(vid.Id)).Count() > 0)
                {
                    string vidFileName = vidFiles.Where(v => v.Contains(vid.Id)).First();
                    File.Move(vidFileName, vid.Title + Path.GetExtension(vidFileName));
                }
            }
        }
    }
}