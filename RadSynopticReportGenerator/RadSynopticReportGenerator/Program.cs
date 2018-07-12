using Hl7.Fhir.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Environment;

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
    public string Code => "CT Abdomen-Pelvis"; //dx.Code.Text;
    public string CodedDiagnosisDisplay => dx.CodedDiagnosis[0].Coding[0].Display;//.Text;
  }

  class Program {

    //static Dictionary<string,string> RadlexToLoinc

    // from https://www.hl7.org/fhir/observation-example-f204-creatinine.json.html
    static Observation getExampleObservation() =>
      new Observation() {
        Id = "f204",
        Text = new Narrative() {
          Status = Narrative.NarrativeStatus.Generated,
          Div = "<div xmlns=\"http://www.w3.org/1999/xhtml\"><p><b>Generated Narrative with Details</b></p><p><b>id</b>: f204</p><p><b>identifier</b>: 1304-03720-Creatinine</p><p><b>status</b>: final</p><p><b>code</b>: Creatinine(Serum) <span>(Details : {https://intranet.aumc.nl/labtestcodes code '20005' = '20005', given as 'Creatinine(Serum)'})</span></p><p><b>subject</b>: <a>Roel</a></p><p><b>issued</b>: 04/04/2013 2:34:00 PM</p><p><b>performer</b>: <a>Luigi Maas</a></p><p><b>value</b>: 122 umol/L<span> (Details: SNOMED CT code 258814008 = 'umol/L')</span></p><p><b>interpretation</b>: Serum creatinine raised <span>(Details : {SNOMED CT code '166717003' = 'Serum creatinine raised', given as 'Serum creatinine raised'}; {http://hl7.org/fhir/v2/0078 code 'H' = 'High)</span></p><h3>ReferenceRanges</h3><table><tr><td>-</td><td><b>Low</b></td><td><b>High</b></td><td><b>Type</b></td></tr><tr><td>*</td><td>64</td><td>104</td><td>Normal Range <span>(Details : {http://hl7.org/fhir/referencerange-meaning code 'normal' = 'Normal Range', given as 'Normal Range'})</span></td></tr></table></div>"
        },
        Identifier = new List<Identifier>() {
          new Identifier() {
            System = "https://intranet.aumc.nl/labvalues",
            Value = "1304-03720-Creatinine"
          }
        },
        Status = ObservationStatus.Final,
        Code = new CodeableConcept() {
          Coding = new List<Coding>() {
            new Coding(){
              System = "https://intranet.aumc.nl/labtestcodes",
              Code = "20005",
              Display = "Creatinine(Serum)"
            }
          }
        },
        Subject = new ResourceReference("Patient/f201", "Roel"),
        Issued = new DateTimeOffset(2013, 04, 04, 14, 34, 00, new TimeSpan(1, 0, 0)),
        Performer = new List<ResourceReference>() {
          new ResourceReference() {
            Reference = "Practitioner/f202",
            Display = "Luigi Maas"
          }
        },
        Value = new Quantity() {
          Value = 122,
          Unit = "umol/L",
          System = "http://snomed.info/sct",
          Code = "258814008"
        },
        Interpretation = new CodeableConcept() {
          Coding = new List<Coding>() {
            new Coding(){
              System = "http://snomed.info/sct",
              Code = "166717003",
              Display = "Serum creatinine raised"
            },
            new Coding() {
              System = "http://hl7.org/fhir/v2/0078",
              Code = "H"
            }
          }
        },
        ReferenceRange = new List<Observation.ReferenceRangeComponent> {
          new Observation.ReferenceRangeComponent() {
            Low = new SimpleQuantity() { Value=64 },
            High = new SimpleQuantity() { Value=104 },
            Type = new CodeableConcept() {
              Coding = new List<Coding>() {
                new Coding(){
                  System = "http://hl7.org/fhir/referencerange-meaning",
                  Code = "normal",
                  Display= "Normal Range"
                }
              }
            }
          }
        }
      };

    static List<string> radlex_includes = new List<string>();

    static void Demo() {
      var customDxRpt = new DiagnosticReportGet("a654061970756517");
      Console.WriteLine($"report id: {customDxRpt.Identifier}");
      Console.WriteLine($"report category: {customDxRpt.Category}");
      Console.WriteLine($"report code: {customDxRpt.Code}");
      Console.WriteLine($"report coded diagnosis: {customDxRpt.CodedDiagnosisDisplay}");
    }

    static void Main(string[] args) {

      var diagnoses = new List<string>();

      foreach (var diagnosis in radlex_includes) {
        if (diagnosis == "RID4226") diagnoses.Add("35917007");
        else diagnoses.Add(Mapping.RidToLoinc[diagnosis]);
      }

      var url = "http://hackathon.siim.org/fhir/DiagnosticReport";
      var query = $"diagnosis={diagnoses.ToArray()}";
      //var headers = n {
      //  { "apikey", "eee630b7-2669-4a56-843b-eb88b4dff02f" },
      //          { "Cache-Control", "no-cache"},
      //          { "Postman-Token","81271c96-c884-412d-ab15-cff1dca4e342" },
      //          { "accept", "text/xml"}
      //};

      Console.WriteLine("Press Enter to continue");
      Console.ReadLine();
    }

    //information for the comoparison studies
    static void getComparisonAttributesBySubjectByCode(string subject, string procedureCode) {
      var dxReportBundle = RestfulProcedures.GetBundleDiagnosticReportForOptionalCriteria(new string[] { $"subject={subject}", $"code={procedureCode}" });

      var dxReport = (DiagnosticReport)dxReportBundle.Entry[0].Resource;
      var mostRecentDate = dxReport.Effective;
      var mostRecentConclusion = dxReport.Conclusion;

      //System.IO.File.WriteAllText(@"C:\Users\Peter\Documents\GitHub\resource.txt", entries);

      //var diagnosticReports = JsonDomFhirNavigator.Create(response.Content);
    }

    // VNA stuff:

    static void queryForResourcesByParameters(/* TODO: insert parameters of interest */) {
      var observation = getExampleObservation();

      var result = observation.Interpretation.Coding;

      Console.WriteLine(result);


      //var obsTerms = new string[] { $"code={code}" };
      var observationsList = RestfulProcedures.GetListOfObservationResourcesForSearchTerms();

      foreach (var o in observationsList) {
        Console.WriteLine(o.Id);
      }


    }

  }
}
