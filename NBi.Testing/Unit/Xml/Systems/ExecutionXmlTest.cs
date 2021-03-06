﻿using System.IO;
using System.Reflection;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class ExecutionXmlTest
    {

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
           
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void GetQuery_FilenameSpecified_RetrieveContentOfFile()
        {
            //create a text file on disk
            var filename = DiskOnFile.CreatePhysicalFile("QueryFile.sql", "NBi.Testing.Unit.Xml.Resources.QueryFile.sql");
           
            //Instantiate a Test Case and specify to find the sql in the file created above
            var testCase = new ExecutionXml()
            {
                Item = new QueryXml() { File = filename }
            };

            // A Stream is needed to read the text file from the assembly.
            string expectedContent;
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.QueryFile.sql"))
                using (StreamReader reader = new StreamReader(stream))
                   expectedContent = reader.ReadToEnd();
            
            Assert.AreEqual(expectedContent, (testCase.Item as QueryableXml).GetQuery());
        }

        [Test]
        public void GetQuery_FilenameNotSpecified_RetrieveContentOfInlineQuery()
        {
            //Instantiate a System Under Test
            var systemUnderTest = new ExecutionXml() 
            {
                Item = new QueryXml() { InlineQuery = "SELECT * FROM Product" }
            };

            Assert.That(((QueryXml)systemUnderTest.Item).GetQuery(), Is.EqualTo("SELECT * FROM Product"));
            Assert.That(((QueryXml)systemUnderTest.Item).InlineQuery, Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(((QueryXml)systemUnderTest.Item).File, Is.Null);
        }

        [Test]
        public void GetQuery_FileNameSpecified_RetrieveContentOfFile()
        {
            //Create the queryfile to read
            var filename = "Select all products.sql";
            DiskOnFile.CreatePhysicalFile(filename, "NBi.Testing.Unit.Xml.Resources.SelectAllProducts.sql");

            var systemUnderTest = new ExecutionXml()
            {
                Item = new QueryXml() { 
                    File = filename, 
                    Settings = new NBi.Xml.Settings.SettingsXml() { BasePath=DiskOnFile.GetDirectoryPath() }
                }
            };

            // Check the properties of the object.
            Assert.That(((QueryXml)systemUnderTest.Item).File, Is.Not.Null.And.Not.Empty);
            Assert.That(((QueryXml)systemUnderTest.Item).InlineQuery, Is.Null);
            Assert.That(((QueryXml)systemUnderTest.Item).GetQuery(), Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            
        }


        [Test]
        public void GetQuery_FilenameSpecified_RetrieveContentWithEuroSymbol()
        {
            //create a text file on disk
            var filename = DiskOnFile.CreatePhysicalFile("QueryFile€.mdx", "NBi.Testing.Unit.Xml.Resources.QueryFileEuro.mdx");

            //Instantiate a Test Case and specify to find the sql in the file created above
            var testCase = new ExecutionXml()
            {
                Item = new QueryXml() { File = filename }
            };

            // A Stream is needed to read the text file from the assembly.
            string expectedContent = "select [measure].[price €/Kg] on 0;";

            Assert.AreEqual(expectedContent, ((QueryableXml)testCase.Item).GetQuery());
        }
       
    }
}