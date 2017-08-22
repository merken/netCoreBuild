using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;

namespace Merken.NetCoreBuild.Transform
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string xml = args[0];
                string xsl = args[1];
                string output = args[2];

                var xmlFile = ReadTextFile(xml);
                var xslFile = ReadTextFile(xsl);

                var transformedXml = Transform(xmlFile, xslFile);
                transformedXml.Save(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("exit");
        }

        static string ReadTextFile(string path)
        {
            string text = System.IO.File.ReadAllText(path);
            return text;
        }

        static void WriteTextFile(string path, string contents)
        {
            System.IO.File.WriteAllText(path, contents);
        }

        static XDocument Transform(string xml, string xsl)
        {
            var originalXml = XDocument.Load(new StringReader(xml));

            var transformedXml = new XDocument();
            using (var xmlWriter = transformedXml.CreateWriter())
            {
                var xslt = new XslCompiledTransform();
                xslt.Load(XmlReader.Create(new StringReader(xsl)));

                XsltArgumentList xsltArguments = null;

                xslt.Transform(originalXml.CreateReader(), xsltArguments, xmlWriter);
            }

            return transformedXml;
        }
    }
}
