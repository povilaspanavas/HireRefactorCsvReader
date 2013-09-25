using AddressProcessing.Address.v1;
using AddressProcessing.CSV;
using System;
using System.Collections.Generic;

namespace AddressProcessing.Address
{
    public class AddressFileProcessor
    {
        private readonly IMailShot _mailShot;

        public AddressFileProcessor(IMailShot mailShot)
        {
            if (mailShot == null) throw new ArgumentNullException("mailShot");
            _mailShot = mailShot;
        }

        public void Process(string inputFile)
        {
            var reader = new CSVReaderWriter();
            reader.Open(inputFile, CSVReaderWriter.Mode.Read);
            List<string[]> fileLines = reader.ReadWholeFile();
            reader.Close();

            for (int i = 0; i < fileLines.Count; i++)
            {
                string[] line = fileLines[i];
                _mailShot.SendMailShot(line[0], line[1]);
            }

            reader.Close();
        }
    }
}
