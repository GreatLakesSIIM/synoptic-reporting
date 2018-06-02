using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RadSynopticReportGenerator {
  class FindingsBasedOnExampleTemplate235 {

    static string Liver;
    static string Gallbladder;
    static string BiliaryTree;
    static string Pancreas;
    static string Spleen;
    static string Adrenals;
    static string KidneysAndUreters;
    static string Bladder;
    static string ReproductiveOrgans;
    static string Bowel;

    public FindingsBasedOnExampleTemplate235(string jsonFormatString) {
      var splitters = new char[] { ':', ',' };
      var arr = (String[])jsonFormatString.Split(splitters);

    }
  }
}
