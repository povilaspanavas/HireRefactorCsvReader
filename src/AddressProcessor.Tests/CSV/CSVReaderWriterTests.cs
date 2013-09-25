using AddressProcessing.CSV;
using AddressProcessing.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private CSVReaderWriter csvReaderWriter = new CSVReaderWriter();
        private const string TestOutputFile = @"test_data\writeOutput.csv";
        private const string TestInputFile = AddressFileProcessorTests.TestInputFile;
    	
        [Test]
    	public void WriteLine_Nothing()
    	{
    		csvReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Write);
    		csvReaderWriter.WriteLine();
    		csvReaderWriter.Close();
    		
    		StreamReader reader = new FileInfo(TestOutputFile).OpenText();
    		Assert.That(reader.ReadToEnd(), Is.EqualTo("\r\n"));
    		reader.Close();
    	}
    	
        [Test]
    	public void WriteLine()
    	{
    		csvReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Write);
    		csvReaderWriter.WriteLine("column1", "column2");
    		csvReaderWriter.Close();
    		
    		StreamReader reader = new FileInfo(TestOutputFile).OpenText();
    		Assert.That(reader.ReadToEnd(), Is.EqualTo("column1" + "\t" + "column2" + "\r\n"));
    		reader.Close();
    	}
    	
    	 [Test]
    	public void WriteWholeFile()
    	{
    		var data = new List<string[]>();
    		data.Add(new string[] { "column1line1", "column2line1" });
    		data.Add(new string[] { "column1line2", "column2line2" });
    		csvReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Write);
    		csvReaderWriter.WriteWholeFile(data);
    		csvReaderWriter.Close();
    		
    		StreamReader reader = new FileInfo(TestOutputFile).OpenText();
    		Assert.That(reader.ReadLine(), Is.EqualTo("column1line1" + "\t" + "column2line1"));
    		Assert.That(reader.ReadLine(), Is.EqualTo("column1line2" + "\t" + "column2line2"));
    		reader.Close();
    	}
    	
    	[Test]
    	public void ReadLine()
    	{
    		csvReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
    		string[] line = csvReaderWriter.ReadLine();
    		csvReaderWriter.Close();
    		
    		Assert.That(line[0], Is.EqualTo("Shelby Macias"));
    		Assert.That(line[1], Is.EqualTo("3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|Finland"));
    	}
    	
    	[Test]
    	public void ReadWholeFile()
    	{
    		csvReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
    		List<string[]> lines = csvReaderWriter.ReadWholeFile();
    		csvReaderWriter.Close();
    		
    		Assert.That(lines.Count, Is.EqualTo(229));
    		Assert.That(lines[0][0], Is.EqualTo("Shelby Macias"));
    		Assert.That(lines[0][1], Is.EqualTo("3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|Finland"));
    		
    		// checking the last line
    		Assert.That(lines[lines.Count - 1][0], Is.EqualTo("Leila Neal"));
    		Assert.That(lines[lines.Count - 1][1], 
    		            Is.EqualTo("P.O. Box 359, 5784 Sociis Rd.|Bartlesville|North Dakota|86561|Martinique"));
    	}
    	
    	[Test]
    	public void WriteReadLine()
    	{
    		csvReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Write);
    		csvReaderWriter.WriteLine("column1", "column2");
    		csvReaderWriter.Close();
    		
    		csvReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Read);
    		string[] line = csvReaderWriter.ReadLine();
    		csvReaderWriter.Close();
    		
    		Assert.That(line[0], Is.EqualTo("column1"));
    		Assert.That(line[1], Is.EqualTo("column2"));
    	}
    	
    }
}
