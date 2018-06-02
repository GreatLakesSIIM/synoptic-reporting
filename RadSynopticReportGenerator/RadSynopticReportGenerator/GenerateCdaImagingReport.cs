using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static System.Environment;

namespace RadSynopticReportGenerator {
  class GenerateCdaImagingReport {

    public static XDocument CreateDicomCdaTemplateInXml() {
      var cda = new XDocument(
        new XElement("ClinicalDocument", new XAttribute("uri", "http://dicom.nema.org/medical/dicom/current/output/html/part20.html#sect_7.1"),
          new XElement("templateId"),
          new XElement("code"),
          new XElement("component",
            new XElement("structuredBody",
              new XElement("component",
                new XElement("section", new XAttribute("id", "clinicalInformation"), new XAttribute("class", "level1"),
                  new XElement("label", ""),
                  new XElement("select", new XAttribute("for", "SELECTION_LIST_1527609006130"), new XAttribute("name", ""), new XAttribute("data-field-type", "SELECTION_LIST"), new XAttribute("data-field-completion-action", "NONE"), new XAttribute("value", "Normal"))),
                new XElement("section", new XAttribute("id", "procedureDescription")),
                new XElement("section", new XAttribute("id", "comparisonStudy")),
                new XElement("section", new XAttribute("id", "findings")),
                new XElement("section", new XAttribute("id", "impression"))
              )
            )
          )
        )
      );
      cda.Save(SpecialFolder.MyDocuments + @"\GitHub\RadReportTemplate.xml");
      return cda;
    }

    public static void AddElementsToTemplate() {
      var element = XElement.Load(SpecialFolder.MyDocuments + @"\GitHub\RadReportTemplate.xml");
      element.Elements("ClinicalDocument").Elements("component").Elements("section").Attributes();
    }

    //public static void ConvertHtmlToXml() {
    //  var text = File.OpenText(@"C:\Users\Peter\Documents\GitHub\synoptic-reporting\CT Chest Abdomen Pelvis.2018.05.29B.html");
    //  var line = text.ReadLine();
    //}

    public static void UpdateFindingsSectionWithChangedValues(Dictionary<string, string> dic) {
      //based on dictionary entries, update the values for the selected options of the report
      var element = XElement.Load(SpecialFolder.MyDocuments + @"\GitHub\RadReportTemplate.xml");
      //element.Elements
    }
  }
}
