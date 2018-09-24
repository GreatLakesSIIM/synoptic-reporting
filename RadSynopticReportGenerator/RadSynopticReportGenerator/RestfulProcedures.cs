using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using ResultDotNet;
using static ResultDotNet.Result;

namespace RadSynopticReportGenerator {
  public class RestfulProcedures {
    private static string baseSiimFhir => "http://hackathon.siim.org/fhir/";
    private static string baseDicomWeb => "http://hackathon.siim.org/dicomweb/";
    private static string baseTestFhir => "http://test.fhir.org/r4/";

    //TODO: create field for referencing source database url in GUI

    private static string exampleDiagnosticReportId => "a654061970756517";
    private static string exampleStudyInstanceUid => "1.3.6.1.4.1.14519.5.2.1.7777.9002.701296064147831952903543555759";

    private static string _StudyInstanceUid { get; set; } // dicom
    private static string _Identifier { get; set; } // fhir

    public static FhirClient OpenFhirClient(string endpoint) {
      var client = new FhirClient(baseSiimFhir) {
        PreferredFormat = ResourceFormat.Json
      };
      client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) => {
        e.RawRequest.Headers.Add("apikey", Environment.GetEnvironmentVariable("SiimApiKey")); //requires environment variable to match
      };
      return client;
    }

    // VNA paper stuff:

    public static List<Observation> GetListOfObservationResourcesForSearchTerms(string[] search = null) {
      var searchTerms = search ?? new string[] { "" };
      var resourceList = new List<Observation>();
      var client = new FhirClient(baseTestFhir) {
        PreferredFormat = ResourceFormat.Json
      };
      foreach (var entry in client.Search<Observation>(searchTerms).Entry) {
        if (entry.HasResource()) {
          resourceList.Add((Observation)entry.Resource);
        }
      }
      return resourceList;
    }

    public static List<DiagnosticReport> GetListOfDiagnosticReportsForSearchTerms(string[] search = null) {
      var searchTerms = search ?? new string[] { "" };
      var resourceList = new List<DiagnosticReport>();
      var client = new FhirClient(baseTestFhir) {
        PreferredFormat = ResourceFormat.Json
      };
      foreach (var entry in client.Search<DiagnosticReport>(searchTerms).Entry) {
        if (entry.HasResource()) {
          resourceList.Add((DiagnosticReport)entry.Resource);
        }
      }
      return resourceList;
    }

    public static DiagnosticReport GetDiagnosticReportObjectById(string id = null) {
      _Identifier = id ?? exampleDiagnosticReportId;
      return OpenFhirClient(baseSiimFhir).Read<DiagnosticReport>($"DiagnosticReport/{_Identifier}");
    }

    public static Bundle GetBundleDiagnosticReportForOptionalCriteria(string[] searchTerms = null) =>
      OpenFhirClient(baseSiimFhir).Search<DiagnosticReport>(searchTerms ?? new string[] { "" });

    public static Patient GetPatientByName(string name) =>
      OpenFhirClient(baseSiimFhir).Read<Patient>($"Patient/{name}");

    public static DiagnosticReport GetDiagnosticReport(string id = null) =>
      OpenFhirClient(baseSiimFhir).Read<DiagnosticReport>($"DiagnosticReport/{id ?? exampleDiagnosticReportId}");

    public static ImagingStudy GetImagingStudy(string id = null) =>
      OpenFhirClient(baseSiimFhir).Read<ImagingStudy>($"ImagingStudy/{id ?? exampleDiagnosticReportId}");

    public static List<Bundle.EntryComponent> GetEntryListFromFhirDiagnosticReportForSubjectByCode(string subject, string procedureCode) =>
      GetBundleDiagnosticReportForOptionalCriteria(new string[] { $"subject={subject}", $"code={procedureCode}" }).Entry;

    public static List<Bundle.EntryComponent> GetEntryListFromFhirDiagnosticReportForSubject(ResourceReference subject, CodeableConcept procedureCode) =>
      GetBundleDiagnosticReportForOptionalCriteria(new string[] { $"subject={subject.Reference}", $"code={procedureCode.Text}" }).Entry;

    private static IRestResponse runApiRequest(RestSharp.Method method, string endpoint, string query) {
      var request = new RestRequest(method);
      var client = new RestClient(endpoint + query);
      request.AddHeader("apikey", Environment.GetEnvironmentVariable("SiimApiKey"));
      return client.Execute(request);
    }

    public static object GetValueFromStudyByDicomAttributeByUid(DicomAttributeKeyword keyword, string uid = null) {
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
              return reader.ReadElementContentAsObject();
            }
          }
        }
      }
      return "error: something went wrong";
    }

    public static Result<bool, string> PostDiagnosticReport() {
      var patient = FhirResource.DefaultPatient;
      OpenFhirClient(baseSiimFhir).Create(patient);

      //create diagnostic report with random identifier, assigning authority = MCW
      var study = GetImagingStudy();

      var dx = new DiagnosticReport {
        Identifier = patient.Identifier,
        Id = "siimdxrpt77",
        ImagingStudy = study.ProcedureReference,
        Code = new CodeableConcept("MCW", "birdhouse")
      };
      OpenFhirClient(baseSiimFhir).Create(dx);

      return Ok<bool, string>(true);
    }
  }
}
