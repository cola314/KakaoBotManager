using KakaoManagerBeta.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace KakaoManagerBeta.Models
{
    public static class Globals
    {
        #region Data
        public static string Session { get; set; } = Guid.NewGuid().ToString();

        public static List<string> Domains { get; set; } = new List<string>();
        #endregion

        #region File Backup
        readonly static string BACKUP_FILE = Path.Join(Directory.GetCurrentDirectory(), "/data", "backup.dat");

        static Globals()
        {
            Load();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Save()
        {
            try
            {
                var data = new BackupData(Session, Domains);
                File.WriteAllText(BACKUP_FILE, data.ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine("Fail to Save Globals");
                Console.WriteLine(e);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Load()
        {
            try
            {
                var backupDirectory = Path.GetDirectoryName(BACKUP_FILE);
                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                if (File.Exists(BACKUP_FILE))
                {
                    var data = File.ReadAllText(BACKUP_FILE).ConvertJsonToObject<BackupData>();

                    Session = data.Session;
                    Domains = data.Domains;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fail to Load Globals");
                Console.WriteLine(e);
            }
        }
        #endregion
    }

    public class BackupData
    {
        public string Session { get; set; }
        public List<string> Domains { get; set; }

        public BackupData(string session, List<string> domains)
        {
            Session = session;
            Domains = domains.ToList();
        }
    }
}
