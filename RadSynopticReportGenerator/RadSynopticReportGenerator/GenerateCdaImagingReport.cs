using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
                new XElement("section", new XAttribute("busName", "ClinicalInformation")),
                new XElement("section", new XAttribute("busName", "ProcedureDescription")),
                new XElement("section", new XAttribute("busName", "ComparisonStudy")),
                new XElement("section", new XAttribute("busName", "Findings")),
                new XElement("section", new XAttribute("busName", "Impression"))
              )
            )
          )
        )
      );
      cda.Save(@"C:\Users\Peter\Documents\GitHub\RadReportTemplate.xml");
      return cda;
    }

    public static void AddElementsToTemplate() {

    }
  }
}
