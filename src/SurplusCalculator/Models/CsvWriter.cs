using System;
using System.IO;
using System.Text;

namespace SurplusCalculator.Models
{
    public class CsvWriter : IDisposable
    {
        #region Fields

        private readonly StreamWriter _streamWriter;
        
        #endregion

        #region Ctor

        public CsvWriter(string path)
        {
            _streamWriter = new StreamWriter(path, false, Encoding.UTF8);
        }

        #endregion

        public void Write(object content)
        {
            _streamWriter.Write(content);
            _streamWriter.Write(';');
        }

        public void AddNewLine()
        {
            _streamWriter.Write(Environment.NewLine);
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _streamWriter.Close();
                _streamWriter.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
