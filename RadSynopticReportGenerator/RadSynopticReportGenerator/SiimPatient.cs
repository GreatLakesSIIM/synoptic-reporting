using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RadSynopticReportGenerator {
  class SiimPatient : Patient {
    public SiimPatient(Identifier id, HumanName humanName) {
      base.Identifier.Add(id);
      base.Name.Add(humanName);
    }
  }
}
