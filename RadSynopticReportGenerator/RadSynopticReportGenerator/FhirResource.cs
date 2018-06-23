using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;


namespace RadSynopticReportGenerator {
  class FhirResource {

    public static Patient DefaultPatient => new Patient() {
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

  }
}
