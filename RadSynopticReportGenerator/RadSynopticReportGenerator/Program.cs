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

    static List<string> radlex_includes = new List<string>();

    static void Main(string[] args) {

      queryForResourcesByParameters();
      RestfulProcedures.GetListOfDiagnosticReportsForSearchTerms(new string[] { $"diagnosis={Mapping.RidToLoinc["RID4226"]}" });

      Console.WriteLine("Press Enter to continue");
      Console.ReadLine();
    }

    // VNA stuff:
    //loosly based on https://github.com/GreatLakesSIIM/ai-portal/blob/d5a88c621e0c557c19ca26fd5219b4dd30b17920/src/AI_Portal_GUI.py#L376

    static void queryForResourcesByParameters(/* TODO: insert parameters of interest */) {
      getListOfDiagnosesFromGUI(); //still TODO

      var diagnoses = new List<string>();
      foreach (var diagnosis in radlex_includes) {
        if (diagnosis == "RID4226") diagnoses.Add("35917007");
        else diagnoses.Add(Mapping.RidToLoinc[diagnosis]);
      }

      if (diagnoses.Count > 0) {
        var idList = new List<string>();

        var response = RestfulProcedures.GetListOfDiagnosticReportsForSearchTerms(new string[] { $"diagnosis={diagnoses.ToArray()}" }); //this format for search params could be messy
        foreach (var dx in response) {
          Console.WriteLine(dx.Text);
        }
      }
    }

    static void previewDataSetCriteria() {
      //modality, date range, procedure, findings, impression, recommendation, critical results, diagnosis,
      //  age range, sex, smoking, ethnicity, max number of studies, 
      //  source, get reports, get studies with reports, create research PID
    }

    //TODO: get list of search diagnoses from GUI
    static List<string> getListOfDiagnosesFromGUI() => new List<string>();


  }
}
