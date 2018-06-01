using Newtonsoft.Json;
using RestSharp;
using System;

namespace RadSynopticReportGenerator {
  class Program {

    private static string baseFhir => "http://hackathon.siim.org/fhir/";
    private static string baseDicomWeb => "http://hackathon.siim.org/dicomweb/";

    static void Main(string[] args) {
      //var response = runApiRequest(Method.GET, baseFhir, "Patient?name=siimjoe");
      //var results = JsonConvert.DeserializeObject<dynamic>(response.Content);
      //var entry = results.entry;

      getDiagnosticReportsForSubjectByProcedureCode("siimjoe", "24627-2");
    }

    //information for the comoparison studies
    static void getDiagnosticReportsForSubjectByProcedureCode(string subject, string procedureCode) {
      var response = runApiRequest(Method.GET, baseFhir, $"DiagnosticReport?subject={subject}&code={procedureCode}");
      var results = JsonConvert.DeserializeObject<dynamic>(response.Content);

      var mostRecentDate = results.entry[0].resource.effectiveDateTime.Value;
      var mostRecentConclusion = results.entry[0].resource.conclusion.Value;

      System.IO.File.WriteAllText(@"C:\Users\Peter\Documents\GitHub\output.txt", response.Content.ToString());

      //var diagnosticReports = JsonDomFhirNavigator.Create(response.Content);
    }

    static IRestResponse runApiRequest(RestSharp.Method method, string endpoint, string query) {
      var request = new RestRequest(method);
      var client = new RestClient(endpoint + query);
      request.AddHeader("apikey", Environment.GetEnvironmentVariable("SiimApiKey"));
      var result = client.Execute(request);
      return result;
    }
  }
}
