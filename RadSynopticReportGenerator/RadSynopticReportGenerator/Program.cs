using Newtonsoft.Json;
using RestSharp;
using System;

namespace RadSynopticReportGenerator {
  class Program {

    class DiagnosticReportGet {
      private static dynamic resource(string subject, string procedureCode) =>
        RestfulProcedures.GetEntryListFromFhirDiagnosticReportForSubjectByCode(subject, procedureCode);

      public static string Identifier(string subject, string procedureCode) =>
        resource(subject, procedureCode).identifier.Value;
      public static string Category(string subject, string procedureCode) =>
        resource(subject, procedureCode).category.Value;
      public static string Code(string subject, string procedureCode) =>
        resource(subject, procedureCode).code.Value;
      public static string CodedDiagnosis(string subject, string procedureCode) =>
        resource(subject, procedureCode).codedDiagnosis.Value;
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

      GenerateCdaImagingReport.CreateDicomCdaTemplateInXml();
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
