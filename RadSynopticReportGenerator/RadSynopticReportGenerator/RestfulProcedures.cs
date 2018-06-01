﻿using Newtonsoft.Json;
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
    private static string baseFhir => "http://hackathon.siim.org/fhir/";
    private static string baseDicomWeb => "http://hackathon.siim.org/dicomweb/";

    private static string exampleDiagnosticReportId => "a654061970756517";
    private static string exampleStudyInstanceUid => "1.3.6.1.4.1.14519.5.2.1.7777.9002.701296064147831952903543555759";

    private static string _StudyInstanceUid { get; set; } // dicom
    private static string _Identifier { get; set; } // fhir

    public static FhirClient OpenFhirClient(string endpoint) {
      var client = new FhirClient(baseFhir) {
        PreferredFormat = ResourceFormat.Json
      };
      client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) => {
        e.RawRequest.Headers.Add("apikey", Environment.GetEnvironmentVariable("SiimApiKey"));
      };
      return client;
    }

    public static DiagnosticReport GetDiagnosticReportObjectById(string id = null) {
      _Identifier = id ?? exampleDiagnosticReportId;
      return OpenFhirClient(baseFhir).Read<DiagnosticReport>($"DiagnosticReport/{_Identifier}");
    }

    public static Bundle GetBundleDiagnosticReportForOptionalCriteria(string[] searchTerms = null) =>
      OpenFhirClient(baseFhir).Search<DiagnosticReport>(searchTerms ?? new string[] { "" });

    public static Patient GetPatientByName(string name) =>
      OpenFhirClient(baseFhir).Read<Patient>($"Patient/{name}");

    public static DiagnosticReport GetDiagnosticReport(string id = null) =>
      OpenFhirClient(baseFhir).Read<DiagnosticReport>($"DiagnosticReport/{id ?? exampleDiagnosticReportId}");

    public static ImagingStudy GetImagingStudy(string id = null) =>
      OpenFhirClient(baseFhir).Read<ImagingStudy>($"ImagingStudy/{id ?? exampleDiagnosticReportId}");

    private static string getDiagnosticReportIdForSubjectByCode(string subject, string procedureCode) {
      return exampleDiagnosticReportId;
      //return "";
    }

    public static List<Bundle.EntryComponent> GetEntryListFromFhirDiagnosticReportForSubjectByCode(string subject, string procedureCode) =>
      GetBundleDiagnosticReportForOptionalCriteria(new string[] { $"subject={subject}", $"code={procedureCode}" }).Entry;

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
      var patient = new Patient();
      patient.Name.Add(new HumanName());
      patient.Identifier.Add(new Identifier("MCW", "yuengling"));
      //create diagnostic report with random identifier, assigning authority = MCW
      var study = GetImagingStudy();
      var dx = new DiagnosticReport {
        Identifier = patient.Identifier,
        Id = "siimdxrpt77",
        ImagingStudy = study.ProcedureReference,
        Code = new CodeableConcept("MCW", "birdhouse")
      };
      OpenFhirClient(baseFhir).Create(dx);
      return Ok<bool, string>(true);
    }
  }
}
