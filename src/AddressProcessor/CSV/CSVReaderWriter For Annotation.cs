using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /// <summary>
    /// 1. There wasn't a way to read or write whole file at once. These functions would allow to seprate reading
    /// from checking data and from doing actions with them. The general rule is that code often has better 
    /// structure when you can things like this:
    /// data = fileReader.ReadFile();
    /// fileReader.Close();
    /// 
    /// checker.CheckData(data);
    /// 
    /// action.ProcessData(data);
    /// 
    /// instead of this:
    /// fileReader = new FileReader(filename);
    /// fileReader.ReadLine();
    /// CheckLine();
    /// ProcessLine();
    /// fileReader.CloseFile();
    /// 
    /// 2. There wasn't any tests for CSVReaderWriter, so added some to CSVReaderWriterTests.cs
    /// </summary>
    public class CSVReaderWriterForAnnotation
    {
        // Here should be fields' separating constant

        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        // No need for Flags attribute, because it would just enable us to do bitwise operations.
        // For example, var doubleMode = Mode.Read | Mode.Write.
        // But it's imposible to open the same file in two different modes.
        [Flags] 
        public enum Mode { Read = 1, Write = 2}; // No need to number enum - less code, less complexity

        public void Open(string fileName, Mode mode)
        {
            // We should check parameters. For example, whether file name is valid.
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                // No point in making different reading than writing. We can here use File.CreateText()
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            // There is no point in inventing such a thing. You can just use string.Join.
            // The less new code, the less space for mistakes.
            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        // This function doesn't do anything usefull, so, I removed it.
        public bool Read(string column1, string column2)
        {
            // If you were to use these constants and had two places where you need them
            // then you should make them global class constant
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            // Here should be used constant
            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
                // No check, that column length is at least size of 2
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        // This function allows only reading of two collumns, doesn't look usefull.
        // For simplicity of this exercise, I made it to return string array.
        public bool Read(out string column1, out string column2)
        {
            // If you were to use these constants and had two places where you need them
            // then you should make them global class constant
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            // Here should be used constant
            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            // No check, that column length is at least size of 2
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        // Function for one line looks like an additional code, which isn't needed here.
        // So, I removed WriteLine and ReadLine
        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
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
