using Newtonsoft.Json;
using RestSharp;
using System;

namespace RadSynopticReportGenerator {
  class Program {

    class DiagnosticReportGet {
      private static dynamic resource(string subject, string procedureCode) =>
        RestfulProcedures.GetFirstEntryResourceFromFhirDiagnosticReportForSubjectByCode(subject, procedureCode);

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
      //var response = runApiRequest(Method.GET, baseFhir, "Patient?name=siimjoe");
      //var results = JsonConvert.DeserializeObject<dynamic>(response.Content);
      //var entry = results.entry;

      //getDiagnosticReportsForSubjectByProcedureCode("siimjoe", "24627-2");
      //var val = RestfulProcedures.GetValueFromStudyByDicomAttributeByUid(DicomAttributeKeyword.PatientName);

      var dx = RestfulProcedures.GetDiagnosticReportObjectById();
      var status = dx.Status;
      Console.WriteLine(status);
      Console.ReadLine();
    }

    //information for the comoparison studies
    static void getDiagnosticReportsForSubjectByProcedureCode(string subject, string procedureCode) {
      var resource = RestfulProcedures.GetFirstEntryResourceFromFhirDiagnosticReportForSubjectByCode(subject, procedureCode);

      var mostRecentDate = resource.resource.effectiveDateTime.Value;
      var mostRecentConclusion = resource.resource.conclusion.Value;

      System.IO.File.WriteAllText(@"C:\Users\Peter\Documents\GitHub\resource.txt", resource);

      //var diagnosticReports = JsonDomFhirNavigator.Create(response.Content);
    }

  }
}
