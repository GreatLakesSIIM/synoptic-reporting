using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RadSynopticReportGenerator {
  class ExampleResources {
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
  }
}
