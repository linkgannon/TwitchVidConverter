using System;
using System.Collections.Generic;
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
                var tst = getVideosAsync();
                tst.Wait();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static async Task getVideosAsync()
        {
            List<string> vidIds = new List<string>();
            string[] files = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory());
            foreach (var file in files)
                if (!file.EndsWith(".exe") && !file.EndsWith(".xml") && !file.EndsWith(".dll") && !file.EndsWith(".pdb") && !file.EndsWith(".config"))
                    vidIds.Add(System.IO.Path.GetFileName(file.Split('-')[0]));

            var twitchApi = new TwitchApiBuilder("xxxxx").WithClientSecret("xxxxx").Build();
            var result = await twitchApi.Videos.GetVideos(vidIds.ToArray());
            HelixVideo[] videoData = result.Data;
            foreach (HelixVideo vid in result.Data)
            {
                string[] vidFiles = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory());
                if (vidFiles.Where(v => v.Contains(vid.Id)).Count() > 0)
                {
                    string vidFileName = vidFiles.Where(v => v.Contains(vid.Id)).First();
                    System.IO.File.Move(vidFileName, vid.Title + System.IO.Path.GetExtension(vidFileName));
                }
            }
        }
    }
}