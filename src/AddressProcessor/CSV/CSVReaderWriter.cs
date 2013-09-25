using System;
using System.Collections.Generic;
using System.IO;

namespace AddressProcessing.CSV
{
    public class CSVReaderWriter
    {
        private const string Separator = "\t";

        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        public enum Mode { Read, Write };

        public void Open(string fileName, Mode mode)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Filename must be not empty");

            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                _writerStream = File.CreateText(fileName);
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void WriteLine(params string[] columns)
        {
            string outPut = string.Join(Separator, columns);
            _writerStream.WriteLine(outPut);
        }

        public void WriteWholeFile(List<string[]> columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                WriteLine(columns[i]);
            }
        }

        public string[] ReadLine()
        {
            string line = _readerStream.ReadLine();
            if (line == null)
                return null;

            string[] columns = line.Split(Separator.ToCharArray());
            if (columns.Length == 0)
                return null;

            return columns;
        }

        public List<string[]> ReadWholeFile()
        {
            var result = new List<string[]>();
            while (true)
            {
                string[] columns = ReadLine();
                if (columns == null)
                    break;

                result.Add(columns);
            }
            return result;
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
