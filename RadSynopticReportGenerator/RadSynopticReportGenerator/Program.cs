using Hl7.Fhir.Model;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace RadSynopticReportGenerator {

  public class DiagnosticReportGet {
    private static string identifierCode;
    private static DiagnosticReport dx =>
      RestfulProcedures.GetDiagnosticReportObjectById();

    public DiagnosticReportGet(string id) {
      identifierCode = id;
    }

    public string Identifier => dx.Identifier[0].Value;
    public string Category => dx.Category.Coding[0].Code;
    public string Code => dx.Code.Text;
    public string CodedDiagnosis => dx.CodedDiagnosis[0].Text;
  }

  class Program {

    static void Demo() {
      var customDxRpt = new DiagnosticReportGet("a654061970756517");
      Console.WriteLine($"report id: {customDxRpt.Identifier}");
      Console.WriteLine($"report category: {customDxRpt.Category}");
    }

    static void Main(string[] args) {

      //var findings = args[0];

      //var response = runApiRequest(Method.GET, baseFhir, "Patient?name=siimjoe");
      //var results = JsonConvert.DeserializeObject<dynamic>(response.Content);
      //var entry = results.entry;

      //getDiagnosticReportsForSubjectByProcedureCode("siimjoe", "24627-2");
      //var val = RestfulProcedures.GetValueFromStudyByDicomAttributeByUid(DicomAttributeKeyword.PatientName);

      //var dx = RestfulProcedures.GetDiagnosticReportObjectById();
      //var status = dx.Status;
      //Console.WriteLine(status);

      //getComparisonAttributesBySubjectByCode("siimjoe", "24627-2");
      //      RestfulProcedures.PostDiagnosticReport();

      //GenerateCdaImagingReport.CreateDicomCdaTemplateInXml();

      Demo();
      Console.ReadLine();
    }

    //information for the comoparison studies
    static void getComparisonAttributesBySubjectByCode(string subject, string procedureCode) {
      var bundle = RestfulProcedures.GetBundleDiagnosticReportForOptionalCriteria(new string[] { $"subject={subject}", $"code={procedureCode}" });

      dynamic resource = bundle.Entry[0].Resource;
      var mostRecentDate = resource.Effective;
      var mostRecentConclusion = resource.Conclusion;

      //System.IO.File.WriteAllText(@"C:\Users\Peter\Documents\GitHub\resource.txt", entries);

      //var diagnosticReports = JsonDomFhirNavigator.Create(response.Content);
    }

  }
}
