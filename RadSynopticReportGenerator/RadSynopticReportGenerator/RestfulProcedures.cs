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
      var patient = new Patient() {
        Identifier = new List<Identifier> { new Identifier("MCW", "yuengling") },
        Active = true,
        Name = new List<HumanName> { new HumanName() { GivenElement = new List<FhirString> { new FhirString("example") } } },
        Telecom = new List<ContactPoint> { new ContactPoint() },
        Gender = AdministrativeGender.Unknown,
        BirthDateElement = new Date(1996),
        Address = new List<Address> { new Address() { City = "Milwaukee" } },
        MaritalStatus = new CodeableConcept("http://hl7.org/fhir/v3/MaritalStatus", "S", "Never Married"),
        Photo = new List<Attachment> { new Attachment() },
        Contact = new List<Patient.ContactComponent> { new Patient.ContactComponent {
            Relationship = new List<CodeableConcept> { new CodeableConcept("http://hl7.org/fhir/v2/0131", "C", "Emergency Contact") },
            Name = new HumanName() { GivenElement = new List<FhirString> { new FhirString("contact") } },
            Telecom = new List<ContactPoint> { new ContactPoint() },
            Address = new Address() { City = "Milwaukee" } ,
            Gender = AdministrativeGender.Unknown,
            Organization = new ResourceReference("ref","disp"),
            Period = new Period(new FhirDateTime(1996),new FhirDateTime(2019))} },
        Communication = new List<Patient.CommunicationComponent> { new Patient.CommunicationComponent() {
          Language = new CodeableConcept("http://hl7.org/fhir/ValueSet/languages", "en-US", "English (United States)"),
          Preferred = true } },
        GeneralPractitioner = new List<ResourceReference>(),
        //ManagingOrganization = new ResourceReference("MU-MCW", "TeamGreatLakes"),
        //Link = new List<Patient.LinkComponent> { new Patient.LinkComponent { Other = new ResourceReference("MU-MCW", "TeamGreatLakes"), Type = Patient.LinkType.Refer } }
      };
      OpenFhirClient(baseFhir).Create(patient);
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
