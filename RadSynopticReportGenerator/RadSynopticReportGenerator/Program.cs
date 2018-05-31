using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace RadSynopticReportGenerator {
  class Program {

    class RadAppDelegatingHandler : DelegatingHandler {
      private string apiKey => Environment.GetEnvironmentVariable("SiimApiKey");
      private DateTime beginningOfEpoch => new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);

      protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        var response = new HttpResponseMessage();

        request.Headers.Authorization = new AuthenticationHeaderValue("apikey", apiKey);

        response = await base.SendAsync(request, cancellationToken);
        return response;
      }
    }

    public class Patient {
      public string PatientID { get; set; }
    }

    private static string baseFhir => "http://hackathon.siim.org/fhir/";
    private static string baseDicomWeb => "http://hackathon.siim.org/dicomweb/";

    private static string exampleExtension => "Patient?family=SIIM";

    static async Task RunAsync() {
      var delegatingHandler = new RadAppDelegatingHandler();
      var client = HttpClientFactory.Create(delegatingHandler);
      var patient = new Patient { PatientID = "1234" };

      var postString = baseFhir + ""; //could also use baseDicomWeb
      var response = await client.PostAsXmlAsync(postString, patient);

      if (response.IsSuccessStatusCode) {
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
        Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode, response.ReasonPhrase);
      } else {
        Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
      }
      Console.ReadLine();
    }

    static void Main(string[] args) {
      RunAsync().Wait();
    }
  }
}
