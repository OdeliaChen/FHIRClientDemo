//The code above demonstrates how to use the FHIR .NET API to retrieve a list of devices and device metrics from a FHIR server. The code creates a FhirClient instance with the base URI of the FHIR server and sets the access token for authentication. It then creates a Bundle object to contain the requests for the devices and device metrics. The code adds GET requests for the Device and DeviceMetric resources to the bundle and sends the transaction request to the FHIR server. The response is then processed to extract the devices and device metrics from the bundle entries. 
// The code uses the HttpClientEventHandler class to handle the authentication by adding the access token to the request headers. The OnBeforeRequest event is used to set the Authorization header with the access token. 
// The code demonstrates how to use the FHIR .NET API to interact with a FHIR server and retrieve resources using the transaction operation.It shows how to create a bundle of requests, send the transaction request to the server, and process the response to extract the resources. 
// Conclusion 
// In this article, we have explored how to use the FHIR .NET API to interact with FHIR servers and retrieve resources using the transaction operation.We have discussed the key concepts of FHIR, the FHIR .NET API, and how to use the API to perform transactions with a FHIR server. We have also provided a code example demonstrating how to use the FHIR .NET API to retrieve devices and device metrics from a FHIR server using the transaction operation.
// The FHIR .NET API provides a convenient way to work with FHIR resources and interact with FHIR servers in .NET applications. It simplifies the process of working with FHIR resources and provides a high-level API for performing CRUD operations and other FHIR operations. 
// I hope this article has been helpful in understanding how to use the FHIR .NET API to interact with FHIR servers and perform transactions. 
// Owner: Xiaomeng Chen
// Created Date: 2024/07/30

using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.Net.Http.Headers;
using System.Reflection.Metadata;


namespace FHIR.Demo
{
    public class Program
    {
        private static readonly string accessToken = "";
        private static readonly Uri _baseUri = new Uri("your uri");

        public static void Main(string[] args)
        {
            using (var handler = new HttpClientEventHandler())
            {
                using (FhirClient client = new FhirClient(_baseUri, messageHandler: handler))
                {
                    client.Settings.PreferredFormat = ResourceFormat.Json;
                    client.RequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));
                    handler.OnBeforeRequest += (sender, e) =>
                    {
                        e.RawRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    };
                    var requestBundle = new Bundle
                    {
                        Type = Bundle.BundleType.Transaction
                    };

                    // Add requests to the bundle
                    requestBundle.Entry.Add(new Bundle.EntryComponent
                    {
                        Request = new Bundle.RequestComponent
                        {
                            Method = Bundle.HTTPVerb.GET,
                            Url = "Device"
                        }
                    });

                    requestBundle.Entry.Add(new Bundle.EntryComponent
                    {
                        Request = new Bundle.RequestComponent
                        {
                            Method = Bundle.HTTPVerb.GET,
                            Url = "DeviceMetric"
                        }
                    });

                    var responseBundle = client.TransactionAsync(requestBundle).GetAwaiter().GetResult();
                    IList<Device> deviceList = new List<Device>();
                    IList<DeviceMetric> deviceMetricList = new List<DeviceMetric>();
                    if (requestBundle != null)
                    {
                        foreach (var entry in responseBundle.Entry)
                        {
                            if (entry.Resource is Bundle bundle)
                            {
                                foreach (var resource in bundle.Entry)
                                {
                                    if (resource.Resource is Device device)
                                    {
                                        deviceList.Add(device);
                                    }
                                    else if (resource.Resource is DeviceMetric deviceMetric)
                                    {
                                        deviceMetricList.Add(deviceMetric);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
 
