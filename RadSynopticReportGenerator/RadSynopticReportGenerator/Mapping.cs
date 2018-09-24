using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RadSynopticReportGenerator {

  static class ObjectExtension {
    public static Dictionary<string, string> ToKeysInDictionary(this List<string> keys, List<string> values) =>
      keys.Select((key, index) => new { key, val = values[index] })
        .ToDictionary(x => x.key, x => x.val);
  }

  class Mapping {

    private static List<string> loadColumn(string path, string head) {
      var list = new List<string>();
      using (var stream = new StreamReader(path)) {
        var offset = stream.ReadLine().Split(',')
                      .Select((s, i) => new { Index = i, Content = s })
                      .Where(o => o.Content == head)
                      .FirstOrDefault();
        while (!stream.EndOfStream) {
          list.Add(stream.ReadLine().Split(',')[offset.Index]);
        }
      }
      return list;
    }

    private static string radlexDotCsvResource = Properties.Resources.Radlex;

    private static List<string> loincCodesFromCsv => loadColumn(@".\lib\RADLEX_to_LOINC.csv", "LoincNumber");

    private static List<string> radlexCodesFromCsv => loadColumn(@".\lib\RADLEX_to_LOINC.csv", "RID");
    public static Dictionary<string, string> RidToLoinc =
      radlexCodesFromCsv.ToKeysInDictionary(loincCodesFromCsv);

    private static List<string> rpidCodesFromCsv => loadColumn(@"..\lib\RADLEX_to_LOINC.csv", "RPID");
    public static Dictionary<string, string> RpidToLoinc =
      rpidCodesFromCsv.ToKeysInDictionary(loincCodesFromCsv);

    private static List<string> radlexNamesFromCsv => loadColumn(@"..\lib\Radlex.csv", "Name or Synonym");
    private static List<string> radlexNumsFromCsv => loadColumn(@"..\lib\Radlex.csv", "RID");
    public static Dictionary<string, string> Radlex =
      radlexNamesFromCsv.ToKeysInDictionary(radlexNumsFromCsv);

    private static List<string> playbookNamesFromCsv => loadColumn(@"..\lib\complete-playbook-2_5.csv", "AUTOMATED_SHORT_NAME");
    private static List<string> playbookNumsFromCsv => loadColumn(@"..\lib\complete-playbook-2_5.csv", "RPID");
    public static Dictionary<string, string> Playbook =
      playbookNamesFromCsv.ToKeysInDictionary(playbookNumsFromCsv);
  }
}
