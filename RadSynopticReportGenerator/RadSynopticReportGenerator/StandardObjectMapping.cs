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

  public enum DiagnosticReportFhir {
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

    public static string DiagnosticReportFhirMapTable(DiagnosticReportFhir keyword) =>
      (keyword == DiagnosticReportFhir.Identifier) ? "identifier"
      : (keyword == DiagnosticReportFhir.BasedOn) ? "basedOn"
      : (keyword == DiagnosticReportFhir.Status) ? "status"
      : (keyword == DiagnosticReportFhir.Category) ? "category"
      : (keyword == DiagnosticReportFhir.Code) ? "code"
      : (keyword == DiagnosticReportFhir.Subject) ? "subject"
      : (keyword == DiagnosticReportFhir.Context) ? "context"
      : "";

  }
}
