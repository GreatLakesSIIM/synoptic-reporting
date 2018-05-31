using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace RadSynopticReportGenerator {
  class RadAppDelegatingHandler : DelegatingHandler {

    private string baseFhir => "http://hackathon.siim.org/fhir/";
    private string baseDicomWeb => "http://hackathon.siim.org/dicomweb/";
    private string apiKey => Environment.GetEnvironmentVariable("SiimApiKey");
    private DateTime beginningOfEpoch => new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
      var response = new HttpResponseMessage();

      request.Headers.Authorization = new AuthenticationHeaderValue("apikey", apiKey);

      response = await base.SendAsync(request, cancellationToken);
      return response;
    }
  }
}
