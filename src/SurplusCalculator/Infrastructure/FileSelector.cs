using Microsoft.Win32;

namespace SurplusCalculator.Infrastructure
{
    public interface IFileSelector
    {
        string SelectFile();
    }

    public class FileSelector : IFileSelector
    {
        #region Implementation of IFileSelector

        public string SelectFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "csv|*.csv|Text files|*.txt|All Files|*.*"
            };
            if (openFileDialog.ShowDialog() != true)
                return null;
            return openFileDialog.FileName;
        }

        #endregion
    }
}
