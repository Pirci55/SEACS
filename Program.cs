using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using IWshRuntimeLibrary;

namespace SFACS {
    internal class Program {
        static void CreateShortcut(string shortcutPath, string targetPath) {
            WshShell wshShell = new WshShell();
            IWshShortcut Shortcut = (IWshShortcut)wshShell.CreateShortcut(shortcutPath);
            Shortcut.TargetPath = targetPath;
            Shortcut.Save();
        }
        static void Main(string[] args) {
            // переменные
            string searchPath, filesType, finalPath;
            // параметры консоли
            Console.ForegroundColor = ConsoleColor.White;
            // лого
            Console.WriteLine();
            Console.WriteLine("   ######  #######    ###         #####      ######  ");
            Console.WriteLine("  ######   ######    #####      #########   ######   ");
            Console.WriteLine("  ##       ##       ##   ##    ###     ###  ##       ");
            Console.WriteLine("  ##       ##       ##   ##    ##           ##       ");
            Console.WriteLine("  ######   ######  ##     ##   ##           ######   ");
            Console.WriteLine("   ######  #####   ##     ##   ##            ######  ");
            Console.WriteLine("       ##  ##     ###########  ##                ##  ");
            Console.WriteLine("       ##  ##     ###     ###  ###     ###       ##  ");
            Console.WriteLine("   ######  ##     ##       ##   #########    ######  ");
            Console.WriteLine("  ######   ##     ##       ##     #####     ######   ");
            Console.WriteLine();
            // получаем данные от пользователя
            Console.Write("Тип файлов: "); filesType = Console.ReadLine();
            Console.Write("Путь для поиска: "); searchPath = Console.ReadLine();
            Console.Write("Путь для файлов: "); finalPath = Console.ReadLine();
            // убирает кавычки по краям
            if (finalPath.StartsWith("\"")) finalPath = finalPath.Substring(1);
            if (finalPath.EndsWith("\"")) finalPath = finalPath.Substring(0, finalPath.Length - 1);
            if (searchPath.StartsWith("\"")) searchPath = searchPath.Substring(1);
            if (searchPath.EndsWith("\"")) searchPath = searchPath.Substring(0, searchPath.Length - 1);
            // добавляет слэш в конец строки
            if (!finalPath.EndsWith("\\")) finalPath += "\\";
            if (!searchPath.EndsWith("\\")) searchPath += "\\";
            //Directory.CreateDirectory(finalPath + folder.Replace(searchPath, ""));
            void Search(string path) {
                if (path.EndsWith(filesType)) {
                    List<string> folderOrFile = new List<string>();
                    foreach (string element in path.Replace(searchPath, "").Split(new char[] { '\\' })) {
                        folderOrFile.Add(element);
                    };
                    if (folderOrFile.Count <= 1) {
                        folderOrFile.Add(folderOrFile.First());
                        folderOrFile.Insert(0, folderOrFile.Last().Substring(0, folderOrFile.Last().Length - filesType.Length));
                        Directory.CreateDirectory(finalPath + folderOrFile.First());
                    };
                    CreateShortcut(finalPath +
                        folderOrFile.First() +
                        "\\" +
                        folderOrFile.Last().Substring(0, folderOrFile.Last().Length - filesType.Length) +
                        ".lnk", path);
                    Console.WriteLine(path);
                };
                try {
                    foreach (string element in Directory.GetFileSystemEntries(searchPath)) {
                        Search(element);
                    };
                } catch { };
            };
            foreach (string element in Directory.GetFileSystemEntries(searchPath)) {
                //Search(element);
                Console.WriteLine(element);
            };

            Console.WriteLine("Готово! Для закрытия нажмите любую клавишу...");
            Console.ReadLine();
        }
    }
}
