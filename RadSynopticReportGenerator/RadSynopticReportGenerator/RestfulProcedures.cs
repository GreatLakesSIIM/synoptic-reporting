using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace RadSynopticReportGenerator {
  public enum DicomAttributeKeyword {
    StudyDate,
    AccessionNumber,  // CDA: identifer
    PatientName,      // CDA: subject
    PatientId,
    StudyInstanceUid,
    NumberOfStudyRelatedSeries,
    NumberOfStudyRelatedInstances
  }

  class RestfulProcedures {
    private static string baseFhir => "http://hackathon.siim.org/fhir/";
    private static string baseDicomWeb => "http://hackathon.siim.org/dicomweb/";

    private static string exampleStudyInstanceUID => "1.3.6.1.4.1.14519.5.2.1.7777.9002.701296064147831952903543555759";

    private static string _StudyInstanceUid { get; set; }

    private static string dicomAttributeMapTable(DicomAttributeKeyword keyword) =>
      (keyword == DicomAttributeKeyword.StudyDate) ? "StudyDate"
      : (keyword == DicomAttributeKeyword.AccessionNumber) ? "AccessionNumber"
      : (keyword == DicomAttributeKeyword.PatientName) ? "PatientName"
      : (keyword == DicomAttributeKeyword.PatientId) ? "PatientID"
      : (keyword == DicomAttributeKeyword.StudyInstanceUid) ? "StudyInstanceUID"
      : (keyword == DicomAttributeKeyword.NumberOfStudyRelatedSeries) ? "NumberOfStudyRelatedSeries"
      : (keyword == DicomAttributeKeyword.NumberOfStudyRelatedInstances) ? "NumberOfStudyRelatedInstances"
      : "default";

    private static IRestResponse runApiRequest(RestSharp.Method method, string endpoint, string query) {
      var request = new RestRequest(method);
      var client = new RestClient(endpoint + query);
      request.AddHeader("apikey", Environment.GetEnvironmentVariable("SiimApiKey"));
      return client.Execute(request);
    }

    public static dynamic GetFirstEntryResourceFromFhirDiagnosticReportForSubjectByCode(string subject, string procedureCode) =>
        JsonConvert.DeserializeObject<dynamic>(
            runApiRequest(Method.GET, baseFhir, $"DiagnosticReport?subject={subject}&code={procedureCode}").Content)
          .entry[0].resource;

    public static string GetAttributesFromStudyByUid(DicomAttributeKeyword keyword, string uid = null) {
      _StudyInstanceUid = uid ?? exampleStudyInstanceUID;
      var attribute = dicomAttributeMapTable(keyword);
      var response = runApiRequest(Method.GET, baseDicomWeb, $"studies/?StudyInstanceUID={_StudyInstanceUid}");
      if (response.IsSuccessful) {
        using (var reader = XmlReader.Create(new StringReader(response.Content))) {
          reader.ReadToDescendant("NativeDicomModel");
          reader.ReadToFollowing($"keyword={attribute}");
          reader.MoveToFirstAttribute();
          var val = reader.Value;
          return val;
        }
      }
      return "error: something went wrong";
    }
  }
}
