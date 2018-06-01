using System;
using System.Collections.Generic;
using System.Text;

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

  public enum FhirAttributesForDiagnosticReport {
    Identifier,
    BasedOn,
    Status,
    Category,
    Code,
    Subject,
    Context,
    Issued,
    PerformerRole,
    PerformerActor,
    Result,
    ImagingStudy,
    ImageLink,
    Conclusion,
    CodedDiagnosis,
    PresentedForm
  }

  class StandardObjectMapping {

    public static string DicomAttributeMapTable(DicomAttributeKeyword keyword) =>
      (keyword == DicomAttributeKeyword.StudyDate) ? "StudyDate"
      : (keyword == DicomAttributeKeyword.AccessionNumber) ? "AccessionNumber"
      : (keyword == DicomAttributeKeyword.PatientName) ? "PatientName"
      : (keyword == DicomAttributeKeyword.PatientId) ? "PatientID"
      : (keyword == DicomAttributeKeyword.StudyInstanceUid) ? "StudyInstanceUID"
      : (keyword == DicomAttributeKeyword.NumberOfStudyRelatedSeries) ? "NumberOfStudyRelatedSeries"
      : (keyword == DicomAttributeKeyword.NumberOfStudyRelatedInstances) ? "NumberOfStudyRelatedInstances"
      : "";

    public static string DiagnosticReportFhirMapTable(FhirAttributesForDiagnosticReport keyword) =>
      (keyword == FhirAttributesForDiagnosticReport.Identifier) ? "identifier"
      : (keyword == FhirAttributesForDiagnosticReport.BasedOn) ? "basedOn"
      : (keyword == FhirAttributesForDiagnosticReport.Status) ? "status"
      : (keyword == FhirAttributesForDiagnosticReport.Category) ? "category"
      : (keyword == FhirAttributesForDiagnosticReport.Code) ? "code"
      : (keyword == FhirAttributesForDiagnosticReport.Subject) ? "subject"
      : (keyword == FhirAttributesForDiagnosticReport.Context) ? "context"
      : "";

  }
}
