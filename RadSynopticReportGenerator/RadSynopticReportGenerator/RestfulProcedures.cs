using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;

namespace RadSynopticReportGenerator {

  class RestfulProcedures {
    private static string baseFhir => "http://hackathon.siim.org/fhir/";
    private static string baseDicomWeb => "http://hackathon.siim.org/dicomweb/";

    private static string exampleDiagnosticReportId => "a654061970756517";
    private static string exampleStudyInstanceUid => "1.3.6.1.4.1.14519.5.2.1.7777.9002.701296064147831952903543555759";

    private static string _StudyInstanceUid { get; set; } // dicom
    private static string _Identifier { get; set; } // fhir

    private static FhirClient openClient(string endpoint) {
      var client = new FhirClient(baseFhir) {
        PreferredFormat = ResourceFormat.Json
      };
      client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) => {
        e.RawRequest.Headers.Add("apikey", Environment.GetEnvironmentVariable("SiimApiKey"));
      };
      return client;
    }

    //trying it out
    public static DiagnosticReport GetDiagnosticReportObjectById(string id = null) {
      _Identifier = id ?? exampleDiagnosticReportId;
      return openClient(baseFhir).Read<DiagnosticReport>($"DiagnosticReport/{_Identifier}");
    }

    private static IRestResponse runApiRequest(RestSharp.Method method, string endpoint, string query) {
      var request = new RestRequest(method);
      var client = new RestClient(endpoint + query);
      request.AddHeader("apikey", Environment.GetEnvironmentVariable("SiimApiKey"));
      return client.Execute(request);
    }

    private static string getDiagnosticReportIdForSubjectByCode(string subject, string procedureCode) {
      return exampleDiagnosticReportId;
      //return "";
    }

    public static dynamic GetFirstEntryResourceFromFhirDiagnosticReportForSubjectByCode(string subject, string procedureCode) =>
        JsonConvert.DeserializeObject<dynamic>(
            runApiRequest(Method.GET, baseFhir, $"DiagnosticReport?subject={subject}&code={procedureCode}").Content)
          .entry[0].resource;

    public static string GetValueFromDiagnosticReportByReportIdByAttribute(string id, DiagnosticReportFhir key) {
      var response = runApiRequest(Method.GET, baseFhir, $"DiagnosticReport/{id}?_element={StandardObjectMapping.DiagnosticReportFhirMapTable(key)}");

      return "";
    }

    public static string GetValueFromStudyByDicomAttributeByUid(DicomAttributeKeyword keyword, string uid = null) {
      _StudyInstanceUid = uid ?? exampleStudyInstanceUid;
      var attribute = StandardObjectMapping.DicomAttributeMapTable(keyword);
      var response = runApiRequest(Method.GET, baseDicomWeb, $"studies/?StudyInstanceUID={_StudyInstanceUid}");
      if (response.IsSuccessful) {
        var removeFirstPart = response.Content.Substring(response.Content.IndexOf('<'));
        var stream = new StringReader(removeFirstPart.Substring(0, removeFirstPart.LastIndexOf('>')));
        using (var reader = XmlReader.Create(stream)) {
          while (reader.Read()) {
            reader.ReadToFollowing("DicomAttribute");
            if (reader.GetAttribute("keyword") == attribute) {
              reader.ReadToFollowing("Value");
              return reader.ReadElementContentAsString();
            }
          }
        }
      }
      return "error: something went wrong";
    }
  }
}
