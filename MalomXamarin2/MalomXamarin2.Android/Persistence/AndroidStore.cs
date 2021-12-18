using MalomXamarin2.Persistence;
using Xamarin.Forms;
using MalomXamarin2.Droid.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[assembly: Dependency(typeof(AndroidStore))]
namespace MalomXamarin2.Droid.Persistence
{
    public class AndroidStore : IStore
    {
        public IEnumerable<string> GetFiles()
        {
            return Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).Select(file => Path.GetFileName(file));
        }

        public DateTime GetModifiedTime(string name)
        {
            FileInfo info = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name));

            return info.LastWriteTime;
        }
    }
}